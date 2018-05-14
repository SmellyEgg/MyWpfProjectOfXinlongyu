using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using xinlongyuOfWpf.Controller.CommonPath;

namespace xinlongyuOfWpf.CustomControls.Extension
{
    /// <summary>
    /// 字符串与图像转换类
    /// </summary>
    public class StringToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string uristring = value as string;
            if (!uristring.StartsWith("http"))
            {
                uristring = ConfigManagerSection.serverUrl + uristring;
            }
            return new BitmapImage(new Uri(uristring, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
