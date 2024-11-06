
namespace syncfusion.pivotgriddemos.wpf
{
    using Syncfusion.Windows.Shared;
    using System.Collections.ObjectModel;

    public class OnDemandLoadingViewModel : NotificationObject
    {
        #region Private Variable
        private ObservableCollection<ItemObject> itemObjectCollection;

        #endregion

        #region Method

        /// <summary>
        /// Initializes the ViewModel class
        /// </summary>
        public OnDemandLoadingViewModel()
        {
            itemObjectCollection = ItemObjects.GetList();
        }

        #endregion

        #region Public Properties
        public ObservableCollection<ItemObject> ItemObjectCollection
        {
            get { return itemObjectCollection; }
            set
            {
                itemObjectCollection = value;
                RaisePropertyChanged(() => ItemObjectCollection);
            }
        }

        #endregion
    }
}