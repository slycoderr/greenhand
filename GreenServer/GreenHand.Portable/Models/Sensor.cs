using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Portable.Models
{

    public class Sensor
    {
        public ObservableCollection<SensorValue> Readings { get; set; }
        public string Name { get; }
        public string IpAddress { get; }
        public string Id { get; set; }
        public string Type { get; set; }

        public void Connect()
        {
            
        }

        public void Disconnect()
        {
            
        }

        public void ReadValue()
        {
            
        }
    }
}
