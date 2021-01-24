using FitnessTracker.Helper;
using FitnessTracker.Models;
using FitnessTracker.Pages;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessTracker.ViewModels
{
    public class MainVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static MainVm _instance;
        public static MainVm Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainVm();
                }
                return _instance;
            }
        }
        private ObservableCollection<BodyWeight> bodyWeightList;
        public ObservableCollection<BodyWeight> BodyWeightList
        {
            get { return bodyWeightList; }
            set
            {
                bodyWeightList = value;
                OnPropertyChanged("BodyWeightList");
            }
        }
        public ObservableCollection<Jogging> JoggingList { get; set; }

        private string _startOrPause = "Start";
        public string StartOrPause
        {
            get { return _startOrPause; }
            set
            {
                _startOrPause = value;
                OnPropertyChanged("StartOrPause");
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartOrPause"));
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

        public List<ChartEntry> entriesBodyWeight = new List<ChartEntry>();

        private Chart chartBodyWeight;

        public Chart ChartBodyWeight
        {
            get { return chartBodyWeight; }
            set
            {
                chartBodyWeight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChartBodyWeight"));
            }
        }

        private string charDateBodyWeightMin;

        public string CharDateBodyWeightMin
        {
            get { return charDateBodyWeightMin; }
            set
            {
                charDateBodyWeightMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CharDateBodyWeightMin"));
            }
        }

        private string errorDateBodyWeightChartView;

        public string ErrorDateBodyWeightChartView
        {
            get { return errorDateBodyWeightChartView; }
            set
            {
                errorDateBodyWeightChartView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorDateBodyWeightChartView"));
            }
        }
        private string charDateBodyWeightMax;
        public string CharDateBodyWeightMax
        {
            get { return charDateBodyWeightMax; }
            set
            {
                charDateBodyWeightMax = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CharDateBodyWeightMax"));
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
                OnPropertyChanged("Date");
            }
        }

        private float _weight;
        public float Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged("Weight");
            }
        }
        private float _bodyFat;
        public float BodyFat
        {
            get { return _bodyFat; }
            set
            {
                _bodyFat = value;
                OnPropertyChanged("BodyFat");
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
                return _navigationCommand ?? (_navigationCommand = new RelayCommand(SelectNewPage));
            }
        }
        public bool CheckBodyWeightChange { get; set; }
        private BodyWeight selectedBodyWeight;

        public BodyWeight SelectedBodyWeight
        {
            get { return selectedBodyWeight; }
            set { selectedBodyWeight = value; }
        }



        public RelayCommand SaveCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand StartWatchCommand { get; set; }
        public RelayCommand ResetWatchCommand { get; set; }
        public RelayCommand SaveJoggingCommand { get; set; }
        public RelayCommand DeleteJoggingCommand { get; set; }
        public RelayCommand DeleteBodyWeightCommand { get; set; }
        public RelayCommand CreateBodyWeightChartCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeBodyWeightCommand { get; set; }


        public MainVm()
        {
            _instance = this;
            BodyWeightHandler.Instance.Load();
            BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());
            JoggingHandler.Instance.Load();
            JoggingList = new ObservableCollection<Jogging>(JoggingHandler.Instance.GetJogging());

            SaveCommand = new RelayCommand((o) =>
            {
                if (BodyWeightHandler.Instance.GetBodyWeight().Find(e => e.ShowDate == Date) != null && CheckBodyWeightChange)
                {
                    BodyWeightError = "There alredy exists an entry for this day";
                }
                else if (Weight <= 0)
                {
                    BodyWeightError = "Enter your bodyweight";
                }
                else
                {
                    if (DateTime.TryParse(Date, out DateTime notused) == true)
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
                        ClearBodyWeight();
                        Back(o);
                    }
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
                    ResetStopWatch(o);
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
            ExitCommand = new RelayCommand((o) => System.Environment.Exit(0));

            ResetWatchCommand = new RelayCommand(ResetStopWatch);

            StartWatchCommand = new RelayCommand((o) =>
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
            });

            CreateBodyWeightChartCommand = new RelayCommand((o) =>
            {
                if (CharDateBodyWeightMin != null && CharDateBodyWeightMax != null)
                {
                    try
                    {
                        DateTime start = DateTime.Parse(CharDateBodyWeightMin);
                        DateTime end = DateTime.Parse(CharDateBodyWeightMax);
                        CreateChart(start, end);
                        ErrorDateBodyWeightChartView = "";
                    }
                    catch (System.FormatException)
                    {
                        entriesBodyWeight.Clear();
                        ErrorDateBodyWeightChartView = "Enter Date";
                    }
                }
                else
                {
                    CreateChart();
                    ErrorDateBodyWeightChartView = "";
                }
                ChartBodyWeight = new LineChart()
                {
                    Entries = entriesBodyWeight,
                    PointSize = 50,
                    LabelTextSize = 50,
                    LineSize = 40
                };
            });
            ChangeBodyWeightCommand = new RelayCommand((o) =>
            {

                CheckBodyWeightChange = true;
                Instance.Date = SelectedBodyWeight.ShowDate;
                Instance.Weight = SelectedBodyWeight.Weight;
                Instance.BodyFat = SelectedBodyWeight.BodyFat;

                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new NewEntryBodyWeight());

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

        public void ClearBodyWeight()
        {
            Date = DateTime.Now.ToShortDateString();
            Weight = 0;
            BodyFat = 0;
            BodyWeightError = "";
            CheckBodyWeightChange = false;
        }
        public void SelectBodyWeight(object o)
        {
            //try
            //{
            //    BodyWeight bodyWeight = SelectedBodyWeight;//SelectedKunde.LastOrDefault<Kunde>();
            //    KundenNummer = k.KundenNummer;
            //    NameFirma = k.NameFirma;
            //    AdresseKunde = k.AdresseKunde;
            //    Ansprechpartner = k.Ansprechpartner;
            //    Telefonnummer = k.Telefonnummer;
            //    Aktiv = k.Aktiv;
            //    AendernButton = true;
            //    KundenNummerTextBox = true;

            //}
            //catch (System.ArgumentNullException)
            //{

            //}

        }
        public void Back(object o)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
        }
        public void SelectNewPage(object message)
        {
            try
            {
                if (message == null)
                {
                    return;
                }
                INavigationHandler handler = NavigationHandlerFactory.GetNavigationHandler(message.ToString());
                handler.ExecuteNewPage(message, new EventArgs());
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
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void ResetStopWatch(object o)
        {
            TokenSourceJoggingTimer.Cancel();
            TokenSourceJoggingTimer.Dispose();
            TimeBeforePause = TimeSpan.Zero;
            Timejogging = TimeSpan.Zero;
            StartOrPause = "Start";
        }
        public void CreateChart(DateTime start, DateTime end)
        {
            entriesBodyWeight.Clear();
            List<BodyWeight> ListFromToBodyWeight = BodyWeightHandler.Instance.GetBodyWeight().Where(d => d.Date_asDate >= start && d.Date_asDate <= end).ToList();
            foreach (var item in ListFromToBodyWeight)
            {
                ChartEntry entry = new ChartEntry(item.Weight)
                {
                    Color = SKColor.Parse("#00CED1"),
                    Label = item.ShowDate,
                    ValueLabel = item.Weight.ToString() + " kg | " + item.BodyFat.ToString() + " %"
                };
                entriesBodyWeight.Add(entry);
            }
        }
        public void CreateChart()
        {
            entriesBodyWeight.Clear();
            foreach (var item in BodyWeightHandler.Instance.GetBodyWeight())
            {
                ChartEntry entry = new ChartEntry(item.Weight)
                {
                    Color = SKColor.Parse("#00CED1"),
                    Label = item.ShowDate,
                    ValueLabel = item.Weight.ToString() + " kg | " + item.BodyFat.ToString() + " %"
                };
                entriesBodyWeight.Add(entry);
            }
        }

    }
}

