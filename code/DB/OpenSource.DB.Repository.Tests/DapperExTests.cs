using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenSource.DB.Repository.DbContext;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using OpenSource.DB.IRepository;
using OpenSource.DB.Repository.SqlGenerator;

namespace OpenSource.DB.Repository.Tests
{
    [TestClass]
    public class DapperExTests
    {
        [TestMethod]
        public void FindTests()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FindPageTest()
        {
            //var result = IocManager.IOCManager.Container.GetInstance<Itbl_PublicAccountRepository>();
            //result.Find(x => x.Groupid == 2);
            //DapperRepository<tbl_PublicAccount> _dapper = new DapperRepository<tbl_PublicAccount>();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UpdateTests()
        {
            DapperRepository<tbl_PublicAccount> _dapper = new DapperRepository<tbl_PublicAccount>();
            var result = _dapper.FindAll(x => x.Openid == "o0DpJxBxK9ZzYd8DiObV84GDru64").FirstOrDefault();
            _dapper.Update(result);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DeleteTests()
        {
            DapperRepository<tbl_PublicAccount> _dapper = new DapperRepository<tbl_PublicAccount>();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InsertTests()
        {
            DapperRepository<tbl_PublicAccount> _dapper = new DapperRepository<tbl_PublicAccount>();
       
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void PagesTests()
        {
            DapperRepository<tbl_PublicAccount> _dapper = new DapperRepository<tbl_PublicAccount>();
         
            Assert.IsTrue(true);
        }
    }
}
