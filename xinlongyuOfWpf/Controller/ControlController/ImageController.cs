using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;

namespace xinlongyuOfWpf.Controller.ControlController
{
    public class ImageController : BaseConnection
    {
        /// <summary>
        /// 图片请求
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public Image GetImage(string url)
        {
            //检查传入的值，对于函数来说，所有传入的值都要进行检查
            if (string.IsNullOrEmpty(url))
            {
                //加载默认图片
                return Properties.Resources.picture;
            }
            //这里由于暂时未加域名，所以这里拼上测试域名，后面应该不用做这个操作
            if (!url.StartsWith("http"))
            {
                url = @"https://icityservice.cn" + url;
            }
            //先尝试从本地中获取
            Image image = this.GetLocalImage(url);
            if (!object.Equals(image, null))
            {
                return image;
            }
            //通过服务获取图片
            image = this.GetWebImage(url);

            if (object.Equals(image, null))
            {
                image = Properties.Resources.picture;
            }
            else
            {
                //这里由于前面的文件流已经被垃圾回收器回收，所以这里需要拷贝然后新建一份
                using (Bitmap bmp = new Bitmap(image))
                {
                    bmp.Save(this.GetFullPathNameForImage(url), ImageFormat.Png);
                }
            }
            return image;
        }

        /// <summary>
        /// 获取本地图片
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="ctrlid"></param>
        /// <returns>获取到的图片</returns>
        public Image GetLocalImage(string url)
        {
            string filePath = this.GetFullPathNameForImage(url);
            if (File.Exists(filePath))
            {
                return Image.FromFile(filePath);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetFullPathNameForImage(string url)
        {
            //使用正则去除地址中的特殊符号
            string extension = Path.GetExtension(url);
            string pattern = @"[^a-z&&^0-9]";
            url = System.Text.RegularExpressions.Regex.Replace(url, pattern, string.Empty);
            string filepath = configManagerSection.localImageFolder + "\\" + url + extension;
            if (!Directory.Exists(configManagerSection.localImageFolder))
            {
                Directory.CreateDirectory(configManagerSection.localImageFolder);
            }
            return filepath;
        }
    }
}
