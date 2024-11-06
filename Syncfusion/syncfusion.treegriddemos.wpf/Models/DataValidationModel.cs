﻿
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace syncfusion.treegriddemos.wpf
{
    public class DataValidationModel : NotificationObject, IDataErrorInfo
    {
        #region Private Fields

        private static int _globalId = 0;
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _cake = String.Empty;
        private DateTime _dob;
        private string _eyecolor;
        private ObservableCollection<DataValidationModel> _children;
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        private string email;
        private int salary;
        private string contactNo;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public ObservableCollection<DataValidationModel> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
                RaisePropertyChanged("Children");
            }
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The first name.</value>
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
            }

        }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        /// <summary>
        /// Gets or sets the DOB.
        /// </summary>
        /// <value>The DOB.</value>
        public DateTime DOB
        {
            get
            {
                return _dob;
            }
            set
            {
                _dob = value;
                RaisePropertyChanged("DOB");
            }
        }

        /// <summary>
        /// Gets or sets the color of my eye.
        /// </summary>
        /// <value>The color of my eye.</value>
        public string MyEyeColor
        {
            get
            {
                return _eyecolor;
            }
            set
            {
                _eyecolor = value;
                RaisePropertyChanged("MyEyeColor");
            }
        }

        [Range(10000, 30000, ErrorMessage = "The “Salary” field can range from 10000 through 30000.")]
        public int Salary
        {
            get { return salary; }
            set { salary = value; RaisePropertyChanged("Salary"); }
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
        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="PersonInfo"/> class.
        /// </summary>
        public DataValidationModel()
            : this("Enter FirstName", "Enter LastName", "Enter EyeColor", new DateTime(2008, 10, 26), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonInfo"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="eyecolor">The eyecolor.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="maxGenerations">The max generations.</param>
        public DataValidationModel(string firstName, string lastName, string eyecolor, DateTime dob, ObservableCollection<DataValidationModel> child)
        {
            _firstName = firstName;
            _lastName = lastName;
            _eyecolor = eyecolor;
            _cake = "Chocolate";
            _dob = dob;
            _id = _globalId++;
            _children = child;
        }

        #endregion Constructors


        #region IDataErrorInfo

        [Bindable(false)]
        public string Error
        {
            get
            {
                return !emailRegex.IsMatch(this.EMail) ? "Email ID is invalid!" : null;
            }
        }

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

        #endregion
    }
}