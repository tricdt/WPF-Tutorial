using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    public class DemoBrowserViewModel : NotificationObject
    {
        /// <summary>
        /// Property to store busy status of sample browser while launch the show case demo.
        /// </summary>
        private bool isShowCaseDemoBusy = false;
        public bool IsShowCaseDemoBusy
        {
            get { return isShowCaseDemoBusy; }
            set
            {
                isShowCaseDemoBusy = value;
                RaisePropertyChanged("IsShowCaseDemoBusy");
            }
        }

        /// <summary>
        /// Property to store visibility state of blur layer in sample browser.
        /// </summary>
        private Visibility blurVisibility = Visibility.Collapsed;
        public Visibility BlurVisibility
        {
            get { return blurVisibility; }
            set
            {
                blurVisibility = value;
                RaisePropertyChanged(nameof(BlurVisibility));
            }
        }
    }
}
