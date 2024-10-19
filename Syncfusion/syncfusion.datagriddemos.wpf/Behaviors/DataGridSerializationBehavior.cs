using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;
using System.IO;

namespace syncfusion.datagriddemos.wpf
{
    public class DataGridSerializationBehavior : Behavior<SfDataGrid>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.SerializationController = new SerializationController(this.AssociatedObject);
            this.AssociatedObject.Loaded += OnSfDataGridLoaded;
            base.OnAttached();
        }

        private void OnSfDataGridLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var dataGrid = this.AssociatedObject as SfDataGrid;
            if (dataGrid == null) return;
            try
            {
                using (var file = File.Create("Reset.xml"))
                {
                    dataGrid.Serialize(file);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= OnSfDataGridLoaded;
            base.OnDetaching();
        }
    }
}
