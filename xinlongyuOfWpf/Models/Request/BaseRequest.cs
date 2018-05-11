using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinlongyuOfWpf.Models.Request
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class BaseRequest
    {
        public CommonLoginRequest auth;
        public string api_type = string.Empty;
        public object data;

        public BaseRequest(string userName, string password, string apitype)
        {
            auth = new CommonLoginRequest(userName, password);
            this.api_type = apitype;
        }
    }
}
