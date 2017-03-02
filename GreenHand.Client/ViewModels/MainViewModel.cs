using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slycoder.Portable.MVVM;
using Environment = GreenHand.Portable.Models.Environment;

namespace GreenHand.Client.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<GreenHand.Portable.Models.Environment> Environments { get; set; } = new ObservableCollection<Environment>();

        public MainViewModel()
        {
            Load();
        }

        public void Load()
        {
            if (Environments.Count == 0)
            {
                Environments.Add(new Environment(){Name = "My Environment"});
            }
        }
    }
}
