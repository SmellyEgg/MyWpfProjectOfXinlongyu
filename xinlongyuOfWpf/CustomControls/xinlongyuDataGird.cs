using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.CustomControls.Extension;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 自定义的表格控件
    /// </summary>
    public class xinlongyuDataGird : MyDataGridControl, IControl
    {
        /// <summary>
        /// 加载数据
        /// </summary>
        //public void LoadMyData(List<ControlDetailForPage> listControlObj, ControlDetailForPage gridObj)
        //{
        //   // var listControlObj = CommonFunction.GetPageByControl(this)._currentControlObjList;
        //    LoadData(listControlObj, gridObj, this);
        //}
        ///// <summary>
        ///// 这里是用来刷新表格的
        ///// </summary>
        ///// <param name="inText"></param>
        //public void SetA0(object inText)
        //{
        //    var listControlObj = CommonFunction.GetPageByControl(this)._currentControlObjList;
        //    LoadData(listControlObj, this.Tag as ControlDetailForPage);
        //}

        ///// <summary>
        ///// 从数据源中获取
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public object SetA3(object value)
        //{
        //    if (object.Equals(value, null) || string.IsNullOrEmpty(value.ToString())) return string.Empty;
        //    return GetDataFromDatagrid(value.ToString());
        //}

        
        public void SetD0(string text)
        {
            var listControlObj = CommonFunction.GetPageByControl(this)._currentControlObjList;
            LoadData(listControlObj, this.Tag as ControlDetailForPage, this);
        }
    }
}
