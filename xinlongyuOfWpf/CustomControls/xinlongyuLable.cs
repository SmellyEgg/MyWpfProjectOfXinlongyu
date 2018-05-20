using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
using xinlongyuOfWpf.CustomControls.Extension;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 标签控件
    /// </summary>
    public class xinlongyuLable : TextBlockWithAlignMent, IControl
    {
        public xinlongyuLable()
        {
            //this.TextAlignment = System.Windows.TextAlignment.Center;
            
        }

        private string _clickEvent = string.Empty;

        /// <summary>
        /// 设置按钮的单击事件
        /// </summary>
        /// <param name="value"></param>
        public void SetP0(string value)
        {
            //string test = value;
            if (!string.IsNullOrEmpty(value))
            {
                _clickEvent = value;
            }
            this.txtContent.PreviewMouseDown += TxtContent_PreviewMouseDown;
        }

        /// <summary>
        /// 标签的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtContent_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //throw new System.NotImplementedException();
            if (!string.IsNullOrEmpty(_clickEvent))
            {
                EventAssitant.CallEventDerectly(_clickEvent, this);
            }
        }

        /// <summary>
        /// 设置主值
        /// </summary>
        /// <param name="value"></param>
        public void SetD0(object value)
        {
            if (value is Dictionary<string, string>[])
            {
                if ((value as Dictionary<string, string>[]).Length > 0)
                {
                    //this.Text = (value as Dictionary<string, string>[])[0].
                    foreach (string key in (value as Dictionary<string, string>[])[0].Keys)
                    {
                        this.txtContent.Text = (value as Dictionary<string, string>[])[0][key];
                        return;
                    }
                }
            }
            else if (value is string)
            {
                this.txtContent.Text = value.ToString();
            }
        }


        /// <summary>
        /// 设置字体样式
        /// </summary>
        /// <param name="value"></param>
        public void SetD6(string value)
        {
            if (!string.IsNullOrEmpty(value.Trim()))
            {
                Font font = JsonController.DeSerializeToClass<Font>(value);
                this.FontStyle = font.Style == System.Drawing.FontStyle.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
                this.FontFamily = CommonConverter.FontToMediaFontFamily(font);
                this.FontSize = font.Size;
                this.FontWeight = font.Bold ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
            }

        }

        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <param name="value"></param>
        public void SetD7(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return;
            this.Foreground = new SolidColorBrush(CommonConverter.ConvertStringToColor(value));
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="value"></param>
        public void SetD8(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return;
            this.Background = new SolidColorBrush(CommonConverter.ConvertStringToColor(value));
        }

        /// <summary>
        /// 设置字体水平对齐方式
        /// </summary>
        /// <param name="text"></param>
        public void SetD9(string text)
        {
            switch (text)
            {
                case "0":
                    this.txtContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    break;
                case "1":
                    this.txtContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    break;
                case "2":
                    this.txtContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置字体垂直对齐方式
        /// </summary>
        /// <param name="text"></param>
        public void SetD25(string text)
        {
            switch (text)
            {
                case "0":
                    this.txtContent.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    break;
                case "1":
                    this.txtContent.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    break;
                case "2":
                    this.txtContent.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    break;
                default:
                    break;
            }
        }
    }
}
