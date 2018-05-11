
namespace xinlongyuOfWpf.Models.Return
{
    /// <summary>
    /// 通用返回类
    /// </summary>
    public class BaseReturn
    {
        public string api_type = string.Empty;
        public object data;
    }

    public class ReturnCode
    {
        public object data;
        public string error_code = string.Empty;
    }
}
