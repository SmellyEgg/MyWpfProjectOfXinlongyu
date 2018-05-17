using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.PageInfo;

namespace xinlongyuOfWpf.Controller.PageController
{
    /// <summary>
    /// 页面缓存类
    /// </summary>
    public class PageCacher : dataBaseController
    {
        //hs_new_pages
        private string _pageTableName = "hs_new_pages";

        //private string _pageControlTableName = "hs_new_page_ctrls";
        /// <summary>
        /// 缓存页面信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="listControlObject"></param>
        /// <returns></returns>
        public int CachePageInfo(PageInfoDetail obj)
        {
            List<ControlDetailForPage> listControlObject = ControlCaster.CastArrayToControl(obj.data.control_list);
            //
            this.CachePageBaseInfo(obj);
            //
            this.CachePageControlInfo(listControlObject);
            return 1;
        }

        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public PageInfoDetail GetPageInfo(int pageid)
        {
            PageInfoDetail pd = new PageInfoDetail();
            pd.data = this.GetPageBaseInfo(pageid);
            if (object.Equals(pd.data, null))
            {
                return null;
            }
            pd.data.control_list = ControlCaster.CastControlToObjectArray(this.GetControlBaseInfo(pageid));
            return pd;
        }

        /// <summary>
        /// 获取缓存界面的时间戳
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public int GetTimeStampOfPage(int pageid)
        {
            string sql = @"select last_time from hs_new_pages where page_id = '{0}'";
            sql = string.Format(sql, pageid);
            string value = this.ExcuteReturnOne(sql);
            return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        }

        /// <summary>
        /// 获取页面基本信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        private PageBaseInfo GetPageBaseInfo(int pageid)
        {
            string sql = @"select page_id, page_name, page_width, page_height, last_time from hs_new_pages where page_id = '{0}'";
            sql = string.Format(sql, pageid);
            SQLiteDataReader reader = this.ExcuteReader(sql);

            while (reader.Read())
            {
                PageBaseInfo obj = new PageBaseInfo();
                obj.page_id = int.Parse(reader[0].ToString());
                obj.page_name = reader[1].ToString();
                obj.page_width = int.Parse(reader[2].ToString());
                obj.page_height = int.Parse(reader[3].ToString());
                obj.last_time = int.Parse(reader[4].ToString());
                return obj;
            }
            reader.Close();
            this._connection.Close();
            return null;
        }

        /// <summary>
        /// 获取控件基本信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        private List<ControlDetailForPage> GetControlBaseInfo(int pageid)
        {
            string sql = @"select * from hs_new_page_ctrls t where t.page_id = '{0}'";
            sql = string.Format(sql, pageid);
            SQLiteDataReader reader = this.ExcuteReader(sql);
            List<ControlDetailForPage> listControl = new List<ControlDetailForPage>();
            while (reader.Read())
            {
                //int index = 0;
                ControlDetailForPage obj = new ControlDetailForPage();
                foreach (var prop in obj.GetType().GetFields())
                {
                    string value = reader[prop.Name].ToString();
                    //这里判断一下类型
                    if ("Boolean".Equals(prop.FieldType.Name))
                    {
                        prop.SetValue(obj, CommonConverter.StringToBool(value));
                    }
                    else if ("Int32".Equals(prop.FieldType.Name))
                    {
                        prop.SetValue(obj, CommonConverter.StringToInt(value));
                    }
                    else
                    {
                        prop.SetValue(obj, value);
                    }
                }
                listControl.Add(obj);
            }
            reader.Close();
            this._connection.Close();
            return listControl;
        }

        public int DeletePageInfo(PageInfoDetail obj)
        {
            string sql = this.GetDeleteSql(obj.data);
            this.ExcuteNoQuery(sql);
            return 1;
        }

        /// <summary>
        /// 缓存页面基本信息
        /// </summary>
        /// <param name="obj"></param>
        private void CachePageBaseInfo(PageInfoDetail obj)
        {
            string deleteSql = string.Format(@"delete from hs_new_pages where page_id = '{0}'", obj.data.page_id);
            this.ExcuteNoQuery(deleteSql);
            string sql = @"insert into hs_new_pages (page_id, page_name, page_width, page_height,last_time) values ('{0}', '{1}', '{2}', '{3}', '{4}')";
            sql = string.Format(sql, obj.data.page_id, obj.data.page_name, obj.data.page_width, obj.data.page_height, obj.data.last_time);
            this.ExcuteNoQuery(sql);
        }

        /// <summary>
        /// 缓存控件基本信息
        /// </summary>
        /// <param name="obj"></param>
        private void CachePageControlInfo(List<ControlDetailForPage> listControlObject)
        {
            if (object.Equals(listControlObject, null) || listControlObject.Count < 1)
            {
                return;
            }
            using (SQLiteConnection conn = new SQLiteConnection(this.GetDatabaseName()))//创建连接  
            {
                conn.Open();//打开连接  
                using (SQLiteTransaction tran = conn.BeginTransaction())//实例化一个事务  
                {
                    SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
                    cmd.Transaction = tran;
                    //删除所有控件先
                    string deleteSql = string.Format(@"delete from hs_new_page_ctrls where page_id = '{0}'", listControlObject[0].page_id);
                    cmd.CommandText = deleteSql;
                    cmd.ExecuteNonQuery();

                    string sqlInsert = "insert into hs_new_page_ctrls values({0})";
                    StringBuilder sqlInsertValues = new StringBuilder(1000);
                    int currentIndex = 0;
                    foreach (var prop in listControlObject[0].GetType().GetFields())
                    {
                        if (currentIndex == 0)
                        {
                            sqlInsertValues.Append("@" + prop.Name);
                            currentIndex++;
                        }
                        else
                        {
                            sqlInsertValues.Append(", @" + prop.Name);
                        }
                    }
                    sqlInsert = string.Format(sqlInsert, sqlInsertValues.ToString());
                    foreach (ControlDetailForPage controlObj in listControlObject)
                    {

                        foreach (var prop in controlObj.GetType().GetFields())
                        {
                            cmd.CommandText = sqlInsert;//设置带参SQL语句  
                            cmd.Parameters.Add(new SQLiteParameter("@" + prop.Name, prop.GetValue(controlObj)));
                        }
                        cmd.ExecuteNonQuery();//执行查询  
                    }
                    tran.Commit();//提交  
                }
            }
        }

        /// <summary>
        /// 获取更新的语句
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetUpdateSql(PageBaseInfo obj)
        {
            string updateSql = "update {0} set {1} where page_id = '{2}'";
            string values = string.Empty;
            StringBuilder sb = new StringBuilder(500);
            foreach (var prop in obj.GetType().GetFields())
            {
                if (sb.Length < 1)
                {
                    sb.Append(prop.Name + "=" + prop.GetValue(obj));
                }
                else
                {
                    sb.Append("," + prop.Name + "=" + prop.GetValue(obj));
                }
            }
            updateSql = string.Format(updateSql, _pageTableName, sb.ToString(), obj.page_id);
            return updateSql;
        }

        private string GetDeleteSql(PageBaseInfo obj)
        {
            string sql = "delete from {0} where page_id = '{1}'";
            sql = string.Format(sql, _pageTableName, obj.page_id);
            return sql;
        }

        /// <summary>
        /// 测试数据库的链接
        /// </summary>
        public void TestConnect()
        {
            string sql = "insert into testxinlongyu values ('1', 'wubiqiu')";
            this.ExcuteNoQuery(sql);
        }
    }
}
