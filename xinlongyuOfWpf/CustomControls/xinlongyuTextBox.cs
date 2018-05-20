using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls.Extension;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 文本输入框控件
    /// </summary>
    public class xinlongyuTextBox : textBoxWithPlaceHolderText, IControl
    {
        public xinlongyuTextBox()
        {
            //this.txtContent.BorderBrush = new SolidColorBrush(Colors.Red);
            #region 手动创建整个界面以及触发器
            //TextBox txtBox = new TextBox();
            //txtBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //txtBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //txtBox.Margin = new System.Windows.Thickness(5);

            //txtTips = new TextBlock();
            //txtTips.Text = "我是提示";
            //txtTips.IsHitTestVisible = false;
            //txtTips.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //txtTips.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //txtTips.Margin = new System.Windows.Thickness(10,0,0,0);
            //txtTips.Foreground = new System.Windows.Media.SolidColorBrush(Colors.DarkGray);

            //Style style = new Style { TargetType = typeof(TextBlock) };
            //Setter setter = new Setter();
            //setter.Property = FrameworkElement.VisibilityProperty;
            //setter.Value = Visibility.Collapsed;

            //DataTrigger datatrigger = new DataTrigger();
            //datatrigger.Binding = new Binding() { Path = new PropertyPath("Text"), ElementName = txtBox.Name};

            //Setter settertxt = new Setter();
            //settertxt.Property = FrameworkElement.VisibilityProperty;
            //settertxt.Value = Visibility.Visible;

            //datatrigger.Setters.Add(settertxt);
            //style.Triggers.Add(datatrigger);

            //txtTips.Style = style;

            //this.Children.Add(txtBox);
            //this.Children.Add(txtTips);
            //(this as TextBox).Style = style;
            #endregion

        }

        /// <summary>
        /// 设置提示语
        /// </summary>
        /// <param name="text"></param>
        public void SetD13(string text)
        {
            this.txtTips.Text = text;
        }

        /// <summary>
        /// 获取主值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(string value)
        {
            return txtContent.Text;
        }

    }
}
