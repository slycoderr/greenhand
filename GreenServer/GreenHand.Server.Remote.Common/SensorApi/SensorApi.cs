using System;
using System.Collections.Generic;
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
                await db.SensorValues.AddAsync(value);

                var result = await db.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception("Failed to insert sensor value into database");
                }
            }
            
        }
    }
}
