using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ListViewPage.xaml
    /// </summary>
    public partial class ListViewPage : Page
    {
        public ListViewPage()
        {
            InitializeComponent();
            this.Unloaded += ListViewPage_Unloaded;
        }

        private void ListViewPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }
            this.Unloaded -= ListViewPage_Unloaded;
        }
    }
}
