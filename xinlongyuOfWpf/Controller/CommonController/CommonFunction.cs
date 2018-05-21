
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using xinlongyuOfWpf.CustomControls;

namespace xinlongyuOfWpf.Controller.CommonController
{
    public class CommonFunction
    {
        /// <summary>
        /// 显示等待窗体
        /// </summary>
        public static void ShowWaitingForm(Window window, bool isDialog = false)
        {
            Xceed.Wpf.Toolkit.BusyIndicator btControl = new Xceed.Wpf.Toolkit.BusyIndicator();
            btControl.IsBusy = true;
            btControl.BusyContent = "正在加载中";
            window.Content = btControl;
            if (isDialog) window.ShowDialog();
            else window.Show();
        }

        /// <summary>
        /// 获取当前页面
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static xinlongyuForm GetPageByControl(DependencyObject obj)
        {
            try
            {
                var parent = VisualTreeHelper.GetParent(obj);
                bool isFinded = false;
                while (!(parent is xinlongyuForm) && !(parent is Page))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                    isFinded = true;
                }
                if (!isFinded) return null;
                var page = parent as xinlongyuForm;
                if (object.Equals(page._pageCache, null)) page._pageCache = new System.Collections.Generic.Dictionary<string, string>();
                return page;
            }
            catch (System.Exception ex)
            {
                var window = Window.GetWindow(obj);
                var page = window.Content as xinlongyuForm;
                if (object.Equals(page._pageCache, null)) page._pageCache = new System.Collections.Generic.Dictionary<string, string>();
                return page;
            }
        }
    }
}
