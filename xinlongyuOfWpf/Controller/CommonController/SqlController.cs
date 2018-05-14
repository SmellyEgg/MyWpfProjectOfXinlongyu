using System;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Models.Request;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// sql逻辑控制层
    /// </summary>
    public class SqlController : BaseConnection
    {
        /// <summary>
        /// sql请求
        /// </summary>
        /// <param name="pageId"></param>
        public SqlExcuteReturn ExcuteSqlWithReturn(string sql)
        {
            string apitype = JsonApiType.execute;
            BaseRequest bj = GetCommonBaseRequest(apitype);
            string type = "db";
            SqlExcuteRequest sr = new SqlExcuteRequest(sql, type);
            bj.data = sr;
            try
            {
                var result =  this.PostForSql(bj);
                BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result);
                try
                {
                    SqlExcuteReturn ser = JsonController.DeSerializeToClass<SqlExcuteReturn>(brj.data.ToString());
                    if (ser.error_code.Equals("1"))
                    {
                        ser.data = null;
                    }
                    return ser;
                }
                catch
                {
                    SqlExcuteReturnError ser = JsonController.DeSerializeToClass<SqlExcuteReturnError>(brj.data.ToString());
                    SqlExcuteReturn serr = new SqlExcuteReturn();
                    serr.data = null;
                    serr.error_code = ser.error_code;
                    serr.to = ser.to;
                    return serr;
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex.Message);
                throw ex;
            }
        }
    }
}
