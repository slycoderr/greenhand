using Newtonsoft.Json;
using Slycoder.Portable.MVVM;

namespace GreenHand.Portable.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User : BindableBase
    {
        private string email;

        [JsonProperty]
        public string Email { get { return email; } set { SetValue(ref email, value); } }

        [JsonProperty]
        public int Id { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string Salt { get; set; }
    }
}