using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable.Models;
using Newtonsoft.Json;

namespace GreenHand.Client
{

    //ServicePointManager.ServerCertificateValidationCallback +=(sender, certificate, chain, errors) => certificate.GetCertHashString() == "35D197D73B546C232E4AB4BE7D8CB502000116A7";
    public class RestClient
    {
        public bool IsAuthorized => !string.IsNullOrEmpty(authCode);

        private string authCode = null;

        private HttpClientHandler HttpOptions { get; } = new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Manual };
        //private static readonly string ServiceUrl = "https://greenhandrest.cloudapp.net:443/";
        private static readonly string ServiceUrl = "http://localhost:55758/";

        private static readonly string sensorValuesUrl = ServiceUrl+ "sensor/values/";
        private static readonly string addTempDataUrl = ServiceUrl+ "getdata/";
        private static readonly string loginUrl = ServiceUrl+ "user/login/";
        private static readonly string registerUrl = ServiceUrl+ "user/register/";
		
		#region GenericMethods
		private async Task<T> Get<T>(string address)
        {
            using (var client = new HttpClient(HttpOptions, false))
            {
                if (IsAuthorized)
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {authCode}");
                }

                var response = await client.GetAsync(address);
                string responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    HandleError(response);
                }

                return JsonConvert.DeserializeObject<T>(responseText);
            }
        }

        public async Task Put(string address, object data = null)
        {
            using (var client = new HttpClient(HttpOptions, false))
            {
                if (IsAuthorized)
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {authCode}");
                }

                var response = await client.PutAsync(address, data != null ? new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") : new StringContent("", Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    HandleError(response);
                }
            }
        }


        public async Task<T> Post<T>(string address, object data = null, bool needsAuthorization = true)
        {
            using (var client = new HttpClient(HttpOptions, false))
            {
                if (needsAuthorization && IsAuthorized)
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {authCode}");
                }

                var response = await client.PostAsync(address, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                string responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    HandleError(response);
                }

                return JsonConvert.DeserializeObject<T>(responseText);
            }
        }


        public async Task Delete(string address)
        {
            using (var client = new HttpClient(HttpOptions, false))
            {
                if (IsAuthorized)
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {authCode}");
                }

                var response = await client.DeleteAsync(address);

                if (!response.IsSuccessStatusCode)
                {
                    HandleError(response);
                }
            }
        }

        private void HandleError(HttpResponseMessage response)
        {
            throw new WebException(response.ReasonPhrase);
        }

        #endregion

        public async Task Login(string email, string password)
        {
            using (var client = new HttpClient(HttpOptions, false))
            {
               // var form = new Dictionary<string, string>
               //{
               //    {"grant_type", "password"},
               //    {"username", email},
               //    {"password", password}
               //};

                //var tokenResponse = await client.PostAsync($"{loginUrl}{email}/{password}", new FormUrlEncodedContent(form));
                var tokenResponse = await client.PostAsync($"{loginUrl}{email}/{password}", new StringContent("", Encoding.UTF8, "application/json"));
                var responseString = await tokenResponse.Content.ReadAsStringAsync();
                //var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                if (tokenResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(responseString))
                //if (tokenResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(response["Token"]))
                {
                    //authCode = response["Token"];
                    authCode = responseString;
                }

                else
                {
                    HandleError(tokenResponse);
                }
            }
        }

        public async Task<User> CreateUser(string email, string password)
        {
            return await Post<User>($"{registerUrl}{email}/{password}", null, false);
        }

        public async Task AddData(SensorValue value)
        {
            await Post<SensorValue>($"{registerUrl}", value);
        }

        public async Task<IEnumerable<SensorValue>> GetSensorValues(string email, string password)
        {
            return await Get <IEnumerable<SensorValue>>($"{sensorValuesUrl}");
        }
    }
}
