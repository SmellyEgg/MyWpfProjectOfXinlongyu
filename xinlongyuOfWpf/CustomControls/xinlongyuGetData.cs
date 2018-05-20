using System;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.DecodeModel;

namespace xinlongyuOfWpf.CustomControls
{
    public class xinlongyuGetData : Button, IControl
    {
        public xinlongyuGetData()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void SetD0(object value)
        {
            if (!string.IsNullOrEmpty(value.ToString()))
            {
                ControlDetailForPage tag = this.Tag as ControlDetailForPage;
                tag.d0 = value.ToString();
                this.Tag = tag;
            }
        }

        public void SetA1(string text)
        {
            try
            {
                DecoderOfControl dct = new DecoderOfControl();
                //dct.CtrlId = int.Parse(this.Name);
                dct.RightDirectValue = (this.Tag as ControlDetailForPage).d0;
                //执行d0sql的数据获取
                if (EventAssitant.dealWithSqlRequest(dct, this))
                {
                    //触发后触发的事件
                    EventAssitant.CallEventDerectly((this.Tag as ControlDetailForPage).p9, this);
                }
                else
                {
                    //触发后触发的事件
                    EventAssitant.CallEventDerectly((this.Tag as ControlDetailForPage).p12, this);
                }
            }
            catch (Exception ex)
            {
                Logging.Error("数据控件出错:" + ex.Message);
            }

        }
    }
}
