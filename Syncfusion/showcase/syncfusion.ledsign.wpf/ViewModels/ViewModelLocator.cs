using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace syncfusion.ledsign.wpf
{
    public class ViewModelLocator
    {
        private LocationViewModel locationViewModel;

        public LocationViewModel LocationViewModel
        {
            get {
                if (locationViewModel == null) locationViewModel = new LocationViewModel();
                return locationViewModel;
            }
            set { locationViewModel = value; }
        }

    }
}
