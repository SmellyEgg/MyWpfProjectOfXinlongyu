using System.Threading.Tasks;
using System.Windows.Controls;

namespace xinlongyuOfWpf.Controller
{
    /// <summary>
    /// 页面工厂类，负责生成页面
    /// </summary>
    public class _pageFactory
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
        public _pageFactory()
        {
            _pageConnection = new PageConnection();
            _pageDecoder = new PageDecoder();
        }

        /// <summary>
        /// 生成页面
        /// </summary>
        /// <param name="pageId">页面ID</param>
        /// <returns></returns>
        public async Task<Page> ProducePage(int pageId)
        {
            var pageInfo = await _pageConnection.GetPageInfo(pageId);

            if (object.Equals(pageInfo, null)) return null;

            var page = _pageDecoder.DecodePage(pageInfo);

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


    }
}
