using System;
using System.Linq;
using System.Threading.Tasks;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenHand.Tests.Server
{
    [TestClass]
    public class Sensor
    {
        [TestMethod]
        public async Task InsertSensorTemp()
        {
            SensorApi api = new SensorApi();

            await api.StoreSensorData("50ea847f-ad7a-4a1a-a5be-438f94e1372086da0c1a-46b8-4428-b549-a7b6f7857831ee3dd4e2-db11-4ab6-acf1-5e77e2f8e4ce", 2, "temp", 99);
        }

        [TestMethod]
        public async Task GetAllSensorValues()
        {
            SensorApi api = new SensorApi();

            var results = (await api.GetSensorValues(1, 60)).ToList();

            Assert.IsTrue(results.Any(), "Count:" + results.Count);
        }
    }
}
