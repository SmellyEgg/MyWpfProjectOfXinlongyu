using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 选择框控件
    /// </summary>
    public class xinlongyuCheckbox : CheckBox, IControl
    {
        /// <summary>
        /// 设置主值
        /// </summary>
        /// <param name="text"></param>
        public void SetD0(string text)
        {
            this.Content = text;
        }
    }
}
