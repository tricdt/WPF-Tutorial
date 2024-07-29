using EffectiveValidation.UpdateAddress;
using System.Windows;
namespace EffectiveValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new UpdateAddressViewModel();
        }
    }
}