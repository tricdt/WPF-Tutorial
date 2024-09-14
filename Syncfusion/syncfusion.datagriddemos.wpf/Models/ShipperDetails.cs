
using Syncfusion.Windows.Shared;

namespace syncfusion.datagriddemos.wpf
{
    public class ShipperDetails : NotificationObject
    {
        private int _ShipperID;

        private string _CompanyName;

        public int ShipperID
        {
            get
            {
                return _ShipperID;
            }
            set
            {
                _ShipperID = value;
                RaisePropertyChanged("ShipperID");
            }
        }

        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set
            {
                _CompanyName = value;
                RaisePropertyChanged("CompanyName");
            }
        }
    }
}
