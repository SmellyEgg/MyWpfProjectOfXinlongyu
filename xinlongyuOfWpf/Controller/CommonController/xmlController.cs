using System;
using System.Xml;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// xml控制类
    /// </summary>
    public class xmlController
    {
        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="xpath">xpath路径</param>
        /// <param name="filepath">配置文件路径</param>
        /// <returns></returns>
        public static string GetNodeByXpath(string xpath, string filepath)
        {
            XmlDocument xc = new XmlDocument();
            xc.Load(filepath);
            if (!object.Equals(xc, null))
            {
                XmlNode node = xc.SelectSingleNode(xpath);
                if (node == null)
                {
                    throw new Exception("没有找到配置的节点结点！");
                }
                string result = node.Attributes[0].Value;
                if (!string.IsNullOrEmpty(result)) return result;
            }
            return string.Empty;
        }

    }
}
