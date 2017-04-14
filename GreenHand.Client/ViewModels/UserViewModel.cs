using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GreenHand.Portable.Models;
using Slycoder.Portable.MVVM;

namespace GreenHand.Client.ViewModels
{
    public class UserViewModel : BindableBase
    {
        public RelayCommand LoginCommand => new RelayCommand(async()=> await Login());

        public EventHandler OnLoginSuccessful;

        public string LoginUsername { get { return loginUsername; } set { SetValue(ref loginUsername, value); } }
        public string LoginPassword { get { return loginPassword; } set { SetValue(ref loginPassword, value); } }
        public bool IsUserAuthenticated { get { return isUserAuthenticated; } set { SetValue(ref isUserAuthenticated, value); } }
        public bool IsLoggingIn { get { return isUserAuthenticated; } set { SetValue(ref isUserAuthenticated, value); } }

        private string loginUsername = "slycoder";
        private string loginPassword = "M@gic345";
        private bool isUserAuthenticated;

        public async Task Login()
        {
            IsLoggingIn = true;
            IsUserAuthenticated = false;
            var result = await MainViewModel.RestClient.Login(LoginUsername, LoginPassword);

            if (result.StatusIsGood)
            {
                LoginPassword = null;
                LoginUsername = null;
                IsUserAuthenticated = true;
                OnLoginSuccessful?.Invoke(this, EventArgs.Empty);
            }

            IsLoggingIn = false;
        }
    }
}
