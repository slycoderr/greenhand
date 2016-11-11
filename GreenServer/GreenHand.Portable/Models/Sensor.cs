using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Slycoder.Portable.MVVM;
using Slycoder.Portable.Networking;

namespace GreenHand.Portable.Models
{

    public class Sensor : BindableBase
    {
        public ObservableCollection<SensorValue> Readings { get; set; }
        public string Name { get; }
        public string IpAddress { get; }
        public string Port { get; }
        public string Id { get; set; }
        public string Type { get; set; }

        public async Task ReadValue()
        {
            //HttpWebRequest http = HttpWebRequest.CreateHttp($"http://{IpAddress}:{Port}/");

            //http.BeginGetResponse()

  //          WebRequest request = WebRequest.Create(
  //"http://www.contoso.com/default.html");
  //          // If required by the server, set the credentials.
  //          //request.Credentials = NetworkCredential
  //          // Get the response.
  //          WebResponse response = await request.BeginGetRequestStream();
  //          // Display the status.
  //          Debug.WriteLine(((HttpWebResponse)response).StatusDescription);
  //          // Get the stream containing content returned by the server.
  //          Stream dataStream = response.GetResponseStream();
  //          // Open the stream using a StreamReader for easy access.
  //          using (StreamReader reader = new StreamReader(dataStream))
  //          {
  //              string responseFromServer = reader.ReadToEnd();
  //              // Display the content.
  //              Debug.WriteLine(responseFromServer);
  //              // Clean up the streams and the response.
  //              var message = await reader.ReadLineAsync();
  //          }
  //          response.Close();

            
        }
    }
}
