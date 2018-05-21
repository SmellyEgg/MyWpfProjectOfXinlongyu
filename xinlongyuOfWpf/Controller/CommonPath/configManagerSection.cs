
using System;
using System.IO;

namespace xinlongyuOfWpf.Controller.CommonPath
{
    /// <summary>
    /// 通用节点值
    /// </summary>
    public class ConfigManagerSection
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

        /// <summary>
        /// 通用api地址
        /// </summary>
        //public static readonly string serverUrl = @"https://icityservice.cn";
        public static readonly string serverUrl = @"http://192.168.1.157";

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public static int TitleBarHeight = 42;

        /// <summary>
        /// 控件名称前缀
        /// 这里是由于wpf的控件名称不能直接设置为id，就是123之类的这种数字类型的
        /// </summary>
        public static string ControlNamePrefix = "xinlongyuControl";
    }
}
