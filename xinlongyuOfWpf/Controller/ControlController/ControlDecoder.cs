using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.CustomControls;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.Controller.ControlController
{
    public class ControlDecoder
    {
        /// <summary>
        /// 父控件集合
        /// </summary>
        private List<string> _fatherControlList = new List<string>()
        { xinLongyuControlType.pageType, xinLongyuControlType.navigationBarType,
          xinLongyuControlType.superViewType,xinLongyuControlType.listsType,
          xinLongyuControlType.bannerType, xinLongyuControlType.cellType,
          xinLongyuControlType.channerBarType,xinLongyuControlType.sectionType,
          xinLongyuControlType.radioType, xinLongyuControlType.checkboxType,
          xinLongyuControlType.multilineListType, xinLongyuControlType.PCGrid,
          xinLongyuControlType.tabMenuControlType, xinLongyuControlType.pcnavigationBarType};

        /// <summary>
        /// D0父控件集合
        /// </summary>
        private List<string> _D0FatherControlList = new List<string>()
        { xinLongyuControlType.pageType,
          xinLongyuControlType.superViewType,
          xinLongyuControlType.navigationBarType};

        /// <summary>
        /// 构造函数
        /// </summary>
        public ControlDecoder()
        {
        }
        /// <summary>
        /// 解析控件为界面控件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Page ProduceControl(ControlDetailForPage pageobj, List<IControl> listControl, List<ControlDetailForPage> listControlObj)
        {
            Grid _currentForm = new Grid();
            if (_fatherControlList.Contains(pageobj.ctrl_type))
            {
                this.ProduceFatherControl(pageobj, listControlObj, listControl, _currentForm);
            }
            else
            {
                return null;
            }

            
            _currentForm.HorizontalAlignment = HorizontalAlignment.Stretch;
            _currentForm.VerticalAlignment = VerticalAlignment.Stretch;
            _currentForm.Margin = new Thickness(10);
            //实现窗体的滚动条
            //ScrollViewer scrollView = new ScrollViewer();
            //scrollView.HorizontalAlignment = HorizontalAlignment.Stretch;
            //scrollView.VerticalAlignment = VerticalAlignment.Stretch;
            //scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            //scrollView.Content = _currentForm;

            Page page = new Page();
            page.Content = _currentForm;
            page.Width = pageobj.d1;
            page.Height = pageobj.d2;

            //返回page
            return page;
        }

        /// <summary>
        /// 获取控件接口
        /// </summary>
        /// <param name="controlObj"></param>
        /// <returns></returns>
        private IControl GetIControl(ControlDetailForPage obj)
        {
            IControl control = null;

            if (xinLongyuControlType.buttonType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuButton();
                //control = new xinlongyuTextBox();
            }
            else if (xinLongyuControlType.inputType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuTextBox();
            }
            else if (xinLongyuControlType.pageType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuParentControl();
            }
            else if (xinLongyuControlType.imgType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuImageControl();
            }
            else if (xinLongyuControlType.textType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuLable();
            }
            else
            {
                return null;
            }

            //_currentForm.Name  = obj.ctrl_id.ToString();
            (control as FrameworkElement).VerticalAlignment = VerticalAlignment.Top;
            (control as FrameworkElement).HorizontalAlignment = HorizontalAlignment.Left;

            (control as FrameworkElement).Tag = obj;
            

            return control;
        }

        /// <summary>
        /// 生成子控件
        /// </summary>
        private IControl ProductChildControl(ControlDetailForPage controlObj, List<IControl> listControl, UIElement fatherControl)
        {
            IControl control = this.GetIControl(controlObj);
            listControl.Add(control);

            if (fatherControl.GetType() == typeof(Page))
            {
                (fatherControl as Page).Content = control as UIElement;
            }
            else if (fatherControl.GetType() == typeof(xinlongyuParentControl))
            {
                (fatherControl as Grid).Children.Add(control as UIElement);
            }
            else if (fatherControl.GetType() == typeof(ScrollViewer))
            {
                (fatherControl as ScrollViewer).Content = control as UIElement;
            }
            else
            {
                //(fatherControl as Canvas).Children.Add(control as UIElement);
                (fatherControl as Grid).Children.Add(control as UIElement);
            }
            return control;
        }

        /// <summary>
        /// 生成父控件
        /// </summary>
        private void ProduceFatherControl(ControlDetailForPage controlObj, List<ControlDetailForPage> listControlObj, List<IControl> listControl, UIElement fatherControl)
        {
            IControl newfatherControl = this.ProductChildControl(controlObj, listControl, fatherControl);

            //获取子控件列表
            string controlList = _D0FatherControlList.Contains(controlObj.ctrl_type) ? controlObj.d0 : controlObj.d17;
            List<int> controlIdList = JsonController.DeSerializeToClass<List<int>>(controlList);
            if (controlIdList.Count < 1)
            {
                return;
            }
            //这里对层级进行排序
            List<ControlDetailForPage> childrenList = listControlObj.Where(p => controlIdList.Contains(p.ctrl_id)).OrderBy(p => p.ctrl_level).ToList();
            foreach (ControlDetailForPage ctObj in childrenList)
            {
                //这里使用递归循环生成控件
                if (_fatherControlList.Contains(ctObj.ctrl_type))
                {
                    ProduceFatherControl(ctObj, listControlObj, listControl, newfatherControl as UIElement);
                }
                else
                {
                    this.ProductChildControl(ctObj, listControl, newfatherControl as UIElement);
                }
            }
        }

        /// <summary>
        /// 设置控件的事件
        /// </summary>
        public void SetControlEvent(IControl control, ControlDetailForPage controlObj)
        {
            //这里也可以设置成通用的，不过需要对语法进行解析
            control.SetP0(controlObj.p0);
            //双击事件
            control.SetP1(controlObj.p1);
            //设置长按
            control.SetP2(controlObj.p2);

        }

        /// <summary>
        /// 设置控件的属性
        /// </summary>
        /// <param name="control"></param>
        /// <param name="controlObj"></param>
        public void SetControlProperty(IControl control, ControlDetailForPage controlObj)
        {
            //宽度
            control.SetD1(controlObj.d1);
            //高度
            control.SetD2(controlObj.d2);
            //坐标
            control.SetD3D4(controlObj.d3, controlObj.d4);
            //主值
            control.SetD0(controlObj.d0);

            //行高属性
            control.SetD5(controlObj.d5);
            //字体大小
            control.SetD6(controlObj.d6);
            //字体颜色
            control.SetD7(controlObj.d7);
            //背景颜色
            control.SetD8(controlObj.d8);
            //字体对齐方式
            control.SetD9(controlObj.d9);
            //
            control.SetD10(controlObj.d10);
            //设置选中时颜色
            control.SetD10(controlObj.d10, controlObj.d11);
            //设置限制长度
            control.SetD11(controlObj.d11);
            //是否自适应高度
            control.SetD12(controlObj.d12);
            //
            control.SetD13(controlObj.d13);
            //
            control.SetD14(controlObj.d14);
            //设置边框颜色
            control.SetD15(controlObj.d15);
            //设置是否密码输入框
            control.SetD16(controlObj.d16);
            //设置可见性
            control.SetD18(controlObj.d18);
            //
            control.SetD19(controlObj.d19);
            //
            control.SetD20(controlObj.d20);
            //
            control.SetD21(controlObj.d21);
            //
            control.SetD25(controlObj.d25);
            //设置是否可以编辑
            control.SetD30(controlObj.d30);

        }
    }
}
