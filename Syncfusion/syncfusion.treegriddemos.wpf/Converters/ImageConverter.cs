using System.Globalization;
using System.Windows.Data;

namespace syncfusion.treegriddemos.wpf
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagesource = value.ToString();
            if (imagesource != "DriveNode" && imagesource != "Directory")
                return null;
            imagesource = imagesource.Replace(" ", "");
            if (imagesource != null)
            {
                return @"/syncfusion.treegriddemos.wpf;component/Assets/treegrid/" + imagesource + ".png";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
