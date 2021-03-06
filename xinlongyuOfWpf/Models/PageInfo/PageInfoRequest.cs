﻿
namespace xinlongyuOfWpf.Models.PageInfo
{
    /// <summary>
    /// 页面信息请求类
    /// </summary>
    public class PageInfoRequest
    {
        public int page_id = 0;

        public int time = 0;

        public string page_platform = string.Empty;

        public string version_type = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="platform"></param>
        /// <param name="versiontype"></param>
        public PageInfoRequest(int id, int time, string platform, string versiontype)
        {
            this.page_id = id;
            this.time = time;
            this.page_platform = platform;
            this.version_type = versiontype;
        }
    }
}
