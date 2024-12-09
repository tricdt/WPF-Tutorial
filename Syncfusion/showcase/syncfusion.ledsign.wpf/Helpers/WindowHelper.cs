using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace syncfusion.ledsign.wpf
{
    public static class WindowHelper
    {
        private static MainWindow _mainWindow;

        public static MainWindow MainWindow
        {
            get { return _mainWindow; }
            set { _mainWindow = value; }
        }

    }
}
