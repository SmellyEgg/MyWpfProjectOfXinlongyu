using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 自定义按钮
    /// </summary>
    public class xinlongyuButton : Button, IControl
    {
        public xinlongyuButton()
        {
            this.SetResourceReference(Control.StyleProperty, "ButtonWithRoundedCorner");
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
                        this.Content = (value as Dictionary<string, string>[])[0][key];
                        return;
                    }
                }
            }
            else if (value is string)
            {
                this.Content = value.ToString();
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
                this.FontStyle = font.Style == System.Drawing.FontStyle.Italic ? FontStyles.Italic : FontStyles.Normal;
                this.FontFamily = CommonConverter.FontToMediaFontFamily(font);
                this.FontSize = font.Size;
                this.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Normal;
            }
            
        }

        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <param name="value"></param>
        public void SetD7(string value)
        {
            this.Foreground = new SolidColorBrush(CommonConverter.ConvertStringToColor(value));
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="value"></param>
        public void SetD8(string value)
        {
            this.Background = new SolidColorBrush(CommonConverter.ConvertStringToColor(value));
        }

        /// <summary>
        /// 启动按钮
        /// 这里相当于按下按钮
        /// </summary>
        /// <param name="text"></param>
        public void SetA1(string text)
        {
            this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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
                    this.HorizontalContentAlignment = HorizontalAlignment.Left;
                    break;
                case "1":
                    this.HorizontalContentAlignment = HorizontalAlignment.Center;
                    break;
                case "2":
                    this.HorizontalContentAlignment = HorizontalAlignment.Right;
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
                    this.VerticalContentAlignment = VerticalAlignment.Top;
                    break;
                case "1":
                    this.VerticalContentAlignment = VerticalAlignment.Center;
                    break;
                case "2":
                    this.VerticalContentAlignment = VerticalAlignment.Bottom;
                    break;
                default:
                    break;
            }
        }
    }
}
