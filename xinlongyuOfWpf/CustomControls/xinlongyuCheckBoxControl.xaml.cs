using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 多选框
    /// </summary>
    public partial class xinlongyuCheckBoxControl : UserControl, IControl
    {
        /// <summary>
        /// 当前数组
        /// </summary>
        private List<CheckBoxListItem> checkListItems;

        public xinlongyuCheckBoxControl()
        {
            InitializeComponent();
            checkListItems = new List<CheckBoxListItem>();
        }

        /// <summary>
        /// 主值
        /// </summary>
        /// <param name="text"></param>
        public void SetD0(object value)
        {
            checkListItems.Clear();
            MyListBox.ItemsSource = null;

            if (!object.Equals(value, null))
            {
                if (value is string)
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        ControlDetailForPage currentObj = this.Tag as ControlDetailForPage;
                        //数组处理

                        if (System.Text.RegularExpressions.Regex.IsMatch(value.ToString().Replace("\r\n", string.Empty), @".*\[.*\].*"))
                        {
                            List<string> listArray = JsonController.DeSerializeToClass<List<string>>(value.ToString().Replace("\r\n", string.Empty));
                            //判断是否需要显示与值分开
                            if (!string.IsNullOrEmpty(currentObj.d11))
                            {
                                List<string> listValue = JsonController.DeSerializeToClass<List<string>>(currentObj.d11);
                                for (int i = 0; i < listArray.Count; i ++)
                                {
                                    checkListItems.Add(new CheckBoxListItem() { DisPlayValue = listArray[i], ActualValue = listValue [i]});
                                }
                            }
                            else
                            {
                                listArray.AsParallel().ForAll(p => checkListItems.Add(new CheckBoxListItem() { ActualValue = p, DisPlayValue = p}));
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(currentObj.d19) || string.IsNullOrEmpty(currentObj.d20))
                            {
                                return;
                            }
                            //sql处理
                            SqlController cn = new SqlController();
                            value = EventAssitant.FormatSql(value.ToString(), this);
                            var returnDic = cn.ExcuteSqlWithReturn(value.ToString().Trim());
                            var result = returnDic.data;
                            if (!object.Equals(result, null) && result.Length > 0)
                            {
                                foreach (Dictionary<string, string> dic in result)
                                {
                                    checkListItems.Add(new CheckBoxListItem() { ActualValue = dic[currentObj.d19], DisPlayValue = dic[currentObj.d20] });
                                }
                            }
                        }
                    }
                    if (!object.Equals(checkListItems, null)) MyListBox.ItemsSource = checkListItems;
                }
                
            }
        }

        public class CheckBoxListItem
        {
            public string DisPlayValue { get; set; }

            public string ActualValue { get; set; }

            private bool isSelected = false;
            public bool IsSelected { get => isSelected; set => isSelected = value; }
        }

        /// <summary>
        /// 勾选事件
        /// 这里的逻辑是为了控制智能单选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkitems_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItem = MyListBox.SelectedItem as CheckBoxListItem;
            if (object.Equals(selectedItem, null)) return;

            checkListItems.ForEach(p => p.IsSelected = false);

            var index = checkListItems.FindIndex(p => selectedItem.DisPlayValue.Equals(p.DisPlayValue));
            if (index != -1)
            {
                checkListItems[index].IsSelected = true;
            }
            MyListBox.ItemsSource = null;
            MyListBox.ItemsSource = checkListItems;
        }

        /// <summary>
        /// 获取主值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(object value)
        {
            if (!object.Equals(checkListItems, null) && checkListItems.FindIndex(p => p.IsSelected) != -1)
            {
                return checkListItems[checkListItems.FindIndex(p => p.IsSelected)].ActualValue;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置点击datatemplate之后当前行被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCurrentItem(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = true;
        }
    }
}
