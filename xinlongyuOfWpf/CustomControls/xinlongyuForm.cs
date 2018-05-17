using System.Collections.Generic;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    public class xinlongyuForm : Page
    {
        /// <summary>
        /// 页面缓存
        /// </summary>
        public Dictionary<string, string> _pageCache;

        /// <summary>
        /// 窗体控件列表
        /// </summary>
        public List<ControlDetailForPage> _currentControlObjList;
        public List<IControl> _currentControlList;

        private int _pageId = 0;

        /// <summary>
        /// 
        /// </summary>
        public xinlongyuForm(int pageId)
        {
            _pageId = pageId;
        }

        public xinlongyuForm(int pageId, Dictionary<string, string> parameter)
        {
            _pageCache = new Dictionary<string, string>();
            SetParameters(parameter);
        }

        /// <summary>
        /// 当前页面ID
        /// </summary>
        public int PageId { get => _pageId; }

        /// <summary>
        /// 设置参数，相当于添加缓存
        /// </summary>
        /// <param name="parameter"></param>
        private void SetParameters(Dictionary<string, string> parameter)
        {
            if (!object.Equals(parameter, null) && parameter.Keys.Count > 0)
            {
                this._pageCache = parameter;
            }
        }
    }
}
