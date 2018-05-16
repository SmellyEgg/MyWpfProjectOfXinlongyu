
using System.Windows;
using System.Windows.Navigation;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 视频上传控件
    /// </summary>
    public partial class xinlongyuVideoUploader : System.Windows.Controls.UserControl, IControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public xinlongyuVideoUploader()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
            MyWebBrowse.Navigate("https://icityservice.cn/uploadVideo.html");
            MyWebBrowse.LoadCompleted += XinlongyuVideoUploader_DocumentCompleted;
        }

        /// <summary>
        /// 加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XinlongyuVideoUploader_DocumentCompleted(object sender, NavigationEventArgs e)
        {
            string api = "https://icityservice.cn/uploadVideo.html";
            if (_currentBrowserMode == browserMode.result && !MyWebBrowse.Source.AbsoluteUri.Equals(api))
            {
                //CommonFunction.IsFinishLoading = true;
                string success = "icity://uploadvideo/?code=1";
                string failure = "icity://uploadVidel/?code=0";
                ControlDetailForPage ctObj = this.Tag as ControlDetailForPage;
                if ((MyWebBrowse.Source.AbsoluteUri.ToString().StartsWith(success)))
                {
                    ctObj.d0 = MyWebBrowse.Source.AbsoluteUri.ToString().Replace("icity://uploadvideo/?code=1&id=", string.Empty); //保存视频ID
                    this.Tag = ctObj;
                    //DecoderAssistant.CallEventDerectly(ctObj.p9, this);
                }
                else if (MyWebBrowse.Source.AbsoluteUri.ToString().StartsWith(failure))
                {
                    //DecoderAssistant.CallEventDerectly(ctObj.p12, this);
                }
                MyWebBrowse.Navigate(api);
                _currentBrowserMode = browserMode.normal;
            }
            else if (_currentBrowserMode == browserMode.start)
            {
                try
                {
                    //使用的mshtml空间下的控件，不知道有没有效果
                    var document = MyWebBrowse.Document as mshtml.HTMLDocument;
                    var script = document.createElement("script");
                    script.setAttribute("type", "text/javascript");
                    _isAppended = true;
                    script.setAttribute("text", "function _func(){$('#uploadVideoNow-file').click();}");
                    var head = document.body.children(script);
                    MyWebBrowse.InvokeScript("_func");
                    this._currentBrowserMode = browserMode.result;

                    //if (CommonFunction.IsFinishLoading)
                    //{
                    //    CommonFunction.IsFinishLoading = false;
                    //    CommonFunction.ShowWaitingForm();
                    //}
                }
                catch (System.Exception ex)
                {
                    string error = ex.Message;
                }
            }
        }


        private enum browserMode
        {
            start,
            result,
            normal
        }

        private browserMode _currentBrowserMode = browserMode.normal;
        private bool _isAppended = false;


        public void SetA1(string text)
        {
            _currentBrowserMode = browserMode.start;
            XinlongyuVideoUploader_DocumentCompleted(this, null);
        }

        /// <summary>
        /// 获取主值
        /// </summary>
        /// <returns></returns>
        public string GetD0(string text)
        {
            ControlDetailForPage ctObj = this.Tag as ControlDetailForPage;
            if (!string.IsNullOrEmpty(ctObj.d0))
            {
                return ctObj.d0;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
