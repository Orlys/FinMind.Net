
namespace FinMind.Net.Tests
{
    using FinMind.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting; 
    using System.Threading.Tasks;
    using System;
    using System.Linq;

    [TestClass()]
    public class FinMindClientTests
    {
        private static FinMindClient? _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        { 
            _client = new FinMindClient();
        } 

        [TestMethod()]
        public void LoginFailTest()
        { 
            var userId = "123";
            var password = "123" + Guid.NewGuid().ToString();

            Assert.ThrowsExceptionAsync<FinMindException>(() => _client!.Login(userId, password));
        }

        [TestMethod()]
        public async Task LoginTest()
        {
            var _client = new FinMindClient();
            var userId = Environment.GetEnvironmentVariable("FINMIND_USERID", EnvironmentVariableTarget.User)!;
            var password = Environment.GetEnvironmentVariable("FINMIND_PASSWORD", EnvironmentVariableTarget.User)!;
            await _client.Login(userId, password);

            Assert.IsTrue(_client.IsAuthenticated);
        }

        [TestMethod()]
        public async Task GetTaiwanStockInfoTest()
        {
            var list = await _client!.GetTaiwanStockInfo();

            Assert.IsTrue(list.Any());
        }


        [TestMethod()]
        
        public async Task GetUserInfoTest()
        {
            var _client = new FinMindClient();
            var userId = Environment.GetEnvironmentVariable("FINMIND_USERID", EnvironmentVariableTarget.User)!;
            var password = Environment.GetEnvironmentVariable("FINMIND_PASSWORD", EnvironmentVariableTarget.User)!;
            await _client.Login(userId, password);

            Assert.IsTrue(_client.IsAuthenticated);

            var userInfo = await _client!.GetUserInfo();

            Assert.IsTrue(userInfo.IsEmailVerified);
            Assert.AreEqual(userInfo.UserId, userId);
        }
    }
}