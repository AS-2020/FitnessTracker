using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Helper
{
    public interface INavigationHandler
    {
        void ExecuteNewPage(object o, EventArgs e);
        //void ExecuteNewPage(object o);

    }
}
