
namespace FinMind.Net.Tests
{
    using FinMind.Net;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass()]
    public class FinMindTest
    {
        [TestMethod]
        public void LoginTest()
        {
            var userId = Environment.GetEnvironmentVariable("FINMIND_USERID", EnvironmentVariableTarget.User)!;
            var password = Environment.GetEnvironmentVariable("FINMIND_PASSWORD", EnvironmentVariableTarget.User)!;

            var client = new FinMindClient(userId, password);
            Assert.IsTrue(client.IsAuthenticated);
        }

        //[TestMethod]
        //public void LoginFailTest()
        //{
        //    var userId = "123";
        //    var password = Guid.NewGuid().ToString();

        //    Assert.ThrowsException<FinMindException>(() => new FinMindClient(userId, password));
        //}

        [TestMethod]
        public async Task GetUserInfoTest()
        {
            var userId = Environment.GetEnvironmentVariable("FINMIND_USERID", EnvironmentVariableTarget.User)!;
            var password = Environment.GetEnvironmentVariable("FINMIND_PASSWORD", EnvironmentVariableTarget.User)!;

            var client = new FinMindClient(userId, password);

            var result = await client.GetUserInfo();
            Assert.AreEqual(userId, result.UserId);
        }


        [TestMethod]
        public async Task GetTaiwanStockInfoTest()
        {
            var userId = Environment.GetEnvironmentVariable("FINMIND_USERID", EnvironmentVariableTarget.User)!;
            var password = Environment.GetEnvironmentVariable("FINMIND_PASSWORD", EnvironmentVariableTarget.User)!;

            var client = new FinMindClient(userId, password);

            var result = await client.GetTaiwanStockInfo();
            Assert.IsTrue(result.Any());
        }


        [TestMethod]
        public async Task GetTaiwanStockPriceTickTest()
        {
            var userId = Environment.GetEnvironmentVariable("FINMIND_USERID", EnvironmentVariableTarget.User)!;
            var password = Environment.GetEnvironmentVariable("FINMIND_PASSWORD", EnvironmentVariableTarget.User)!;

            var client = new FinMindClient(userId, password);

            var result = await client.GetTaiwanStockPriceTick(new Models.TaiwanStockPriceTickOptions
            {
                StockId = "0050",
                Date = new DateOnly(2021, 11, 30)
            });

            Assert.IsTrue(result.Any());
        }
    }
}