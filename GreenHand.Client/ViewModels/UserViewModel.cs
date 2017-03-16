using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenHand.Portable.Models;
using Slycoder.Portable.MVVM;

namespace GreenHand.Client.ViewModels
{
    public class UserViewModel : BindableBase
    {
        public string LoginUsername { get { return loginUsername; } set { SetValue(ref loginUsername, value); } }
        public string LoginPassword { get { return loginPassword; } set { SetValue(ref loginPassword, value); } }

        private string loginUsername;
        private string loginPassword;

        public async Task Login()
        {
            if (await RestClient.Login(LoginUsername, LoginPassword))
            {
                LoginPassword = null;
                LoginUsername = null;
            }
        }
    }
}
