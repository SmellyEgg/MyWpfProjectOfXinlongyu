using System.Collections.Generic;
using System.Linq;
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

        public void LoadData(List<ControlDetailForPage> listControlObj, ControlDetailForPage gridObj)
        {
            SetDataGridColumns(listControlObj, gridObj);
            
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        private void SetDataSource(ControlDetailForPage obj)
        {
            string sql = obj.d0;

            mydatagridview.ItemsSource = listsource;
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
                DataGridColumn column = GetDataGridColumn(ctObj, ctrlIdToColumnName[ctObj.ctrl_id], listControlObj);
                mydatagridview.Columns.Add(column);
            }

        }

        /// <summary>
        /// 获取对应的列
        /// </summary>
        /// <returns></returns>
        private DataGridColumn GetDataGridColumn(ControlDetailForPage ctObj, string ColumnName, List<ControlDetailForPage> listControlObj)
        {
            List<int> listChild = JsonController.DeSerializeToClass<List<int>>(ctObj.d17);
            List<ControlDetailForPage> listAllChild = listControlObj.Where(p => listChild.Contains(p.ctrl_id)).ToList();
            DataGridTemplateColumn tcolumn = new DataGridTemplateColumn();
            DataTemplate template = new DataTemplate();
            template.DataType = typeof(ObjectHelperForDataGrid);
            tcolumn.Header = ctObj.d0;

            if (listAllChild.Count > 1)
            {
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Grid));
                foreach(ControlDetailForPage obj in listAllChild)
                {
                    factory.AppendChild(GetFrameworkElement(obj.ctrl_type, _propertyNameToColumnName[ColumnName]));
                }
                template.VisualTree = factory;
            }
            else
            {
                FrameworkElementFactory factory = GetFrameworkElement(listAllChild[0].ctrl_type, _propertyNameToColumnName[ColumnName]);
                template.VisualTree = factory;
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
        private FrameworkElementFactory GetFrameworkElement(string ctrlType, string propertyName)
        {
            FrameworkElementFactory fac;
            if (xinLongyuControlType.textType.Equals(ctrlType))
            {
                fac = new FrameworkElementFactory(typeof(TextBlock));
                fac.SetBinding(TextBlock.TextProperty, new Binding(propertyName));
            }
            else if (xinLongyuControlType.buttonType.Equals(ctrlType))
            {
                fac = new FrameworkElementFactory(typeof(TextBlock));
                //之后还要设置一下事件
            }
            else if (xinLongyuControlType.imgType.Equals(ctrlType))
            {
                fac = new FrameworkElementFactory(typeof(Image));
                fac.SetBinding(Image.SourceProperty, new Binding(propertyName) { Converter = new StringToBitmapImageConverter() });
            }
            //默认使用textblock
            else
            {
                fac = new FrameworkElementFactory(typeof(TextBlock));
                fac.SetBinding(TextBlock.TextProperty, new Binding(propertyName));
            }
            return fac;
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
                    _propertyNameToColumnName.Add(value[1], "Value" + propertyIndex.ToString());
                    propertyIndex++;
                }

            }
            return dicresult;
        }

       

    }
}
