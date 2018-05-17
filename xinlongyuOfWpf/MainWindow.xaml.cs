using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Controller.PageController;

namespace xinlongyuOfWpf
{
    /// <summary>
    /// 原始主窗体界面
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 页面工厂类
        /// </summary>
        private PageFactory _pageFactory;

        /// <summary>
        /// 打开的页面列表
        /// </summary>
        private List<Page> _listPageHistory;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _pageFactory = new PageFactory();
            _listPageHistory = new List<Page>();
            LoadPage();
        }

        /// <summary>
        /// 获取需要加载的第一个页面ID
        /// </summary>
        /// <returns></returns>
        private int GetFirstPageID()
        {
            try
            {
                string id = xmlController.GetNodeByXpath(ConfigManagerSection.firstPageID, ConfigManagerSection.CommonconfigFilePath);
                return int.Parse(id);
            }
            catch
            {
                //默认导航到2073
                return ConfigManagerSection.defaultPageId;
            }
        }

        /// <summary>
        /// 导航控件主窗体
        /// </summary>
        Window _winMain;

        /// <summary>
        /// 加载第一个页面
        /// </summary>
        private async void LoadPage()
        {
            _winMain = new Window();
            _winMain.Tag = _listPageHistory;
            _winMain.ResizeMode = ResizeMode.CanResize;
            //设置图标
            var uri = new Uri("pack://application:,,,/Resources/MyLogo.jpg");
            BitmapImage bitmapImage = new BitmapImage(uri);
            _winMain.Icon = bitmapImage;
            _winMain.Title = "城市服务";
            _winMain.Closed -= Win_Closed;
            _winMain.Closed += Win_Closed;
            _winMain.SizeChanged -= Window_SizeChanged;
            _winMain.SizeChanged += Window_SizeChanged;
            //_winMain.SizeChanged += _winMain_SizeChanged;
            //加载默认的第一个页面
            int pageId = GetFirstPageID();
            //
            pageId = 1001;
            //
            CommonFunction.ShowWaitingForm(_winMain);
            await _pageFactory.ShowPage(_winMain, pageId, _listPageHistory);
        }

        /// <summary>
        /// 尺寸改变事件
        /// 居中控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            (sender as Window).WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            (sender as Window).Left = (SystemParameters.WorkArea.Width - (sender as Window).Width) / 2 + SystemParameters.WorkArea.Left;
            (sender as Window).Top = (SystemParameters.WorkArea.Height - (sender as Window).Height) / 2 + SystemParameters.WorkArea.Top;
        }

        /// <summary>
        /// 关联关闭主窗体以退出整个程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Win_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 释放资源，不知道有没有用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            //_winMain = null;
        }
    }
}
