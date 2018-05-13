using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 下拉菜单控件
    /// </summary>
    public class xinlongyuCombobox : ComboBox, IControl
    {
        //存储真实的值的列表
        private List<string> _listValue;

        public xinlongyuCombobox()
        {
            //不能让用户寄几进行输入
            this.IsEditable = false;
            //
            this.SelectionChanged += XinlongyuCombobox_SelectedIndexChanged;
            //
            _listValue = new List<string>();
        }

        /// <summary>
        /// 主值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XinlongyuCombobox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            if (this.SelectedIndex != -1)
            {
                string p9 = (this.Tag as ControlDetailForPage).p9;
                if (!string.IsNullOrEmpty(p9))
                {
                    //暂时注释
                    //DecoderAssistant.CallEventDerectly(p9, this);
                }
            }
        }

        /// <summary>
        /// 启动控件
        /// </summary>
        /// <param name="text"></param>
        public void SetA1(string text)
        {
            this.SetD0((this.Tag as ControlDetailForPage).d0);
        }

        public void SetA0(string value)
        {
            this.SetD0((this.Tag as ControlDetailForPage).d0);
        }

        /// <summary>
        /// 设置显示
        /// </summary>
        /// <param name="value"></param>
        public void SetD0(object value)
        {
            this.Items.Clear();
            _listValue.Clear();

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
                            string[] array = new string[1];
                            try
                            {
                                array = JsonController.DeSerializeToClass<string[]>(value.ToString().Replace("\r\n", string.Empty));
                            }
                            catch
                            {
                                array = JsonController.DeSerializeToClass<string[]>(value.ToString());
                            }
                            this.ItemsSource = array;
                            if (System.Text.RegularExpressions.Regex.IsMatch(currentObj.d5.Trim().Replace("\r\n", string.Empty), @".*\[.*\].*"))
                            {
                                //_listValue.AddRange(CommonFunction.StringToArray(currentObj.d5.Trim().Replace("\r\n", string.Empty)));
                                _listValue.AddRange(JsonController.DeSerializeToClass<string[]>(currentObj.d5.Trim()));
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(currentObj.d19) || string.IsNullOrEmpty(currentObj.d20))
                            {
                                return;
                            }
                            //sql处理
                            ConnectionManager cn = new ConnectionManager();
                            //暂时注释
                            //value = DecoderAssistant.FormatSql(value.ToString(), this);
                            Dictionary<string, string>[] result = cn.ExcuteSqlWithReturn(value.ToString().Trim()).data;
                            if (!object.Equals(result, null) && result.Length > 0)
                            {
                                foreach (Dictionary<string, string> dic in result)
                                {
                                    this.Items.Add(dic[currentObj.d20]);
                                    _listValue.Add(dic[currentObj.d19]);
                                }
                            }
                        }
                        if (this.Items.Count > 0)
                        {
                            this.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    //这里应该是用于.语法调用处理的时候进行处理，就是传进来的

                }
            }
        }

        /// <summary>
        /// 获取主值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetD0(object value)
        {
            if (this.SelectedIndex != -1)
            {
                if (_listValue.Count > 0 && _listValue.Count - 1 >= this.SelectedIndex)
                {
                    return _listValue[this.SelectedIndex];
                }
                else
                {
                    return this.Text;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
