using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
            //BindingContext = new MainVm();
            //Models.BodyWeightHandler.localPath = Path.Combine(FileSystem.AppDataDirectory, BodyWeightHandler.FILENAME);
        }

        private static Menu _instance;
        public static Menu Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Menu();
                }
                return _instance;
            }
        }
               

        private void NewEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewEntry());
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void ViewEntries_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewEntries());
        }


        private void DeleteEntries_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DeleteEntries());
        }
    }
}