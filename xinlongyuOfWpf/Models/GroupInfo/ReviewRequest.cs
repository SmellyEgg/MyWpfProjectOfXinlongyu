using System;

namespace xinlongyuOfWpf.Models.GroupInfo
{
    /// <summary>
    /// 审核页面请求
    /// </summary>
    [Serializable]
    public class ReviewRequest
    {
        public int version = 0;

        public string page_id = string.Empty;

        public ReviewRequest(int ver, string pageId)
        {
            this.version = ver;
            this.page_id = pageId;
        }
    }
}
