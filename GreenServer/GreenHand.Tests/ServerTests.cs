using System;
using System.Threading.Tasks;
using GreenHand.Portable;
using GreenHand.Portable.Models;
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
    }
}
