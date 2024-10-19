
namespace syncfusion.gridcontroldemos.wpf
{
    public class PortfolioAccounts
    {
        public PortfolioAccounts() { }
        public PortfolioAccounts(string name, double value) { AccountName = name; AssetValue = value; }
        public string AccountName { get; set; }
        public double AssetValue { get; set; }
    }
}
