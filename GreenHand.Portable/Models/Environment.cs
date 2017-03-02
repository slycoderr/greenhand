using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GreenHand.Portable.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Environment
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public virtual ObservableCollection<Sensor> Sensors { get; set; }
    }
}