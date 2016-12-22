using System.Threading.Tasks;
using GreenHand.Portable.Models;

namespace GreenHand.Portable
{
    public interface INetworkConnection
    {
        NetworkStatus NetworkStatus { get; set; }
        Task Connect(string ip, int port);
        void Disconnect();

        /// <summary>
        ///     Get a reading from the sesnor
        /// </summary>
        /// <returns>The value from the sensor</returns>
        Task<SensorValue> RetrieveValue(SensorReadingType type);
    }
}