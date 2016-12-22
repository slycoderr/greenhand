using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Portable
{
    public enum NetworkStatus
    {
        NotConnected,
        Connected,
        ConnectionFailed,
        ConnectionLost
    }

    public enum SensorReadingType
    {
        Temperature,
        Humidity,
        pH

    }
}
