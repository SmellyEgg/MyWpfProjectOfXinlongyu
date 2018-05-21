using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls;
using xinlongyuOfWpf.Models.ControlInfo;

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
        /// 控件解析类
        /// 放到这里是因为可视化树的特性
        /// </summary>
        private ControlDecoder _controlDecode;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageFactory()
        {
            _pageConnection = new PageConnection();
            _pageDecoder = new PageDecoder();
            _controlDecode = new ControlDecoder();
        }

        /// <summary>
        /// 生成页面
        /// </summary>
        /// <param name="pageId">页面ID</param>
        /// <returns></returns>
        public async Task<xinlongyuForm> ProducePage(int pageId, bool IsNavigationWindow = false)
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
        public Task<xinlongyuForm> GetDefaultPage()
        {
            Grid grid = new Grid();
            TextBlock txt = new TextBlock() { Text = "获取页面失败！"};
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Center;
            txt.FontSize = 30;
            txt.FontWeight = FontWeights.Bold;
            grid.Children.Add(txt);
            xinlongyuForm page = new xinlongyuForm(2073);
            page.Content = grid;
            return Task.Run(() => page);
        }

        
        /// <summary>
        /// 显示页面
        /// </summary>
        /// <param name="window"></param>
        /// <param name="pageId"></param>
        /// <param name="listPage"></param>
        /// <param name="parameter">传入的参数字典</param>
        /// <returns></returns>
        public async Task ShowPage(ContentControl window, int pageId, List<Page> listPage, Dictionary<string, string> parameter = null)
        {
            //window.Content = null;
            var page = await ProducePage(pageId);
            if (object.Equals(page, null))
            {
                page = await GetDefaultPage();
            }
            window.Content = null;
            if (!object.Equals(parameter, null))
            {
                page.SetParameters(parameter);
            }
            window.Content = page;
            //listPage.Add(page);
            window.Width = page.Width;
            window.Height = page.Height + ConfigManagerSection.TitleBarHeight;

            //设置页面控件基本属性
            foreach (IControl control in page._currentControlList)
            {
                ControlDetailForPage controlObj = (control as FrameworkElement).Tag as ControlDetailForPage;
                _controlDecode.SetControlProperty(control, controlObj);
            }

            //设置控件基本事件
            foreach (IControl control in page._currentControlList)
            {
                _controlDecode.SetControlEvent(control, (control as FrameworkElement).Tag as ControlDetailForPage);
            }

            //页面初始化事件
            IControl mypageControl = page._currentControlList.First(p => xinLongyuControlType.pageType.Equals(((p as FrameworkElement).Tag as Models.ControlInfo.ControlDetailForPage).ctrl_type));
            var pageObj = page._currentControlObjList.First(p => xinLongyuControlType.pageType.Equals(p.ctrl_type));
            mypageControl.SetP7(pageObj.p7);
            //

            if (window.GetType() == typeof(Window))
            {
                listPage.Add(page);
                (window as Window).Title = page.Title;
                (window as Window).Show();
            }
        }
    }
}
