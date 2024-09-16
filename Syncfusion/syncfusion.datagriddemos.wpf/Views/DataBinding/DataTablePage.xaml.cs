using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DataTablePage.xaml
    /// </summary>
    public partial class DataTablePage : Page
    {
        public DataTablePage()
        {
            InitializeComponent();
            this.Unloaded += DataTablePage_Unloaded;
        }

        private void DataTablePage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.sfDataGrid != null)
            {
                this.sfDataGrid.Dispose();
                this.sfDataGrid = null;
            }
            this.Unloaded -= DataTablePage_Unloaded;
        }
    }
}