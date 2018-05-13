using System.Collections.Generic;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 缓存控件
    /// </summary>
    public class xinlongyuCacher : Button, IControl
    {
        public xinlongyuCacher()
        {
            //隐藏辅助控件
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="value"></param>
        public void SetD0(object value)
        {
            if (value is Dictionary<string, string>[])
            {
                if ((value as Dictionary<string, string>[]).Length > 0)
                {
                    foreach (Dictionary<string, string> dic in value as Dictionary<string, string>[])
                    {
                        foreach (string key in dic.Keys)
                        {
                            LocalCacher.AddCache(key, dic[key]);
                        }
                    }
                }
            }
            else if (value is Dictionary<string, string>)
            {
                var dic = value as Dictionary<string, string>;
                if (!object.Equals(dic, null))
                {
                    foreach (string key in dic.Keys)
                    {
                        LocalCacher.AddCache(key, dic[key]);
                    }
                }
            }
        }

        /// <summary>
        /// 缓存到本地
        /// </summary>
        /// <param name="value"></param>
        public void SetA2(object value)
        {
            //缓存的本地里面
            string valueStr = value.ToString().Trim();
            string[] valueArray = valueStr.Split('&');
            Dictionary<string, string> dicTemp = new Dictionary<string, string>();
            foreach (string str in valueArray)
            {
                dicTemp.Add(str.Split('=')[0], str.Split('=')[1]);
            }
            foreach (string key in dicTemp.Keys)
            {
                //暂时注释，后面h实现事件的时候顺便实现这个
                //LocalCacher.AddCache(key, DecoderAssistant.FormatSql(dicTemp[key], this));
            }
        }



        /// <summary>
        /// 从本地中获取
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SetA3(object value)
        {
            if (!object.Equals(value, null))
            {
                return LocalCacher.GetCache(value.ToString());
            }
            return null;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="value"></param>
        public void SetA4(object value)
        {
            if (!object.Equals(value, null))
            {
                LocalCacher.DeleteCache(value.ToString());
            }
        }

    }
}
