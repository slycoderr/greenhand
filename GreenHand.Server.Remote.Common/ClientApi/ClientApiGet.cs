using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            using (var db = new GreenHandContext())
            {
                return await db.SensorValues.ToListAsync();
            }
        }
    }
}
