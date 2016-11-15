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
                var rawValue = await Network.SendAndReceiveData("ReadValue", 0);
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

        float getTemp()
        {
            //returns the temperature from one DS18S20 in DEG Celsius
            OneWire ds;
            byte[] data = new byte[12];
            byte[] addr = new byte[8];

            if (!ds.search(addr))
            {
                //no more sensors on chain, reset search
                ds.reset_search();
                return -1000;
            }

            if (OneWire::crc8(addr, 7) != addr[7])
            {
                //Serial.println("CRC is not valid!");
                return -1000;
            }

            if (addr[0] != 0x10 && addr[0] != 0x28)
            {
                //Serial.print("Device is not recognized");
                return -1000;
            }

            ds.reset();
            ds.select(addr);
            ds.write(0x44, 1); // start conversion, with parasite power on at the end

            byte present = ds.reset();
            ds.select(addr);
            ds.write(0xBE); // Read Scratchpad

            for (int i = 0; i < 9; i++)
            { // we need 9 bytes
                data[i] = ds.read();
            }

            ds.reset_search();

            byte MSB = data[1];
            byte LSB = data[0];

            float tempRead = ((MSB << 8) | LSB); //using two's compliment
            float TemperatureSum = tempRead / 16;

            return (TemperatureSum * 18 + 5) / 10 + 32;
        }
    }
}