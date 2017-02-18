using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slycoder.Portable.MVVM;

namespace GreenHand.Portable.Models
{
    public class User : BindableBase
    {
        private string email;

        public string Email { get { return email; } set { SetValue(ref email, value); } }

        public int Id { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
