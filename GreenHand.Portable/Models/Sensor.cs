using Slycoder.Portable.MVVM;

namespace GreenHand.Portable.Models
{
    public class Sensor : BindableBase
    {
        public string Name { get { return name; } set { SetValue(ref name, value); } }

        public int Id { get; set; }

        public int UserId { get; set; }

        private string name;
    }
}