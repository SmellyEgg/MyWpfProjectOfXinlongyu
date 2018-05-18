using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonPath;

namespace xinlongyuOfWpf.Controller.PageController
{
    /// <summary>
    /// 页面工厂类，负责生成页面
    /// </summary>
    public class PageFactory
    {
        /// <summary>
        /// 页面请求类
        /// </summary>
        private PageConnection _pageConnection;

        /// <summary>
        /// 页面解析类
        /// </summary>
        private PageDecoder _pageDecoder;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageFactory()
        {
            _pageConnection = new PageConnection();
            _pageDecoder = new PageDecoder();
        }

        /// <summary>
        /// 生成页面
        /// </summary>
        /// <param name="pageId">页面ID</param>
        /// <returns></returns>
        public async Task<Page> ProducePage(int pageId, bool IsNavigationWindow = false)
        {
            var pageInfo = await _pageConnection.GetPageInfo(pageId);
            if (object.Equals(pageInfo, null)) return null;
            var page = _pageDecoder.DecodePage(pageInfo, IsNavigationWindow);
            return page;
        }

        /// <summary>
        /// 获取默认页面，一般是获取页面失败的时候用
        /// </summary>
        /// <returns></returns>
        public Task<Page> GetDefaultPage()
        {
            Grid grid = new Grid();
            TextBlock txt = new TextBlock() { Text = "获取页面失败！"};
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            txt.FontSize = 30;
            txt.FontWeight = System.Windows.FontWeights.Bold;
            grid.Children.Add(txt);
            Page page = new Page();
            page.Content = grid;
            return Task.Run(() => page);
        }

        /// <summary>
        /// 显示页面
        /// </summary>
        /// <param name="window"></param>
        /// <param name="pageId"></param>
        public async Task ShowPage(ContentControl window, int pageId, List<Page> listPage)
        {
            //window.Content = null;
            var page = await ProducePage(pageId);
            if (object.Equals(page, null))
            {
                page = await GetDefaultPage();
            }
            window.Content = null;
            window.Content = page;
            //listPage.Add(page);
            window.Width = page.Width;
            window.Height = page.Height + ConfigManagerSection.TitleBarHeight;
           
            if (window.GetType() == typeof(Window))
            {
                listPage.Add(page);
                (window as Window).Show();
            }
            
        }

        
    }
}
