using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Helper
{
    public static class NavigationHandlerFactory
    {
        public static INavigationHandler GetNavigationHandler(string navigationName)
        {
            switch (navigationName)
            {
                case "Back":
                    return new BackNavigation();
                case "NewEntry":
                    return new NewEntryNavigation();
                case "NewEntryBodyWeight":
                    return new NewEntryBodyWeightNavigation();
                case "NewEntryJogging":
                    return new NewEntryJoggingNavigation();
                case "ViewEntries":
                    return new ViewEntriesNavigation();
                case "ViewBodyWeight":
                    return new ViewBodyWeightNavigation();
                case "ViewBodyWeightChart":
                    return new ViewBodyWeightChartNavigation();
                case "ViewJogging":
                    return new ViewJoggingNavigation();
                case "DeleteEntries":
                    return new DeleteEntriesNavigation();
                default:
                    return null;
                    break;
            }
        }
    }
}
