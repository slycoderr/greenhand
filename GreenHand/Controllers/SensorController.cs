using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;
using Swashbuckle.Swagger.Annotations;

namespace GreenHand.Controllers
{
    [RoutePrefix("sensor"), Authorize]
    public class SensorController : ApiController
    {
        private readonly SensorApi api = new SensorApi();

        [Route("store/temp/{data}")]
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was stored.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> PostSensorData(double data)
        {
            try
            {
                await api.StoreSensorData(new SensorValue(){ ReadingType = SensorReadingType.Temperature, Timestamp = DateTime.Now, ReadResult = data});

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
            }
        }


        [Route("values")]
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "The data was retrived.", typeof(IEnumerable<SensorValue>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "An unknown error has occured.", Type = typeof(string))]
        public async Task<IHttpActionResult> GetSensorValues()
        {
            try
            {
                var results = await api.GetSensorValues();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
            }
        }
    }
}