
namespace xinlongyuOfWpf.Models.Request
{
    /// <summary>
    /// 短信验证
    /// </summary>
    public class SmsRequest
    {
        /// <summary>
        /// 类型，phone、email
        /// </summary>
        public string type = string.Empty;

        /// <summary>
        /// 手机号码或邮箱地址
        /// </summary>
        public string to = string.Empty;

        /// <summary>
        /// 验证码，不为空是校验模式，为空的话就是发送短信模式
        /// </summary>
        public string code = string.Empty;
    }

    /// <summary>
    /// 验证方式类型
    /// </summary>
    public enum smsType
    {
        phone,
        email
    }
}
