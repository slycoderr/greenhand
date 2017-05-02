using System;
using Newtonsoft.Json;

namespace GreenHand.Portable.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SensorValue
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public int SensorId { get; set; }

        [JsonProperty]
        public int UserId { get; set; }

        [JsonProperty]
        public SensorReadingType ReadingType { get; set; }

        /// <summary>
        ///     The resulting value read from the sensor
        /// </summary>
        [JsonProperty]
        public double ReadResult { get; set; }

        [JsonProperty]
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        public Sensor Sensor { get; set; }
    }
}