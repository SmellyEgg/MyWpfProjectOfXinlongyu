using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;

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
            MessageBox.Show(text, "提示");
        }


    }
}
