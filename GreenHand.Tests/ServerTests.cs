using System;
using System.Linq;
using System.Threading.Tasks;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.ClientApi;
using GreenHand.Server.Remote.Common.SensorApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreenHand.Tests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public async Task InsertSensorValue()
        {
            SensorApi api = new SensorApi();

            await api.StoreSensorData(new SensorValue(){Timestamp = DateTime.Now, ReadingType = SensorReadingType.Humidity, ReadResult = 50});
        }

        [TestMethod]
        public async Task GetAllSensorValues()
        {
            ClientApi api = new ClientApi();

            var results = (await api.GetSensorValues()).ToList();

            Assert.IsTrue(results.Any(), "Count:" +results.Count);
        }
    }
}
