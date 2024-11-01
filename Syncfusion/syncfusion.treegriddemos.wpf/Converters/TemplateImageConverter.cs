using Syncfusion.UI.Xaml.TreeGrid.Cells;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace syncfusion.treegriddemos.wpf
{
    public class TemplateImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var dataContextHelper = value as TreeGridDataContextHelper;
            var record = dataContextHelper.Record as FileInfoModel;
            if (record.FileType == "DriveNode")
                return new BitmapImage(new Uri(@"/syncfusion.treegriddemos.wpf;component/Assets/treegrid/DriveNode.png", UriKind.Relative));
            else if (record.FileType == "Directory")
                return new BitmapImage(new Uri(@"/syncfusion.treegriddemos.wpf;component/Assets/treegrid/Directory.png", UriKind.Relative));
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
