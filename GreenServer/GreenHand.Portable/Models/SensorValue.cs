using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Portable.Models
{

    public class SensorValue
    {
        public Guid Id { get; set; }
        public SensorReadingType ReadingType { get; set; }

        /// <summary>
        /// The resulting value read from the sensor
        /// </summary>
        public double ReadResult { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
