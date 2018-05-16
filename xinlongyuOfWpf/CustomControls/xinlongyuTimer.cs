using System;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 计时器控件
    /// </summary>
    public class xinlongyuTimer : Button, IControl
    {
        private int _countedTime = 0;

        System.Windows.Threading.DispatcherTimer _timer;

        public xinlongyuTimer()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Tick += _timer_Tick;
        }

        /// <summary>
        /// 触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_countedTime >= this._totalTime)
            {
                _timer.Stop();
                //DecoderAssistant.CallEventDerectly((this.Tag as ControlDetailForPage).p9, this);
                return;
            }

            try
            {
                //DecoderAssistant.CallEventDerectly((this.Tag as ControlDetailForPage).p3, this);
                _countedTime += (int)_timer.Interval.TotalSeconds;
            }
            catch (Exception ex)
            {
                Logging.Error("执行timer的事件出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 启动控件
        /// </summary>
        /// <param name="text"></param>
        public void SetA1(string text)
        {
            _countedTime = 0;
            _timer.Start();
        }

        /// <summary>
        /// 获取时长
        /// </summary>
        public object GetD0(object value)
        {
            return this._countedTime.ToString();
        }

        /// <summary>
        /// 设置时间间隔
        /// </summary>
        /// <param name="text"></param>
        public void SetD11(string text)
        {
            int interval = CommonConverter.StringToInt(text);
            if (interval != -1)
            {
                _timer.Interval = new TimeSpan(interval * 1000);
            }
        }

        private int _totalTime = 0;

        /// <summary>
        /// 设置总时长
        /// </summary>
        /// <param name="text"></param>
        public void SetD10(string text)
        {
            int value = CommonConverter.StringToInt(text);
            if (value != -1)
            {
                this._totalTime = value;
            }
        }
    }
}
