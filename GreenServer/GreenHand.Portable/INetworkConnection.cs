using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenHand.Portable
{
    public interface INetworkConnection
    {
        Task Connect(string ip, int port);
        void Disconnect();
        /// <summary>
        /// Sends a message, then waits for a response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        Task<string> SendAndReceiveData(string message, int pin);

        bool IsConnected { get; set; }
    }
}
