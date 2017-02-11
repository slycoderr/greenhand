using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public async Task LogValue()
        {
            var value = new SensorValue() {Timestamp = DateTime.Now, ReadingType = SensorReadingType.Humidity, ReadResult = 50};

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://greenhandrest.cloudapp.net:80/greenhand/StoreData/", new StringContent(JsonConvert.SerializeObject(value)));

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public async Task LogValue2()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://greenhandrest.cloudapp.net:80/greenhand/StoreData/temp/65.5", null);

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }
    }
}
