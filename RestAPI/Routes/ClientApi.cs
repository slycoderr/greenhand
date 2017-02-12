using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestAPI.Routes
{
    class ClientApi
    {
        [RoutePrefix("client")]
        public class SensorController : ApiController
        {
            private readonly GreenHand.Server.Remote.Common.ClientApi.ClientApi api = new GreenHand.Server.Remote.Common.ClientApi.ClientApi();

            [Route("values")]
            //[Authorize]
            public async Task<IHttpActionResult> GetSensorValues()
            {
                try
                {
                    var results = await api.GetSensorValues();

                    Console.WriteLine("GetSensorValues successful");
                    return Ok(results);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetSensorValues failed");
                    Console.WriteLine(ex.Message);
                    //await Telemetry.Client.LogException(ex);
                    File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                    return Content(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
                }
            }
        }
    }
}
