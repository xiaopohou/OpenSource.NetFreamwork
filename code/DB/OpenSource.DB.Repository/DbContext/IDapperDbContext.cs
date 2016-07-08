#if COREFX
using IDbConnection = System.Data.Common.DbConnection;
#endif

using System;
using System.Data;

namespace OpenSource.DB.Repository.DbContext
{

    /// <summary>
    /// Class is helper for use and close IDbConnection
    /// </summary>
    public interface IDapperDbContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
