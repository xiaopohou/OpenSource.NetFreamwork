using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenSource.DB.Repository.DbContext;

namespace OpenSource.DB.Repository.Cache
{
    public class DapperDbContextDir : IDisposable
    {

        private static Dictionary<string, DapperDbContext> dicDbConnectionPool = new Dictionary<string, DapperDbContext>();
        private static Dictionary<string, DataSource> settingDis = new Dictionary<string, DataSource>();

        private DataSource config;

        public DapperDbContextDir()
        {
        }

        public DapperDbContextDir(Type obj)
        {
            var name = obj.Name;
            var key = settingDis.ContainsKey(name) ? name : "default";

            SqlConnectionString();
            config = settingDis[key];

            if (dicDbConnectionPool.ContainsKey(key)) return;

            lock (dicDbConnectionPool)
            {
                if (!dicDbConnectionPool.ContainsKey(key))
                {
                    dicDbConnectionPool[key] = new DapperDbContext(config.ConnectionString);
                }
            }
        }

        /// <summary>
        /// 读取数据库连接配置文件
        /// </summary>
        public void SqlConnectionString()
        {
            if (settingDis.Count < 1)
            {
                XElement root = XElement.Load("OpenSource.DB.Repositroy.xml");
                var objects = from obj in root.Elements() select obj;
                settingDis = objects.ToDictionary(k => k.Attribute("key").Value, v => new DataSource { Key = v.Attribute("key").Value, ConnectionString = v.Attribute("connectionString").Value, ProviderName = v.Attribute("providerName").Value, DBtype = v.Attribute("DBtype").Value });
            }
        }

        public DapperDbContext dbConnPool
        {
            get
            {
                return dicDbConnectionPool[config.Key];
            }
        }

        #region 释放资源

        public void Dispose()
        {

        }

        #endregion


    }


    public class DataSource
    {
        public string Key { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string DBtype { get; set; }
    }
}
