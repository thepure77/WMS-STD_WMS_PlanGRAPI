using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanGRAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGRAPI.Test
{
    [TestClass]
    public class FilterTableControllerRealDBTest
    {
        [TestMethod]
        public void im_PlanGoodsReceiveTest()
        {
            var options = new DbContextOptionsBuilder<PlanGRDbContext>()
                .UseSqlServer("connection string").Options;

            using (var context = new PlanGRDbContext(options))
            {
                var controller = new FilterTableController(context);

                var result = controller.im_PlanGoodsReceive(new Newtonsoft.Json.Linq.JObject());

                Assert.AreNotEqual(null, result);
            }
        }
    }
}
