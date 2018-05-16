using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

        /// <summary>
        /// 当前控件实体
        /// </summary>
        private ControlDetailForPage _CurrentCtObj;

        /// <summary>
        /// 当前过滤列的状态
        /// </summary>
        private List<BoolStringClass> _currentFilterState;

        /// <summary>
        /// 总共的数据条数
        /// </summary>
        private int _rowCount = 0;

        public MyDataGridControl()
        {
            InitializeComponent();
            //添加提示文字
            txtBoxOfFilterStr.TipText = "请输入过滤的关键字";
            txtJumpPage.TipText = "页面序号";
            //
            _propertyNameToColumnName = new Dictionary<string, string>();
            _currentFilterState = new List<BoolStringClass>();
            _sqlController = new SqlController();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="listControlObj"></param>
        /// <param name="gridObj"></param>
        public void LoadData(List<ControlDetailForPage> listControlObj, ControlDetailForPage gridObj)
        {
            _CurrentCtObj = gridObj;
            SetDataGridColumns(listControlObj, _CurrentCtObj);
            DealWithPageIndex();
            PageTurning();
            //SetDataSource(gridObj.d0);
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        private void SetDataSource(string sql)
        {
            //var sql = obj.d0;
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
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Grid));
                FrameworkElementFactory factorychild = GetFrameworkElement(propertyName, listAllChild[0]);
                factorychild.SetValue(VerticalAlignmentProperty, VerticalAlignment.Stretch);
                factorychild.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                factory.AppendChild(factorychild);
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
                fac.SetValue(FrameworkElement.StyleProperty, (Style)FindResource("ButtonWithRoundedCorner"));
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
            if (!string.IsNullOrEmpty(ctObj.d8)) fac.SetValue(BackgroundProperty, (System.Windows.Media.Brush)(new System.Windows.Media.BrushConverter().ConvertFromString(ctObj.d8)));
            if (!string.IsNullOrEmpty(ctObj.d7)) fac.SetValue(ForegroundProperty, (System.Windows.Media.Brush)(new System.Windows.Media.BrushConverter().ConvertFromString(ctObj.d7)));


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

        /// <summary>
        /// 当前页面序号
        /// </summary>
        private int _currentPageIndex = 1;

        /// <summary>
        /// 总页面数量
        /// </summary>
        private int _totalPage = 0;

        /// <summary>
        /// 每一页显示的数目
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// 翻页行为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPageTurning(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Content)
            {
                case "首页":
                    _currentPageIndex = 1;
                    break;
                case "尾页":
                    _currentPageIndex = _totalPage;
                    break;
                case "下一页":
                    if (_currentPageIndex < _totalPage)
                    {
                        _currentPageIndex++;
                    }
                    else
                    {
                        MessageBox.Show("当前已经是最后一页！");
                        return;
                    }
                    break;
                case "前一页":
                    if (_currentPageIndex > 1)
                    {
                        _currentPageIndex--;
                    }
                    else
                    {
                        MessageBox.Show("当前已经是第一页了！");
                        return;
                    }
                    break;
                default:
                    break;
            }
            lblCurrentPageIndex.Text = _currentPageIndex.ToString() + " / " + _totalPage.ToString();//当前页面设置
            PageTurning();
        }

        /// <summary>
        /// 翻页行为
        /// </summary>
        private void PageTurning()
        {
            string sql = _CurrentCtObj.d0;
            if (sql.ToLower().Contains("limit"))
            {
                //如果本身以及进行了limit限制那就不进行处理
            }
            else
            {
                sql = this.formatExcuteSql(sql);
                if (_currentPageIndex < 1) _currentPageIndex++;
                string limit = string.Format(" LIMIT {0}, {1}", (_currentPageIndex - 1) * _pageSize, _pageSize);
                sql += limit;
            }
            SetDataSource(sql);
        }

        /// <summary>
        /// 格式化sql语句以实现翻页功能
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string formatExcuteSql(string sql)
        {
            string newSql = sql.Clone().ToString();
            sql = sql.ToLower();
            if (!object.Equals(_currentFilterState, null) && _currentFilterState.Count > 0)
            {
                bool isNeedAnd = false;

                int insertLocation = 0;
                if (sql.ToLower().IndexOf("where") != -1)
                {
                    insertLocation = sql.IndexOf("where") + 5;
                    isNeedAnd = true;
                }
                else
                {
                    if (sql.ToLower().Contains("order by"))
                    {
                        insertLocation = sql.IndexOf("order by");
                    }
                    else if (sql.ToLower().Contains("group by"))
                    {
                        insertLocation = sql.IndexOf("group by");
                    }
                    else if (sql.ToLower().Contains("limit"))
                    {
                        insertLocation = sql.IndexOf("limit");
                    }
                    else
                    {
                        insertLocation = sql.Length;
                    }
                }
                string filter = AppenConditionForSql(sql);
                if (isNeedAnd)
                {
                    filter = " (" + filter + ")" + " and ";
                }
                else
                {
                    filter = " where " + filter;
                }
                newSql = newSql.Insert(insertLocation, filter);
                return newSql;
            }
            else
            {
                return newSql;
            }
        }

        /// <summary>
        /// 为sql拼接条件语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string AppenConditionForSql(string sql)
        {
            StringBuilder sb = new StringBuilder(500);
            string filterText = this.txtBoxOfFilterStr.Text.Trim();
            int index = 0;
            foreach (string str in _currentFilterState.Where(p => p.IsSelected).Select(p => p.TheText))
            {
                if (index == 0)
                {
                    sb.Append(" CAST(" + str + " AS char) " + " like '%" + filterText + "%' ");
                    index++;
                }
                else
                {
                    sb.Append(" or " + " CAST(" + str + " AS char) " + " like '%" + filterText + "%' ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 页面跳转事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJumpPage(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtJumpPage.Text))
            {
                return;
            }
            int pageIndex = CommonConverter.StringToInt(this.txtJumpPage.Text);
            if (pageIndex <= _totalPage && pageIndex >= 1)
            {
                _currentPageIndex = pageIndex;
                PageTurning();
            }
            else
            {
                MessageBox.Show("输入的页面不存在！");
            }
        }

        /// <summary>
        /// 过滤按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFilterClick(object sender, RoutedEventArgs e)
        {
            FrmColumnSelector frm = new FrmColumnSelector();
            frm.TheList = GetFilterDic();
            if (frm.ShowDialog() == true)
            {
                _currentFilterState.Clear();
                frm.TheList.ToList().ForEach(p => _currentFilterState.Add(p));
            }
        }

        
        /// <summary>
        /// 获取过滤列状态的观察者数组
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<BoolStringClass> GetFilterDic()
        {
            ObservableCollection<BoolStringClass> oc = new ObservableCollection<BoolStringClass>();
            if (object.Equals(_currentFilterState, null) || _currentFilterState.Count < 1)
            {
                //这里因为是初始状态
                var listcolumnName = _propertyNameToColumnName.Keys.ToList();
                listcolumnName.ForEach(p => _currentFilterState.Add(new BoolStringClass() { IsSelected = false, TheText = p }));
            }
            _currentFilterState.ForEach(p => oc.Add(p));
            return oc;
        }

        /// <summary>
        /// 设置底部显示的一些相关信息
        /// </summary>
        private void DealWithPageIndex()
        {
            _rowCount = GetTotalRowCount();
            _pageSize = CommonConverter.StringToInt(_CurrentCtObj.d15);
            if (_pageSize != 0 && _pageSize != -1) _totalPage = _rowCount / _pageSize;

            lblCurrentPageIndex.Text = _currentPageIndex.ToString() + " / " + _totalPage.ToString();//当前页面设置
            lblTotalPageCount.Text = "共" + _rowCount.ToString() + " 条";//总数据条数设置
            
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <returns></returns>
        private int GetTotalRowCount()
        {
            string _rowCountSql = _CurrentCtObj.d14;
            if (!string.IsNullOrEmpty(_rowCountSql))
            {
                var result = _sqlController.ExcuteSqlWithReturn(_rowCountSql);
                if (!object.Equals(result.data, null) && result.data.Length > 0)
                {
                    int totalCount = CommonConverter.StringToInt(result.data[0].First().Value);
                    return totalCount;
                }
            }
            return 0;
        }

        /// <summary>
        /// 开始搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBeginFilter(object sender, RoutedEventArgs e)
        {
            //这里之后要考虑之后要不要重新计算总条数
            _currentPageIndex = 1;
            lblCurrentPageIndex.Text = _currentPageIndex.ToString() + " / " + _totalPage.ToString();//当前页面设置
            PageTurning();
        }

        /// <summary>
        /// 搜索框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBoxOfFilterStr_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnBeginFilter(null, null);
            }
        }

        /// <summary>
        /// 导出datagrid到Excel文件
        /// </summary>
        public void ExportToExcel()
        {
            //int i = 0;
            //int k = 1, h = 1;
            //string strFullFilePathNoExt = @"C:\Users\TestExcel4.xls";
            //GetDataGridRows(dgDisplay);
            //var rows = GetDataGridRows(dgDisplay);
            //Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel._Workbook ExcelBook;
            //Microsoft.Office.Interop.Excel._Worksheet ExcelSheet;
            //ExcelBook = (Microsoft.Office.Interop.Excel._Workbook)ExcelApp.Workbooks.Add(1);
            //ExcelSheet = (Microsoft.Office.Interop.Excel._Worksheet)ExcelBook.ActiveSheet;
            //for (i = 1; i <= dgDisplay.Columns.Count; i++)
            //{
            //    ExcelSheet.Cells[1, i] = dgDisplay.Columns[i - 1].Header.ToString();
            //}
            //foreach (DataGridRow r in (DataGridRow)mydatagridview.ItemContainerGenerator.ContainerFromIndex)
            //{
            //    DataRowView rv = (DataRowView)r.Item;
            //    foreach (DataGridColumn column in dgDisplay.Columns)
            //    {
            //        if (column.GetCellContent(r) is TextBlock)
            //        {
            //            TextBlock cellContent = column.GetCellContent(r) as TextBlock;
            //            ExcelSheet.Cells[h + 1, k] = cellContent.Text.Trim();
            //            k++;
            //        }

            //    }
            //    k = 1;
            //    h++;
            //}
            //ExcelApp.Visible = false;
            //ExcelBook.SaveAs(strFullFilePathNoExt, Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
            //Excel.XlSaveConflictResolution.xlUserResolution, true,
            //Missing.Value, Missing.Value, Missing.Value);
            //ExcelBook.Close(strFullFilePathNoExt, Missing.Value, Missing.Value);
            //ExcelSheet = null;
            //ExcelBook = null;
            //ExcelApp = null;


        }

        /// <summary>
        /// 导出Excel按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExportExcel(object sender, RoutedEventArgs e)
        {
            //ExportToExcelAndCsv();
            MessageBox.Show("该功能暂未开发完成！");
        }

        /// <summary>
        /// 显示序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + (_currentPageIndex - 1) * _pageSize).ToString();
        }
    }
}
