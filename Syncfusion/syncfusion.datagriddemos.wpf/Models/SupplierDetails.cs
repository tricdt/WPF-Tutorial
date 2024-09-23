using Syncfusion.Windows.Shared;

namespace syncfusion.datagriddemos.wpf
{
    public class SupplierDetails : NotificationObject
    {
        private int _supplierId;

        public int SupplierID
        {
            get
            {
                return this._supplierId;
            }
            set
            {
                this._supplierId = value;
                RaisePropertyChanged("SupplierID");
            }
        }

        private string _supplierName;

        public string SupplierName
        {
            get
            {
                return _supplierName;
            }
            set
            {
                _supplierName = value;
                RaisePropertyChanged("SupplierName");
            }
        }

        private string _supplierCity;

        public string SupplierCity
        {
            get
            {
                return this._supplierCity;
            }
            set
            {
                this._supplierCity = value;
                RaisePropertyChanged("SupplierCity");
            }
        }
    }
}
