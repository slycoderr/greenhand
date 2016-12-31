using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;

namespace GreenHand.Server.Remote.Routes
{
    [RoutePrefix("GreenHand")]
    public class SensorController : ApiController
    {
        private readonly SensorApi api = new SensorApi();

        [Route("StoreData")]
        //[Authorize]
        public async Task<IHttpActionResult> PostSensorData(SensorValue value)
        {
            try
            {
                await api.StoreSensorData(value);

                Console.WriteLine("store data successful");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("store data failed");
                Console.WriteLine(ex.Message);
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                return Content(HttpStatusCode.BadRequest, new HttpError(ex.Message));
            }
        }
    }
}