using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Models.GroupInfo;
using xinlongyuOfWpf.Models.PageInfo;
using xinlongyuOfWpf.Models.Request;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.Controller.PageController
{
    public class PageConnection : BaseConnection
    {
        /// <summary>
        /// 页面缓存类
        /// </summary>
        private PageCacher _pageCacher;

        /// <summary>
        /// 平台
        /// </summary>
        private string _platForm = "pc";

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageConnection()
        {
            _pageCacher = new PageCacher();
        }

        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public async Task<PageInfoDetail> GetPageInfo(int pageId)
        {
            //string pageVersion = string.IsNullOrEmpty(LocalCacher.GetCache("page_version")) ? "0" : LocalCacher.GetCache("page_version");
            string pageVersion = "4";
            BaseRequest bj = this.GetPageRequest(pageId, pageVersion);
            try
            {
                PageInfoDetail prd;
                try
                {
                    var result = await Post(bj);

                    BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result);
                    ReturnCode code = JsonController.DeSerializeToClass<ReturnCode>(brj.data.ToString());
                    if (code.error_code.Equals("1"))
                    {
                        throw new Exception("获取页面失败");
                    }
                    prd = JsonController.DeSerializeToClass<PageInfoDetail>(brj.data.ToString());
                }
                catch (Exception ex)
                {
                    Logging.Error(ex.Message);
                    //这种情况是连不上服务的处理或者调用接口失败
                    prd = new PageInfoDetail();
                    prd.data = null;
                }
                if (object.Equals(prd.data, null))
                {
                    return _pageCacher.GetPageInfo(pageId);
                }
                else
                {
                    //获取页面成功后进行缓存
                    if ("0".Equals(pageVersion))
                    {
                        _pageCacher.CachePageInfo(prd);
                    }
                    return prd;
                }

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取页面的基本请求
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        private BaseRequest GetPageRequest(int pageId, string pageVersion)
        {
            int timestamp = _pageCacher.GetTimeStampOfPage(pageId);
            //int timestamp = 0;
            string apitype = JsonApiType.page;
            BaseRequest bj = this.GetCommonBaseRequest(apitype);
            //string pageVersion = "4";
            PageInfoRequest pagerequeset = new PageInfoRequest(pageId, timestamp, _platForm, pageVersion);
            bj.data = pagerequeset;

            return bj;
        }

        /// <summary>
        /// 获取最近上传页面
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<List<PageGroupDetail>> GetRecentUploadPages()
        {
            string apitype = JsonApiType.groupPageGet;
            BaseRequest bj = GetCommonBaseRequest(apitype);
            string reviewState = "0";
            RecentUploadPageRequest pgd = new RecentUploadPageRequest(reviewState);
            bj.api_type = apitype;
            bj.data = pgd;
            try
            {
                var result = await Post(bj);
                BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result);
                pageGroupReturnData pgr = JsonController.DeSerializeToClass<pageGroupReturnData>(brj.data.ToString());
                if (!object.Equals(pgr.data, null) && pgr.data.Length > 0)
                {
                    List<PageGroupDetail> listreturn = new List<PageGroupDetail>();
                    listreturn.AddRange(pgr.data);
                    return listreturn;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取页面分组信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<pageDetailForGroup>> GetPageGroupInfo(string pageid)
        {
            string apitype = JsonApiType.groupPageGet;
            BaseRequest bj = GetCommonBaseRequest(apitype);
            //string review = "0";
            string review = string.Empty;
            PageGroupData pgd = new PageGroupData(pageid, review);
            bj.api_type = apitype;
            bj.data = pgd;
            try
            {
                var result = await Post(bj);
                BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result);
                pageGroupReturnData pgr = JsonController.DeSerializeToClass<pageGroupReturnData>(brj.data.ToString());
                if (!object.Equals(pgr.data, null) && pgr.data.Length > 0)
                {
                    List<pageDetailForGroup> listpage = new List<pageDetailForGroup>();
                    listpage.AddRange(pgr.data[0].page_list);
                    return listpage;
                }
                return null;
            }
            catch (Exception ex)
            {
                string test = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 审核页面
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<bool> ReviewPage(string pageid, int version)
        {
            string apitype = JsonApiType.reviewPage;
            BaseRequest bj = GetCommonBaseRequest(apitype);
            ReviewRequest re = new ReviewRequest(version, pageid);
            bj.api_type = apitype;
            bj.data = re;
            try
            {
                var result = await Post(bj);
                BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result);
                CommonReturn cr = JsonController.DeSerializeToClass<CommonReturn>(brj.data.ToString());
                if (cr.error_code.Equals(ReturnConst.right))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


    }
}
