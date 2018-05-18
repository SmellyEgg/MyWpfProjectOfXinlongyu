using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Controller.PageController;
using xinlongyuOfWpf.CustomControls;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.Controller.ControlController
{
    /// <summary>
    /// 控件解析类
    /// </summary>
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
          xinLongyuControlType.tabMenuControlType, xinLongyuControlType.pcnavigationBarType,
          xinLongyuControlType.pcnavigationBarItemType};

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
        public xinlongyuForm ProduceControl(ControlDetailForPage pageobj, List<IControl> listControl, List<ControlDetailForPage> listControlObj, bool isNavigationWindow)
        {
            Grid _currentForm = new Grid();
            AddTitleBar(_currentForm, isNavigationWindow);
            if (_fatherControlList.Contains(pageobj.ctrl_type))
            {
                this.ProduceFatherControl(pageobj, listControlObj, listControl, _currentForm, true);
            }
            else
            {
                return null;
            }

            
            _currentForm.HorizontalAlignment = HorizontalAlignment.Stretch;
            _currentForm.VerticalAlignment = VerticalAlignment.Stretch;
            //_currentForm.Margin = new Thickness(10);


            //实现窗体的滚动条
            //ScrollViewer scrollView = new ScrollViewer();
            ////scrollView.HorizontalAlignment = HorizontalAlignment.Stretch;
            ////scrollView.VerticalAlignment = VerticalAlignment.Stretch;
            //scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            //scrollView.Content = _currentForm;

            xinlongyuForm page = new xinlongyuForm(pageobj.page_id);
            page.Content = _currentForm;
            page.Width = pageobj.d1;
            page.Height = pageobj.d2;

            //返回page
            return page;
        }

        /// <summary>
        /// 添加标题栏
        /// </summary>
        /// <param name="gridControl"></param>
        private void AddTitleBar(Grid gridControl, bool isNavigationWindow)
        {
            //设置为两行，一行显示返回按钮以及刷新按钮，另外一行显示内容
            RowDefinition gridRowTitle = new RowDefinition();
            gridRowTitle.Height = GridLength.Auto;
            RowDefinition gridRowContent = new RowDefinition();
            gridRowContent.Height = new GridLength(1, GridUnitType.Star);
            gridControl.RowDefinitions.Add(gridRowTitle);
            gridControl.RowDefinitions.Add(gridRowContent);

            StackPanel gridTitle = new StackPanel();
            gridTitle.Orientation = Orientation.Horizontal;
            gridTitle.HorizontalAlignment = HorizontalAlignment.Left;
            gridTitle.VerticalAlignment = VerticalAlignment.Top;
            //返回按钮
            xinlongyuButton btnBack = new xinlongyuButton();
            Image imgBack = new Image();
            //imgBack.Stretch = Stretch.Fill;
            imgBack.Source = new BitmapImage(
   new Uri("pack://application:,,,/xinlongyuOfWpf;component/Resources/mybackbutton.png"));
            btnBack.Content = imgBack;
            btnBack.Width = 36;
            btnBack.Height = 36;
            btnBack.Background = Brushes.Transparent;
            btnBack.Margin = new Thickness(5);
            btnBack.BorderThickness = new Thickness(0);
            //刷新按钮
            xinlongyuButton btnRefresh = new xinlongyuButton();
            Image imgRefresh = new Image();
            imgRefresh.Stretch = Stretch.Fill;
            imgRefresh.Source = new BitmapImage(
   new Uri("pack://application:,,,/xinlongyuOfWpf;component/Resources/refreshButton.png"));
            btnRefresh.Content = imgRefresh;
            btnRefresh.Background = Brushes.Transparent;
            btnRefresh.Width = 25;
            btnRefresh.Height = 25;
            btnRefresh.Margin = new Thickness(5);
            btnRefresh.BorderThickness = new Thickness(0);

            gridTitle.Children.Add(btnBack);
            gridTitle.Children.Add(btnRefresh);
            gridTitle.SetValue(Grid.RowProperty, 0);
            gridControl.Children.Add(gridTitle);
            //添加事件
            btnBack.Click += (s, e) => {
                var window = Window.GetWindow(btnBack);
                var listPage = window.Tag as List<Page>;
                if (!object.Equals(listPage, null) && listPage.Count > 1)
                {
                    //CommonFunction.ShowWaitingForm(window);
                    window.Content = null;
                    listPage.RemoveAt(listPage.Count - 1);
                    var page = listPage[listPage.Count - 1];
                    window.Content = page;
                    window.Width = page.Width;
                    window.Height = page.Height + ConfigManagerSection.TitleBarHeight;
                }
                else
                {

                }
            };
            //刷新事件
            btnRefresh.Click += async (s, e) => {
                
                var window = Window.GetWindow(btnRefresh);
                var page = window.Content as xinlongyuForm;
                var listPage = window.Tag as List<Page>;
                if (!object.Equals(page, null))
                {
                    listPage.RemoveAt(listPage.Count - 1);
                    PageFactory pageFactory = new PageFactory();
                    await pageFactory.ShowPage(window, page.PageId, listPage);
                    MessageBox.Show("刷新成功");
                }
            };
            //这里不考虑处于导航栏中的情况，直接隐藏这两个按钮
            if (isNavigationWindow)
            {
                btnBack.Visibility = Visibility.Hidden;
                btnRefresh.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 获取控件接口
        /// </summary>
        /// <param name="controlObj"></param>
        /// <returns></returns>
        private IControl GetIControl(ControlDetailForPage obj)
        {
            IControl control = null;

            #region 根据控件类型实例化相应控件
            if (xinLongyuControlType.buttonType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuButton();
                //control = new xinlongyuTextBox();
            }
            else if (xinLongyuControlType.inputType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuTextBox();
            }
            else if (xinLongyuControlType.superViewType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuParentControl();
            }
            else if (xinLongyuControlType.pageType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuPage();
            }
            else if (xinLongyuControlType.imgType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuImageControl();
            }
            else if (xinLongyuControlType.textType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuLable();
            }
            else if (xinLongyuControlType.pcnavigationBarType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuNavigationControl();
            }
            else if (xinLongyuControlType.pcnavigationBarItemType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuNavigationItem();
            }
            else if (xinLongyuControlType.articleEditorType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuArticleEditor();
            }
            else if (xinLongyuControlType.PCGrid.Equals(obj.ctrl_type))
            {
                control = new xinlongyuDataGird();
            }
            else if (xinLongyuControlType.reviewControlType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuReviewControl();
            }
            else if (xinLongyuControlType.comboboxMenuType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuCombobox();
            }
            else if (xinLongyuControlType.cacheType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuCacher();
            }
            else if (xinLongyuControlType.tooltipType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuToolTip();
            }
            else if (xinLongyuControlType.LogicJudgmentType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuLogicControl();
            }
            else if (xinLongyuControlType.getDataType.Equals(obj.ctrl_type))
            {
                control = new xinlongyuGetData();
            }
            else
            {
                return null;
            }
            #endregion

            //设置一些附属的属性
            (control as FrameworkElement).Name  = ConfigManagerSection.ControlNamePrefix + obj.ctrl_id.ToString();//设置名称标记
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
            if (object.Equals(control, null)) return null;
            listControl.Add(control);

            #region 判断类型
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
            else if (fatherControl.GetType() == typeof(xinlongyuNavigationControl))
            {
                (fatherControl as TreeView).Items.Add(control as UIElement);
            }
            else if (fatherControl.GetType() == typeof(xinlongyuNavigationItem))
            {
                (fatherControl as TreeViewItem).Header = control as UIElement;
            }
            //else if (fatherControl.GetType() == typeof(StackPanel))
            //{
            //    (fatherControl as StackPanel).Children.Add(control as UIElement);
            //}
            else
            {
                (fatherControl as Grid).Children.Add(control as UIElement);
            }
            #endregion

            return control;
        }

        /// <summary>
        /// 生成父控件
        /// </summary>
        public void ProduceFatherControl(ControlDetailForPage controlObj, List<ControlDetailForPage> listControlObj, List<IControl> listControl, UIElement fatherControl, bool isNeedSetRow = false)
        {
            IControl newfatherControl = this.ProductChildControl(controlObj, listControl, fatherControl);
            if (isNeedSetRow) (newfatherControl as FrameworkElement).SetValue(Grid.RowProperty, 1);

            //导航栏控件由自身进行初始化
            if (xinLongyuControlType.pcnavigationBarType.Equals(controlObj.ctrl_type))
            {
                (newfatherControl as xinlongyuNavigationControl).InitControl(controlObj, listControlObj, listControl);
                return;
            }
            //表格控件也由自身进行初始化
            if (xinLongyuControlType.PCGrid.Equals(controlObj.ctrl_type))
            {
                (newfatherControl as xinlongyuDataGird).LoadData(listControlObj, controlObj);
                return;
            }

            //获取子控件列表
            string controlList = _D0FatherControlList.Contains(controlObj.ctrl_type) ? controlObj.d0 : controlObj.d17;
            if (string.IsNullOrEmpty(controlList)) return;
            List<int> controlIdList = JsonController.DeSerializeToClass<List<int>>(controlList);
            if (object.Equals(controlIdList, null) || controlIdList.Count < 1)
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
            //单击事件
            control.SetP0(controlObj.p0);
            //双击事件
            //control.SetP1(controlObj.p1);
            ////设置长按
            //control.SetP2(controlObj.p2);

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
