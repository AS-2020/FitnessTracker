using FitnessTracker.Helper;
using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        private DateTime date = DateTime.Now.Date;
        public DateTime Date
        {
            get { return date.Date; }
            set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }
        private DateTime templateTodayDate;
        public DateTime TemplateTodayDate
        {
            get { return templateTodayDate; }
            set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TemplateTodayDate"));
            }
        }

        private DateTime todayDate;
        public DateTime TodayDate
        {
            get { return todayDate = DateTime.Now; }
            set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }
        private decimal _weight;
        public decimal Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Weight"));
            }
        }
        private decimal _bodyFat;
        public decimal BodyFat
        {
            get { return _bodyFat; }
            set
            {
                _bodyFat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BodyFat"));
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand FileExistCommand { get; set; }

        public void Back(object o)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
        }
        public  MainVm()
        {
            BodyWeightHandler.Instance.Load();
            BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());

            SaveCommand = new RelayCommand((o) =>
            {
                if (Weight > 0)
                {
                    BodyWeight bodyWeight = new BodyWeight()
                    {
                        DateTime = Date,
                        Weight = Weight,
                        BodyFat = BodyFat

                    };
                    BodyWeightHandler.Instance.AddBodyWeight(bodyWeight);
                    BodyWeightList.Add(bodyWeight);
                    BodyWeightHandler.Instance.Save();
                }
            });

            BackCommand = new RelayCommand(Back);


            FileExistCommand = new RelayCommand((o) =>
            {
                if (File.Exists(BodyWeightHandler.FILENAME))
                {
                    Console.WriteLine("File exists");
                }
                else
                {
                    Console.WriteLine("File does not exist");
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
    }
}

