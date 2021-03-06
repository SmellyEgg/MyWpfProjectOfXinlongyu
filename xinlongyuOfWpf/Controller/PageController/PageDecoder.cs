﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
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
        public xinlongyuForm DecodePage(PageInfoDetail pageObj, bool isNavigationWindow)
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

                var page = this.DecodeListControlObj(listControlObject, isNavigationWindow);
                if (!object.Equals(page, null)) page.Title = pageObj.data.page_name;//设置标题
                return page;
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
        private xinlongyuForm DecodeListControlObj(List<ControlDetailForPage> listControlObject, bool isNavigationWindow)
        {
            int pageIndex = listControlObject.FindIndex(p => xinLongyuControlType.pageType.Equals(p.ctrl_type));
            if (pageIndex == -1)
            {
                throw new Exception("页面缺少主要控件");
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
            var page = _controlDecode.ProduceControl(pageControl, listControl, listControlObject, isNavigationWindow);
           
            page._currentControlList = listControl;
            page._currentControlObjList = listControlObject;
            return page;

            
        }
    }
}
