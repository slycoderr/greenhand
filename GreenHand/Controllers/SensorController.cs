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
using Environment = GreenHand.Portable.Models.Environment;

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


        [Route("values/{span}")]
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was retrived.", typeof(IEnumerable<SensorValue>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> GetSensorValues(int days)
        {
            var userId = SecurityHelpers.ValidateToken(Request.Headers);
            return Ok(await api.GetSensorValues(userId, days));
        }

        [Route("values/latest/{sensorId}")]
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was retrived.", typeof(SensorValue))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> GetLatestValue(int sensorId)
        {
            var userId = SecurityHelpers.ValidateToken(Request.Headers);
            return Ok(await api.GetLastReading(userId, sensorId));
        }

        [Route("register/{sensorId}/{environmentId}")]
        [SwaggerOperation("Put")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was processed.", typeof(string))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> PutUpdateSensorUser(int sensorId, int environmentId)
        {
            var userId = SecurityHelpers.ValidateToken(Request.Headers);
            var apiKey = await api.RegisterSensor(userId, sensorId, environmentId);


            return apiKey.StartsWith("Error") ? (IHttpActionResult)BadRequest(apiKey) : Ok(apiKey); 
        }

        [Route("environments")]
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was retrived.", typeof(IEnumerable<Environment>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> GetUserEnvironments()
        {
            var userId = SecurityHelpers.ValidateToken(Request.Headers);
            return Ok(await api.GetUserEnvironments(userId));
        }
    }
}