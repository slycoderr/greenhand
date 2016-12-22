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
        private SensorReadingType type;
        private bool autoRetryConnection = true;

        public INetworkConnection Network { get; set; }

        public ObservableCollection<SensorValue> Readings { get; set; } = new ObservableCollection<SensorValue>();

        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }

        public bool AutoRetryConnection
        {
            get { return autoRetryConnection; }
            set { SetValue(ref autoRetryConnection, value); }
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

        public SensorReadingType Type
        {
            get { return type; }
            set { SetValue(ref type, value); }
        }

        public async Task<SensorValue> ReadValue()
        {
            return await Network.RetrieveValue(Type);
        }
    }
}