using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

namespace GreenServer.Networking
{
    public class RemoteDeviceConnection : INetworkConnection
    {
        private Microsoft.Maker.RemoteWiring.RemoteDevice arduino;
        private IStream connection;

        public Task Connect(string ip, int port)
        {
            return Task.Run(() =>
            {
                connection = new BluetoothSerial("MyBluetoothDevice");
                //I am using a constructor that accepts a device name or ID.
                arduino = new Microsoft.Maker.RemoteWiring.RemoteDevice(connection);

                //add a callback method (delegate) to be invoked when the device is ready, refer to the Events section for more info
                arduino.DeviceReady += Setup;

                //always begin your IStream
                connection.begin(115200, SerialConfig.SERIAL_8N1);
            });
        }

        //treat this function like "setup()" in an Arduino sketch.
        public void Setup()
        {
            //set digital pin 13 to OUTPUT
            arduino.pinMode(13, PinMode.OUTPUT);

            //set analog pin A0 to ANALOG INPUT
            arduino.pinMode("A0", PinMode.ANALOG);
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public Task<string> SendAndReceiveData(string message)
        {
            return Task.Run(() => arduino.analogRead("A0").ToString());
        }

        public bool IsConnected { get; set; }
    }
}
