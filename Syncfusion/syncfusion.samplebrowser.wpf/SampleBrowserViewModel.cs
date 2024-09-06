using syncfusion.datagriddemos.wpf;
using syncfusion.demoscommon.wpf;
using syncfusion.gridcontroldemos.wpf;

namespace syncfusion.samplebrowser.wpf
{
    public class SamplesViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new DataGridProductDemos());
            productdemos.Add(new GridControlProductDemos());
            return productdemos;
        }
    }
}
