using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.OtherModel;

namespace xinlongyuOfWpf.CustomControls.Extension
{
    /// <summary>
    /// MyDataGridControl.xaml 的交互逻辑
    /// </summary>
    public partial class MyDataGridControl : UserControl
    {
        /// <summary>
        /// 列名与属性名对应的字典
        /// 用于设置databinding
        /// 以及获取数据时将diactrion数组转换成list<object>
        /// </summary>
        private Dictionary<string, string> _propertyNameToColumnName;

        /// <summary>
        /// sql逻辑层
        /// </summary>
        private SqlController _sqlController;

        public MyDataGridControl()
        {
            InitializeComponent();
            _propertyNameToColumnName = new Dictionary<string, string>();
            _sqlController = new SqlController();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="listControlObj"></param>
        /// <param name="gridObj"></param>
        public void LoadData(List<ControlDetailForPage> listControlObj, ControlDetailForPage gridObj)
        {
            SetDataGridColumns(listControlObj, gridObj);
            SetDataSource(gridObj);
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        private void SetDataSource(ControlDetailForPage obj)
        {
            var sql = obj.d0;
            var dicarray = _sqlController.ExcuteSqlWithReturn(sql).data;
            //var dicarray = dicresult.Result.data;
            var listsource = DicArrayToListObject(dicarray);

            Binding binding = new Binding() { Source = listsource, IsAsync = true };
            binding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            mydatagridview.SetBinding(DataGrid.ItemsSourceProperty, binding);
            //mydatagridview.ItemsSource = listsource;
        }

        /// <summary>
        /// 将字典数组转换为泛型数组
        /// </summary>
        /// <param name="dicArray"></param>
        /// <returns></returns>
        private List<ObjectHelperForDataGrid> DicArrayToListObject(Dictionary<string, string>[] dicArray)
        {
            List<ObjectHelperForDataGrid> listreturn = new List<ObjectHelperForDataGrid>();
            var listProperty = typeof(ObjectHelperForDataGrid).GetProperties().Select(p => p.Name).ToList();
            foreach (Dictionary<string, string> dic in dicArray)
            {
                ObjectHelperForDataGrid obj = new ObjectHelperForDataGrid();
                
                foreach(string key in dic.Keys)
                {
                    if (!_propertyNameToColumnName.ContainsKey(key) || !listProperty.Contains(_propertyNameToColumnName[key])) continue;
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(_propertyNameToColumnName[key]);
                    propertyInfo.SetValue(obj, Convert.ChangeType(dic[key], propertyInfo.PropertyType), null);
                }
                listreturn.Add(obj);
            }
            return listreturn;
        }

        /// <summary>
        /// 设置表格的列
        /// </summary>
        /// <param name="listControlObj"></param>
        /// <param name="gridObj"></param>
        private void SetDataGridColumns(List<ControlDetailForPage> listControlObj, ControlDetailForPage gridObj)
        {
            mydatagridview.Columns.Clear();
            //获取所有的列
            List<int> listChildId = JsonController.DeSerializeToClass<List<int>>(gridObj.d17);
            List<ControlDetailForPage> listAllColumns = listControlObj.Where(p => listChildId.Contains(p.ctrl_id) 
            && xinLongyuControlType.GridColumnName.Equals(p.ctrl_type)).ToList();
            //获取键值对应字典
            Dictionary<int, string> ctrlIdToColumnName = this.GetDicCtrlToColumn(gridObj.d22);
            //生成列类型
            foreach (ControlDetailForPage ctObj in listAllColumns)
            {
                DataGridColumn column = GetDataGridColumn(ctObj, ctrlIdToColumnName, listControlObj);
                column.Header = ctObj.d0;
                mydatagridview.Columns.Add(column);
            }
            //设置行高
            mydatagridview.RowHeight = CommonConverter.StringToInt(gridObj.d19);

        }

        /// <summary>
        /// 获取对应的列
        /// </summary>
        /// <returns></returns>
        private DataGridColumn GetDataGridColumn(ControlDetailForPage ctObj, Dictionary<int, string> ctrlIdToColumnName, List<ControlDetailForPage> listControlObj)
        {
            List<int> listChild = JsonController.DeSerializeToClass<List<int>>(ctObj.d17);
            List<ControlDetailForPage> listAllChild = listControlObj.Where(p => listChild.Contains(p.ctrl_id)).ToList();
            DataGridTemplateColumn tcolumn = new DataGridTemplateColumn();
            DataTemplate template = new DataTemplate();
            template.DataType = typeof(ObjectHelperForDataGrid);

            if (listAllChild.Count > 1)
            {
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Grid));
                factory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Stretch);
                factory.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                foreach (ControlDetailForPage obj in listAllChild)
                {
                    if (ctrlIdToColumnName.ContainsKey(obj.ctrl_id))
                    {
                        factory.AppendChild(GetFrameworkElement(_propertyNameToColumnName[ctrlIdToColumnName[obj.ctrl_id]], obj));
                    }
                    else
                    {
                        factory.AppendChild(GetFrameworkElement(string.Empty, obj));
                    }
                }
                template.VisualTree = factory;
            }
            else
            {
                string propertyName = ctrlIdToColumnName.ContainsKey(listAllChild[0].ctrl_id)? _propertyNameToColumnName[ctrlIdToColumnName[listAllChild[0].ctrl_id]] : string.Empty;
                FrameworkElementFactory factory = GetFrameworkElement(propertyName, listAllChild[0]);
                template.VisualTree = factory;
                //if (xinLongyuControlType.textType.Equals(ctObj.ctrl_type))
                //{
                //    DataGridTextColumn dtcolumn = new DataGridTextColumn();
                //    dtcolumn.Binding = new Binding(_propertyNameToColumnName[ctrlIdToColumnName[listAllChild[0].ctrl_id]]);
                //    dtcolumn.Header = ctObj.d0;
                //    return dtcolumn;
                //}
                //else if (xinLongyuControlType.buttonType.Equals(ctObj.ctrl_type))
                //{
                //    FrameworkElementFactory factory = GetFrameworkElement(_propertyNameToColumnName[ctrlIdToColumnName[listAllChild[0].ctrl_id]], ctObj);
                //    factory.SetValue(Button.ContentProperty, ctObj.d0);
                //    template.VisualTree = factory;
                //}
                //else if (xinLongyuControlType.imgType.Equals(ctObj.ctrl_type))
                //{
                //    FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Image));
                //    factory.SetBinding(Image.SourceProperty, new Binding(_propertyNameToColumnName[ctrlIdToColumnName[listAllChild[0].ctrl_id]]) { Converter = new StringToBitmapImageConverter() });
                //    template.VisualTree = factory;
                //}
                ////默认使用textblock
                //else
                //{
                //    DataGridTextColumn dtcolumn = new DataGridTextColumn();
                //    dtcolumn.Binding = new Binding(_propertyNameToColumnName[ctrlIdToColumnName[listAllChild[0].ctrl_id]]);
                //    dtcolumn.Header = ctObj.d0;
                //    return dtcolumn;
                //}
            }
            tcolumn.CellTemplate = template;
            return tcolumn;
        }

        /// <summary>
        /// 获取对应控件的模板
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private FrameworkElementFactory GetFrameworkElement(string propertyName, ControlDetailForPage ctObj)
        {
            FrameworkElementFactory fac;
            if (xinLongyuControlType.textType.Equals(ctObj.ctrl_type))
            {
                fac = new FrameworkElementFactory(typeof(TextBlock));
                fac.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
                fac.SetBinding(TextBlock.TextProperty, new Binding(propertyName));
            }
            else if (xinLongyuControlType.buttonType.Equals(ctObj.ctrl_type))
            {
                fac = new FrameworkElementFactory(typeof(Button));
                fac.SetValue(Button.ContentProperty, ctObj.d0);
                //之后还要设置一下事件
                //fac.AddHandler(Button.ClickEvent, new EventHandler(fac, SomeHandler));
            }
            else if (xinLongyuControlType.imgType.Equals(ctObj.ctrl_type))
            {
                fac = new FrameworkElementFactory(typeof(Image));
                fac.SetBinding(Image.SourceProperty, new Binding(propertyName) { Converter = new StringToBitmapImageConverter(), UpdateSourceTrigger= UpdateSourceTrigger.Explicit });
            }
            //默认使用textblock
            else
            {
                fac = new FrameworkElementFactory(typeof(TextBlock));
                fac.SetBinding(TextBlock.TextProperty, new Binding(propertyName));
            }
            fac.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);
            fac.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
            fac.SetValue(MarginProperty, new Thickness(ctObj.d3, ctObj.d4, 0, 0));
            fac.SetValue(WidthProperty, (double)ctObj.d1);
            fac.SetValue(HeightProperty, (double)ctObj.d2);
            return fac;
        }

        public void SomeHandler(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((sender as Button).Name);
        }

        /// <summary>
        /// 获取列与值对应
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        private Dictionary<int, string> GetDicCtrlToColumn(string inText)
        {
            Dictionary<int, string> dicresult = new Dictionary<int, string>();
            //顺便获取列名与属性名对应的字典
            _propertyNameToColumnName.Clear();
            string[] array = inText.Split('&');
            int propertyIndex = 0;
            //var listProperty = typeof(ObjectHelperForDataGrid).GetProperties().Select(p => p.Name).ToList();
            foreach (string str in array)
            {
                string[] value = str.Split('=');
                int tagId = CommonConverter.StringToInt(value[0].Split('.')[0]);
                if (!dicresult.ContainsKey(tagId))
                {
                    dicresult.Add(tagId, value[1]);
                    if (!_propertyNameToColumnName.ContainsKey(value[1]))
                    {
                        _propertyNameToColumnName.Add(value[1], "Value" + propertyIndex.ToString());
                        propertyIndex++;
                    }
                }

            }
            return dicresult;
        }

       

    }
}
