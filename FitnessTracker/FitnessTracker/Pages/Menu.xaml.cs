using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private void NewEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewEntry());
        }

        private void ExitButton_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}