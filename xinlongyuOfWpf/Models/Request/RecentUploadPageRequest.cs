

namespace xinlongyuOfWpf.Models.Request
{
    /// <summary>
    /// 获取最近上传页面请求
    /// </summary>
    public class RecentUploadPageRequest
    {
        private string Review = string.Empty;

        public string review
        {
            get
            {
                return Review;
            }

            set
            {
                Review = value;
            }
        }

        public RecentUploadPageRequest(string reviewNew)
        {
            this.review = reviewNew;
        }
    }
}
