
using System;
using System.IO;

namespace xinlongyuOfWpf.Controller.CommonPath
{
    /// <summary>
    /// 通用节点值
    /// </summary>
    public class configManagerSection
    {
        /// <summary>
        /// 默认加载的页面ID
        /// </summary>
        public static int defaultPageId = 2073;

        /// <summary>
        /// url
        /// </summary>
        public static string urlPath = "/Settings/Url";

        /// <summary>
        /// 第一个加载的页面ID
        /// </summary>
        public static string firstPageID = "/Settings/FirstPageID";

        /// <summary>
        /// 通用配置文件
        /// </summary>
        public static string CommonconfigFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\xinLongIDE.xml";

        /// <summary>
        /// 本地缓存图片
        /// </summary>
        public static string localImageFolder = AppDomain.CurrentDomain.BaseDirectory + @"\PageInfo\Image";

        /// <summary>
        /// 本地缓存信息
        /// </summary>
        public static string localCache = AppDomain.CurrentDomain.BaseDirectory + @"\Cache\LocalCacher.xml";

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public static string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ss_Egg_temp");

        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string dataBasePath = AppDomain.CurrentDomain.BaseDirectory + @"\Database\xinlongyuClient.db";
    }
}
