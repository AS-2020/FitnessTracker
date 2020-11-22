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
    public partial class ViewEntries : ContentPage
    {
        public ViewEntries()
        {
            InitializeComponent();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ViewBodyWeight_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewBodyWeight());
        }
    }
}