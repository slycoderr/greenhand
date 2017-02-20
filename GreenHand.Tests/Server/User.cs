using System;
using System.Threading.Tasks;
using GreenHand.Server.Remote.Common.UserApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenHand.Tests.Server
{
    [TestClass]
    public class User
    {

        [TestMethod]
        public async Task RegisterUser()
        {
            UserApi api = new UserApi();

            var results = await api.CreateUser("testerer", "test123");

            Assert.IsNotNull(results, "user was null");
        }
    }
}
