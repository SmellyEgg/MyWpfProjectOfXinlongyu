using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.PageInfo;

namespace xinlongyuOfWpf.Controller.PageController
{
    /// <summary>
    /// 页面解析类
    /// 这个类应该是负责将实体解析为真正的界面控件
    /// </summary>
    public class PageDecoder
    {
        /// <summary>
        /// 控件解析层
        /// </summary>
        private ControlDecoder _controlDecode;

        public PageDecoder()
        {
            _controlDecode = new ControlDecoder();
        }

        /// <summary>
        /// 页面实体解析成页面类
        /// </summary>
        /// <param name="pageObj"></param>
        /// <returns></returns>
        public xinlongyuForm DecodePage(PageInfoDetail pageObj)
        {
            //页面基本信息
            PageBaseInfo dtObj = pageObj.data;
            if (object.Equals(dtObj.control_list, null))
            {
                return null;
            }
            try
            {
                List<ControlDetailForPage> listControlObject = ControlCaster.CastArrayToControl(dtObj.control_list);

                //判断一下解析出来的控件数组是不是为空
                if (object.Equals(listControlObject, null) || listControlObject.Count < 1)
                {
                    return null;
                }

                return this.DecodeListControlObj(listControlObject);
            }
            catch (Exception ex)
            {
                Logging.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 解析页面控件
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="listControlObject"></param>
        private xinlongyuForm DecodeListControlObj(List<ControlDetailForPage> listControlObject)
        {
            int pageIndex = listControlObject.FindIndex(p => xinLongyuControlType.pageType.Equals(p.ctrl_type));
            if (pageIndex == -1)
            {
                throw new System.Exception("页面缺少主要控件");
            }
            listControlObject[pageIndex].d18 = "1";//这里是防止页面控件为隐性
            ControlDetailForPage pageControl = listControlObject[pageIndex];

            if (string.IsNullOrEmpty(pageControl.d0))
            {
                //一个控件都没有就退出解析过程
                return null;
            }
            //界面控件数组
            List<IControl> listControl = new List<IControl>();
            var page = _controlDecode.ProduceControl(pageControl, listControl, listControlObject);
            //设置页面控件基本属性
            foreach (IControl control in listControl)
            {
                ControlDetailForPage controlObj = (control as FrameworkElement).Tag as ControlDetailForPage;
                _controlDecode.SetControlProperty(control, controlObj);
            }

            //设置控件基本事件
            foreach (IControl control in listControl)
            {
                _controlDecode.SetControlEvent(control, (control as FrameworkElement).Tag as ControlDetailForPage);
            }
            page._currentControlList = listControl;
            page._currentControlObjList = listControlObject;

            return page;

            //frm._currentControlObjList = listControlObject;
            //frm._currentControlList = listControl;

            //List<IControl> tempobj = listControl;
            
            ////设置控件基本事件
            //foreach (IControl control in listControl)
            //{
            //    _controlDecode.SetControlEvent(control, (control as Control).Tag as ControlDetailForPage);
            //}
            ////DecoderAssistant.CurrentControlList = tempobj;
            ////DecoderAssistant.CurrentControlObjList = DecoderAssistant.DicOfAllControlObj[frm.Name];

            ////DecoderAssistant.CurrentControlList = (frm as xinlongyuForm)._currentControlList;
            ////DecoderAssistant.CurrentControlObjList = (frm as xinlongyuForm)._currentControlObjList;

            ////页面初始化事件
            //IControl page = listControl.First(p => ((p as Control).Tag as ControlDetailForPage).ctrl_type.Equals(xinLongyuControlType.pageType));
            //page.SetP7(((page as Control).Tag as ControlDetailForPage).p7);
        }
    }
}
