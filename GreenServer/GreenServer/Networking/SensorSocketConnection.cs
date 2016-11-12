using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ArraySegments;
using GreenHand.Portable;
using Slycoder.Portable.MVVM;

namespace GreenServer.Networking
{
    public class SensorSocketConnection : BindableBase, INetworkConnection, IDisposable
    {
        private Socket socket;
        private bool isConnected;

        public async Task Connect(string ip, int port)
        {
            if (socket == null)
            {
                socket = new Socket(SocketType.Stream, ProtocolType.IP) {ReceiveTimeout = 2000};
            }

            await socket.ConnectAsync(ip, port);
            IsConnected = true;
        }

        public void Disconnect()
        {
            socket.Dispose();
            socket = null;
            IsConnected = false;
        }

        public async Task<string> SendAndReceiveData(string message)
        {
            if (socket == null)
            {
                IsConnected = false;
                throw new Exception("Socket is null");
            }

            //IsConnected = socket.Connected; //update the connection status

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Empty message");
            }

            if (socket != null)
            {
                var response = new byte[5].AsArraySegment();


                Encoding.ASCII.GetBytes(message);
                await socket.SendAsync(Encoding.ASCII.GetBytes(message).AsArraySegment(), SocketFlags.None);
                await socket.ReceiveAsync(response, SocketFlags.None);

                //IsConnected = socket.Connected; //update the connection status, just in case

                return Encoding.ASCII.GetString(response.Array);
            }

            throw new Exception("No Response");
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetValue(ref isConnected, value); }
        }


        public void Dispose()
        {
            socket?.Dispose();
        }
    }
}
