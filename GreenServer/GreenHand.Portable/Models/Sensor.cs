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

        public string IpAddress
        {
            get { return ipAddress; }
            set { SetValue(ref ipAddress, value); }
        }

        public int Port
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

        public async Task ReadValue()
        {
            if (Type == "TEMP")
            {
                var rawValue = await Network.SendAndReceiveData("ReadValue");
                double value;

                if (double.TryParse(rawValue, out value))
                {
                    Readings.Add(new SensorValue {Timestamp = DateTime.Now, Type = ValueType.Temperature, Value = value});
                }
                else
                {
                    throw new Exception("TEMP wasn't a valid response: "+ rawValue);
                }
            }
        }
    }
}