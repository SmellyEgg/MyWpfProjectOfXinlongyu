using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Controller.ControlController;

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
            if (string.IsNullOrEmpty(uristring.Trim()))
            {
                return CommonConverter.ImageToBitMapImage(Properties.Resources.picture);
            }
            if (!uristring.StartsWith("http"))
            {
                uristring = ConfigManagerSection.serverUrl + uristring;
            }
            if (LocalCacher._ListCachedPhoto.ContainsKey(uristring))
            {
                return LocalCacher._ListCachedPhoto[uristring];
            }

            BitmapImage img = new BitmapImage(new Uri(uristring, UriKind.RelativeOrAbsolute));
            if (!object.Equals(img, null))
            {
                LocalCacher._ListCachedPhoto.Add(uristring, img);
                return img;
            }
            else if (object.Equals(img, null))
            {
                return CommonConverter.ImageToBitMapImage(Properties.Resources.picture);
            }
            else
            {
                return img;
            }
            //return new BitmapImage(new Uri(uristring, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
