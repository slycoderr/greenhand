using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GreenHand.Portable;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using Slycoder.Portable.MVVM;

namespace GreenServer.Networking
{
    public class RemoteDeviceConnection : BindableBase, INetworkConnection
    {
        private RemoteDevice arduino;
        private IStream connection;
        private bool isConnected;

        public Task Connect(string ip, int port)
        {
            return Task.Run(() =>
            {
                connection = new UsbSerial("VID_2341", "PID_0043");
                //I am using a constructor that accepts a device name or ID.
                arduino = new RemoteDevice(connection);

                //add a callback method (delegate) to be invoked when the device is ready, refer to the Events section for more info
                arduino.DeviceReady += Setup;

                //always begin your IStream
                connection.begin(57600, SerialConfig.SERIAL_8N1);
                connection.ConnectionLost += ConnectionOnConnectionLost;
                connection.ConnectionFailed += ConnectionOnConnectionFailed;
                connection.ConnectionEstablished += ConnectionOnConnectionEstablished;
            });
        }

        public void Disconnect()
        {
            connection.end();
        }

        public Task<string> SendAndReceiveData(string message, int pin)
        {
            //return Task.Run(() => "75");
            return Task.Run(() => arduino.analogRead("A"+pin).ToString());
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetValue(ref isConnected, value, true); }
        }

        private async void ConnectionOnConnectionEstablished()
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { IsConnected = true; });
        }

        private async void ConnectionOnConnectionFailed(string message)
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { IsConnected = false; });
        }

        private async void ConnectionOnConnectionLost(string message)
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { IsConnected = false; });
        }

        //treat this function like "setup()" in an Arduino sketch.
        public void Setup()
        {
            //set digital pin 13 to OUTPUT
            arduino.pinMode(13, PinMode.OUTPUT);

            //set analog pin A0 to ANALOG INPUT
            arduino.pinMode("A0", PinMode.ANALOG);
        }

        public void ChangePin(string pin, PinMode m)
        {
            arduino.pinMode(pin, m);
        }
    }
}