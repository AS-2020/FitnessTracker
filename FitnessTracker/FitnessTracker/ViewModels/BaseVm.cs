using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FitnessTracker.ViewModels
{
    class BaseVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
       
    }
}
