﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Portable.Models
{

    public class SensorValue
    {
        public Guid Id { get; set; }
        public SensorReadingType Type { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
