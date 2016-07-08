using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenSource.DB.Repository.DbContext;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using OpenSource.DB.Repository.SqlGenerator;

namespace OpenSource.DB.Repository.Tests
{
    [TestClass]
    public class DapperExTests
    {
        [TestMethod]
        public void FindTests()
        {

            IDbConnection conn = new SqlConnection("server=192.168.1.186;database=WeatServices;uid=sa;pwd=123");
            IDapperRepository<weat_LoginLogs> _dapper = new DapperRepository<weat_LoginLogs>(conn, ESqlConnector.MSSQL);
            var result = _dapper.FindAll(x => x.loginName == "15013797373");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FindPageTest()
        {
            IDbConnection conn = new SqlConnection("server=192.168.1.186;database=WeatServices;uid=sa;pwd=123");
            IDapperRepository<weat_LoginLogs> _dapper = new DapperRepository<weat_LoginLogs>(conn, ESqlConnector.MSSQL);
            var result = _dapper.FindAllBetween(1, 10, (x => x.id));
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UpdateTests()
        {
            IDbConnection conn = new SqlConnection("server=192.168.1.186;database=WeatServices;uid=sa;pwd=123");
            IDapperRepository<weat_LoginLogs> _dapper = new DapperRepository<weat_LoginLogs>(conn, ESqlConnector.MSSQL);
            var result = _dapper.FindAll(x => x.loginName == "15013797373").FirstOrDefault();
            _dapper.Update(result);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DeleteTests()
        {
            IDbConnection conn = new SqlConnection("server=192.168.1.186;database=WeatServices;uid=sa;pwd=123");
            IDapperRepository<weat_LoginLogs> _dapper = new DapperRepository<weat_LoginLogs>(conn, ESqlConnector.MSSQL);
            var result = _dapper.FindAll(x => x.loginName == "15013797373").FirstOrDefault();
            result.loginName = "9999";
            _dapper.DeleteAsync(result);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InsertTests()
        {
            IDbConnection conn = new SqlConnection("server=192.168.1.186;database=WeatServices;uid=sa;pwd=123");
            IDapperRepository<weat_LoginLogs> _dapper = new DapperRepository<weat_LoginLogs>(conn, ESqlConnector.MSSQL);
            var result = _dapper.FindAll(x => x.loginName == "15013797373" && x.id == 28).FirstOrDefault();
            result.loginName = "9999";
            _dapper.Insert(result);
            Assert.IsTrue(true);
        }
    }
}
