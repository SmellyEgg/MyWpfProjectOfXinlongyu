using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinlongyuOfWpf.Models.Return
{
    /// <summary>
    /// sql执行返回类
    /// </summary>
    public class SqlExcuteReturn
    {
        public string error_code = string.Empty;

        public Dictionary<string, string>[] data;

        public string allow_null = string.Empty;

        public string to = string.Empty;
    }
}
