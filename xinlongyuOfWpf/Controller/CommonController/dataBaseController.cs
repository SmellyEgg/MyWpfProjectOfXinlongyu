using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xinlongyuOfWpf.Controller.CommonPath;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// 数据库逻辑层
    /// 本程序使用sqllite作为缓存的数据库
    /// 需要使用数据库的逻辑层直接继承这个父类就可以了
    /// </summary>
    public class dataBaseController
    {
        public SQLiteConnection _connection;
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExcuteNoQuery(string sql)
        {
            string databaseName = this.GetDatabaseName();
            using (SQLiteConnection conn = new SQLiteConnection(databaseName))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        //public void ExcuteNoQueryWithTransAction(string sql)
        //{
        //    using (SQLiteConnection conn = new SQLiteConnection(this.GetDatabaseName()))//创建连接  
        //    {
        //        conn.Open();//打开连接  
        //        using (SQLiteTransaction tran = conn.BeginTransaction())//实例化一个事务  
        //        {
        //            for (int i = 0; i < 100000; i++)
        //            {
        //                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL命令  
        //                cmd.Transaction = tran;
        //                cmd.CommandText = "insert into student values(@id, @name, @sex)";//设置带参SQL语句  
        //                cmd.Parameters.AddRange(new[] {//添加参数  
        //                   new SQLiteParameter("@id", i),
        //                   new SQLiteParameter("@name", "中国人"),
        //                   new SQLiteParameter("@sex", "男")
        //               });
        //                cmd.ExecuteNonQuery();//执行查询  
        //            }
        //            tran.Commit();//提交  
        //        }
        //    }
        //}

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExcuteQuery(string sql)
        {
            string databaseName = this.GetDatabaseName();
            using (SQLiteConnection conn = new SQLiteConnection(databaseName))
            {
                SQLiteCommand command = conn.CreateCommand();
                command.CommandText = sql;
                DataSet ds = new DataSet();
                conn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter(command);
                da.Fill(ds);
                da.Dispose();
                command.Dispose();
                conn.Close();
                return ds;
            }
        }



        /// <summary>
        /// 获取唯一返回值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExcuteReturnOne(string sql)
        {
            DataSet dtset = this.ExcuteQuery(sql);
            if (!object.Equals(dtset, null) && dtset.Tables.Count > 0 && dtset.Tables[0].Rows.Count > 0)
            {
                string value = dtset.Tables[0].Rows[0][0].ToString();
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 执行sql读取返回内容
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SQLiteDataReader ExcuteReader(string sql)
        {
            _connection = new SQLiteConnection(this.GetDatabaseName());
            SQLiteCommand command = new SQLiteCommand(sql, _connection);
            _connection.Open();
            SQLiteDataReader reader = command.ExecuteReader();

            return reader;
        }

        /// <summary>
        /// 获取数据库路径
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            string url = ConfigManagerSection.dataBasePath;
            string dbPath = "Data Source =" + url;
            return dbPath;
        }
    }
}
