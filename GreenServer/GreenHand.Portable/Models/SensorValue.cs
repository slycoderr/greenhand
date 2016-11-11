using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Portable.Models
{
    public enum ValueType
    {
        Temperature,
        Humidity,
        pH

    }
    public class SensorValue
    {
        public ValueType Type { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
