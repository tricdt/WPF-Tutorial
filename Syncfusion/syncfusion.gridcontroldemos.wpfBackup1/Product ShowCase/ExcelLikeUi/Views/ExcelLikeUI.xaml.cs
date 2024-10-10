using Syncfusion.Windows.Tools.Controls;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelLikeUI.xaml
    /// </summary>
    public partial class ExcelLikeUI : RibbonWindow, IExcelLikeUi
    {
        public ExcelLikeUI()
        {
            InitializeComponent();
            this.DataContext = new ExcelLikeUiViewModel(this);
        }

        public void CurrentCellStyleChanged(int ActiveTabIndex, string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCopyCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void ExecuteCutCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void ExecuteFontSizeCommand(int ActiveTabIndex, bool IsIncrement)
        {
            throw new NotImplementedException();
        }

        public void ExecuteIndentCommand(int ActiveTabIndex, bool IsIncrement)
        {
            throw new NotImplementedException();
        }

        public void ExecuteOrientationCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void ExecutePasteCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void ExecutePrintCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void ExecuteRedoCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void ExecuteUndoCommand(int ActiveTabIndex)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            ExcelLikeUiGridExcelMarkerMouseController controller = new ExcelLikeUiGridExcelMarkerMouseController(this.grid1);
            ExcelLikeUiGridExcelMarkerMouseController controller2 = new ExcelLikeUiGridExcelMarkerMouseController(this.grid2);
            ExcelLikeUiGridExcelMarkerMouseController controller3 = new ExcelLikeUiGridExcelMarkerMouseController(this.grid3);
            this.grid1.Model.CommandStack.Enabled = true;
            this.grid2.Model.CommandStack.Enabled = true;
            this.grid3.Model.CommandStack.Enabled = true;
            this.grid1.MouseControllerDispatcher.Add(controller);
            this.grid2.MouseControllerDispatcher.Add(controller2);
            this.grid3.MouseControllerDispatcher.Add(controller3);
        }
    }
}
