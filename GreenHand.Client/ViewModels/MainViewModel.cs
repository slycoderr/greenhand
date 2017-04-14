using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Slycoder.Portable.MVVM;
using Environment = GreenHand.Portable.Models.Environment;

namespace GreenHand.Client.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Commands

        public RelayCommand LoadCommand => new RelayCommand(async () => await Load());

        #endregion

        //Collections
        public ObservableCollection<Environment> Environments { get; private set; } = new ObservableCollection<Environment>();

        //public members

        public UserViewModel UserViewModel { get; } = new UserViewModel();

        //Private members
        internal static readonly RestClient RestClient = new RestClient();

        public MainViewModel()
        {
            UserViewModel.OnLoginSuccessful += OnLoginSuccessful;            
        }

        private async void OnLoginSuccessful(object sender, EventArgs eventArgs)
        {
            await Load();
        }

        public async Task Load()
        {
            Environments = new ObservableCollection<Environment>((await RestClient.GetEnvironments()).environments ?? new Environment[0]);
        }
    }
}