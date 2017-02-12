

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage.Streams;
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

            else if (Sensor.Type == SensorReadingType.Temperature)
            {
                SetupTempSensor();
            }
        }

        public Task<SensorValue> RetrieveValue(SensorReadingType type)
        {
            return Task.Run(() =>
            {
                switch (Sensor.Type)
                {
                    case SensorReadingType.Temperature:
                        return new SensorValue { Timestamp = DateTime.Now, ReadingType = Sensor.Type, ReadResult = 75 };
                    case SensorReadingType.Humidity:
                        return new SensorValue { Timestamp = DateTime.Now, ReadingType = Sensor.Type, ReadResult = arduino.analogRead("A0") };
                    case SensorReadingType.pH:
                        return new SensorValue { Timestamp = DateTime.Now, ReadingType = Sensor.Type, ReadResult = 75 };
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        private const uint MCP9808_I2CADDR_DEFAULT = 0x18;
 private const uint MCP9808_REG_CONFIG       =      0x01;

private const ushort MCP9808_REG_CONFIG_SHUTDOWN  =  0x0100;
private const ushort MCP9808_REG_CONFIG_CRITLOCKED = 0x0080;
private const ushort MCP9808_REG_CONFIG_WINLOCKED  = 0x0040;
private const ushort MCP9808_REG_CONFIG_INTCLR   =   0x0020;
private const ushort MCP9808_REG_CONFIG_ALERTSTAT   =0x0010;
private const ushort MCP9808_REG_CONFIG_ALERTCTRL =  0x0008;
private const ushort MCP9808_REG_CONFIG_ALERTSEL   = 0x0004;
private const ushort MCP9808_REG_CONFIG_ALERTPOL =   0x0002;
private const ushort MCP9808_REG_CONFIG_ALERTMODE  = 0x0001;

private const ushort MCP9808_REG_UPPER_TEMP    =     0x02;
private const ushort MCP9808_REG_LOWER_TEMP   =      0x03;
private const ushort MCP9808_REG_CRIT_TEMP     =     0x04;
private const ushort MCP9808_REG_AMBIENT_TEMP   =    0x05;
private const ushort MCP9808_REG_MANUF_ID      =     0x06;
private const ushort MCP9808_REG_DEVICE_ID     =     0x07;

        private void SetupTempSensor()
        {
            arduino.I2c.enable();
            arduino.I2c.I2cReplyEvent += TempSensorI2cReply;

            arduino.I2c.beginTransmission(0x18);

            arduino.I2c.write(0x06);

            arduino.I2c.requestFrom(0x18, 4);


            arduino.I2c.write(0x07);


            arduino.I2c.requestFrom(0x18, 4);

            arduino.I2c.endTransmission();
        }

        private void TempSensorI2cReply(byte address, byte reg, DataReader response)
        {
            if (address == 0x18) //temp sensor
            {
                if (reg == 0x06 && response.ReadInt32() != 0x0054) //check manuaftuerer id
                {
                    //bad
                }

                else if (reg == 0x07 && response.ReadInt32() != 0x0400) //check device id
                {
                    //bad
                }

                else if (reg == 0x01)
                {
                    var register = response.ReadUInt16();
                    var shutdown = register ^ 0x0100;
                    //arduino.I2c.write(shutdown);
                }
            }
        }

        private void WakeupTempSensor(ushort sw_ID)
        {
            arduino.I2c.beginTransmission(0x18);

            ushort conf_shutdown;
            arduino.I2c.requestFrom(0x01, 16);
            arduino.I2c.write(0x01);

            
            //if (sw_ID == 1)
            //{
            //    conf_shutdown = conf_register | MCP9808_REG_CONFIG_SHUTDOWN;
            //    write16(MCP9808_REG_CONFIG, conf_shutdown);
            //}
            //if (sw_ID == 0)
            //{
            //    conf_shutdown = conf_register ^ MCP9808_REG_CONFIG_SHUTDOWN;
            //    write16(MCP9808_REG_CONFIG, conf_shutdown);
            //}

            arduino.I2c.endTransmission();
        }
        //Celcius
        private double ReadTempSensor(bool inCelcius)
        {
            throw new NotImplementedException();
        }
    }
}