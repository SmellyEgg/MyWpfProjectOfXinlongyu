using System.Collections.Generic;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 页面缓存控件
    /// </summary>
    public class xinlongyuPageCacher : Button, IControl
    {
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
                            var page = CommonFunction.GetPageByControl(this);
                            if (object.Equals(page._pageCache, null)) page._pageCache = new Dictionary<string, string>();
                            if (page._pageCache.ContainsKey(key))
                            {
                                page._pageCache[key] = dic[key];
                            }
                            else
                            {
                                page._pageCache.Add(key, dic[key]);
                            }
                        }
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
            var page = CommonFunction.GetPageByControl(this);
            foreach (string str in valueArray)
            {
                var key = str.Split('=')[0];
                var myvalue = str.Split('=')[1];
                //这里不用进行判断，如果存在相同的key，直接进行覆盖
                if (page._pageCache.ContainsKey(key))
                {
                    page._pageCache[key] = myvalue;
                }
                else
                {
                    page._pageCache.Add(key, myvalue);
                }
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
                var page = CommonFunction.GetPageByControl(this);
                if (page._pageCache.ContainsKey(value.ToString()))
                {
                    return page._pageCache[value.ToString()];
                }
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
                var page = CommonFunction.GetPageByControl(this);
                if (page._pageCache.ContainsKey(value.ToString()))
                {
                    page._pageCache.Remove(value.ToString());
                }
            }
        }

        /// <summary>
        /// 获取当前的缓存实体
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(object value)
        {
            var page = CommonFunction.GetPageByControl(this);
            return page._pageCache;
        }
    }
}
