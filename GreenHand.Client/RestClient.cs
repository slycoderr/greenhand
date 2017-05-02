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
using Newtonsoft.Json.Linq;
using Environment = GreenHand.Portable.Models.Environment;

namespace GreenHand.Client
{
    internal enum WebActionStatus
    {
        Success,
        Unauthorized,
        RequestFailed,
        ServerError
    }

    internal class WebActionResult
    {
        public WebActionResult(WebActionStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public WebActionResult(WebActionStatus status)
        {
            Status = status;
        }

        public WebActionResult(WebActionStatus status, string message, long? contentLength)
        {
            Status = status;
            Message = message;
            ContentLength = contentLength;
        }

        public bool StatusIsGood => Status == WebActionStatus.Success;
        public WebActionStatus Status { get; }
        public string Message { get; }
        public long? ContentLength { get; }
    }

    //ServicePointManager.ServerCertificateValidationCallback +=(sender, certificate, chain, errors) => certificate.GetCertHashString() == "35D197D73B546C232E4AB4BE7D8CB502000116A7";
    internal class RestClient
    {
        private readonly HttpClient httpClient = new HttpClient(new HttpClientHandler(){AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip})
        {
            BaseAddress = new Uri("https://greenhand.azurewebsites.net/")
            //BaseAddress = new Uri("http://localhost:55758/")
        };

        public RestClient()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            //httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
        }

        public EventHandler OnUserCredentialsInvalid;

        private static readonly string sensorValuesUrl = "sensor/values/";
        private static readonly string getEnvironmentsUrl = "sensor/environments/";
        private static readonly string addTempDataUrl = "getdata/";
        private static readonly string loginUrl = "user/login/";
        private static readonly string registerUrl =  "user/register/";

        #region GenericMethods
        private async Task<(WebActionResult webAction, T returnData)> Get<T>(string address)
        {
            try
            {
                var response = await httpClient.GetAsync(address);
                var webResponse = HandleResponse(response);

                return webResponse.StatusIsGood ? (webResponse, JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())) : (webResponse, default(T));
            }

            catch (TaskCanceledException)
            {
                return (new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out."), default(T));
            }

            catch (HttpRequestException)
            {
                return (new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working."), default(T));
            }
        }

        public async Task<WebActionResult> Put(string address, object data = null)
        {
            try
            {
                var response = await httpClient.PutAsync(address, data != null ? new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") : new StringContent("", Encoding.UTF8, "application/json"));
                return HandleResponse(response);
            }

            catch (TaskCanceledException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out.");
            }

            catch (HttpRequestException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working.");
            }
        }

        public async Task<(WebActionResult webActionResult, T returnData)> Put<T>(string address, object data = null)
        {
            try
            {
                var response = await httpClient.PutAsync(address, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                var webResponse = HandleResponse(response);

                return webResponse.StatusIsGood ? (webResponse, JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())) : (webResponse, default(T));
            }

            catch (TaskCanceledException)
            {
                return (new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out."), default(T));
            }

            catch (HttpRequestException)
            {
                return (new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working."), default(T));
            }
        }

        public async Task<WebActionResult> Post(string address, object data = null, bool needsAuthorization = true)
        {
            try
            {
                var response = await httpClient.PostAsync(address, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                return HandleResponse(response);
            }

            catch (TaskCanceledException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out.");
            }

            catch (HttpRequestException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working.");
            }
        }

        public async Task<(WebActionResult webAction, T returnData)> Post<T>(string address, object data = null, bool needsAuthorization = true)
        {
            try
            {
                var response = await httpClient.PostAsync(address, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
                var webResponse = HandleResponse(response);

                return webResponse.StatusIsGood ? (webResponse, JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync())) : (webResponse, default(T));
            }

            catch (TaskCanceledException)
            {
                return (new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out."), default(T));
            }

            catch (HttpRequestException)
            {
                return (new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working."), default(T));
            }
        }


        public async Task<WebActionResult> Delete(string address)
        {
            try
            {
                return HandleResponse(await httpClient.DeleteAsync(address));
            }

            catch (TaskCanceledException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out.");
            }

            catch (HttpRequestException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working.");
            }
        }

        private WebActionResult HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    httpClient.DefaultRequestHeaders.Authorization = null;
                    OnUserCredentialsInvalid?.Invoke(this, EventArgs.Empty);
                    return new WebActionResult(WebActionStatus.Unauthorized, "Your session has timed out.", response.Content.Headers.ContentLength);
                }

                //400's client error, 500's server error. 300's redirect(shouldn't have redirects)
                return new WebActionResult(WebActionStatus.ServerError, "Unable to process request.", response.Content.Headers.ContentLength);
            }

            return new WebActionResult(WebActionStatus.Success, "Success", response.Content.Headers.ContentLength);
        }

        #endregion

        public async Task<WebActionResult> Login(string email, string password)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = null;

                var tokenResponse = await httpClient.PostAsync($"{loginUrl}", new StringContent(JsonConvert.SerializeObject(new User {Email = email, Password = password}), Encoding.UTF8, "application/json"));
                var webResponse = HandleResponse(tokenResponse);
                var responseString = await tokenResponse.Content.ReadAsStringAsync();

                if (webResponse.StatusIsGood && !string.IsNullOrEmpty(responseString))
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", responseString.Trim('\"'));
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {responseString.Trim('\"')}");
                }

                return webResponse;
            }

            catch (TaskCanceledException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. The request timed out.");
            }

            catch (HttpRequestException)
            {
                return new WebActionResult(WebActionStatus.RequestFailed, "Unable to connect. Please ensure your internet connection is working.");
            }
        }

        public async Task<WebActionResult> CreateUser(string email, string password)
        {
            return await Post($"{registerUrl}", new User() { Email = email, Password = password }, false);
        }

        public async Task<WebActionResult> AddData(SensorValue value)
        {
            return await Post($"{registerUrl}", value);
        }

        public async Task<(WebActionResult webActionResult, IEnumerable<SensorValue> sensorValues)> GetSensorValues()
        {
            return await Get <IEnumerable<SensorValue>>($"{sensorValuesUrl}");
        }

        public async Task<(WebActionResult webActionResult, IEnumerable<Environment> environments)> GetEnvironments()
        {
            return await Get<IEnumerable<Environment>>($"{getEnvironmentsUrl}");
        }

        public async Task<(WebActionResult webActionResult, SensorValue value)> GetLatestSensorValue(Sensor sensor)
        {
            return await Get<SensorValue>($"sensor/latest/{sensor.Id}");
        }

        public async Task<(WebActionResult webActionResult, string returnData)> RegisterSensor(int sensorId, Environment environment)
        {
            return await Put<string>($"sensor/register/{sensorId}/{environment.Id}");
        }
    }
}
