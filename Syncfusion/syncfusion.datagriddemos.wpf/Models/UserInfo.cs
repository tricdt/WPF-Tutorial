using Syncfusion.Windows.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace syncfusion.datagriddemos.wpf
{
    public class UserInfo : NotificationObject, IDataErrorInfo
    {
        #region Fields
        private int userId;
        private string name;
        private DateTime dateofBirth;
        private string contactNo;
        private string city;
        private int salary;
        private string email;
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        #endregion

        #region Properties
        public int UserId
        {
            get { return userId; }
            set { userId = value; RaisePropertyChanged("UserId"); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged("Name"); }
        }

        public DateTime DateofBirth
        {
            get { return dateofBirth; }
            set { dateofBirth = value; RaisePropertyChanged("DateofBirth"); }
        }
        public string EMail
        {
            get { return email; }
            set { email = value; RaisePropertyChanged("EMail"); }
        }
        [StringLength(14, ErrorMessage = "The “ContactNo” field must be a string with a maximum length of 14.")]
        public string ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; RaisePropertyChanged("ContactNo"); }
        }

        public string City
        {
            get { return city; }
            set { city = value; RaisePropertyChanged("City"); }
        }
        [Range(10000, 30000, ErrorMessage = "The “Salary” field can range from 10000 through 30000.")]
        public int Salary
        {
            get { return salary; }
            set { salary = value; RaisePropertyChanged("Salary"); }
        }
        #endregion

        #region IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                if (columnName == "EMail")
                {
                    return !emailRegex.IsMatch(this.EMail) ? "Email ID is invalid!" : null;
                }
                return null;
            }
        }
        [Bindable(false)]
        public string Error
        {
            get
            {
                return !emailRegex.IsMatch(this.EMail) ? "Error Email ID is invalid!" : null;
            }
        }
        #endregion
    }
}
