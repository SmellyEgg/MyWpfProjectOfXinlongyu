

namespace xinlongyuOfWpf.Models.Request
{
    /// <summary>
    /// sql执行请求类
    /// </summary>
    public class SqlExcuteRequest
    {
        public string sql = string.Empty;

        public string type = string.Empty;

        public SqlExcuteRequest(string newsql, string newtype)
        {
            this.sql = newsql;
            this.type = newtype;
        }
    }
}
