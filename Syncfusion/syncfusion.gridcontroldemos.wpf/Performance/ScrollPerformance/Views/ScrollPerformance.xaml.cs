using syncfusion.demoscommon.wpf;
using System.Windows;
using System.Windows.Threading;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ScrollPerformance.xaml
    /// </summary>
    public partial class ScrollPerformance : DemoControl
    {

        DispatcherTimer timer = new DispatcherTimer();
        Random r = new Random();
        public ScrollPerformance()
        {
            InitializeComponent();
            timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            vGrid.ContentUpdated += VGrid_ContentUpdated;
            this.Unloaded += VirtualGrid_Unloaded;
        }

        private void VirtualGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= VirtualGrid_Unloaded;
        }

        string[] actions = new string[]{
            "ScrollLines",
            "MoveVerticalPageDown",
            "MoveVerticalToEnd",
            "MoveVerticalPageUp",
            "MoveVerticalTop"
        };
        int counter0 = 0;
        private void timer_Tick(object? sender, EventArgs e)
        {
            if (timer.IsEnabled)
            {
                if (counter0 > 5)
                {
                    counter0 = 5;
                }
                var action = actions[counter0 % actions.Length];
                switch (action)
                {
                    case "ScrollLines":
                        for (int i = 0; i < 100; i++)
                        {
                            this.vGrid.LineDown();
                            this.vGrid.InvalidateArrange();
                        }
                        break;
                    case "MoveVerticalPageDown":
                        for (int i = 0; i < 10; i++)
                        {
                            this.vGrid.PageDown();
                            this.vGrid.InvalidateArrange();
                        }
                        break;
                    case "MoveVerticalToEnd":
                        this.vGrid.ScrollToBottom();
                        this.vGrid.InvalidateArrange();
                        break;
                    case "MoveVerticalPageUp":
                        for (int i = 0; i < 3; i++)
                        {
                            this.vGrid.PageRight();
                            this.vGrid.InvalidateArrange();
                        }
                        break;
                    case "MoveVerticalTop":
                        this.vGrid.ScrollToTop();
                        this.vGrid.InvalidateArrange();
                        break;
                }
                counter0 += 1;
            }
        }

        private void VGrid_ContentUpdated(object? sender, EventArgs e)
        {
            lblScroll.Content = this.vGrid.displayText;
        }

        private void rdo1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.RowHeights.LineCount = 1000000;
            vGrid.InvalidateVisual(true);
        }

        private void rdo2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.RowHeights.LineCount = 10000000;
            vGrid.InvalidateVisual(true);
        }

        private void rdo3_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.RowHeights.LineCount = 1000000000;
            vGrid.InvalidateVisual(true);
        }

        private void rdoCol1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.ColumnWidths.LineCount = 1000000;
            vGrid.InvalidateVisual(true);
        }

        private void rdoCol2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.ColumnWidths.LineCount = 10000000;
            vGrid.InvalidateVisual(true);
        }

        private void rdoCol3_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.ColumnWidths.LineCount = 1000000000;
            vGrid.InvalidateVisual(true);
        }

        private void scrollRight_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToRightEnd();
        }

        private void scrollLeft_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToLeftEnd();
        }

        private void scrollBottom_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToBottom();
        }

        private void scrollTop_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToTop();
        }
        bool startLog = false;
        private void btnTimer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (btnTimer.Content.Equals("Start ScrollTimer"))
            {
                timer.Start();
                startLog = true;
                btnTimer.Content = "Stop ScrollTimer";

            }
            else
            {
                timer.Stop();
                startLog = false;
                btnTimer.Content = "Start ScrollTimer";

            }
        }
        protected override void Dispose(bool disposing)
        {
            if (this.timer != null)
            {
                this.timer.Tick -= new EventHandler(timer_Tick);
                this.timer.IsEnabled = false;
                this.timer.Stop();
            }
            if (this.vGrid != null)
                this.vGrid.ContentUpdated -= new EventHandler(VGrid_ContentUpdated);

            this.Unloaded -= new RoutedEventHandler(VirtualGrid_Unloaded);
            vGrid.Dispose();
            base.Dispose(disposing);
        }
    }
}
