using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Models.CommonModel;

namespace xinlongyuOfWpf.Controller.ControlController
{
    /// <summary>
    /// 本地缓存类
    /// 暂时用于存放登陆信息
    /// </summary>
    public class LocalCacher
    {
        /// <summary>
        /// 缓存的图片
        /// </summary>
        public static Dictionary<string, BitmapImage> _ListCachedPhoto = new Dictionary<string, BitmapImage>();

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddCache(string key, string value)
        {

            List<KeyToValue> _commonDataDictionay = new List<KeyToValue>();
            if (File.Exists(ConfigManagerSection.localCache))
            {
                _commonDataDictionay = ClassToXml.ReadFromXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache);
            }
            if (object.Equals(_commonDataDictionay, null))
            {
                _commonDataDictionay = new List<KeyToValue>();
            }
            if (_commonDataDictionay.FindIndex(p => p.Key.Equals(key)) == -1)
            {
                _commonDataDictionay.Add(new KeyToValue(key, value));
                ClassToXml.WriteToXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache, _commonDataDictionay);
            }
            else
            {
                int index = _commonDataDictionay.FindIndex(p => p.Key.Equals(key));
                _commonDataDictionay[index].Value = value;
                ClassToXml.WriteToXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache, _commonDataDictionay);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCache(string key)
        {
            List<KeyToValue> _commonDataDictionay = new List<KeyToValue>();
            if (File.Exists(ConfigManagerSection.localCache))
            {
                _commonDataDictionay = ClassToXml.ReadFromXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache);
            }
            if (object.Equals(_commonDataDictionay, null))
            {
                return string.Empty;
            }
            else if (_commonDataDictionay.FindIndex(p => p.Key.Equals(key)) != -1)
            {
                return _commonDataDictionay.First(p => p.Key.Equals(key)).Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteCache(string key)
        {
            List<KeyToValue> _commonDataDictionay = new List<KeyToValue>();
            if (File.Exists(ConfigManagerSection.localCache))
            {
                _commonDataDictionay = ClassToXml.ReadFromXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache);
            }
            if (object.Equals(_commonDataDictionay, null))
            {
                return;
            }
            else if (_commonDataDictionay.FindIndex(p => p.Key.Equals(key)) != -1)
            {
                _commonDataDictionay.RemoveAt(_commonDataDictionay.FindIndex(p => p.Key.Equals(key)));
                ClassToXml.WriteToXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache, _commonDataDictionay);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void ClearAllCache()
        {
            List<KeyToValue> _commonDataDictionay = new List<KeyToValue>();
            if (File.Exists(ConfigManagerSection.localCache))
            {
                _commonDataDictionay = ClassToXml.ReadFromXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache);
            }
            if (object.Equals(_commonDataDictionay, null) || _commonDataDictionay.Count < 1)
            {
                return;
            }
            else
            {
                _commonDataDictionay.Clear();
                ClassToXml.WriteToXmlFile<List<KeyToValue>>(ConfigManagerSection.localCache, _commonDataDictionay);
            }

        }
    }
}
