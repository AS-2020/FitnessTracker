using FitnessTracker.Helper;
using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessTracker.ViewModels
{
    public class MainVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BodyWeight> BodyWeightList { get; set; }

        string todayDate = string.Empty;
        public string TodayDate
        {
            get => todayDate = DateTime.Now.ToString("dd/MM/yyyy");
            private set
            {
                todayDate = value;
            }
        }
        private string _weight;
        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Weight"));
            }
        }
        private string _bodyFat;
        public string BodyFat
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
        public MainVm()
        {
            BodyWeightHandler.Instance.Load();
            BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());

            SaveCommand = new RelayCommand((o) =>
            {
                BodyWeight bodyWeight = new BodyWeight()
                {
                    DateTime = DateTime.Now,
                    // DateTime = Convert.ToDateTime(TodayDate),
                    Weight = decimal.Parse(Weight),
                    BodyFat = decimal.Parse(BodyFat)

                };
                BodyWeightHandler.Instance.AddBodyWeight(bodyWeight);
                BodyWeightList.Add(bodyWeight);
                BodyWeightHandler.Instance.Save();
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


        //void OnPropertyChanged(string name)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}
    }
}

