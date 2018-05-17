using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.Controller.ControlController
{
    /// <summary>
    /// 控件的统一接口
    /// 所有控件都需要实现这个接口
    /// </summary>
    public interface IControl
    {
    }

    /// <summary>
    /// 这里是需要给子类的扩展方法
    /// </summary>
    public static class IControlAddtion
    {
        //属性设置
        public static void SetD1<T>(this T inControl, int width) where T : IControl
        {
            if (width != -1)
            {
                (inControl as FrameworkElement).Width = width;
            }
        }

        public static void SetD2<T>(this T inControl, int height) where T : IControl
        {
            (inControl as FrameworkElement).Height = height;
        }

        /// <summary>
        /// 坐标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void SetD3D4<T>(this T inControl, int X, int Y) where T : IControl
        {
            (inControl as FrameworkElement).Margin = new System.Windows.Thickness(X, Y, 0, 0);
            //(inControl as FrameworkElement).SetValue(Canvas.LeftProperty, X);
            //(inControl as FrameworkElement).SetValue(Canvas.RightProperty, Y);
        }

        /// <summary>
        /// 设置D5属性，一般是行高属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD5<T>(this T inControl, string text) where T : IControl
        {
            if (!object.Equals(text, null))
            {
                ControlDetailForPage ctobj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
                ctobj.d5 = text;
            }
            ExcuteClassMethodByname(inControl, "SetD5", text);
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD6<T>(this T inControl, string text) where T : IControl
        {
            if (!object.Equals(text, null))
            {
                ControlDetailForPage ctobj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
                ctobj.d6 = text;
            }
            //这里增加一个判断流程是为了兼容字体样式之前的版本，防止需要全部回去修改配置
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    if (!string.IsNullOrEmpty(text) && CommonConverter.StringToInt(text) == -1)
                        ExcuteClassMethodByname(inControl, "SetD6", text);
                }
                catch
                { }
                //if (ExcuteClassMethodByname(inControl, "SetD6", text) == ReturnConst.CancleReturn)
                //{
                //}
            }
        }

        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD7<T>(this T inControl, string text) where T : IControl
        {
            if (!object.Equals(text, null))
            {
                ControlDetailForPage ctobj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
                ctobj.d7 = text;
                ExcuteClassMethodByname(inControl, "SetD7", text);
            }

            //(inControl as UIElement).Foreground = new SolidColorBrush(CommonConverter.ConvertStringToColor(text));
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD8<T>(this T inControl, string text) where T : IControl
        {
            if (!object.Equals(text, null))
            {
                ControlDetailForPage ctobj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
                ctobj.d8 = text;
                ExcuteClassMethodByname(inControl, "SetD8", text);
            }

            if (!string.IsNullOrEmpty(text))
            {
                //(inControl as FrameworkElement).Background = new SolidColorBrush(CommonConverter.ConvertStringToColor(text));
            }
        }

        /// <summary>
        /// 字体对齐方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD9<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetD9", text);
        }

        public static void SetD10<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d10 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD10", text);
        }

        public static void SetD11<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d11 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD11", text);
        }


        /// <summary>
        /// 设置控件是否接收事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD30<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d30 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            if ("0".Equals(text.Trim()))
            {
                (inControl as FrameworkElement).IsEnabled = false;
            }
            else
            {
                (inControl as FrameworkElement).IsEnabled = true;
            }
            ExcuteClassMethodByname(inControl, "SetD30", text);
        }
        /// <summary>
        /// 激活以及离开时背景颜色和字体颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD10<T>(this T inControl, string backcolorText, string fontColortext) where T : IControl
        {
            if (!(inControl as FrameworkElement).Visibility.Equals(System.Windows.Visibility.Visible)) return;
            //暂时不进行处理
            //Color originalColor = (inControl as FrameworkElement).Background;
            //Color originalFont = (inControl as FrameworkElement).ForeColor;
            //(inControl as FrameworkElement).GotFocus += (s, e) =>
            //{
            //    if (!string.IsNullOrEmpty(backcolorText))
            //    {
            //        (inControl as FrameworkElement).BackColor = ColorTranslator.FromHtml(backcolorText);
            //    }
            //    if (!string.IsNullOrEmpty(fontColortext))
            //    {
            //        (inControl as FrameworkElement).ForeColor = ColorTranslator.FromHtml(fontColortext);

            //    }
            //};
            //(inControl as FrameworkElement).LostFocus += (s, e) =>
            //{
            //    (inControl as FrameworkElement).BackColor = originalColor;
            //    (inControl as FrameworkElement).ForeColor = originalFont;
            //};

            //Color.FromArgb
        }

        /// <summary>
        /// 是否自适应大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD12<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d12 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD12", text);
        }

        public static void SetD13<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d13 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD13", text);
        }

        /// <summary>
        /// 设置输入框字数提示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD14<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d14 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD14", text);
        }

        /// <summary>
        /// 设置输入框边框颜色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD15<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d15 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD15", text);
        }

        /// <summary>
        /// 设置D16属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD16<T>(this T inControl, string text) where T : IControl
        {
            ControlDetailForPage ctObj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            ctObj.d16 = text;
            (inControl as FrameworkElement).Tag = ctObj;

            ExcuteClassMethodByname(inControl, "SetD16", text);
        }

        /// <summary>
        /// 可见性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD18<T>(this T inControl, string text) where T : IControl
        {
            if (string.IsNullOrEmpty(text))
            {

            }
            (inControl as FrameworkElement).Visibility = CommonConverter.StringToBool(text) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public static void SetD19<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetD19", text);
        }

        public static void SetD20<T>(this T inControl, object text) where T : IControl
        {
            //ControlDetailForPage obj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
            //obj.d20 = text;

            //(inControl as FrameworkElement).Tag = obj;
            ExcuteClassMethodByname(inControl, "SetD20", text);
        }

        public static void SetD21<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetD21", text);
        }

        /// <summary>
        /// 垂直对齐方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD25<T>(this T inControl, string text) where T : IControl
        {
            ExcuteClassMethodByname(inControl, "SetD25", text);
        }

        /// <summary>
        /// 设置控件的值
        /// 这个是父类的方法，如果是图片控件什么的，需要自己重新重写一下这个方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="text"></param>
        public static void SetD0<T>(this T inControl, object value) where T : IControl
        {
            //更新一下d0的值
            if (value is string)
            {
                ControlDetailForPage obj = (inControl as FrameworkElement).Tag as ControlDetailForPage;
                obj.d0 = value.ToString();
                (inControl as FrameworkElement).Tag = obj;
            }

            if (ExcuteClassMethodByname(inControl, "SetD0", value) == ReturnConst.CancleReturn)
            {
                //if (!object.Equals(value, null))
                //{
                //    //(inControl as FrameworkElement) = value.ToString();
                //}
            }

            //数据获取后执行的事件
            //if (!object.Equals(value, null) && !string.IsNullOrEmpty(value.ToString()))
            //{
            //    if (!string.IsNullOrEmpty(((inControl as FrameworkElement).Tag as ControlDetailForPage).p4))
            //    {
            //        DecoderAssistant.CallEventDerectly(((inControl as FrameworkElement).Tag as ControlDetailForPage).p4, inControl);
            //    }
            //}
        }

        /// <summary>
        /// 根据方法名调用类里面的方法
        /// </summary>
        /// <param name="inControl"></param>
        /// <param name="methodName"></param>
        /// <param name="text"></param>
        private static int ExcuteClassMethodByname(object inControl, string methodName, object text)
        {
            MethodInfo[] methodArray = inControl.GetType().GetMethods();
            if (!object.Equals(methodArray, null))
            {
                List<MethodInfo> listmethod = new List<MethodInfo>();
                listmethod.AddRange(methodArray);
                int methodIndex = listmethod.FindIndex(p => methodName.Equals(p.Name));
                if (methodIndex != -1)
                {
                    inControl.GetType().GetMethod(methodName).Invoke(inControl, new[] { text });
                    return ReturnConst.OkReturn;
                }
            }
            return ReturnConst.CancleReturn;
        }


        private static object GetValueExcuteClassMethodByname(object inControl, string methodName, object text)
        {
            MethodInfo[] methodArray = inControl.GetType().GetMethods();
            if (!object.Equals(methodArray, null))
            {
                List<MethodInfo> listmethod = new List<MethodInfo>();
                listmethod.AddRange(methodArray);
                int methodIndex = listmethod.FindIndex(p => methodName.Equals(p.Name));
                if (methodIndex != -1)
                {
                    return inControl.GetType().GetMethod(methodName).Invoke(inControl, new[] { text });
                }
            }
            return null;
        }

        //触发设置

        /// <summary>
        /// 设置当前控件列表
        /// </summary>
        /// <param name="ic"></param>
        private static void SetCurrentControlList(IControl inControl)
        {
            //xinlongyuForm frm = (inControl as FrameworkElement).FindForm() as xinlongyuForm;
            //DecoderAssistant.CurrentControlList = frm._currentControlList;
            //DecoderAssistant.CurrentControlObjList = frm._currentControlObjList;
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetP0<T>(this T inControl, string text) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetP0", text) == ReturnConst.CancleReturn)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    //DecoderAssistant.SetControlClickEvent(inControl, text);

                }
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetP1<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetP1", eventText) == ReturnConst.CancleReturn)
            {
                //DecoderAssistant.SetControlDoubleClickEvent(inControl, eventText);
            }
        }

        /// <summary>
        /// 长按事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetP2<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetP2", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 设置P7事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetP7<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetP7", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }


        /// <summary>
        /// 执行成功调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetP9<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetP9", eventText) == ReturnConst.CancleReturn)
            {
                //DecoderAssistant.CallEventDerectly(eventText, inControl);
            }
        }

        /// <summary>
        /// 执行失败进行调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetP12<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetP12", eventText) == ReturnConst.CancleReturn)
            {
                //DecoderAssistant.CallEventDerectly(eventText, inControl);
            }
        }

        /// <summary>
        /// 主值改变事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetKeyChangeEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetKeyChangeEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 数据获取事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetGetDataEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetGetDataEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 获取焦点事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetGetFocusEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetGetFocusEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetLoseFocusEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetLoseFocusEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetPageInitEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetPageInitEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 页面消失事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetPageDisappearEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetPageDisappearEvent", eventText) == ReturnConst.CancleReturn)
            {
                //

            }
        }

        /// <summary>
        /// 触发后触发的事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="eventText"></param>
        public static void SetFinishedEvent<T>(this T inControl, string eventText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetFinishedEvent", eventText) == ReturnConst.CancleReturn)
            {

            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA0<T>(this T inControl, object inText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetA0", inText) == ReturnConst.CancleReturn)
            {
                //
                //(inControl as FrameworkElement).Refresh();
            }
        }

        /// <summary>
        /// 设置启动事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA1<T>(this T inControl, string inText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetA1", inText) == ReturnConst.CancleReturn)
            {
                //
            }
        }

        /// <summary>
        /// 设置缓存事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA2<T>(this T inControl, string inText) where T : IControl
        {
            if (ExcuteClassMethodByname(inControl, "SetA2", inText) == ReturnConst.CancleReturn)
            {
                //
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        /// <returns></returns>
        public static object SetA3<T>(this T inControl, object inText) where T : IControl
        {
            return GetValueExcuteClassMethodByname(inControl, "SetA3", inText);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        /// <returns></returns>
        public static object SetA4<T>(this T inControl, object inText) where T : IControl
        {
            return GetValueExcuteClassMethodByname(inControl, "SetA4", inText);
        }

        /// <summary>
        /// 设置A5事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static object SetA5<T>(this T inControl, string inText) where T : IControl
        {
            //if (ExcuteClassMethodByname(inControl, "SetA5", inText) == ReturnConst.CancleReturn)
            //{
            //    //
            //}
            //SetCurrentControlList(inControl);
            return GetValueExcuteClassMethodByname(inControl, "SetA5", inText);
        }

        /// <summary>
        /// 设置A6事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA6<T>(this T inControl, string inText) where T : IControl
        {
            //SetCurrentControlList(inControl);
            if (ExcuteClassMethodByname(inControl, "SetA6", inText) == ReturnConst.CancleReturn)
            {
                //
            }
        }

        /// <summary>
        /// A7事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA7<T>(this T inControl, string inText) where T : IControl
        {
            //SetCurrentControlList(inControl);
            if (ExcuteClassMethodByname(inControl, "SetA7", inText) == ReturnConst.CancleReturn)
            {
                //
            }
        }

        /// <summary>
        /// A8事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <param name="inText"></param>
        public static void SetA8<T>(this T inControl, string inText) where T : IControl
        {
            //SetCurrentControlList(inControl);
            if (ExcuteClassMethodByname(inControl, "SetA8", inText) == ReturnConst.CancleReturn)
            {
                //
            }
        }

        //取值
        /// <summary>
        /// 获取主值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <returns></returns>
        public static object GetD0<T>(this T inControl, object parameter) where T : IControl
        {
            //SetCurrentControlList(inControl);
            object obj = GetValueExcuteClassMethodByname(inControl, "GetD0", parameter);
            if (object.Equals(obj, null))
            {
                //obj = (inControl as FrameworkElement).Text;
            }
            return obj;
        }

        /// <summary>
        /// 获取宽度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <returns></returns>
        public static object GetD1<T>(this T inControl, object parameter) where T : IControl
        {
            object obj = (inControl as FrameworkElement).Width;
            //ExcuteClassMethodByname(inControl, "GetWidth", obj);
            return obj;
        }

        /// <summary>
        /// 获取高度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inControl"></param>
        /// <returns></returns>
        public static object GetD2<T>(this T inControl, object parameter) where T : IControl
        {
            object obj = (inControl as FrameworkElement).Height;
            return obj;
        }

        public static object GetD20<T>(this T inControl, object parameter) where T : IControl
        {
            object obj = ((inControl as FrameworkElement).Tag as ControlDetailForPage).d20;
            return obj;
        }


    }
}
