using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 图片控件
    /// </summary>
    public class xinlongyuImageControl : System.Windows.Controls.Image, IControl
    {
        private ImageController _imageController;

        public xinlongyuImageControl()
        {
            this.Stretch = System.Windows.Media.Stretch.Uniform;
            _imageController = new ImageController();
        }

        /// <summary>
        /// 设置控件的值
        /// 这里由于是图片控件，所以需要重写一下
        /// </summary>
        /// <param name="text"></param>
        public void SetD0(object text)
        {
            string url = string.Empty;
            if (text is Dictionary<string, string>[])
            {
                Dictionary<string, string>[] dicdate = text as Dictionary<string, string>[];
                if (dicdate.Length > 0)
                {
                    url = dicdate[0][dicdate[0].First().Key];
                }
            }
            else if (text is string)
            {
                url = text.ToString();
            }

            if (string.IsNullOrEmpty(url.Trim()))
            {
                this.Source = CommonConverter.ImageToBitMapImage(Properties.Resources.picture);
                //this.Receiver.Source = CommonConverter.ImageToBitMapImage(Properties.Resources.picture);
                return;
            }

            try
            {
                if (!url.StartsWith("http"))
                {
                    //url = @"https://icityservice.cn" + url;
                    //url = @"http://192.168.1.157" + url;
                    url = Controller.CommonPath.ConfigManagerSection.serverUrl + url;
                }
                this.Source = new BitmapImage(new Uri(url));
            }
            catch
            {
                this.Source = CommonConverter.ImageToBitMapImage(Properties.Resources.picture);
            }
        }

        
    }
}
