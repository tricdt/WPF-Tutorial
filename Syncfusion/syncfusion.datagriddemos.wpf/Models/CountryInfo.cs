﻿
using syncfusion.demoscommon.wpf;
using System.Collections.ObjectModel;

namespace syncfusion.datagriddemos.wpf
{
    public class CountryInfo : NotificationObject
    {
        #region Properties

        private string serialNumber;
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                serialNumber = value;
                RaisePropertyChanged("SerialNumber");
            }
        }

        private string countryName;
        public string Country
        {
            get
            {
                return countryName;
            }
            set
            {
                countryName = value;
                RaisePropertyChanged("Country");
            }
        }

        private string countryCapital;
        public string Capital
        {
            get
            {
                return countryCapital;
            }
            set
            {
                countryCapital = value;
                RaisePropertyChanged("Capital");
            }
        }

        private string officialName;
        public string OfficialName
        {
            get
            {
                return officialName;
            }
            set
            {
                officialName = value;
                RaisePropertyChanged("OfficialName");
            }
        }

        private string location;
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                RaisePropertyChanged("Location");
            }
        }

        private string language;
        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                RaisePropertyChanged("Language");
            }
        }

        private string currency;
        public string Currency
        {
            get
            {
                return currency;
            }
            set
            {
                currency = value;
                RaisePropertyChanged("Currency");
            }
        }

        private long population;
        public long Population
        {
            get
            {
                return population;
            }
            set
            {
                population = value;
                RaisePropertyChanged("Population");
            }
        }

        private double literacyRate;
        public double LiteracyRate
        {
            get
            {
                return literacyRate;
            }
            set
            {
                literacyRate = value;
                RaisePropertyChanged("LiteracyRate");
            }
        }

        private ObservableCollection<EconomyGrowth> economyGrowth;
        public ObservableCollection<EconomyGrowth> EconomyRate
        {
            get
            {
                return economyGrowth;
            }
            set
            {
                economyGrowth = value;
                RaisePropertyChanged("EconomyRate");
            }
        }

        #endregion

        #region Ctor

        public CountryInfo()
        {

        }

        public CountryInfo(string _serialNumber, string _countryName, string _countryCapital, string _officialName, string _location, string _language, string _currency, long _Population, double _LiteracyRate)
        {
            this.SerialNumber = _serialNumber;
            this.Country = _countryName;
            this.Capital = _countryCapital;
            this.OfficialName = _officialName;
            this.Location = _location;
            this.Language = _language;
            this.Currency = _currency;
            this.Population = _Population;
            this.LiteracyRate = _LiteracyRate;
        }

        #endregion
    }

    public class EconomyGrowth : NotificationObject
    {
        private int year;
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
                RaisePropertyChanged("Year");
            }
        }

        private double growthPercentage;
        public double GrowthPercentage
        {
            get
            {
                return growthPercentage;
            }
            set
            {
                growthPercentage = value;
                RaisePropertyChanged("GrowthPercentage");
            }
        }
    }
}
