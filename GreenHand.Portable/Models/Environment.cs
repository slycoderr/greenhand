using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GreenHand.Portable.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Environment
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public virtual ObservableCollection<Sensor> Sensors { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}