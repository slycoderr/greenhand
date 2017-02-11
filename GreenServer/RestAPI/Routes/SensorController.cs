﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Portable;
using GreenHand.Portable.Models;
using GreenHand.Server.Remote.Common.SensorApi;

namespace RestAPI.Routes
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
                //await Telemetry.Client.LogException(ex);
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                return Content(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
            }
        }

        [Route("StoreData/temp/{data}")]
        //[Authorize]
        public async Task<IHttpActionResult> PostSensorData(double data)
        {
            try
            {
                await api.StoreSensorData(new SensorValue(){ ReadingType = SensorReadingType.Temperature, Timestamp = DateTime.Now, ReadResult = data});

                Console.WriteLine("store data successful");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("store data failed");
                Console.WriteLine(ex.Message);
                //await Telemetry.Client.LogException(ex);
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                return Content(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
            }
        }
    }
}