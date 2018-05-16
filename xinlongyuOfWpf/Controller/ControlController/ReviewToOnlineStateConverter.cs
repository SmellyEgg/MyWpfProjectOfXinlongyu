using System;
using System.Globalization;
using System.Windows.Data;

namespace xinlongyuOfWpf.Controller.ControlController
{
    /// <summary>
    /// review转在线状态
    /// </summary>
    public class ReviewToOnlineStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            string uristring = value as string;
            if (string.IsNullOrEmpty(uristring)) return "未上线";
            if ("1".Equals(uristring)) return "已上线";
            else return "未上线";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
