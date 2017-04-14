using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Slycoder.Portable.MVVM;

namespace GreenHand.Portable.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Environment : BindableBase
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public virtual ObservableCollection<Sensor> Sensors { get; set; }

        [JsonProperty]
        public int UserId { get; set; }
    }
}