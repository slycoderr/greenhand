using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Client
{
    public interface IPlatformService
    {
        string RetrieveSensorPortName();
        int ExtractSensorId(string portName);
        bool AssignSensorApiKey(string portName, string apiKey);
    }
}
