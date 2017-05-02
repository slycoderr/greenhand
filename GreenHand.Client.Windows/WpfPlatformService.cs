using System;
using System.IO.Ports;

namespace GreenHand.Client.Windows
{
    internal sealed class WpfPlatformService : IPlatformService
    {
        public int ExtractSensorId(string portName)
        {
            try
            {
                using (var port = new SerialPort(portName, 9600) {NewLine = "\n", ReadTimeout = 40000 })
                {
                    port.Open();

                    port.WriteLine("GetId");

                    var returnData = port.ReadLine();
                    int id;

                    return int.TryParse(returnData, out id) ? id : -1;
                }
            }

            catch (Exception)
            {
                return -1;
            }
        }

        public bool AssignSensorApiKey(string portName, string apiKey)
        {
            try
            {
                using (var port = new SerialPort(portName, 9600) {NewLine = "\n", ReadTimeout = 40000 })
                {
                    port.Open();

                    port.WriteLine("SetApiKey");
                    port.WriteLine(apiKey);

                    return port.ReadLine() == "Ok";
                }
            }

            catch (Exception)
            {
                return false;
            }
        }

        public string RetrieveSensorPortName()
        {
            try
            {
                foreach (var portName in SerialPort.GetPortNames())
                {
                    try
                    {
                        using (var port = new SerialPort(portName, 9600) {NewLine = "\n", ReadTimeout = 40000})
                        {
                            port.Open();

                            port.WriteLine("Hello");

                            if (port.ReadLine() == "asuh dude")
                            {
                                return portName;
                            }
                        }
                    }

                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }
}