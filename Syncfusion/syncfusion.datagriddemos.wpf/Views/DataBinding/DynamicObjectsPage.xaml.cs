using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DynamicObjectsPage.xaml
    /// </summary>
    public partial class DynamicObjectsPage : Page
    {
        public DynamicObjectsPage()
        {
            InitializeComponent();
            this.Unloaded += DynamicObjectsPage_Unloaded;
        }

        private void DynamicObjectsPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }
            this.Unloaded -= DynamicObjectsPage_Unloaded;
        }
    }
}
