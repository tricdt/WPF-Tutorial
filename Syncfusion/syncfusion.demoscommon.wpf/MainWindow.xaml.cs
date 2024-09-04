using Syncfusion.Windows.Shared;
using System.Windows;
namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow(DemoBrowserViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DemoControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
