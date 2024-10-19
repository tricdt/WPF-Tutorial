﻿using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    public class SalesByDate : NotificationObject
    {
        private string _name;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private double _qS1;

        /// <summary>
        /// Gets or sets the Q s1.
        /// </summary>
        /// <value>The Q s1.</value>
        public double QS1
        {
            get
            {
                return _qS1;
            }
            set
            {
                _qS1 = value;
                RaisePropertyChanged("QS1");
            }
        }

        private double _qS2;

        /// <summary>
        /// Gets or sets the Q s2.
        /// </summary>
        /// <value>The Q s2.</value>
        public double QS2
        {
            get
            {
                return _qS2;
            }
            set
            {
                _qS2 = value;
                RaisePropertyChanged("QS2");
            }
        }

        private double _qS3;

        /// <summary>
        /// Gets or sets the Q s3.
        /// </summary>
        /// <value>The Q s3.</value>
        public double QS3
        {
            get
            {
                return _qS3;
            }
            set
            {
                _qS3 = value;
                RaisePropertyChanged("QS3");
            }
        }

        private double _qS4;

        /// <summary>
        /// Gets or sets the Q s4.
        /// </summary>
        /// <value>The Q s4.</value>
        public double QS4
        {
            get
            {
                return _qS4;
            }
            set
            {
                _qS4 = value;
                RaisePropertyChanged("QS4");
            }
        }

        private double _qS5;

        /// <summary>
        /// Gets or sets the Q s5.
        /// </summary>
        /// <value>The Q s5.</value>
        public double QS5
        {
            get
            {
                return _qS5;
            }
            set
            {
                _qS5 = value;
                RaisePropertyChanged("QS5");
            }
        }

        private double _qS6;

        /// <summary>
        /// Gets or sets the Q s6.
        /// </summary>
        /// <value>The Q s6.</value>
        public double QS6
        {
            get
            {
                return _qS6;
            }
            set
            {
                _qS6 = value;
                RaisePropertyChanged("QS6");
            }
        }
        private double _total;

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public double Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
                RaisePropertyChanged("Total");
            }
        }

        private double _totalsales;

        /// <summary>
        /// Gets or sets the total salses.
        /// </summary>
        /// <value>The total salses.</value>
        public double TotalSales
        {
            get
            {
                return _totalsales;
            }
            set
            {
                _totalsales = value;
                RaisePropertyChanged("TotalSales");
            }
        }

        private DateTime _date;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        private int _year;

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                RaisePropertyChanged("Year");
            }
        }
    }
}
