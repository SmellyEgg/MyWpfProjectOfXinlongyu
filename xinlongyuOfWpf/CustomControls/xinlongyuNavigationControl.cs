using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.CommonType;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 导航栏控件
    /// </summary>
    public class xinlongyuNavigationControl : TreeView, IControl
    {
        public xinlongyuNavigationControl()
        {
            //this.BorderThickness = new Thickness(1);
            this.BorderThickness = new Thickness(0);
        }

        public void SetD0(string text)
        {
            var controlObj = this.Tag as ControlDetailForPage;
            var page = CommonFunction.GetPageByControl(this);
            //var listControlObj = 
            InitControl(controlObj, page._currentControlObjList, page._currentControlList);
        }

        /// <summary>
        /// 初始化控件，由于比较复杂，所以这种情况由控件自身进行初始化
        /// </summary>
        /// <param name="controlObj"></param>
        /// <param name="listControlObj"></param>
        /// <param name="listControl"></param>
        public void InitControl(ControlDetailForPage controlObj, List<ControlDetailForPage> listControlObj, List<IControl> listControl)
        {
            this.Items.Clear();

            List<ControlDetailForPage> listgroups = this.GetParentGroups(listControlObj, controlObj.d17);
            //开始生成控件
            foreach (ControlDetailForPage groupobj in listgroups)
            {
                //生成树节点
                TreeViewItem itemGroup = new TreeViewItem();
                itemGroup.Header = ProduceTreeviewItem(listControlObj, listControl, groupobj);

                //获取该目录下的子节点
                List<ControlDetailForPage> listItems = listControlObj.Where(p => groupobj.ctrl_id.ToString().Equals(p.d13)
                && xinLongyuControlType.pcnavigationBarItemType.Equals(p.ctrl_type)).ToList();
                if (object.Equals(listItems, null) || listItems.Count < 1) continue;
                //排序
                listItems = listItems.OrderBy(p => CommonConverter.StringToInt(p.d21)).ToList();

                foreach (ControlDetailForPage itemObj in listItems)
                {
                    TreeViewItem childItem = new TreeViewItem();
                    childItem.Header = ProduceTreeviewItem(listControlObj, listControl, itemObj);
                    childItem.Tag = itemObj;
                    childItem.MouseDoubleClick += ChildItem_MouseDoubleClick;
                    itemGroup.Items.Add(childItem);
                }

                this.Items.Add(itemGroup);
            }
        }

        /// <summary>
        /// 子节点的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChildItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as TreeViewItem).Tag as ControlDetailForPage;
            Controller.EventController.EventAssitant.CallEventDerectly(item.p0, this);
        }

        /// <summary>
        /// 生成节点内容
        /// </summary>
        /// <param name="listControlObj"></param>
        /// <param name="listControl"></param>
        /// <param name="treeviewItemObj"></param>
        /// <returns></returns>
        private UIElement ProduceTreeviewItem(List<ControlDetailForPage> listControlObj, List<IControl> listControl, ControlDetailForPage treeviewItemObj)
        {
            List<int> listChild = JsonController.DeSerializeToClass<List<int>>(treeviewItemObj.d17);
            ControlDetailForPage gridObj = listControlObj.Where(p => listChild.Contains(p.ctrl_id)).ToList()[0];
            ControlDecoder controlDecoder = new ControlDecoder();
            xinlongyuParentControl parentControl = new xinlongyuParentControl();
            controlDecoder.ProduceFatherControl(gridObj, listControlObj, listControl, parentControl);
            parentControl.Width = 200;
            parentControl.Height = 50;
            return parentControl;
        }

        /// <summary>
        /// 有效标记
        /// </summary>
        private const string validStr = "1";

        /// <summary>
        /// 获取所有的一级目录
        /// </summary>
        /// <returns></returns>
        private List<ControlDetailForPage> GetParentGroups(List<ControlDetailForPage> listControlObj, string childlistStr)
        {
            List<int> controlIdList = JsonController.DeSerializeToClass<List<int>>(childlistStr);
            if (controlIdList.Count < 1)
            {
                return null ;
            }
            //获取父目录控件并进行排序
            List<ControlDetailForPage> childrenList = listControlObj.Where(p => controlIdList.Contains(p.ctrl_id)
                && validStr.Equals(p.d12))
                .OrderBy(p => CommonConverter.StringToInt(p.d21)).ToList();

            return childrenList;
        }
    }
}
