using System.Windows;
using System.Windows.Controls;

namespace xinlongyuOfWpf.CustomControls.Extension
{
    /// <summary>
    /// MyChartDataControl.xaml 的交互逻辑
    /// </summary>
    public partial class MyChartDataControl : UserControl
    {
        public MyChartDataControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置表格类型
        /// </summary>
        /// <param name="type"></param>
        public void SetType(int type)
        {
            //设置显示类型
            if ((int)chartType.column == type)
            {
                this.columnChart.Visibility = Visibility.Visible;
            }
            else if ((int)chartType.pie == type)
            {
                this.pieChart.Visibility = Visibility.Visible;
            }
            else if ((int)chartType.Area == type)
            {
                this.areaChart.Visibility = Visibility.Visible;
            }
            else if ((int)chartType.Bar == type)
            {
                this.barChart.Visibility = Visibility.Visible;
            }
            else if ((int)chartType.Line == type)
            {
                this.lineChart.Visibility = Visibility.Visible;
            }

        }

        //public void SetD0(object value)
        //{
        //    //1、sql 2、字典实体
        //    if (value is string)
        //    {
        //        string sql = value.ToString();
        //        ConnectionManager cn = new ConnectionManager();
        //        cn.ExcuteSqlWithReturn(sql);
        //    }
        //}

        public enum chartType
        {
            column,//条形图
            pie,//扇形图
            Area,//区域图
            Bar,//柱状图
            Line//连线图
        }
    }
}
