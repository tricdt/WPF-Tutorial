using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Charts;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace syncfusion.gridcontroldemos.wpf
{
    public class PerformanceChartBehavior : Behavior<SfChart>
    {
        protected override void OnAttached()
        {

            this.AssociatedObject.Loaded += new System.Windows.RoutedEventHandler(AssociatedObject_Loaded);
        }

        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.AssociatedObject.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
            new Action(delegate () { CustomizePerformanceChart(); }));
        }

        private void CustomizePerformanceChart()
        {
            DateTime MonthsMax = DateTime.Now;
            DateTime MonthsMin = DateTime.Now.AddMonths(-12);
            ObservableCollection<Performance> seriesData1 = new ObservableCollection<Performance>();
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Performance>));
            TextReader reader1 = new StreamReader(@"Data\GridControl\AmericanFundsPerf.xml");
            seriesData1 = (ObservableCollection<Performance>)serializer.Deserialize(reader1);
            reader1.Close();

            (this.AssociatedObject.Series["AmericanFunds"] as SplineAreaSeries).ItemsSource = seriesData1;
            (this.AssociatedObject.Series["AmericanFunds"] as SplineAreaSeries).XBindingPath = "Date";
            (this.AssociatedObject.Series["AmericanFunds"] as SplineAreaSeries).YBindingPath = "AssetValue";
            (this.AssociatedObject.Series["AmericanFunds"] as SplineAreaSeries).Label = "AmericanFunds";
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= new System.Windows.RoutedEventHandler(AssociatedObject_Loaded);
            base.OnDetaching();
        }
    }
}
