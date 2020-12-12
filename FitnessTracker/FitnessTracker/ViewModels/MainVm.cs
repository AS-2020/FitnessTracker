using FitnessTracker.Helper;
using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessTracker.ViewModels
{
    public class MainVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BodyWeight> BodyWeightList { get; set; }
        public ObservableCollection<Jogging> JoggingList { get; set; }

        private Stopwatch stopwatch = new Stopwatch();
        private string _startOrPause = "Start";
        public string StartOrPause
        {
            get { return _startOrPause; }
            set
            {
                _startOrPause = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartOrPause"));
            }
        }
        private const string THEPW = "123";

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private TimeSpan _timejogging = TimeSpan.FromSeconds(0);
        public TimeSpan Timejogging
        {
            get { return _timejogging; }
            set
            {
                _timejogging = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Timejogging"));
            }
        }
        private string date = DateTime.Now.ToShortDateString();
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }

        private decimal _weight;
        public decimal Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Weight"));
            }
        }
        private decimal _bodyFat;
        public decimal BodyFat
        {
            get { return _bodyFat; }
            set
            {
                _bodyFat = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BodyFat"));
            }
        }

        private string _bodyWeightError;
        public string BodyWeightError
        {
            get { return _bodyWeightError; }
            set
            {
                _bodyWeightError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BodyWeightError"));
            }
        }
        private string _joggingError;
        public string JoggingError
        {
            get { return _joggingError; }
            set
            {
                _joggingError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("JoggingError"));
            }
        }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand StartWatchCommand { get; set; }
        public RelayCommand ResetWatchCommand { get; set; }
        public RelayCommand SaveJoggingCommand { get; set; }
        public RelayCommand DeleteJoggingCommand { get; set; }
        public RelayCommand DeleteBodyWeightCommand { get; set; }

        public void Back(object o)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
        }


        public MainVm()
        {
            BodyWeightHandler.Instance.Load();
            BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());
            JoggingHandler.Instance.Load();
            JoggingList = new ObservableCollection<Jogging>(JoggingHandler.Instance.GetJogging());

            SaveCommand = new RelayCommand((o) =>
            {
                if (BodyWeightHandler.Instance.GetBodyWeight().Find(e => e.ShowDate == Date) != null)
                {
                    BodyWeightError = "There alredy exists an entry for this day";
                }
                else if (Weight <= 0)
                {
                    BodyWeightError = "Enter your bodyweight";
                }
                else
                {
                    BodyWeight bodyWeight = new BodyWeight()
                    {
                        Date_asDate = DateTime.Parse(Date),
                        Weight = Weight,
                        BodyFat = BodyFat

                    };
                    BodyWeightHandler.Instance.AddBodyWeight(bodyWeight);
                    BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());
                    BodyWeightHandler.Instance.Save();
                    BodyWeightError = "";
                    Back(o);
                }
            });

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
                    JoggingList = new ObservableCollection<Jogging>(JoggingHandler.Instance.GetJogging());
                    JoggingHandler.Instance.Save();
                    JoggingError = "";
                    Back(o);
                }
            });

            DeleteJoggingCommand = new RelayCommand((o) =>
            {
                if (Password == THEPW)
                {
                    JoggingHandler.Instance.Delete();
                    JoggingList = new ObservableCollection<Jogging>(JoggingHandler.Instance.GetJogging());
                    Back(o);
                }
            });

            DeleteBodyWeightCommand = new RelayCommand((o) =>
            {
                if (Password == THEPW)
                {
                    BodyWeightHandler.Instance.Delete();
                    BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());
                    Back(o);
                }
            });

            BackCommand = new RelayCommand(Back);
            ResetWatchCommand = new RelayCommand(ResetStopWatch);


            StartWatchCommand = new RelayCommand((o) =>
            {
                if (StartOrPause == "Start")
                {
                    StartStopWatch(o);
                    Device.StartTimer(TimeSpan.FromSeconds(0), () =>
                    {
                        Timejogging = stopwatch.Elapsed;
                        if (!stopwatch.IsRunning)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    });
                    StartOrPause = "Pause";
                }
                else if (StartOrPause == "Pause")
                {
                    PauseStopWatch(o);
                    StartOrPause = "Resume";
                }
                else if (StartOrPause == "Resume")
                {
                    StartStopWatch(o);
                    Device.StartTimer(TimeSpan.FromSeconds(0), () =>
                    {
                        Timejogging = stopwatch.Elapsed;

                        if (!stopwatch.IsRunning)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    });
                    StartOrPause = "Pause";
                }
            });

        }

        public static async Task<PermissionStatus> CheckAndRequestStorageWritePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }

        public static async Task<PermissionStatus> CheckAndRequestStorageReadPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            return status;
        }
        //void OnPropertyChanged(string name)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}
        public void StartStopWatch(object o)
        {
            stopwatch.Start();
        }
        public void PauseStopWatch(object o)
        {
            stopwatch.Stop();
        }
        public void ResetStopWatch(object o)
        {
            stopwatch.Reset();
            Timejogging = TimeSpan.Zero;
            StartOrPause = "Start";
        }
    }
}

