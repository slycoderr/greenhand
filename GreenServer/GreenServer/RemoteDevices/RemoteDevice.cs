using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

namespace GreenServer.RemoteDevices
{
    public class RemoteDevice
    {
        private Microsoft.Maker.RemoteWiring.RemoteDevice arduino;
        private IStream connection;

        public void SetupRemoteArduino()
        {
            //create a bluetooth connection and pass it to the RemoteDevice
            connection = new BluetoothSerial("MyBluetoothDevice");
            //I am using a constructor that accepts a device name or ID.
            arduino = new Microsoft.Maker.RemoteWiring.RemoteDevice(connection);

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
            ushort val = arduino.analogRead("A0");
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