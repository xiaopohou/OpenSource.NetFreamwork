using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSource.DB.Repository.DbContext
{
    /// <summary>
    /// 数据库连接对象创建接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbConnectionBuilder
    {
        /// <summary>
        /// 新创建的连接实例
        /// </summary>
        IDbConnection MyDbConnection
        {
            get;
        }
    }
}
