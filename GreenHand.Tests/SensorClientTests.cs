using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Client;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace GreenHand.Tests
{
    [TestClass]
    public class SensorClientTests
    {
        [TestMethod]
        public async Task LogValue2()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://greenhandrest.cloudapp.net:80/sensor/store/temp/65.5", null);

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public async Task GetSensorValues()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://greenhandrest.cloudapp.net:443/sensor/values");

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public async Task RegisterUser()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var client = new RestClient();

           var newUser = await client.CreateUser("slycoder", "test123");

            Assert.IsNotNull(newUser, "user is null");

            Debug.WriteLine(newUser.Email+" "+newUser.Id);
        }

        [TestMethod]
        public async Task RegisterUser2()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://greenhandrest.cloudapp.net:443/sensor/values");

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }
    }
}
