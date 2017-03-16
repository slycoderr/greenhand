using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable.Models;
using Newtonsoft.Json;
using Environment = GreenHand.Portable.Models.Environment;

namespace GreenHand.Client
{

    //ServicePointManager.ServerCertificateValidationCallback +=(sender, certificate, chain, errors) => certificate.GetCertHashString() == "35D197D73B546C232E4AB4BE7D8CB502000116A7";
    public class RestClient
    {
        public static bool IsAuthorized => !string.IsNullOrEmpty(authCode);

        private static string authCode;

        private static HttpClientHandler HttpOptions { get; } = new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Manual };
        //private static readonly string ServiceUrl = "https://greenhandrest.cloudapp.net:443/";
        private static readonly string ServiceUrl = "https://greenhand.azurewebsites.net/";

        private static readonly string sensorValuesUrl = ServiceUrl+ "sensor/values/";
        private static readonly string getEnvironmentsUrl = ServiceUrl+ "sensor/environments/";
        private static readonly string addTempDataUrl = ServiceUrl+ "getdata/";
        private static readonly string loginUrl = ServiceUrl+ "user/login/";
        private static readonly string registerUrl = ServiceUrl+ "user/register/";

        #region GenericMethods
        private static async Task<T> Get<T>(string address)
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

        public static async Task Put(string address, object data = null)
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


        public static async Task<T> Post<T>(string address, object data = null, bool needsAuthorization = true)
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


        public static async Task Delete(string address)
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

        private static void HandleError(HttpResponseMessage response)
        {
            throw new WebException(response.ReasonPhrase);
        }

        #endregion

        public static async Task<bool> Login(string email, string password)
        {
            using (var client = new HttpClient(HttpOptions, false))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

                try
                {
                    authCode = null;

                    var tokenResponse = await client.PostAsync($"{loginUrl}", new StringContent(JsonConvert.SerializeObject(new User {Email = email, Password = password}), Encoding.UTF8, "application/json"));
                    var responseString = await tokenResponse.Content.ReadAsStringAsync();

                    if (tokenResponse.IsSuccessStatusCode && !string.IsNullOrEmpty(responseString))
                    {
                        authCode = responseString.Trim('\"');
                    }

                    else
                    {
                        HandleError(tokenResponse);
                    }

                    return IsAuthorized;
                }

                catch (WebException)
                {
                    return false;
                }

                catch (TaskCanceledException)
                {
                    return false;
                }

                catch (HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task CreateUser(string email, string password)
        {
            await Post<User>($"{registerUrl}", new User() { Email = email, Password = password }, false);
        }

        public async Task AddData(SensorValue value)
        {
            await Post<SensorValue>($"{registerUrl}", value);
        }

        public async Task<IEnumerable<SensorValue>> GetSensorValues()
        {
            return await Get <IEnumerable<SensorValue>>($"{sensorValuesUrl}");
        }

        public async Task<IEnumerable<Environment>> GetEnvironments()
        {
            return await Get<IEnumerable<Environment>>($"{getEnvironmentsUrl}");
        }
    }
}
