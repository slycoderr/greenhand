using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable;
using GreenHand.Portable.Models;

namespace GreenHand.Server.Remote.Common.SensorApi
{
    public class SensorApi
    {

        public async Task StoreSensorData(string key, int sensorId, string type, double value)
        {
            using (var db = new GreenHandContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.ApiKey == key);

                if (user == null)
                {
                    throw new SecurityException("Invalid Credentials");
                }

                var sensor = await db.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);

                if (sensor == null)
                {
                    throw new ArgumentException("Invalid sensor");
                }

                if (sensor.UserId != user.Id)
                {
                    throw new ArgumentException("Sensor mismatch");
                }

                SensorReadingType valueType;

                switch (type)
                {
                    case "temp":
                        valueType = SensorReadingType.Temperature;
                        break;
                    case "humid":
                        valueType = SensorReadingType.Humidity;
                        break;
                    default:
                        throw new ArgumentException("Invalid type");
                }

                db.SensorValues.Add(new SensorValue{CustomerId = user.Id, ReadingType = valueType, ReadResult = value, SensorId = sensorId, Timestamp = DateTime.Now});

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
