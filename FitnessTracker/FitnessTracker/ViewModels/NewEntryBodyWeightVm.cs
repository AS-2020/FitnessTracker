using FitnessTracker.Helper;
using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace FitnessTracker.ViewModels
{
    class NewEntryBodyWeightVm : BaseVm
    {
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
                OnPropertyChanged("BodyWeightError");
            }
        }
        public bool DateIsReadOnly { get; set; }
        private ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get
            {
                return _navigationCommand ?? (_navigationCommand = new RelayCommand(MainVm.Instance.SelectNewPage));
            }
        }
        public RelayCommand SaveCommand { get; set; }

        public NewEntryBodyWeightVm()
        {
            Date = MainVm.Instance.Date;
            BodyFat = MainVm.Instance.BodyFat;
            Weight = MainVm.Instance.Weight;
            DateIsReadOnly = MainVm.Instance.CheckBodyWeightChange;

            SaveCommand = new RelayCommand((o) =>
            {
                if (BodyWeightHandler.Instance.GetBodyWeight().Find(e => e.ShowDate == Date) != null && !MainVm.Instance.CheckBodyWeightChange)
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
                        BodyWeight old = BodyWeightHandler.Instance.GetBodyWeight().Find(e => e.ShowDate == Date);
                        BodyWeight bodyWeight = new BodyWeight()
                        {
                            Date_asDate = DateTime.Parse(Date),
                            Weight = Weight,
                            BodyFat = BodyFat
                        };
                        if (old != null)
                            BodyWeightHandler.Instance.RemoveBodyWeight(old);
                        BodyWeightHandler.Instance.AddBodyWeight(bodyWeight);
                        MainVm.Instance.BodyWeightList = new ObservableCollection<BodyWeight>(BodyWeightHandler.Instance.GetBodyWeight());
                        BodyWeightHandler.Instance.Save();
                        MainVm.Instance.ClearBodyWeight();
                        MainVm.Instance.Back(o);
                    }
                }
            });
        }


    }
}
