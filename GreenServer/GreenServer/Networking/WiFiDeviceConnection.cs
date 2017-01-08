using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using Slycoder.Portable.MVVM;
//Windows.Networking.Sockets.
namespace GreenServer.Networking
{
    public class WiFiDeviceConnection : BindableBase, INetworkConnection
    {
        public Sensor Sensor { get; }
        private RemoteDevice arduino;
        private IStream connection;
        private NetworkStatus networkStatus;
        private ThreadPoolTimer autoRetryTimer;

        public WiFiDeviceConnection(Sensor sensor)
        {
            Sensor = sensor;
            
        }

        public Task Connect(string ip, int port)
        {
            return Task.Run(() =>
            {
                connection = new NetworkSerial(new Windows.Networking.HostName(ip), (ushort)port);
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

        private async void AutoRetryTimerOnTick(ThreadPoolTimer timer)
        {
            if (NetworkStatus != NetworkStatus.Connected)
            {
                await Connect(Sensor.DeviceAddress, Sensor.SecondaryDeviceAddress);
            }
        }

        public void Disconnect()
        {
            connection.end();
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
            autoRetryTimer = ThreadPoolTimer.CreatePeriodicTimer(AutoRetryTimerOnTick, TimeSpan.FromSeconds(30));

            if (Sensor.Type == SensorReadingType.Humidity)
            {
                arduino.pinMode("A0", PinMode.INPUT);
            }
        }

        public Task<SensorValue> RetrieveValue(SensorReadingType type)
        {
            return Task.Run(() =>
            {
                switch (Sensor.Type)
                {
                    case SensorReadingType.Temperature:
                        return new SensorValue { Timestamp = DateTime.Now, Type = Sensor.Type, Value = 75 };
                    case SensorReadingType.Humidity:
                        return new SensorValue { Timestamp = DateTime.Now, Type = Sensor.Type, Value = arduino.analogRead("A0") };
                    case SensorReadingType.pH:
                        return new SensorValue { Timestamp = DateTime.Now, Type = Sensor.Type, Value = 75 };
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}