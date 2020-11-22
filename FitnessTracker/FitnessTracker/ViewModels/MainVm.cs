using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitnessTracker.ViewModels
{
    public class MainVm : INotifyPropertyChanged
    {
        string todayDate = string.Empty;
        public string TodayDate
        {
            get => todayDate = DateTime.Now.ToString("dd/MM/yyyy");
            private set
            {
                todayDate = value;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
