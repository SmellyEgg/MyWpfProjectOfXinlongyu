using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.Request;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 短信发送以及验证控件
    /// </summary>
    public class xinlongyuSms : Button, IControl
    {
        /// <summary>
        /// 当前电话号码
        /// </summary>
        private string _currentPhoneNumber = string.Empty;
        /// <summary>
        /// 当前验证码
        /// </summary>
        private string _currentCode = string.Empty;
        /// <summary>
        /// 当前验证类型
        /// </summary>
        private string _currentType = string.Empty;

        private SmsController _smsController;

        public xinlongyuSms()
        {
            //设置未不显示
            this.Visibility = System.Windows.Visibility.Collapsed;
            _smsController = new SmsController();
        }

        /// <summary>
        /// 启动发送短信
        /// </summary>
        public void SetA1(string text)
        {
            ControlDetailForPage ctobj = this.Tag as ControlDetailForPage;
            if (string.IsNullOrEmpty(_currentType))
            {
                string typecode = ctobj.d5;
                _currentType = typecode.Equals("1") ? smsType.phone.ToString() : smsType.email.ToString();
            }
            var result = _smsController.DealWithSMS(_currentType, _currentPhoneNumber, _currentCode);
            if (result.Result)
            {
                //发送成功后做的事情
            }
            else
            {
                //发送失败后做的事情
            }

            //if (_smsController.DealWithSMS(_currentType, _currentPhoneNumber, _currentCode))
            //{
            //    DecoderAssistant.CallEventDerectly(ctobj.p9, this);
            //}
            //else
            //{
            //    DecoderAssistant.CallEventDerectly(ctobj.p12, this);
            //}
        }

        /// <summary>
        /// 验证短信
        /// </summary>
        /// <param name="code"></param>
        public void SetA2(string text)
        {
            ControlDetailForPage ctobj = this.Tag as ControlDetailForPage;
            if (string.IsNullOrEmpty(_currentType))
            {
                string typecode = ctobj.d5;
                _currentType = typecode.Equals("1") ? smsType.phone.ToString() : smsType.email.ToString();
            }
            var result = _smsController.DealWithSMS(_currentType, _currentPhoneNumber, _currentCode, true);
            if (result.Result)
            {

            }
            else
            {

            }
            //if (cn.DealWithSMS(_currentType, _currentPhoneNumber, _currentCode, true))
            //{
            //    DecoderAssistant.CallEventDerectly(ctobj.p9, this);
            //}
            //else
            //{
            //    DecoderAssistant.CallEventDerectly(ctobj.p12, this);
            //}
            //cn = null;
        }

        /// <summary>
        /// 设置电话号码或者邮箱
        /// </summary>
        /// <param name="phone"></param>
        public void SetD5(string phone)
        {
            this._currentPhoneNumber = phone;
        }

        /// <summary>
        /// 设置验证码
        /// 验证码为空时为发送模式
        /// </summary>
        /// <param name="code"></param>
        public void SetD6(string code)
        {
            this._currentCode = code;
        }

        /// <summary>
        /// 设置验证类型
        /// 1代表手机号码 2代表邮箱
        /// </summary>
        /// <param name="type"></param>
        public void SetD7(string type)
        {
            _currentType = type.Equals("1") ? smsType.phone.ToString() : smsType.email.ToString();
        }
    }
}
