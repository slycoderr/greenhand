using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenHand.Tests.Client
{
    [TestClass]
    public class Sensor
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


    }
}
