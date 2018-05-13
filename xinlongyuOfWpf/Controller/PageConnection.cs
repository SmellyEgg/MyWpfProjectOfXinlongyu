using System;
using System.Threading.Tasks;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Controller.OtherController;
using xinlongyuOfWpf.Models.PageInfo;
using xinlongyuOfWpf.Models.Request;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.Controller
{
    public class PageConnection : BaseConnection
    {
        /// <summary>
        /// 页面缓存类
        /// </summary>
        private PageCacher _pageCacher;

        ///// <summary>
        ///// 默认请求的账户
        ///// </summary>
        ////private string userName = "guests";
        //private string userName = "icity_test";

        ///// <summary>
        ///// 默认请求的密码，游客的密码采用的是明文
        ///// </summary>
        ////private string password = "123456";
        //private string password = "b46bd96246d8a2776b60202b534c8b92";

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
        /// 构造基本请求
        /// </summary>
        /// <param name="apiType"></param>
        /// <returns></returns>
        //private BaseRequest GetCommonBaseRequest(string apiType)
        //{
        //    //这一步就实现了权限控制了
        //    string newuserName = LocalCacher.GetCache("ICITY_USERNAME");
        //    string newpassword = LocalCacher.GetCache("ICITY_PASSWORD");
        //    if (string.IsNullOrEmpty(newuserName) || string.IsNullOrEmpty(newpassword))
        //    {
        //        newuserName = userName;
        //        newpassword = password;
        //    }
        //    BaseRequest bj = new BaseRequest(newuserName, newpassword, apiType);
        //    return bj;
        //}

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
            //int timestamp = _pageCacher.GetTimeStampOfPage(pageId);
            int timestamp = 0;
            string apitype = JsonApiType.page;
            BaseRequest bj = this.GetCommonBaseRequest(apitype);
            //string pageVersion = "4";
            PageInfoRequest pagerequeset = new PageInfoRequest(pageId, timestamp, _platForm, pageVersion);
            bj.data = pagerequeset;

            return bj;
        }

    }
}
