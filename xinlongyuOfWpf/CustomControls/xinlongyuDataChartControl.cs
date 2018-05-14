using System.Collections.Generic;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls.Extension;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 图表控件
    /// </summary>
    public class xinlongyuDataChartControl: MyChartDataControl, IControl
    {
        /// <summary>
        /// 数据源类型
        /// 1、代表使用sql 2、代表使用键值对字符串
        /// </summary>
        private string _sourceType = "1";

        /// <summary>
        /// 由于通用接口的顺序问题
        /// 这里改为设置设置数据源
        /// </summary>
        /// <param name="value"></param>
        public void SetD0(object value)
        {
            _sourceType = value.ToString();
        }

        /// <summary>
        /// 字典转键值对数组
        /// </summary>
        /// <param name="dicArray"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, int>> DictionaryToListKeyValuePair(Dictionary<string, string>[] dicArray)
        {
            List<KeyValuePair<string, int>> listresult = new List<KeyValuePair<string, int>>();
            foreach(Dictionary<string, string> dic in dicArray)
            {
                List<string> keyList = new List<string>(dic.Keys);
                KeyValuePair<string, int> keyvalue = new KeyValuePair<string, int>(dic[keyList[0]], CommonConverter.StringToInt(dic[keyList[1]]));
                listresult.Add(keyvalue);
            }
            return listresult;
        }

        /// <summary>
        /// 设置图标类型
        /// </summary>
        /// <param name="text"></param>
        public void SetD5(string text)
        {
            this.SetType(CommonConverter.StringToInt(text));
        }
        /// <summary>
        /// 设置表格的数据
        /// </summary>
        /// <param name="value"></param>
        public void SetD6(string value)
        {
            //1、sql 2、字典实体
            if ("1".Equals(_sourceType))
            {
                string sql = value.ToString();
                SqlController cn = new SqlController();
                Dictionary<string, string>[] result = cn.ExcuteSqlWithReturn(sql).data;
                List<KeyValuePair<string, int>> source = this.DictionaryToListKeyValuePair(result);
                this.MyChart.DataContext = source;
            }
            else if ("2".Equals(_sourceType))
            {
                string valueStr = value.Trim();
                string[] valueArray = valueStr.Split('&');
                List<KeyValuePair<string, int>> dicTemp = new List<KeyValuePair<string, int>>();
                foreach (string str in valueArray)
                {
                    dicTemp.Add(new KeyValuePair<string, int>(str.Split('=')[0], CommonConverter.StringToInt(str.Split('=')[1])));
                }
                //foreach (string key in dicTemp.Keys)
                //{
                //    //暂时注释，后面h实现事件的时候顺便实现这个
                //    //LocalCacher.AddCache(key, DecoderAssistant.FormatSql(dicTemp[key], this));
                //}
            }
        }
    }
}
