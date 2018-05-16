using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// 通用转换类
    /// </summary>
    public class CommonConverter
    {
        /// <summary>
        /// 字符串转布尔值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean StringToBool(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            else if ("1".Equals(str) || "True".Equals(str) || "true".Equals(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串转整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StringToInt(string str)
        {
            try
            {
                int result = Convert.ToInt32(str);
                return result;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 字符串转浮点
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float StringToFloat(string str)
        {
            try
            {
                float result = float.Parse(str);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 转换颜色
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ConvertStringToColor(String hex)
        {
            //remove the # at the fron
            if (string.IsNullOrEmpty(hex)) return System.Windows.Media.Colors.AliceBlue;
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// 图片转换
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static BitmapImage ImageToBitMapImage(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        /// <summary>
        /// 转换字体
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static System.Windows.Media.FontFamily FontToMediaFontFamily(Font f)
        {
            Typeface typeface = null;

            System.Windows.Media.FontFamily ff = new System.Windows.Media.FontFamily(f.Name);


            if (typeface == null)
            {
                typeface = new Typeface(ff, (f.Style == System.Drawing.FontStyle.Italic ? FontStyles.Italic : FontStyles.Normal),
                                 (f.Style == System.Drawing.FontStyle.Bold ? FontWeights.Bold : FontWeights.Normal),
                                            FontStretches.Normal);
            }
            if (typeface == null)
            {
                typeface = new Typeface(new System.Windows.Media.FontFamily("Arial"),
                                                FontStyles.Italic,
                                                FontWeights.Normal,
                                                FontStretches.Normal);
            }
            return typeface.FontFamily;

        }
    }
}
