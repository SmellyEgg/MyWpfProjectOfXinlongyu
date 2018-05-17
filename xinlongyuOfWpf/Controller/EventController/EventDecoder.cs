using System.Collections.Generic;
using System.Text.RegularExpressions;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Models.DecodeModel;

namespace xinlongyuOfWpf.Controller.EventController
{
    /// <summary>
    /// 事件解析类
    /// 主要负责把文本转换成事件实体
    /// </summary>
    public class EventDecoder
    {
        /// <summary>
        /// 解析事件数组
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        public List<DecoderOfControl> DecodeEvent(string inText)
        {
            string[] array = JsonController.DeSerializeToClass<string[]>(inText);
            List<DecoderOfControl> list = new List<DecoderOfControl>();
            foreach (string eventText in array)
            {
                list.Add(DecodeNewCharacter(eventText));
            }
            return list;
        }

        /// <summary>
        /// 解析事件实体
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        public DecoderOfControl DecodeNewCharacter(string inText)
        {
            DecoderOfControl control = new DecoderOfControl();
            inText = inText.Trim();
            if (inText.ToLower().StartsWith("sql:"))
            {
                //sql语法的进行单独处理
                return DecodeSqlEvent(inText);
            }

            if (inText.IndexOf("=") != -1 && Regex.IsMatch(inText, @"\d+.d\d+ *= *\w*"))
            {
                if (Regex.IsMatch(inText, @"\d+.d\d+ *=.*"))
                {
                    string leftPart = inText.Trim().Split('=')[0];
                    leftPart = Regex.Match(leftPart, @"\d+.\w\d+").Groups[0].ToString();

                    int rightIndex = inText.Trim().IndexOf("=");
                    string rightPart = inText.Substring(rightIndex + 1, inText.Length - rightIndex - 1).Trim();

                    this.DecodeLeftPart(leftPart, control);
                    this.DecodeRightPart(rightPart, control);
                }
            }
            else if (Regex.IsMatch(inText, @"\d+.a\d+[(]((.|\n)*)[)]"))
            {
                if (Regex.IsMatch(inText, @"\d+.a\d+[(]\d+,\w+=.*"))
                {
                    string leftParttern = Regex.Match(inText, @"[(]((.|\n)*)[)]").Groups[0].ToString().Trim();
                    string leftPart = inText.Replace(leftParttern, string.Empty);
                    leftPart = Regex.Match(leftPart, @"\d+.\w\d+").Groups[0].ToString();
                    control.CtrlId = CommonConverter.StringToInt(leftPart.Split('.')[0]);
                    control.LeftCtrlProperty = leftPart.Split('.')[1];
                    control.RightDirectValue = leftParttern.Substring(1, leftParttern.Length - 2);
                }
                else
                {
                    control.CtrlId = CommonConverter.StringToInt(inText.Split('.')[0]);
                    string rightpart = inText.Substring(inText.IndexOf(".") + 1);
                    control.RightDirectValue = Regex.Match(rightpart, @"[(]((.|\n)*)[)]").Groups[1].ToString();
                    control.LeftCtrlProperty = rightpart.Replace(Regex.Match(rightpart, @"[(]((.|\n)*)[)]").Groups[0].ToString(), string.Empty);
                }
            }
            //事件语法，例如12.a1
            else if (Regex.IsMatch(inText, @"\d+.a|A\d+"))
            {
                control.CtrlId = CommonConverter.StringToInt(inText.Trim().Split('.')[0]);
                control.LeftCtrlProperty = inText.Trim().Split('.')[1];
            }
            //事件语法，例如12.a5(1236)

            return control;
        }

        /// <summary>
        /// sql语法处理
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        private DecoderOfControl DecodeSqlEvent(string inText)
        {
            DecoderOfControl control = new DecoderOfControl();
            control.Type = EventType.SqlType;
            control.RightDirectValue = inText.Trim().Substring(4, inText.Trim().Length - 4);
            return control;
        }

        /// <summary>
        /// 解析左半边部分
        /// </summary>
        /// <param name="leftPart"></param>
        private void DecodeLeftPart(string leftPart, DecoderOfControl control)
        {
            control.CtrlId = CommonConverter.StringToInt(leftPart.Trim().Split('.')[0]);
            //排除掉"."符号
            control.LeftCtrlProperty = leftPart.Trim().Split('.')[1];
        }

        /// <summary>
        /// 解析右半边部分
        /// </summary>
        /// <param name="rightPart"></param>
        /// <param name="control"></param>
        private void DecodeRightPart(string rightPart, DecoderOfControl control)
        {
            //先看看简单的情况
            if (!rightPart.StartsWith("{"))
            {
                control.RightDirectValue = rightPart;
            }
            else
            {
                //右边也包含控件ID的情况
                string pattern = @"{(.*)}";
                string trimPattern = Regex.Match(rightPart, pattern).Groups[1].ToString();
                string[] rightpart = trimPattern.Split('.');
                DecoderOfControl obj = new DecoderOfControl();
                obj.CtrlId = CommonConverter.StringToInt(rightpart[0]);
                //obj.LeftCtrlProperty = rightpart[1];
                if (Regex.IsMatch(trimPattern, @"\d+.a\d+[(]\w*[)]"))
                {
                    //取参数值
                    obj.Patameter = Regex.Match(trimPattern, @"[(](.*)[)]").Groups[1].ToString();
                    obj.LeftCtrlProperty = rightpart[1].Replace(Regex.Match(trimPattern, @"[(](.*)[)]").Groups[0].ToString(), string.Empty);
                }
                else
                {
                    obj.LeftCtrlProperty = rightpart[1];
                }
                control.RightDirectValue = obj;
            }
        }


    }
}
