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
            var value = new SensorValue() {Timestamp = DateTime.Now, Type = SensorReadingType.Temperature, Value = 50};

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:9101/greenhand/StoreData/", new StringContent(JsonConvert.SerializeObject(value)));

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }
    }
}
