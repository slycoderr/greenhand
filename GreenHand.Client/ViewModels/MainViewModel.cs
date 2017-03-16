using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GreenHand.Portable.Models;
using Slycoder.Portable.MVVM;

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

        //Private members
        private readonly RestClient restClient = new RestClient();


        public async Task Load()
        {
            Environments = new ObservableCollection<Environment>(await restClient.GetEnvironments());
        }
    }
}