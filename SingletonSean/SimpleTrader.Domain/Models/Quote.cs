namespace SimpleTrader.Domain.Models
{
    public class Quote
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string changesPercentage { get; set; }
        public string change { get; set; }
        public string dayLow { get; set; }
        public string dayHigh { get; set; }
        public string yearHigh { get; set; }
        public string yearLow { get; set; }
        public string marketCap { get; set; }
        public string priceAvg50 { get; set; }
        public string priceAvg200 { get; set; }
        public string exchange { get; set; }
        public string volume { get; set; }
        public string avgVolume { get; set; }
        public string open { get; set; }
        public string previousClose { get; set; }
        public string eps { get; set; }
        public string pe { get; set; }
        public string earningsAnnouncement { get; set; }
        public string sharesOutstanding { get; set; }
        public string timestamp { get; set; }
    }
}
