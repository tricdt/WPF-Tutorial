using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();
        }
        public string Message
        {
            get { return message.Text; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    message.Text = value;
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.Owner = null;
        }

        public static void Show(string message)
        {
            ErrorLogging.LogError(message);
            var errorWindow = new ErrorWindow();
            errorWindow.Message = message;
            if (DemosNavigationService.MainWindow != null)
            {
                errorWindow.Owner = DemosNavigationService.MainWindow;
            }
            errorWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
