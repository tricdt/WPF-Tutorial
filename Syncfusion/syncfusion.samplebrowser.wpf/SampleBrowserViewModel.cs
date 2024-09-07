using syncfusion.datagriddemos.wpf;
using syncfusion.demoscommon.wpf;
using syncfusion.gridcontroldemos.wpf;
using syncfusion.pivotgriddemos.wpf;
using syncfusion.propertygriddemos.wpf;
using syncfusion.treegriddemos.wpf;
namespace syncfusion.samplebrowser.wpf
{
    public class SamplesViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new DataGridProductDemos());
            productdemos.Add(new GridControlProductDemos());
            productdemos.Add(new TreeGridProductDemos());
            productdemos.Add(new PivotGridProductDemos());
            productdemos.Add(new PropertyGridProductDemos());
            return productdemos;
        }
    }
}
