using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.DecodeModel;

namespace xinlongyuOfWpf.Controller.EventController
{
    /// <summary>
    /// 事件解析辅助类
    /// </summary>
    public class EventAssitant
    {
        /// <summary>
        /// 当前控件列表
        /// </summary>
        public static List<IControl> CurrentControlList { get; set; }

        /// <summary>
        /// 当前控件实体
        /// </summary>
        public static List<ControlDetailForPage> CurrentControlObjList { get; set; }

        /// <summary>
        /// 设置单击事件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        public static void SetControlClickEvent(IControl target, string text)
        {
            //通用单击事件，但是在wpf中暂时不知道要怎么实现
            //主要是控件基类不实现单击事件，所以这里估计没有办法做成通用的
            //if (!object.Equals(commonControl, null))
            //{
            //    commonControl.click
            //}
            //(target as FrameworkElement). += (s, e) => {
            //    if (!string.IsNullOrEmpty(text))
            //    {
            //        InvokeEvent(target, text);
            //    }
            //};
        }

        /// <summary>
        /// 设置当前激活的控件实体数组
        /// </summary>
        /// <param name="ic"></param>
        private static void SetCurrentControlList(IControl ic)
        {
            var page= Window.GetWindow(ic as UIElement).Content as xinlongyuForm;
            CurrentControlList = page._currentControlList;
            CurrentControlObjList = page._currentControlObjList;
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        private static void InvokeEvent(IControl target, string text)
        {
            EventDecoder ed = new EventDecoder();
            List<DecoderOfControl> listEvent = ed.DecodeEvent(text);
            foreach (DecoderOfControl docObj in listEvent)
            {
                try
                {
                    SetCurrentControlList(target);
                    int ctrlIndex = CurrentControlList.FindIndex
                        (p => (p as Control).Name.Equals(ConfigManagerSection.ControlNamePrefix + docObj.CtrlId.ToString()));
                    if (ctrlIndex == -1 && !docObj.Type.Equals(EventType.SqlType))
                    {
                        return;
                    }
                    else
                    {
                        CallMethodByPropertyName(CurrentControlList[ctrlIndex], docObj);
                    }
                }
                catch
                {
                    MessageBox.Show("执行事件出错--" + text);
                }
            }
        }

        /// <summary>
        /// 直接执行事件
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ic"></param>
        public static void CallEventDerectly(string text, IControl ic)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            SetCurrentControlList(ic);
            EventDecoder ed = new EventDecoder();
            List<DecoderOfControl> listEvent = ed.DecodeEvent(text);
            foreach (DecoderOfControl docObj in listEvent)
            {
                SetCurrentControlList(ic);
                ExcuteOneEvent(docObj, ic);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="docObj"></param>
        /// <param name="ic"></param>
        public static void ExcuteOneEvent(DecoderOfControl docObj, IControl ic)
        {
            SetCurrentControlList(ic);
            int ctrlIndex = CurrentControlList.FindIndex
                (p => (p as FrameworkElement).Name.Equals(ConfigManagerSection.ControlNamePrefix + docObj.CtrlId.ToString()));
            if (ctrlIndex == -1 && !docObj.Type.Equals(EventType.SqlType))
            {
                return;
            }
            else
            {
                CallMethodByPropertyName(CurrentControlList[ctrlIndex], docObj);
            }
        }

        /// <summary>
        /// 事件里面的方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private static void CallMethodByPropertyName(IControl control, DecoderOfControl value)
        {

            if (value.Type.Equals(EventType.SqlType))
            {
                dealWithSqlRequest(value, control);
                return;
            }

            object controlValue = null;
            if (value.RightDirectValue is DecoderOfControl)
            {
                DecoderOfControl rightValue = value.RightDirectValue as DecoderOfControl;
                IControl rightControl = CurrentControlList.First(p => rightValue.CtrlId == ((p as FrameworkElement).Tag as ControlDetailForPage).ctrl_id);
                controlValue = GetValueWithParameter(rightControl, rightValue.LeftCtrlProperty, rightValue.Patameter);
            }
            else
            {
                controlValue = object.Equals(value.RightDirectValue, null) ? string.Empty : value.RightDirectValue;
            }
            SetValue(control, controlValue, value.LeftCtrlProperty);
        }

        /// <summary>
        /// 处理sql请求
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ic"></param>
        /// <returns></returns>
        public static bool dealWithSqlRequest(DecoderOfControl value, IControl ic)
        {
            string sqlStr = @FormatSql(value.RightDirectValue.ToString(), ic);
            SqlController cnTemp = new SqlController();
            sqlStr = FormatSql(sqlStr, ic);
            var result = cnTemp.ExcuteSqlWithReturn(sqlStr);
            if (result.error_code.Equals("1"))
            {
                return false;
            }
            string resultControlId = string.Empty;
            string resultControlProperty = string.Empty;
            if (Regex.IsMatch(result.to, @"\d+.d\d+"))
            {
                resultControlId = GetStringExceptChar(result.to).Split('.')[0];
                resultControlProperty = GetStringExceptChar(result.to).Split('.')[1];
            }
            else
            {
                resultControlId = result.to;
            }
            if (!string.IsNullOrEmpty(resultControlId))
            {
                int ctrlIndex = CurrentControlList.FindIndex(p => (p as Control).Name.Equals(resultControlId));
                SetValue(CurrentControlList[ctrlIndex], result.data, resultControlProperty);
            }
            return true;
        }

        /// <summary>
        /// 去除字符串两边的大括号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string GetStringExceptChar(string str)
        {
            if (str.StartsWith("{"))
            {
                return str.Substring(1, str.Length - 2);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 赋值操作
        /// </summary>
        /// <param name="control"></param>
        /// <param name="controlValue"></param>
        /// <param name="propertyName"></param>
        private static void SetValue(IControl control, object controlValue, string propertyName)
        {
            if (!object.Equals(controlValue, null))
            {
                switch (propertyName.ToLower())
                {
                    case "d0":
                        if (controlValue is string)
                        {
                            //处理转义符的换行问题
                            controlValue = FormatSql(controlValue.ToString(), control);
                            controlValue = controlValue.ToString().Replace("#newline#", "\r\n");
                        }
                        control.SetD0(controlValue);
                        break;
                    case "d1":
                        control.SetD1(CommonConverter.StringToInt(controlValue.ToString()));
                        break;
                    case "d2":
                        control.SetD2(CommonConverter.StringToInt(controlValue.ToString()));
                        break;
                    case "d3":
                        control.SetD3D4(CommonConverter.StringToInt(controlValue.ToString()), (int)(control as FrameworkElement).Margin.Left);
                        break;
                    case "d4":
                        control.SetD3D4((int)(control as FrameworkElement).Margin.Top, CommonConverter.StringToInt(controlValue.ToString()));
                        break;
                    case "d5":
                        control.SetD5(controlValue.ToString());
                        break;
                    case "d6":
                        control.SetD6(controlValue.ToString());
                        break;
                    case "d7":
                        control.SetD7(controlValue.ToString());
                        break;
                    case "d15":
                        control.SetD15(controlValue.ToString());
                        break;
                    case "d20":
                        control.SetD20(controlValue.ToString());
                        break;

                    case "a0":
                        control.SetA0(string.Empty);
                        break;
                    case "a1":
                        control.SetA1(controlValue.ToString());
                        break;
                    //缓存事件
                    case "a2":
                        control.SetA2(controlValue.ToString());
                        break;
                    //最小化窗体的方法
                    case "a4":
                        control.SetA4(controlValue.ToString());
                        break;
                    //跳转页面方法
                    case "a5":
                        control.SetA5(controlValue.ToString());
                        break;
                    //对话框跳转页面方法
                    case "a6":
                        control.SetA6(controlValue.ToString());
                        break;
                    case "a7":
                        control.SetA7(string.Empty);
                        break;
                    case "a8":
                        control.SetA8(controlValue.ToString());
                        break;
                    //这里是默认调用的方法
                    case "":
                        control.SetD0(controlValue);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获取控件的值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetValue(IControl control, string property)
        {
            object obj = null;
            switch (property.ToLower())
            {
                case "d0":
                    obj = control.GetD0(string.Empty);
                    break;
                case "d1":
                    obj = control.GetD1(string.Empty);
                    break;
                case "d2":
                    obj = control.GetD2(string.Empty);
                    break;
                case "d20":
                    obj = control.GetD20(string.Empty);
                    break;
                default:
                    break;
            }
            return obj;
        }

        /// <summary>
        /// 根据参数获取控件中的值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="property"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static object GetValueWithParameter(IControl control, string property, string parameter)
        {
            object obj = null;
            switch (property.ToLower())
            {
                case "d0":
                    obj = control.GetD0(parameter);
                    break;
                case "d1":
                    obj = control.GetD1(parameter);
                    break;
                case "d2":
                    obj = control.GetD2(parameter);
                    break;
                case "a3":
                    obj = control.SetA3(parameter);
                    break;
                default:
                    break;
            }
            return obj;
        }

        /// <summary>
        /// 格式化sql，主要就是为了讲里面的参数赋值
        /// </summary>
        /// <param name="inText"></param>
        /// <returns></returns>
        public static string FormatSql(string inText, IControl ic)
        {
            SetCurrentControlList(ic);
            string pattern = @"[{]\d+.d\d+[}]";
            string resultSql = inText.Clone().ToString();
            foreach (var data in Regex.Matches(inText, pattern))
            {
                string ctrlId = data.ToString().Substring(1, data.ToString().Length - 2).Split('.')[0];
                string ctrlProperty = data.ToString().Substring(1, data.ToString().Length - 2).Split('.')[1];

                try
                {
                    int controlIndex = CurrentControlList.FindIndex(p => CommonConverter.StringToInt(ctrlId) == ((p as Control).Tag as ControlDetailForPage).ctrl_id);
                    if (controlIndex != -1)
                    {
                        IControl rightControl = CurrentControlList[controlIndex];
                        string paramter = GetValue(rightControl, ctrlProperty).ToString();
                        resultSql = resultSql.Replace(data.ToString(), paramter);
                    }
                    else
                    {
                        resultSql = resultSql.Replace(data.ToString(), string.Empty);
                    }
                }
                catch
                {
                    resultSql = resultSql.Replace(data.ToString(), string.Empty);
                }
            }

            string secondPattern = @"[{]\d+.a\d+[(]\w+[)][}]";
            foreach (var data in Regex.Matches(inText, secondPattern))
            {
                string pairStr = data.ToString().Substring(1, data.ToString().Length - 2);
                string parameter = Regex.Match(pairStr.Split('.')[1], @"\((.*?)\)").Groups[1].ToString();
                string ctrlProperty = pairStr.Split('.')[1].Replace(Regex.Match(pairStr.Split('.')[1], @"\((.*?)\)").Groups[0].ToString(), string.Empty);
                string ctrlId = pairStr.Split('.')[0];

                try
                {
                    int controlIndex = CurrentControlList.FindIndex(p => CommonConverter.StringToInt(ctrlId) == ((p as Control).Tag as ControlDetailForPage).ctrl_id);
                    if (controlIndex != -1)
                    {
                        IControl rightControl = CurrentControlList[controlIndex];
                        string paramter = GetValueWithParameter(rightControl, ctrlProperty, parameter).ToString();
                        resultSql = resultSql.Replace(data.ToString(), paramter);
                    }
                    else
                    {
                        resultSql = resultSql.Replace(data.ToString(), string.Empty);
                    }
                }
                catch
                {
                    resultSql = resultSql.Replace(data.ToString(), string.Empty);
                }
            }
            return resultSql;
        }
    }
}
