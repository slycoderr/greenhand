using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHand.Portable;
using GreenHand.Portable.Models;

namespace GreenHand.Server.Remote.Common.ClientApi
{
    public partial class ClientApi
    {
        public async Task<IEnumerable<SensorValue>> GetSensorValues()
        {
            var data = new List<SensorValue>();

            using (var db = new GreenHandContext())
            {
                data = db.SensorValues.Where(s => s.ReadingType == SensorReadingType.Temperature).ToList();
            }

            return data;
        }
    }
}
