using System;
using System.Diagnostics;
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
            var sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < 20; i++)
            {
                //await RestClient.Login("slycoder", "M@gic345");
            }

            Debug.WriteLine(sw.ElapsedMilliseconds/20);
        }

        [TestMethod]
        public async Task Register()
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            //var client = new RestClient();

            //await client.CreateUser(Guid.NewGuid().ToString(), "test3");
        }
    }
}
