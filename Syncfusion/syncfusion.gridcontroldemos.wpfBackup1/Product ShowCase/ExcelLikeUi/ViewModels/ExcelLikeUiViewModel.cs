using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiViewModel : NotificationObject
    {
        #region Fields
        private IExcelLikeUi mainView;
        private int activeGridIndex;
        private FontFamily fontFamily = new FontFamily("Arial");
        private double fontSize = 12;
        private FontWeight fontWeight;
        private FontStyle fontStyle;
        private TextDecorationCollection textDecorations;
        private HorizontalAlignment horizontalAlignment;
        private VerticalAlignment verticalAlignment;
        private string cellLocationText;
        //private IEnumerable<FontFamily> fontFamilyList;
        private IEnumerable<double> fontSizeList;

        #endregion
        #region Properties
        public IWorkbook Workbook { get; }

        public IExcelLikeUi MainView
        {
            get
            {
                return mainView;
            }
        }

        public int ActiveTabIndex
        {
            get
            {
                return activeGridIndex;
            }
            set
            {
                activeGridIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }

        }
        #endregion

        public ExcelLikeUiViewModel(IExcelLikeUi mainview)
        {
            this.mainView = mainview;
            Initialize();
        }

        private void Initialize()
        {
            if (MainView != null)
            {
                MainView.Initialize();
            }
        }
    }
}
