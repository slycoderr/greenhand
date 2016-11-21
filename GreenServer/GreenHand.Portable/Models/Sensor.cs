using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Slycoder.Portable.MVVM;

namespace GreenHand.Portable.Models
{
    public class Sensor : BindableBase
    {
        private string name;
        private string ipAddress;
        private int port;
        private string id;
        private string type;

        public Sensor(INetworkConnection connection)
        {
            Network = connection;
            Type = "TEMP";
        }

        public INetworkConnection Network { get; }

        public ObservableCollection<SensorValue> Readings { get; set; } = new ObservableCollection<SensorValue>();

        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }

        public string DeviceAddress
        {
            get { return ipAddress; }
            set { SetValue(ref ipAddress, value); }
        }

        public int SecondaryDeviceAddress
        {
            get { return port; }
            set { SetValue(ref port, value); }
        }

        public string Id
        {
            get { return id; }
            set { SetValue(ref id, value); }
        }

        public string Type
        {
            get { return type; }
            set { SetValue(ref type, value); }
        }

        public async Task<SensorValue> ReadValue()
        {
            if (Type == "TEMP")
            {
                var rawValueAsString = await Network.SendAndReceiveData("ReadValue", 0);

                if (ushort.TryParse( rawValueAsString, out ushort rawAnalogValue))
                {
                    var voltage = (rawAnalogValue * 0.004882814);
                    var degreesC = (voltage - 0.5) * 100.0;
                    var degreesF = degreesC * (9.0 / 5.0) + 32.0;
                    var val = new SensorValue {Timestamp = DateTime.Now, Type = ValueType.Temperature, Value = degreesF};

                    Readings.Add(val);

                    return val;
                }

                else
                {
                    throw new Exception("TEMP wasn't a valid response: "+ rawValueAsString);
                }
            }

            throw new ArgumentException($"The type{Type} is unknown");
        }
    }
}