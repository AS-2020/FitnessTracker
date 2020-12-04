using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEntryBodyWeight : ContentPage
    {
        

        public NewEntryBodyWeight()
        {
            InitializeComponent();
            //Models.BodyWeightHandler.localPath = Path.Combine(FileSystem.AppDataDirectory, BodyWeightHandler.FILENAME);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

       // private void SaveBodyweight_Clicked(object sender, EventArgs e)
       // {
       //     Models.BodyWeight bodyWeight = new Models.BodyWeight()
       //     {
       //        // DateTime = 
       //     };
       //
       //     Navigation.PopAsync();
       // }
                
    }
}