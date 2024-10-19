﻿using System.IO;
using System.Xml.Serialization;

namespace syncfusion.gridcontroldemos.wpf
{
    public class Queries
    {
        public class CurrentHoldings
        {
            public int AccountID { get; set; }
            public int HoldingID { get; set; }
            public bool? Open { get; set; }
            public string Country { get; set; }
            public string Quotes_Symbol { get; set; }
            public string Quote_CompanyName { get; set; }
            public decimal? PricePaid { get; set; }
            public decimal? Quantity { get; set; }
            public double? Quote_Change { get; set; }
            public double? DayChange { get; set; }
            public double? Quote___Change { get; set; }
            public string Account_AccountName { get; set; }
            public string Quote_Industry_Sector_SectorName { get; set; }
            public string Quote_Industry_IndusrtyName { get; set; }
            public string StockExchange_StockExchangeName { get; set; }
            public decimal? Quote_Price { get; set; }
            public decimal? MarketValue { get; set; }
            public decimal? TotalReturn { get; set; }
        }

        static List<CurrentHoldings> holdingsData;
        public static List<CurrentHoldings> GetHoldingsList()
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<CurrentHoldings>));

            using (Stream reader = new FileStream(@"Data\GridControl\HoldingsData.xml", FileMode.Open))
            {
                holdingsData = (List<CurrentHoldings>)xs.Deserialize(reader);
            }
            return holdingsData;
        }

        public class AcountNameAndValue
        {
            public string AccountName { get; set; }
            public Decimal? AssetValue { get; set; }
        }

        public static List<AcountNameAndValue> GetAcountNameAndValue()
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<AcountNameAndValue>));

            List<AcountNameAndValue> accounts;
            using (Stream reader = new FileStream(@"Data\GridControl\Accounts.xml", FileMode.Open))
            {
                accounts = (List<AcountNameAndValue>)xs.Deserialize(reader);
            }

            return accounts;
        }
        public class ExchangeAndValue
        {
            public string ExchangeName { get; set; }
            public Decimal? Value { get; set; }
        }
        public static List<ExchangeAndValue> GetExchangeNamesAndValues(int accountID)
        {
            var Exchanges = from holdings in holdingsData
                            where holdings.AccountID == accountID
                            group (holdings.PricePaid * holdings.Quantity) by holdings.Country into exchanges
                            select new ExchangeAndValue
                            {
                                ExchangeName = exchanges.Key,
                                Value = exchanges.Sum()
                            };

            return Exchanges.ToList<ExchangeAndValue>();
        }
    }
}
