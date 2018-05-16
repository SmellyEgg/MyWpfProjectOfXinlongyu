using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;

namespace xinlongyuOfWpf.CustomControls
{
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
                            //Form frm = (this as Control).FindForm();
                            //if (!object.Equals(frm, null) && !(frm as xinlongyuForm)._pageCache.ContainsKey(key))
                            //{
                            //    (frm as xinlongyuForm)._pageCache.Add(key, dic[key]);
                            //}
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
            //var frm = (this as Control).FindForm();
            Dictionary<string, string> dicTemp = new Dictionary<string, string>();
            foreach (string str in valueArray)
            {
                dicTemp.Add(str.Split('=')[0], str.Split('=')[1]);
                //(frm as xinlongyuForm)._pageCache.Add(str.Split('=')[0], str.Split('=')[1]);
            }
            foreach (string key in dicTemp.Keys)
            {
                //if (!object.Equals(frm, null) && !(frm as xinlongyuForm)._pageCache.ContainsKey(key))
                //{
                //    (frm as xinlongyuForm)._pageCache.Add(key, DecoderAssistant.FormatSql(dicTemp[key], this));
                //}
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
                //Form frm = (this as Control).FindForm();
                //if (!object.Equals(frm, null) && (frm as xinlongyuForm)._pageCache.ContainsKey(value.ToString()))
                //{
                //    return (frm as xinlongyuForm)._pageCache[value.ToString()];
                //}
            }
            return null;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="value"></param>
        public void SetA4(object value)
        {
            //if (!object.Equals(value, null))
            //{
            //    Form frm = (this as Control).FindForm();
            //    if (!object.Equals(frm, null) && (frm as xinlongyuForm)._pageCache.ContainsKey(value.ToString()))
            //    {
            //        (frm as xinlongyuForm)._pageCache.Remove(value.ToString());
            //    }
            //}
        }

        /// <summary>
        /// 获取当前的缓存实体
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(object value)
        {
            //Form frm = (this as Control).FindForm();
            //if (!object.Equals(frm, null) && !object.Equals((frm as xinlongyuForm)._pageCache, null)
            //    && (frm as xinlongyuForm)._pageCache.Keys.Count > 0)
            //{
            //    return (frm as xinlongyuForm)._pageCache;
            //}
            return null;
        }
    }
}
