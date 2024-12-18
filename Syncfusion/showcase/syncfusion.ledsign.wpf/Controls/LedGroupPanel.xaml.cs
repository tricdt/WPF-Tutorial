using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace syncfusion.ledsign.wpf
{
    /// <summary>
    /// Interaction logic for LedGroupPanel.xaml
    /// </summary>
    public partial class LedGroupPanel : UserControl
    {
        public LedGroupPanel()
        {
            InitializeComponent();
        }
        private LEDTHEME _LedPanelTheme;

        public LEDTHEME LedPanelTheme
        {
            get { return _LedPanelTheme; }
            set {
                _LedPanelTheme = value;
                grid.LedTheme = value;
            }
        }


    }
}
