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
            await new UserApi().CreateUser("slycoder", "M@gic345");
        }

        [TestMethod]
        public async Task Login()
        {
            Assert.IsTrue(await new UserApi().Login(new Portable.Models.User(){Email = "slycoder127", Password = "M@gic345"}));
        }
    }
}
