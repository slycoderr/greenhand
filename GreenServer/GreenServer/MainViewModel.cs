using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;
using GreenHand.Portable.Models;

namespace GreenServer
{
    public class MainViewModel
    {
        public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

        public TimeSpan ValueRefeshRate { get; set; } = new TimeSpan(0, 1, 0);

        private ThreadPoolTimer valuePollTimer;

        public MainViewModel()
        {
            valuePollTimer = ThreadPoolTimer.CreatePeriodicTimer(valuePollTimerOnTick, ValueRefeshRate);
        }

        private async void valuePollTimerOnTick(ThreadPoolTimer timer)
        {
            //get values from sensors
            foreach (var sensor in Sensors)
            {
                sensor.ReadValue();
            }

            await Window.Current.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { });
        }

        public void AddSenor()
        {
            
        }

        public void RemoveSensor()
        {
            
        }
    }
}
