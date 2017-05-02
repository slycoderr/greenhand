using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GreenHand.Portable.Models;
using Slycoder.Portable.MVVM;
using Environment = GreenHand.Portable.Models.Environment;

namespace GreenHand.Client.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Commands

        public RelayCommand LoadCommand => new RelayCommand(async () => await Load());
        public RelayCommand<Environment> RegisterDeviceCommand => new RelayCommand<Environment>(async e => await RegisterDevice(e));

        #endregion

        //Collections
        public ObservableCollection<Environment> Environments { get { return environments; } private set { SetValue(ref environments, value); } }
        public ObservableCollection<SensorValue> LatestEnvironmentValues { get { return latestEnvironmentValues; } private set { SetValue(ref latestEnvironmentValues, value); } }

        //public members
        public Environment SelectedEnvironment { get { return selectedEnvironment; } set { SetValue(ref selectedEnvironment, value); } }

        public UserViewModel UserViewModel { get; } = new UserViewModel();

        public IPlatformService PlatformService { get; set; }

        //Private members
        internal static readonly RestClient RestClient = new RestClient();
        private ObservableCollection<Environment> environments = new ObservableCollection<Environment>();
        private Environment selectedEnvironment;
        private ObservableCollection<SensorValue> latestEnvironmentValues = new ObservableCollection<SensorValue>();

        public MainViewModel()
        {
            UserViewModel.OnLoginSuccessful += OnLoginSuccessful;
            PropertyChanged += MainViewModel_PropertyChanged;
        }

        private async void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedEnvironment))
            {
                await LoadEnvironment(SelectedEnvironment);
            }
        }

        private async void OnLoginSuccessful(object sender, EventArgs eventArgs)
        {
            await Load();
        }

        public async Task Load()
        {
            Environments = new ObservableCollection<Environment>((await RestClient.GetEnvironments()).environments ?? new Environment[0]);

            SelectedEnvironment = Environments?.FirstOrDefault();
        }

        private async Task LoadEnvironment(Environment environment)
        {
            if (environment != null)
            {
                LatestEnvironmentValues.Clear();

                if (environment.Sensors != null)
                {
                    foreach (var sensor in environment.Sensors)
                    {
                        var webResult = await RestClient.GetLatestSensorValue(sensor);

                        LatestEnvironmentValues.Add(webResult.Item1.StatusIsGood ? webResult.value : new SensorValue {Sensor = sensor, ReadResult = -1});
                    }
                }
            }
        }

        /// <summary>
        /// Procedure:
        /// 1. User plugs in sensor to usb port.
        /// 2. User ensures only one sensor is plugged in.
        /// 3. I iterate over all COM ports, sending a 'hello' command until one of the ports responds. Must ensure the response is 'suh dude'.
        /// 4. I send the command 'GetId' to get the device ID. The device responds with an int.
        /// 5. I send the int to the server and see if the device ID exists and is not registered.
        /// 6. If the above criteria is met, the server will respond with the api key.
        /// 7. I will assign this api key to the device by sending it 'SetApiKey thekey'
        /// 8. Success
        /// </summary>
        /// <returns></returns>
        private async Task RegisterDevice(Environment environment)
        {
            if (environment == null)
            {
                
            }

            else
            {
                var portName = PlatformService.RetrieveSensorPortName();

                if (!string.IsNullOrEmpty(portName))
                {
                    var deviceId = PlatformService.ExtractSensorId(portName);

                    if (deviceId != -1)
                    {
                        var webResult = await RestClient.RegisterSensor(deviceId, environment);

                        if (webResult.Item1.StatusIsGood)
                        {
                            if (PlatformService.AssignSensorApiKey(portName, webResult.returnData))
                            {
                                //registered successfully
                            }

                            else
                            {
                                //error couldn't set api key
                            }
                        }

                        else
                        {
                            //show error
                        }
                    }

                    else
                    {
                        //error could not get device id
                    }
                }

                else
                {
                    //error, could not find device
                }
            }
        }
    }
}