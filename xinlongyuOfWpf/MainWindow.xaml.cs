using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using xinlongyuOfWpf.Controller;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;

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
        private _pageFactory _pageFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _pageFactory = new _pageFactory();
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
        NavigationWindow _winMain;


        /// <summary>
        /// 加载第一个页面
        /// </summary>
        private async void LoadPage()
        {
            
            _winMain = new NavigationWindow();
            //设置图标
            var uri = new Uri("pack://application:,,,/Resources/MyLogo.jpg");
            BitmapImage bitmapImage = new BitmapImage(uri);
            _winMain.Icon = bitmapImage;
            _winMain.Title = "城市服务";
            //加载默认的第一个页面
            int pageId = GetFirstPageID();
            //pageId = 2073;
            var page = await _pageFactory.ProducePage(pageId);
            if (object.Equals(page, null))
            {
                page = await _pageFactory.GetDefaultPage();

            }
            _winMain.Content = page;

            _winMain.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _winMain.Width = page.Width;
            _winMain.Height = page.Height;

            _winMain.Closed -= Win_Closed;
            _winMain.Closed += Win_Closed;

            _winMain.Show();
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
            _winMain = null;
        }
    }
}
