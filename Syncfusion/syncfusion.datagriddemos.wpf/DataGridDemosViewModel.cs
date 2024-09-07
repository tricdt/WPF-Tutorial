using syncfusion.demoscommon.wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace syncfusion.datagriddemos.wpf
{
    public class DataGridDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new DataGridProductDemos());
            return productdemos;
        }
    }
    public class DataGridProductDemos : ProductDemo
    {
        public DataGridProductDemos()
        {

            this.Product = "DataGrid";
            this.ProductCategory = "GRIDS";
            this.ListViewImagePathData = new System.Windows.Shapes.Path()
            {
                Data = Geometry.Parse("M12 1H2C1.44772 1 1 1.44772 1 2V4H13V2C13 1.44772 12.5523 1 12 1ZM9 5H5V8H9V5ZM10 8V5H13V8H10ZM9 9H5V13H9V9ZM10 13V9H13V12C13 12.5523 12.5523 13 12 13H10ZM2 14C0.89543 14 0 13.1046 0 12V2C0 0.89543 0.895431 0 2 0H12C13.1046 0 14 0.895431 14 2V12C14 13.1046 13.1046 14 12 14H2ZM1 5H4V8H1V5ZM1 9H4V13H2C1.44772 13 1 12.5523 1 12V9Z"),
                Width = 14,
                Height = 14,
            };
            this.Demos = new List<DemoInfo>();
            this.IsHighlighted = true;
            this.ControlDescription = "The DataGrid is a high-performance grid control that displays tabular and hierarchical data. It supports sorting, grouping, filtering, etc.";
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Grids.png", UriKind.RelativeOrAbsolute));
            this.GalleryViewImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/GalleryViewImages/DataGrid.png", UriKind.RelativeOrAbsolute));
        }
    }
}
