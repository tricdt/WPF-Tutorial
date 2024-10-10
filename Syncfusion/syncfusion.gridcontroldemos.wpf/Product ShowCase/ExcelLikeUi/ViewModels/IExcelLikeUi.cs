
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    public interface IExcelLikeUi
    {
        void Initialize();
        void ExecuteCopyCommand(int ActiveTabIndex);
        void ExecuteCutCommand(int ActiveTabIndex);
        void ExecutePasteCommand(int ActiveTabIndex);
        void ExecuteFontSizeCommand(int ActiveTabIndex, bool IsIncrement);
        void ExecuteIndentCommand(int ActiveTabIndex, bool IsIncrement);
        void ExecuteOrientationCommand(int ActiveTabIndex);
        void CurrentCellStyleChanged(int ActiveTabIndex, string propertyName, object value);
        void ExecuteUndoCommand(int ActiveTabIndex);
        void ExecuteRedoCommand(int ActiveTabIndex);
        void ExecutePrintCommand(int ActiveTabIndex);
        void ColorPickerSelectedBrushChanged(string propertyName, Brush value);
    }
}
