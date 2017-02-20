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
        public async Task InsertSensorValue()
        {
            SensorApi api = new SensorApi();

            await api.StoreSensorData(new SensorValue() { Timestamp = DateTime.Now, ReadingType = SensorReadingType.Humidity, ReadResult = 50 });
        }

        [TestMethod]
        public async Task GetAllSensorValues()
        {
            SensorApi api = new SensorApi();

            var results = (await api.GetSensorValues()).ToList();

            Assert.IsTrue(results.Any(), "Count:" + results.Count);
        }
    }
}
