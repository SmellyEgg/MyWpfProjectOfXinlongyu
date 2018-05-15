
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 时间日期选择器
    /// </summary>
    public class xinlongyuDateTimePicker : DatePicker, IControl
    {
        public xinlongyuDateTimePicker()
        {
            
        }

        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <returns></returns>
        public object GetD0(object value)
        {
            return this.Text;
        }
    }
}
