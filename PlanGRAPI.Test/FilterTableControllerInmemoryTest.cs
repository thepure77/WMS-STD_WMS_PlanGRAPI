using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanGRAPI.Controllers;

namespace PlanGRAPI.Test
{
    [TestClass]
    public class FilterTableControllerInmemoryTest
    {
        [TestMethod]
        public void im_PlanGoodsReceiveTest()
        {
            var options = new DbContextOptionsBuilder<PlanGRDbContext>()
                .UseInMemoryDatabase(databaseName: "PlanGRDb").Options;

            using (var context = new PlanGRDbContext(options))
            {
                var controller = new FilterTableController(context);

                var result = controller.im_PlanGoodsReceive(new Newtonsoft.Json.Linq.JObject());

                Assert.AreNotEqual(null, result);
            }
        }
    }
}
