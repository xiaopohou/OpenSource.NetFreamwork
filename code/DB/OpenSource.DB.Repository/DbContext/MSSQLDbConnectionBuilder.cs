using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.DB.Repository.DbContext
{
    public class MSSQLDbConnectionBuilder:IDbConnectionBuilder
    {
        private string ConnectionString;
        public MSSQLDbConnectionBuilder(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region 实现IDbConnectionBuilder 创建MSSQL连接
        /// <summary>
        /// 获取SqlConnection连接对象
        /// </summary>
        public IDbConnection MyDbConnection
        {
            get
            {
                IDbConnection conn = null;
                try
                {
                    conn = new SqlConnection(ConnectionString);
                    conn.Open();
                }
                catch (Exception ex)
                {
                    if (null != conn)
                    {
                        conn.Dispose();
                        conn = null;
                    }
                    throw ex;
                }
                return conn;
            }
        }
        #endregion
    }
}
