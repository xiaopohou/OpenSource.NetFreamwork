#if COREFX
using ConnectionType = System.Data.Common.DbConnection;
#endif

using System;
using System.Collections.Concurrent;
using System.Data;
using OpenSource.DB.Repository.Cache;
using System.Data.SqlClient;

namespace OpenSource.DB.Repository.DbContext
{
    /// <summary>
    /// Class is helper for use and close ConnectionType
    /// </summary>
    public class DapperDbContext : IDisposable
    {
        private string ConnectionString;

        #region 成员
        /// <summary>
        /// 访问锁
        /// </summary>
        private static object lockObj = new object();
        /// <summary>
        /// 连接池
        /// </summary>
        private ConcurrentQueue<IDbConnection> connPool;

        /// <summary>
        /// 创建数据类型
        /// </summary>
        public IDbConnectionBuilder ConnBuilder;

        #endregion


        public DapperDbContext(string connectionString)
        {

            ConnectionString = connectionString;

            connPool = new ConcurrentQueue<IDbConnection>();

            ConnBuilder =new MSSQLDbConnectionBuilder(ConnectionString);
        }

        public virtual IDbConnection Connection
        {
            get { return Pop(); }
        }


        #region 安全释放
        private bool disposed;
        public void Dispose()
        {
            //显示释放
            Dispose(true);
            //阻止被GC终结
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 二种方式
        /// 1.显示
        /// 2.终结器
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //释放托管资源.
                    lock (lockObj)
                    {
                        foreach (IDbConnection conn in connPool)
                        {
                            try
                            {
                                conn.Dispose(); //释放数据库资源
                            }
                            catch (Exception)
                            {

                            }
                        }
                        connPool = new ConcurrentQueue<IDbConnection>();
                    }
                }
            }
            disposed = true;
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~DapperDbContext()
        {
            Dispose(false);
        }

        #endregion

        #region 连接操作

        /// <summary>
        /// 压出一数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection Pop()
        {
            IDbConnection conn = default(IDbConnection);
            if (connPool.TryDequeue(out conn))
            {
                if (ConnectionState.Closed == conn.State)
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return conn;
            }
            else
            {
                //创建连接
                conn = ConnBuilder.MyDbConnection;
            }
            return conn;
        }

        /// <summary>
        /// 推入一数据库连接
        /// </summary>
        /// <param name="conn"></param>
        public void Push(IDbConnection conn)
        {
            if (null != conn)
            {
                if (connPool.Count < 30)
                {
                    connPool.Enqueue(conn);
                }
                else
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// 清空连接池
        /// </summary>
        public void Clear()
        {
            lock (lockObj)
            {
                foreach (IDbConnection conn in connPool)
                {
                    try
                    {
                        conn.Dispose();
                    }
                    catch (Exception)
                    {

                    }
                }
                connPool = new ConcurrentQueue<IDbConnection>();
            }
        }

        #endregion

    }

}