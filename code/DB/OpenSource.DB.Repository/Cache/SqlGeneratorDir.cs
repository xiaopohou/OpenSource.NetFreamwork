using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSource.DB.Repository.SqlGenerator;

namespace OpenSource.DB.Repository.Cache
{
    /// <summary>
    /// Model映射 cache
    /// </summary>
    public static class SqlGeneratorDir
    {
        private static object objLock = new object();
        /// <summary>
        /// 用于缓存对象转换实体
        /// </summary>
        private static Dictionary<long, object> _ModelDesCache = new Dictionary<long, object>();

        /// <summary>
        /// 确定是否已经存在缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ISqlGenerator<TEntiy> ExistModelDesCache<TEntiy>(ESqlConnector eSql) where TEntiy : class
        {
            object value;
            _ModelDesCache.TryGetValue(typeof(TEntiy).Name.GetHashCode(), out value);
            if (null == value)
            {
                value = Add<TEntiy>(eSql);
            }
            return value as ISqlGenerator<TEntiy>;
        }

        public static ISqlGenerator<TEntiy> Add<TEntiy>(ESqlConnector eSql) where TEntiy : class
        {
            ISqlGenerator<TEntiy> generator = null;
            lock (objLock)
            {
                var entityType = typeof(TEntiy).Name.GetHashCode();
                if ((!_ModelDesCache.ContainsKey(entityType)))
                {
                     generator = new SqlGenerator<TEntiy>(eSql);
                    _ModelDesCache.Add(entityType, generator);
                    return generator;
                }
            }
            return generator;
        }
    }
}
