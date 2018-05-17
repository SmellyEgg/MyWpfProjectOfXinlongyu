
using System.Windows;

namespace xinlongyuOfWpf.Controller.CommonController
{
    public class CommonFunction
    {
        /// <summary>
        /// 显示等待窗体
        /// </summary>
        public static void ShowWaitingForm(Window window)
        {
            Xceed.Wpf.Toolkit.BusyIndicator btControl = new Xceed.Wpf.Toolkit.BusyIndicator();
            btControl.IsBusy = true;
            btControl.BusyContent = "正在加载中";
            window.Content = btControl;
            window.Show();
        }
    }
}
