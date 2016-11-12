﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Command;
using GreenHand.Portable.Models;
using GreenServer.Networking;
using Slycoder.Portable.MVVM;

namespace GreenServer
{
    public class MainViewModel : BindableBase
    {
        public Sensor SelectedSensor
        {
            get { return selectedSensor; }
            set { SetValue(ref selectedSensor, value); }
        }

        public ObservableCollection<string> Log { get; } = new ObservableCollection<string>();

        public RelayCommand AddSensorCommand => new RelayCommand(AddSenor);

        public RelayCommand SensorConnectCommand => new RelayCommand(async () =>
        {
            try
            {
                await SelectedSensor.Network.Connect(SelectedSensor.IpAddress, SelectedSensor.Port);
            }
            catch (Exception ex)
            {
                AppendLog($"connected to {SelectedSensor?.Name} with exception:{ex}");
            }
        });

        public RelayCommand SensorDisconnectCommand => new RelayCommand(() => SelectedSensor?.Network?.Disconnect());
        public RelayCommand RemoveSensorCommand => new RelayCommand(RemoveSensor);

        public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

        public TimeSpan ValueRefeshRate { get; set; } = new TimeSpan(0, 0, 30);

        private ThreadPoolTimer valuePollTimer;
        private Sensor selectedSensor;

        public MainViewModel()
        {
            StartPollingValues();
        }

        private async void valuePollTimerOnTick(ThreadPoolTimer timer)
        {
            AppendLog("Starting Read");

            //get values from sensors
            foreach (var sensor in Sensors.Where(s => s.Network.IsConnected))
            {
                try
                {
                    await sensor.ReadValue();
                    AppendLog($"Read from {sensor.Name} without any exceptions");
                }
                catch (Exception ex)
                {
                    AppendLog($"Read from {sensor.Name} with exception:{ex}");
                }

            }
            AppendLog("End Read");
            //await Window.Current.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { });
        }

        private void AddSenor()
        {
            var s = new Sensor(new SensorSocketConnection());
            Sensors.Add(s);
            SelectedSensor = s;
        }

        private async void AppendLog(string line)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Log.Add(line); });
        }

        private void RemoveSensor()
        {
            Sensors.Remove(SelectedSensor);
        }

        private void StartPollingValues()
        {
            valuePollTimer = ThreadPoolTimer.CreatePeriodicTimer(valuePollTimerOnTick, ValueRefeshRate);
        }

        private void StopPollingValues()
        {
            valuePollTimer?.Cancel();
        }
    }
}
