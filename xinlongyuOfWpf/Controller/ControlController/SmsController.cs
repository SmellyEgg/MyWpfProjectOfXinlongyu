
using System.Threading.Tasks;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Models.Request;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.Controller.ControlController
{
    /// <summary>
    /// 短信逻辑层
    /// </summary>
    public class SmsController : BaseConnection
    {
        /// <summary>
        /// 处理短信验证码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="phonenumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> DealWithSMS(string type, string phonenumber, string code = "", bool isVerify = false)
        {
            string apitype = JsonApiType.sendCode;
            BaseRequest bj = GetCommonBaseRequest(apitype);
            bj.api_type = apitype;

            if (isVerify)
            {
                SmsRequest sr = new SmsRequest() { type = type, to = phonenumber, code = code };
                bj.data = sr;
            }
            else
            {
                SmsRequestSend sr = new SmsRequestSend() { type = type, to = phonenumber };
                bj.data = sr;
            }

            try
            {
                string result = await Post(bj);
                BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result);
                CommonReturn cr = JsonController.DeSerializeToClass<CommonReturn>(brj.data.ToString());
                if (cr.error_code.Equals(ReturnConst.right))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
