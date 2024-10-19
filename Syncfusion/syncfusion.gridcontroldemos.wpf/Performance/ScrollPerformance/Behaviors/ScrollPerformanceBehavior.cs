using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Threading;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ScrollPerformanceBehavior : Behavior<ScrollPerformance>
    {
        SampleVirtualGrid vGrid;
        Button btnTimer;
        DispatcherTimer timer = new DispatcherTimer();
        protected override void OnAttached()
        {
            vGrid = AssociatedObject.vGrid;
            btnTimer = AssociatedObject.btnTimer;
            AssociatedObject.rdo1.Checked += Rdo1_Checked;
            AssociatedObject.rdo2.Checked += Rdo2_Checked;
            AssociatedObject.rdo3.Checked += Rdo3_Checked;
            AssociatedObject.rdoCol1.Checked += RdoCol1_Checked;
            AssociatedObject.rdoCol2.Checked += RdoCol2_Checked;
            AssociatedObject.rdoCol3.Checked += RdoCol3_Checked;
            AssociatedObject.scrollLeft.Checked += ScrollLeft_Checked;
            AssociatedObject.scrollRight.Checked += ScrollRight_Checked;
            AssociatedObject.scrollTop.Checked += ScrollTop_Checked;
            AssociatedObject.scrollBottom.Checked += ScrollBottom_Checked;
            timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            vGrid.ContentUpdated += VGrid_ContentUpdated;
            btnTimer.Click += BtnTimer_Click;
            base.OnAttached();
        }
        bool startLog = false;
        private void BtnTimer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (btnTimer.Content.Equals("Start ScrollTimer"))
            {
                timer.Start();
#if !Grid
#endif
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



        private void VGrid_ContentUpdated(object? sender, EventArgs e)
        {
            AssociatedObject.lblScroll.Content = vGrid.displayText;
        }
        string[] actions = new string[]{
            "ScrollLines",
            "MoveVerticalPageDown",
            "MoveVerticalToEnd",
            "MoveVerticalPageUp",
            "MoveVerticalTop"
        };

        int counter0 = 0;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (timer.IsEnabled)
            {
                if (counter0 > 5)
                {
                    counter0 = 0;
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

        private void ScrollBottom_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToBottom();
        }

        private void ScrollTop_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToTop();
        }

        private void ScrollRight_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToRightEnd();
        }

        private void ScrollLeft_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.vGrid.ScrollToLeftEnd();
        }

        private void RdoCol3_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.ColumnWidths.LineCount = 1000000000;
            vGrid.InvalidateVisual(true);
        }

        private void RdoCol2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.ColumnWidths.LineCount = 10000000;
            vGrid.InvalidateVisual(true);
        }

        private void RdoCol1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.ColumnWidths.LineCount = 1000000;
            vGrid.InvalidateVisual(true);
        }

        private void Rdo3_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.RowHeights.LineCount = 1000000000;
            vGrid.InvalidateVisual(true);
        }

        private void Rdo2_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.RowHeights.LineCount = 10000000;
            vGrid.InvalidateVisual(true);
        }

        private void Rdo1_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            vGrid.Model.RowHeights.LineCount = 1000000;
            vGrid.InvalidateVisual(true);
        }
    }
}
