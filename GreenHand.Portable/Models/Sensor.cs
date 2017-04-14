using Newtonsoft.Json;
using Slycoder.Portable.MVVM;

namespace GreenHand.Portable.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Sensor : BindableBase
    {
        [JsonProperty]
        public string Name { get { return name; } set { SetValue(ref name, value); } }
        [JsonProperty]
        public int Id { get; set; }
        [JsonProperty]
        public int UserId { get; set; }
        [JsonProperty]
        public int EnvironmentId { get; set; }


        private string name;
    }
}