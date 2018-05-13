
namespace xinlongyuOfWpf.Models.Return
{
    /// <summary>
    /// 执行sql错误返回类
    /// </summary>
    public class SqlExcuteReturnError
    {
        public string error_code = string.Empty;

        public object data;

        public string allow_null = string.Empty;

        public string to = string.Empty;
    }
}
