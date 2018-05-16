using System.Collections.Generic;
using System.Windows.Forms;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 文件上传控件
    /// </summary>
    public class xinlongyuUploadControl : System.Windows.Controls.Button, IControl
    {
        /// <summary>
        /// 上传文件
        /// 将返回路径存储到d0字段中
        /// </summary>
        /// <param name="value"></param>
        public void SetA1(object value)
        {
            ControlDetailForPage obj = (this.Tag as ControlDetailForPage);
            //上传文件
            var values = new[]
            {
                new KeyValuePair<string, string>("api_type", "upload"),
                new KeyValuePair<string, string>("sql", /*obj.d5*/string.Empty),
                 //other values
            };
            OpenFileDialog opf = new OpenFileDialog();
            string filter = this.GetFilterType();
            if (!string.IsNullOrEmpty(filter))
            {
                opf.Filter = filter;
            }
            if (opf.ShowDialog() == DialogResult.OK)
            {
                if (IsValidImage(opf.FileName))
                {
                    long length = new System.IO.FileInfo(opf.FileName).Length;
                    float lef = length / (1024 * 1024);
                    if (lef > 2.0)
                    {
                        MessageBox.Show("图片大小超过2m！");
                        return;
                    }
                }
                try
                {
                    bool resultbol = false;
                    //if (CommonFunction.IsFinishLoading)
                    //{
                    //    resultbol = true;
                    //    CommonFunction.IsFinishLoading = false;
                    //    CommonFunction.ShowWaitingForm();
                    //}

                    BaseConnection bcc = new BaseConnection();
                    var result =  bcc.PostFile(opf.FileName, values);
                    BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result.Result);
                    FileUploadReturn returnResult = JsonController.DeSerializeToClass<FileUploadReturn>(brj.data.ToString());
                    obj.d0 = returnResult.data.path;
                    this.Tag = obj;
                    this.SetP9(obj.p9);
                    //if (resultbol)
                    //{
                    //    CommonFunction.IsFinishLoading = true;
                    //}
                }
                catch
                {
                    //调用失败后走这里
                    this.SetP12(obj.p12);
                }
            }
        }

        /// <summary>
        /// 获取上传的文件类型
        /// </summary>
        /// <returns></returns>
        private string GetFilterType()
        {
            string type = (this.Tag as ControlDetailForPage).d5;
            string filter = string.Empty;
            switch (type)
            {
                case "1"://图片
                    filter = "Image Files(*.PNG;*.JPG)|*.JPG;*.PNG";
                    break;
                case "2"://文本
                    filter = "Text files (*.txt)|*.txt";
                    break;
                case "3"://apk
                    filter = "Apk files (*.apk)|*.apk";
                    break;
                case "4"://pc的dll以及exe
                    filter = "DLL And EXE files (*.exe)|*.exe;*.dll;*.xml";
                    break;
                default:
                    break;
            }
            return filter;
        }

        /// <summary>
        /// 判断是否是一张正常的图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsValidImage(string filename)
        {
            try
            {
                using (System.Drawing.Image newImage = System.Drawing.Image.FromFile(filename))
                { }
            }
            catch (System.OutOfMemoryException ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取主值
        /// 一般是上传之后服务器返回的地址链接
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(object value)
        {
            return (this.Tag as ControlDetailForPage).d0;
        }
    }
}
