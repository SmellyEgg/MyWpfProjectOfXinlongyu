using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using xinlongyuOfWpf.Controller;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.GroupInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// xinlongyuReviewControl.xaml 的交互逻辑
    /// </summary>
    public partial class xinlongyuReviewControl : UserControl, IControl
    {
        /// <summary>
        /// 页面连接层
        /// </summary>
        private PageConnection _pageConnection;

        public xinlongyuReviewControl()
        {
            InitializeComponent();
            //
            _pageConnection = new PageConnection();
            txtPageId.TipText = "请输入页面ID";
            //
            RefreshTreeView();
        }

        /// <summary>
        /// 刷新树列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefreshClick(object sender, RoutedEventArgs e)
        {
            RefreshTreeView();
            MessageBox.Show("刷新成功！");
        }

        /// <summary>
        /// 刷新树列表
        /// </summary>
        private async void RefreshTreeView()
        {
            var listGroupInfo = await _pageConnection.GetRecentUploadPages();
            MyTreeView.Items.Clear();
            foreach(PageGroupDetail pageGroup in listGroupInfo)
            {
                TreeViewItem groupItem = new TreeViewItem() { Header = pageGroup.group_name };
                //过滤掉id一样的页面
                List<pageDetailForGroup> listpage = new List<pageDetailForGroup>();
                listpage.AddRange(pageGroup.page_list);
                listpage = listpage.GroupBy(p => p.page_id)
                                   .Select(g => g.First()).ToList();
                foreach (pageDetailForGroup page in listpage)
                {
                    TreeViewItem pageItem = new TreeViewItem() { Header = page.page_name, Tag = page.page_id };
                    pageItem.MouseDoubleClick += PageItem_MouseDoubleClick;
                    groupItem.Items.Add(pageItem);
                }
                MyTreeView.Items.Add(groupItem);
            }
        }

        /// <summary>
        /// 双击事件,显示该页面的所有版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RefreshDataGrid((sender as TreeViewItem).Tag.ToString());
        }

        /// <summary>
        /// 刷新版本列表
        /// </summary>
        /// <param name="pageId"></param>
        private async void RefreshDataGrid(string pageId)
        {
            var listpageresult = await _pageConnection.GetPageGroupInfo(pageId);
            listpageresult = listpageresult.OrderBy(p => CommonConverter.StringToInt(p.version)).ToList();
            MyDataGrid.ItemsSource = listpageresult;
        }

        /// <summary>
        /// 审核页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnReviewClick(object sender, RoutedEventArgs e)
        {
            pageDetailForGroup tagItem = (sender as Button).Tag as pageDetailForGroup;
            bool result = await _pageConnection.ReviewPage(tagItem.page_id.ToString(), CommonConverter.StringToInt(tagItem.version));
            if (result)
            {
                RefreshDataGrid(tagItem.page_id.ToString());
                MessageBox.Show("审核成功!");
            }
            else
            {
                MessageBox.Show("审核失败!");
            }
        }

        /// <summary>
        /// 获取该页面所有版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGetPageVersionClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPageId.Text) && CommonConverter.StringToInt(txtPageId.Text) != -1)
            {
                RefreshDataGrid(txtPageId.Text);
            }
        }

        /// <summary>
        /// 页面ID输入框按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPageId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnGetPageVersionClick(null, null);
            }
        }
    }
}
