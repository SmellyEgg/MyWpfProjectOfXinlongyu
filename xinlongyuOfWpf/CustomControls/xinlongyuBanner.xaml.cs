using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// flipview滚动图片控件
    /// </summary>
    public partial class xinlongyuBanner : UserControl, IControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public xinlongyuBanner()
        {
            InitializeComponent();
            listItems = new List<FlipViewItem>();
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private List<FlipViewItem> listItems;

        /// <summary>
        /// 设置主值
        /// </summary>
        /// <param name="value"></param>
        public void SetD0(object value)
        {
            listItems.Clear();
            if (object.Equals(value, null) || string.IsNullOrEmpty(value.ToString())) return;
            ControlDetailForPage currentObj = this.Tag as ControlDetailForPage;
            //获取页面ID数组,默认全都要设置所以这里不进行判断
            List<int> listPageId = JsonController.DeSerializeToClass<List<int>>(currentObj.d11);
            //判断是数组还是sql
            if (System.Text.RegularExpressions.Regex.IsMatch(value.ToString().Replace("\r\n", string.Empty), @".*\[.*\].*"))
            {
                List<string> listImage = JsonController.DeSerializeToClass<List<string>>(value.ToString());
                for (int i = 0; i < listPageId.Count; i ++)
                {
                    listItems.Add(new FlipViewItem() { ImageUrl = listImage[i], pageId = listPageId[i]});
                }
            }
            else
            {
                //sql处理
                SqlController cn = new SqlController();
                value = EventAssitant.FormatSql(value.ToString(), this);
                var returnDic = cn.ExcuteSqlWithReturn(value.ToString().Trim());
                var result = returnDic.data;
                if (!object.Equals(result, null) && result.Length > 0)
                {
                    int index = 0;
                    foreach (Dictionary<string, string> dic in result)
                    {
                        listItems.Add(new FlipViewItem() { ImageUrl = dic[currentObj.d19], pageId = listPageId[index]});
                        index++;
                    }
                }
            }
            MyFlipView.ItemsSource = null;
            MyFlipView.ItemsSource = listItems;
        }

        /// <summary>
        /// 图片滚动类
        /// </summary>
        internal class FlipViewItem
        {
            /// <summary>
            /// 图片
            /// </summary>
            public string ImageUrl { get; set; }
            /// <summary>
            /// 页面ID
            /// </summary>
            public int pageId { get; set; }

        }

        /// <summary>
        /// 图片的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MyFlipView.SelectedItem as FlipViewItem;
            string eventText = string.Format("[\"0.a5({0})\"]", selectedItem.pageId);
            EventAssitant.CallEventDerectly(eventText, this);
        }
    }
}
