using System.Windows;

namespace CustomControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AnalogClock_TimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            Console.WriteLine(e.NewValue);
        }
    }
}