using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using xinlongyuOfWpf.Controller.CommonPath;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// 连接类基类
    /// </summary>
    public class BaseConnection
    {
        /// <summary>
        /// 服务地址，为方便修改，不使用加密
        /// </summary>
        private string _urlService = string.Empty;

        public BaseConnection()
        {
            _urlService = this.GetServiceUrl();
        }

        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <returns></returns>
        private string GetServiceUrl()
        {
            string url = xmlController.GetNodeByXpath(ConfigManagerSection.urlPath, ConfigManagerSection.CommonconfigFilePath);
            return url;
        }

        /// <summary>
        /// 传数据
        /// </summary>
        /// <param name="jsonContent">类实体</param>
        /// <returns></returns>
        public async Task<string> Post(object jsonContent)
        {
            string postContent = JsonController.SerializeToJson(jsonContent);

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(_urlService, new StringContent(postContent));

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            //return await Task.Run(() => JsonController.DeSerializeToObject(content));
            return content;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<string> PostFile(string filepath, KeyValuePair<string, string>[] values)
        {
            if (string.IsNullOrEmpty(_urlService))
            {
                this.GetServiceUrl();
            }
            using (var client = new HttpClient())
            {
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    foreach (var keyValuePair in values)
                    {
                        multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    string fileName = "123" + Path.GetExtension(filepath);
                    multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes(filepath)),
                        '"' + "file" + '"',
                        '"' + fileName + '"');

                    var requestUri = _urlService;
                    try
                    {
                        var result = await client.PostAsync(requestUri, multipartFormDataContent);
                        return result.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        return string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 获取网络图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Image GetWebImage(string url)
        {
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(url);
                using (var ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    webClient.Dispose();
                    webClient = null;
                    imageBytes = null;
                    return image;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 测试是否连上服务
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            try
            {
                if (string.IsNullOrEmpty(_urlService))
                {
                    this.GetServiceUrl();
                }
                WebRequest request = (WebRequest)HttpWebRequest.Create(_urlService);
                request.Method = "POST";
                byte[] postBytes = null;
                request.ContentType = @"application/x-www-form-urlencoded";
                postBytes = Encoding.UTF8.GetBytes("");
                request.ContentLength = postBytes.Length;
                using (Stream outstream = request.GetRequestStream())
                {
                }
                return true;
            }
            catch (System.Exception ex)
            {
                string errorCode = "无法连接到远程服务器";
                if (errorCode.Equals(ex.Message))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


    }
}
