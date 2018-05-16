
namespace xinlongyuOfWpf.Models.GroupInfo
{
    /// <summary>
    /// 页面以及组信息返回类
    /// </summary>
    public class pageGroupReturnData
    {
        /// <summary>
        /// 组细节信息
        /// </summary>
        public PageGroupDetail[] data;
    }

    public class PageGroupDetail
    {
        public int group_id = -1;

        public string group_name = string.Empty;

        public pageDetailForGroup[] page_list;
    }

    public class pageDetailForGroup
    {
        public int page_id { get; set; }
        public string page_name { get; set; }
        public string version { get; set; }
        public string review { get; set; }
    }
}
