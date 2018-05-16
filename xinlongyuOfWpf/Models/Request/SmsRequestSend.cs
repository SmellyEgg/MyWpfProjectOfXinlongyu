
namespace xinlongyuOfWpf.Models.Request
{
    /// <summary>
    /// 短信发送
    /// </summary>
    public class SmsRequestSend
    {
        /// <summary>
        /// 类型，phone、email
        /// </summary>
        public string type = string.Empty;

        /// <summary>
        /// 手机号码或邮箱地址
        /// </summary>
        public string to = string.Empty;
    }
}
