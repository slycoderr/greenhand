using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using Slycoder.Portable.MVVM;

namespace GreenServer.Networking
{
    public class BluetoothDeviceConnection : BindableBase, INetworkConnection
    {
        private RemoteDevice arduino;
        private IStream connection;
        private NetworkStatus networkStatus;

        public Task Connect(string deviceAddress, int port)
        {
            return Task.Run(() =>
            {
                connection = new BluetoothSerial(deviceAddress);
                //I am using a constructor that accepts a device name or ID.
                arduino = new RemoteDevice(connection);

                //add a callback method (delegate) to be invoked when the device is ready, refer to the Events section for more info
                arduino.DeviceReady += Setup;

                //always begin your IStream
                //connection.begin(57600, SerialConfig.SERIAL_8N1);
                connection.ConnectionLost += ConnectionOnConnectionLost;
                connection.ConnectionFailed += ConnectionOnConnectionFailed;
                connection.ConnectionEstablished += ConnectionOnConnectionEstablished;

                connection.begin(115200, SerialConfig.SERIAL_8N1);
            });
        }

        public void Disconnect()
        {
            connection.end();
        }

        public Task<string> SendAndReceiveData(string message, int pin)
        {
            return Task.Run(() => "75");
            //return Task.Run(() => arduino.analogRead("A"+pin).ToString());
        }

        private async void ConnectionOnConnectionEstablished()
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { NetworkStatus = NetworkStatus.Connected; });
        }

        private async void ConnectionOnConnectionFailed(string message)
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { NetworkStatus = NetworkStatus.ConnectionFailed; });
        }

        private async void ConnectionOnConnectionLost(string message)
        {
            await
                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => { NetworkStatus = NetworkStatus.ConnectionLost; });
        }

        public NetworkStatus NetworkStatus { get { return networkStatus; } set { SetValue(ref networkStatus, value); } }

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

        public Task<SensorValue> RetrieveValue(SensorReadingType type)
        {
            throw new NotImplementedException();
        }
    }
}