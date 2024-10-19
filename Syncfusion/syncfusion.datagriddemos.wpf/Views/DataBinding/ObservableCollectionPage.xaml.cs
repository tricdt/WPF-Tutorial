using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ObservableCollectionPage.xaml
    /// </summary>
    public partial class ObservableCollectionPage : Page
    {
        public ObservableCollectionPage()
        {
            InitializeComponent();
            this.Unloaded += ObservableCollectionPage_Unloaded;
        }

        private void ObservableCollectionPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }
            this.Unloaded -= ObservableCollectionPage_Unloaded;
        }
    }
}
