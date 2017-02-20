using System.Threading.Tasks;
using GreenHand.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenHand.Tests.Client
{
    [TestClass]
    public class User
    {
        [TestMethod]
        public async Task Login()
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var client = new RestClient();

            await client.Login("slycoder", "test123");
        }

        [TestMethod]
        public async Task Register()
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var client = new RestClient();

            var result = await client.CreateUser("test3", "test3");
        }
    }
}
