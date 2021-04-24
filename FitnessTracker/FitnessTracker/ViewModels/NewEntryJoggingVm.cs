using Android.App;
using Android.Content;
using Android.OS;
using FitnessTracker.Helper;
using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace FitnessTracker.ViewModels
{
    class NewEntryJoggingVm : BaseVm
    {

        private string _startOrPause = "Start";
        public string StartOrPause
        {
            get { return _startOrPause; }
            set
            {
                _startOrPause = value;
                OnPropertyChanged("StartOrPause");
            }
        }

        private bool _switchIsToggled = false;
        public bool SwitchIsToggled
        {
            get { return _switchIsToggled; }
            set
            {
                _switchIsToggled = value;
                OnPropertyChanged("SwitchIsToggled");
                StartTrackingRoute();
            }
        }


        private TimeSpan _timejogging = TimeSpan.FromSeconds(0);
        public TimeSpan Timejogging
        {
            get { return _timejogging; }
            set
            {
                _timejogging = value;
                OnPropertyChanged("Timejogging");
            }
        }
        private string date = DateTime.Now.ToShortDateString();
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }


        private string _joggingError;
        public string JoggingError
        {
            get { return _joggingError; }
            set
            {
                _joggingError = value;
                OnPropertyChanged("JoggingError");
            }
        }
        private DateTime startTimer;

        public DateTime StartTimer
        {
            get { return startTimer; }
            set
            {
                startTimer = value;
                OnPropertyChanged("StartTimer");
            }
        }
        public TimeSpan TimeBeforePause { get; set; }
        public Task Timer { get; set; }
        private static CancellationTokenSource tokenSourceJoggingTimer; // = new CancellationTokenSource();
        public static CancellationTokenSource TokenSourceJoggingTimer { get; set; }
        private CancellationToken cancellationTokenJoggingTimer; //= ts.Token;
        public CancellationToken CancellationTokenJoggingTimer { get; set; }
        private ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                return _navigationCommand ?? (_navigationCommand = new RelayCommand(MainVm.Instance.SelectNewPage));
            }
        }


        public RelayCommand StartWatchCommand { get; set; }
        public RelayCommand ResetWatchCommand { get; set; }
        public RelayCommand SaveJoggingCommand { get; set; }


        public NewEntryJoggingVm()
        {

            SaveJoggingCommand = new RelayCommand((o) =>
            {
                if (Timejogging == TimeSpan.Zero)
                {
                    JoggingError = "No Time available";
                }
                else
                {
                    Jogging jogging = new Jogging()
                    {
                        Date_asDate = DateTime.Parse(Date),
                        JoggingTime = Timejogging.ToString()
                    };
                    JoggingHandler.Instance.AddJogging(jogging);
                    MainVm.Instance.JoggingList = new ObservableCollection<Jogging>(JoggingHandler.Instance.GetJogging());
                    JoggingHandler.Instance.Save();
                    JoggingError = "";
                    ResetStopWatch(o);
                    MainVm.Instance.Back(o);
                }
            });

            ResetWatchCommand = new RelayCommand(ResetStopWatch);

            StartWatchCommand = new RelayCommand((o) =>
           {

               if (SwitchIsToggled)
               {
                   if (StartOrPause == "Start")
                   {
                       //StartTrackingRoute(); wird beim einschalten des Switches ausgeführt
                       StartJoggingTimer();
                       StartOrPause = "Pause";
                   }
                   else if (StartOrPause == "Pause")
                   {
                       TokenSourceJoggingTimer.Cancel();
                       //ts.Dispose(); erst später?
                       TimeBeforePause = Timejogging;
                       StartOrPause = "Resume";
                   }
                   else if (StartOrPause == "Resume")
                   {
                       StartJoggingTimer();
                       StartOrPause = "Pause";
                   }
               }
               else
               {
                   if (StartOrPause == "Start")
                   {
                       StartJoggingTimer();
                       StartOrPause = "Pause";
                   }
                   else if (StartOrPause == "Pause")
                   {
                       TokenSourceJoggingTimer.Cancel();
                       //ts.Dispose(); erst später?
                       TimeBeforePause = Timejogging;
                       StartOrPause = "Resume";
                   }
                   else if (StartOrPause == "Resume")
                   {
                       StartJoggingTimer();
                       StartOrPause = "Pause";
                   }
               }
           });

        }

        public static async Task<PermissionStatus> CheckAndRequestGetLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.LocationAlways>();

            return status;
        }

        public bool IsGpsAvailable()
        {
            bool value = false;
            Android.Locations.LocationManager manager = (Android.Locations.LocationManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.LocationService);
            if (!manager.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider))
            {
                //gps disable
                value = false;
            }
            else
            {
                //Gps enable
                value = true;
            }
            return value;
        }

        public void OpenSettings()
        {
            Intent intent = new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }

        public async void StartTrackingRoute()
        {
            try
            {
                JoggingError = "Wait for GPS";
                await CheckAndRequestGetLocationPermission();
                if (IsGpsAvailable() == false)
                {
                    OpenSettings();
                }
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location == null)
                    JoggingError = "No GPS";
                else
                    JoggingError = $"GPS Latitude: {location.Latitude}; Longitude: {location.Longitude}";

            }
            catch (Exception)
            {

                throw;
            }
        }


        public void StartJoggingTimer()
        {
            StartTimer = DateTime.Now;
            TokenSourceJoggingTimer = new CancellationTokenSource();
            CancellationTokenJoggingTimer = TokenSourceJoggingTimer.Token;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Timejogging = (DateTime.Now - StartTimer) + TimeBeforePause;
                    Thread.Sleep(1000);
                    if (CancellationTokenJoggingTimer.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }, CancellationTokenJoggingTimer);
        }

        public void ResetStopWatch(object o)
        {
            TokenSourceJoggingTimer.Cancel();
            TokenSourceJoggingTimer.Dispose();
            TimeBeforePause = TimeSpan.Zero;
            Timejogging = TimeSpan.Zero;
            StartOrPause = "Start";
        }


    }
}