using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;

namespace GreenHand.Server.Remote.Routes
{
    class ClientApi
    {
        [RoutePrefix("client")]
        public class SensorController : ApiController
        {
            private readonly ClientApi api = new ClientApi();

            [Route("values")]
            //[Authorize]
            public async Task<IHttpActionResult> GetSensorValues()
            {
                try
                {
                    //var results = await api.GetSensorValues();

                    Console.WriteLine("GetSensorValues successful");
                    return Ok();
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
