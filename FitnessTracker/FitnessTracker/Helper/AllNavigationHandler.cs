using Android.Content.Res;
using FitnessTracker.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Helper
{
    class AllNavigationHandler
    {
    }

    public class BackNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
        }
    }

    public class NewEntryNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new NewEntry());
        }
    }
    public class NewEntryBodyWeightNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new NewEntryBodyWeight());
        }
    }
    public class NewEntryJoggingNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new NewEntryJogging());
        }
    }
    public class ViewEntriesNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ViewEntries());
        }
    }
    public class ViewBodyWeightNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ViewBodyWeight());
        }
    }
    public class ViewBodyWeightChartNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ViewBodyWeightChart());
        }
    }
    public class ViewJoggingNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ViewJogging());
        }
    }
    public class DeleteEntriesNavigation : INavigationHandler
    {
        public void ExecuteNewPage(object o, EventArgs e)
        {
            Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new DeleteEntries());
        }
    }
}
