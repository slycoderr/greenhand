using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable.Models;

namespace GreenHand.Server.Remote.Common.SensorApi
{
    public class SensorApi
    {

        public async Task StoreSensorData(SensorValue value)
        {
            using (var db = new GreenHandContext())
            {
                db.SensorValues.Add(value);

                var result = await db.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception("Failed to insert sensor value into database");
                }
            }
            
        }

        public async Task<IEnumerable<SensorValue>> GetSensorValues()
        {
            using (var db = new GreenHandContext())
            {
                return await db.SensorValues.ToListAsync();
            }
        }
    }
}
