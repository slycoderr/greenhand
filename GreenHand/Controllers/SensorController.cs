using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;
using GreenHand.Utility;
using Swashbuckle.Swagger.Annotations;

namespace GreenHand.Controllers
{
    [RoutePrefix("sensor")]
    public class SensorController : ApiController
    {
        private readonly SensorApi api = new SensorApi();

        [Route("store/{key}/{sensorId}/{type}/{data}")]
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was stored.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> PostSensorData(string key, int sensorId, string type, double data)
        {
            await api.StoreSensorData(key, sensorId, type, data);
            return Ok();
        }


        [Route("values")]
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was retrived.", typeof(IEnumerable<SensorValue>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> GetSensorValues()
        {
            string userId = SecurityHelpers.ValidateToken(Request.Headers);
            return Ok(await api.GetSensorValues());
        }
    }
}