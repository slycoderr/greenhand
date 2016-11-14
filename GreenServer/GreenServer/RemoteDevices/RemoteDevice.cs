using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

namespace GreenServer.RemoteDevices
{
    class RemoteDevice
    {
        IStream connection;
        RemoteDevice arduino;

        public void SetupRemoteArduino()
        {
            //create a bluetooth connection and pass it to the RemoteDevice
            //I am using a constructor that accepts a device name or ID.
            connection = new BluetoothSerial("MyBluetoothDevice");
            //arduino = new RemoteDevice(connection);
            arduino = new RemoteDevice();
            arduino.connection = connection;

            //add a callback method (delegate) to be invoked when the device is ready, refer to the Events section for more info
            arduino.DeviceReady += Setup;

            //always begin your IStream
            connection.begin(115200, SerialConfig.SERIAL_8N1);
        }

        //treat this function like "setup()" in an Arduino sketch.
        public void Setup()
        {
            //set digital pin 13 to OUTPUT
            arduino.pinMode(13, PinMode.OUTPUT);

            //set analog pin A0 to ANALOG INPUT
            arduino.pinMode("A0", PinMode.ANALOG);
        }

        //This function will read a value from our ANALOG INPUT pin A0 (range: 0 - 1023) and set pin 13 HIGH if the value is >= 512.
        public void ReadAndReport()
        {
            UInt16 val = arduino.analogRead("A0");
            if (val >= 512)
            {
                arduino.digitalWrite(13, PinState.HIGH);
            }
            else
            {
                arduino.digitalWrite(13, PinState.LOW);
            }
        }
    }
}
