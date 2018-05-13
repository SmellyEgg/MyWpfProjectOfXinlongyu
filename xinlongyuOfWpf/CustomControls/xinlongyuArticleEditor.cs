using Smith.WPF.HtmlEditor;
using System.Collections.Generic;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonPath;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls.Extension;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 文章编辑器控件
    /// </summary>
    public class xinlongyuArticleEditor : MyHtmlEditor, IControl
    {
        /// <summary>
        /// 当前文本
        /// </summary>
        private string _currentInnerHtml = string.Empty;
        /// <summary>
        /// 获取主值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(object value)
        {
            string resultStr = string.Empty;
            if (string.IsNullOrEmpty(this._currentInnerHtml))
            {
                resultStr = this.myeditor.ContentHtml;
            }
            else
            {
                resultStr = _currentInnerHtml;
            }
            return resultStr;
        }

        public void SetA1(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(this.myeditor.ContentText.Trim()))
                {
                    throw new System.Exception("文本为空");
                }
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(this.myeditor.ContentHtml);

                var htmlNodes = doc.DocumentNode.SelectNodes("//img");

                if (!object.Equals(htmlNodes, null))
                {
                    foreach (var node in htmlNodes)
                    {
                        string srcc = node.GetAttributeValue("src", string.Empty);
                        if (!string.IsNullOrEmpty(srcc) && System.IO.File.Exists(srcc))
                        {
                            string newUrl = this.UploadImage(srcc);
                            if (string.IsNullOrEmpty(newUrl))
                            {
                                throw new System.Exception("上传图片文件出错");
                            }
                            node.SetAttributeValue("src", ConfigManagerSection.serverUrl + newUrl);
                        }

                    }
                }
                _currentInnerHtml = doc.DocumentNode.OuterHtml;
                _currentInnerHtml = _currentInnerHtml.Replace("\"", "\\\"").Replace("'", "''");

                //this.SetP9((this.Tag as ControlDetailForPage).p9);
            }
            catch (System.Exception ex)
            {
                Logging.Error("文章编辑器上传图片出错：" + ex.Message);
                //this.SetP12((this.Tag as ControlDetailForPage).p12);
            }
        }

        BaseConnection bcc = new BaseConnection();
        //上传文件
        KeyValuePair<string, string>[] values = new[]
        {
                new KeyValuePair<string, string>("api_type", "upload"),
                new KeyValuePair<string, string>("sql", string.Empty),
                 };

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string UploadImage(string filePath)
        {
            try
            {
                var result = bcc.PostFile(filePath, values);
                BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result.Result);
                FileUploadReturn returnResult = JsonController.DeSerializeToClass<FileUploadReturn>(brj.data.ToString());
                string fileUrl = returnResult.data.path;
                return fileUrl;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置相关属性
        /// </summary>
        /// <param name="value"></param>
        public void SetD30(object value)
        {
            //if (!object.Equals(value, null) && !string.IsNullOrEmpty(value.ToString()))
            //{
            //    this.Enabled = true;
            //    if ("0".Equals(value.ToString()))
            //    {
            //        this.ShowToolBar = false;
            //        this.ReadOnly = true;
            //    }
            //    else
            //    {
            //        this.ShowToolBar = true;
            //        this.ReadOnly = false;
            //    }
            //}
        }

        public void SetD0(object value)
        {
            if (!object.Equals(value, null))
            {
                this.myeditor.ContentHtml = value.ToString();
            }
        }
    }
}
