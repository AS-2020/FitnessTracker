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
    public partial class NewEntry : ContentPage
    {
        public NewEntry()
        {
            InitializeComponent();
        }

        private void NewEntryBodyWeight_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewEntryBodyWeight());
        }
    }
}