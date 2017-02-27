using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GreenHand.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//E959FD5C80F76DF7A593AAE09686E604F74BE8B0
namespace GreenHand.Tests.Client
{
    [TestClass]
    public class Sensor
    {
        /*
         Connected.
8—Ž‚‚–ƒ‡–ÿÿSerial connected.
sensor initialized
Initializing WiFi...
Attempting to connect to WEP network, SSID: AaronsCave

WiFi connected
IP address: 
192.168.0.101
Connected.
44.30
Connected.
44.59
readinh humidity.... 
45.61%
Connecting to server...
Connected to Server.
certificate matches
Sending request... /sensor/store/50ea847f-ad7a-4a1a-a5be-438f94e1372086da0c1a-46b8-4428-b549-a7b6f7857831ee3dd4e2-db11-4ab6-acf1-5e77e2f8e4ce/3/humid/45.61
Request sent.
Connected.
45.55
Connected.
46.07
Connected.
44.56
Connected.
43.98
Connected.
43.37
Connected.
43.57
Connected.
45.11
Connected.
44.46
Connected.
43.64
             
             */

        [TestMethod]
        public async Task LogValue2()
        {
            using (var client = new HttpClient())
            {
                //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                var key = "50ea847f-ad7a-4a1a-a5be-438f94e1372086da0c1a-46b8-4428-b549-a7b6f7857831ee3dd4e2-db11-4ab6-acf1-5e77e2f8e4ce";

                //var response = await client.PostAsync($"https://greenhand.azurewebsites.net/sensor/store/50ea847f-ad7a-4a1a-a5be-438f94e1372086da0c1a-46b8-4428-b549-a7b6f7857831ee3dd4e2-db11-4ab6-acf1-5e77e2f8e4ce/2/temp/98", null);
                var response = await client.PostAsync($"https://greenhand.azurewebsites.net/sensor/store/50ea847f-ad7a-4a1a-a5be-438f94e1372086da0c1a-46b8-4428-b549-a7b6f7857831ee3dd4e2-db11-4ab6-acf1-5e77e2f8e4ce/3/humid/4561", null);

                Assert.IsTrue(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public async Task GetSensorValues()
        {
            RestClient client = new RestClient();

            await client.Login("testerer", "test123");
            var result = await client.GetSensorValues();

            Assert.IsTrue(result.Any());
        }


    }
}
