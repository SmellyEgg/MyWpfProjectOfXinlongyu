using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 提醒类
    /// </summary>
    public class xinlongyuToolTip : Button, IControl
    {
        //private string _btnOkText = string.Empty;

        //private string _btnCancelText = string.Empty;

        private string _text = string.Empty;

        public xinlongyuToolTip()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        /// <summary>
        /// 设置主值
        /// </summary>
        /// <param name="text"></param>
        public void SetD0(object text)
        {
            if (!object.Equals(text, null))
                this._text = text.ToString();
        }

        //public void SetD23(string text)
        //{
        //    this._btnOkText = text;
        //}

        //public void SetD24(string text)
        //{
        //    this._btnCancelText = text;
        //}
        
        /// <summary>
        /// 启动控件
        /// </summary>
        /// <param name="text"></param>
        public void SetA1(string text)
        {
            MessageBox.Show(_text, "提示");
            //notifier.ShowInformation(_text);
        }


    }
}
