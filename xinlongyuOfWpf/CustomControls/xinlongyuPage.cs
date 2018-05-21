using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
using xinlongyuOfWpf.Controller.PageController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 页面类
    /// </summary>
    public class xinlongyuPage : Grid, IControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public xinlongyuPage()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        /// <summary>
        /// 这里是用来刷新页面的
        /// </summary>
        /// <param name="inText"></param>
        public async void SetA0(string inText)
        {
            int pageId = (this.Tag as ControlDetailForPage).page_id;

            var window = Window.GetWindow(this);
            var page = window.Content as xinlongyuForm;
            var listPage = window.Tag as List<Page>;
            if (!object.Equals(page, null))
            {
                listPage.RemoveAt(listPage.Count - 1);
                PageFactory pageFactory = new PageFactory();
                await pageFactory.ShowPage(window, page.PageId, listPage);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SetA3(object value)
        {
            if (!object.Equals(value, null) && !string.IsNullOrEmpty(value.ToString()))
            {
                //根据key获取页面的缓存的值，这里还没有实现
                var page = CommonFunction.GetPageByControl(this);
                if (page._pageCache.ContainsKey(value.ToString()))
                {
                    return page._pageCache[value.ToString()];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SetA4(object value)
        {
            var window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
            return null;
        }

        /// <summary>
        /// 用于跳转页面
        /// </summary>
        /// <param name="inText"></param>
        public object SetA5(string inText)
        {
            bool isDialog = false;
            OpenPage(inText, isDialog);
            return null;
        }

        /// <summary>
        /// 以对话框方式弹出窗体
        /// </summary>
        /// <param name="inText"></param>
        public void SetA6(string inText)
        {
            bool isDialog = true;
            OpenPage(inText, isDialog);
        }

        /// <summary>
        /// 打开新的窗体
        /// </summary>
        /// <param name="inText"></param>
        /// <param name="isDialog"></param>
        private async void OpenPage(string inText, bool isDialog = false)
        {
            int pageId = CommonConverter.StringToInt(inText);
            if (pageId != -1)
            {
                OpenPageById(pageId, isDialog);
            }
            //接下来是有传值的方法，格式应该是类似(1236, text={1.d0}&type=animal)
            else
            {
                if (Regex.IsMatch(inText, @"\d+, *\w+=.*"))
                {
                    pageId = CommonConverter.StringToInt(inText.Split(',')[0].Trim());
                    string parameters = inText.Split(',')[1].Trim();
                    string[] parameterArray = parameters.Split('&');
                    Dictionary<string, string> dicParameter = new Dictionary<string, string>();
                    Dictionary<string, string> dicParameterResult = new Dictionary<string, string>();
                    foreach (string str in parameterArray)
                    {
                        dicParameter.Add(str.Split('=')[0], str.Split('=')[1]);
                    }
                    foreach (string key in dicParameter.Keys)
                    {
                        dicParameterResult.Add(key, EventAssitant.FormatSql(dicParameter[key], this));
                    }
                    OpenPageById(pageId, isDialog, dicParameterResult);
                }
                else
                {
                    string pageid = EventAssitant.FormatSql(inText, this);
                    pageId = CommonConverter.StringToInt(pageid);
                    if (pageId != -1)
                    {
                        OpenPageById(pageId, isDialog);
                    }
                }
            }
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="parameter">参数字典</param>
        private async void OpenPageById(int pageId, bool isDialog = false, Dictionary<string, string> parameter = null)
        {
            PageFactory pagefactory = new PageFactory();
            if (isDialog)
            {
                var window = new Window();
                CommonFunction.ShowWaitingForm(window);
                var page = await pagefactory.ProducePage(pageId);
                window.Content = null;
                window.Content = page;
                window.Title = page.Title;
                window.Hide();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
            else
            {
                var window = Window.GetWindow(this);
                CommonFunction.ShowWaitingForm(window); 
                await pagefactory.ShowPage(window, pageId, window.Tag as List<Page>);
            }
        }

        /// <summary>
        /// 关闭当前窗体
        /// </summary>
        /// <param name="inText"></param>
        public void SetA7(string inText)
        {
            var window = Window.GetWindow(this as Grid);
            var listPage = window.Tag as List<Page>;
            listPage.RemoveAt(listPage.Count - 1);
        }

        /// <summary>
        /// 退出整个程序
        /// </summary>
        /// <param name="inText"></param>
        public void SetA8(string inText)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        /// <summary>
        /// 页面初始化事件
        /// </summary>
        /// <param name="value"></param>
        public void SetP7(object value)
        {
            if (!object.Equals(value, null) && !string.IsNullOrEmpty(value.ToString()))
            {
                EventAssitant.CallEventDerectly(value.ToString(), this);
            }
        }
    }
}
