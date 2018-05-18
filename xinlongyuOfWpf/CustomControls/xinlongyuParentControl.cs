using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.PageController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 父控件
    /// </summary>
    public class xinlongyuParentControl : Grid, IControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public xinlongyuParentControl()
        {
            _pageFactory = new PageFactory();
        }

        /// <summary>
        /// 当前页面ID
        /// </summary>
        private int _currentPageId = 0;

        /// <summary>
        /// 页面工厂类
        /// </summary>
        private PageFactory _pageFactory;

        /// <summary>
        /// 装载窗体
        /// 这里暂时唯一的用处是用来与菜单栏控件进行关联
        /// </summary>
        /// <param name="value"></param>
        public async void SetA1(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            // 这里加载新的窗口ID的页面
            int pageId = CommonConverter.StringToInt(value);
            if (pageId == -1)
            {
                return;
            }
            _currentPageId = pageId;

            //var window = new Window();
            Frame frame = new Frame();
            var page = await _pageFactory.ProducePage(_currentPageId, true);
            page.HorizontalAlignment = HorizontalAlignment.Stretch;
            page.VerticalAlignment = VerticalAlignment.Stretch;
            frame.Content = page;
            //frame.Content = window;
            frame.Width = page.Width;
            frame.Height = page.Height;
            frame.VerticalAlignment = VerticalAlignment.Top;
            frame.HorizontalAlignment = HorizontalAlignment.Left;
            //window.Content = page;
            this.Children.Clear();
            this.Children.Add(frame);
        }
    }
}
