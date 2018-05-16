using System;
namespace xinlongyuOfWpf.Models.GroupInfo
{
    /// <summary>
    /// 根据平台或者所有的分组信息
    /// 包括组信息以及页面信息
    /// </summary>
    [Serializable]
    public class PageGroupData
    {
        //public string platform = string.Empty;

        public string page_id = string.Empty;

        //public string review = string.Empty;



        public PageGroupData(string pageId, string review)
        {
            //this.platform = plat;
            this.page_id = pageId;
            //this.review = review;
        }
    }
}
