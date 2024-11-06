using System.Globalization;
using System.Windows.Data;

namespace syncfusion.treegriddemos.wpf
{
    public class DriveTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fileNode = value as FileInfoModel;
            if (fileNode != null)
            {
                return fileNode.TotalFreeSpace + " GB free of " + fileNode.TotalSize + " GB";
            }
            else
            {
                var val = value.ToString();
                if (!string.IsNullOrEmpty(val))
                {
                    string[] st = val.Split('\\');
                    return "Local Disc (" + st[0] + ")";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
