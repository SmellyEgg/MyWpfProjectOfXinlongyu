using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Controller.EventController;
using xinlongyuOfWpf.Models.ControlInfo;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 逻辑判断控件
    /// </summary>
    public class xinlongyuLogicControl : Button, IControl
    {
        public xinlongyuLogicControl()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private const string _equalStr = "==";
        private const string _inequalStr = "!=";
        private const string _upperOrEqual = ">=";
        private const string _lowerOrEqual = "<=";
        private const string _upper = ">";
        private const string _lower = "<";

        public void SetA1(string text)
        {
            //判断表达式是否成立
            string d0Text = (this.Tag as ControlDetailForPage).d0;
            string specialChar = _equalStr;
            if (Regex.IsMatch(d0Text, @".+==.+"))
            {
                specialChar = _equalStr;
            }
            else if (Regex.IsMatch(d0Text, @".+!=.+"))
            {
                specialChar = _inequalStr;
            }
            else if ((Regex.IsMatch(d0Text, @".+>=.+")))
            {
                specialChar = _upperOrEqual;
            }
            else if ((Regex.IsMatch(d0Text, @".+<=.+")))
            {
                specialChar = _lowerOrEqual;
            }
            else if ((Regex.IsMatch(d0Text, @".+>.+")))
            {
                specialChar = _upper;
            }
            else if ((Regex.IsMatch(d0Text, @".+<.+")))
            {
                specialChar = _lower;
            }
            CustomEquals(d0Text, specialChar);

        }

        /// <summary>
        /// 相等的逻辑判断
        /// </summary>
        /// <param name="text"></param>
        private void CustomEquals(string text, string specialChar)
        {
            text = text.Trim();
            string rightValue = text.Substring(text.IndexOf(specialChar) + specialChar.Length).Trim();
            string leftValue = text.Substring(0, text.IndexOf(specialChar)).Trim();

            string leftPart = leftValue;
            string rightPart = rightValue;
            if (Regex.IsMatch(leftValue, @"[{].*[}]"))
            {
                leftPart = EventAssitant.FormatSql(leftPart, this);
            }
            if (Regex.IsMatch(rightValue, @"[{].*[}]"))
            {
                rightPart = EventAssitant.FormatSql(rightPart, this);
            }

            bool isSuccess = false;
            if ("''".Equals(rightPart))
            {
                rightPart = string.Empty;
            }
            switch (specialChar)
            {
                case _equalStr:
                    isSuccess = dealWithEqual(leftPart, rightPart);
                    break;
                case _inequalStr:
                    isSuccess = dealWithInEqual(leftPart, rightPart);
                    break;
                case _upperOrEqual:
                    isSuccess = dealWithUpperAndEqual(leftPart, rightPart);
                    break;
                case _lowerOrEqual:
                    isSuccess = dealWithLowerAndEqual(leftPart, rightPart);
                    break;
                case _upper:
                    isSuccess = dealWithUpper(leftPart, rightPart);
                    break;
                case _lower:
                    isSuccess = dealWithLower(leftPart, rightPart);
                    break;
                default:
                    break;
            }
            if (isSuccess)
            {
                this.SetP9((this.Tag as ControlDetailForPage).p9);
            }
            else
            {

                this.SetP12((this.Tag as ControlDetailForPage).p12);
            }

        }

        private string GetInsideValue(string value)
        {
            string controlId = Regex.Match(value, @"\d+.\w\d+").Groups[0].ToString().Split('.')[0];
            string property = Regex.Match(value, @"\d+.\w\d+").Groups[0].ToString().Split('.')[1];
            return GetValue(controlId, property);
        }

        private string GetValue(string controlId, string property)
        {
            //Window parentWindow = Window.GetWindow(this);
            return string.Empty;
            //xinlongyuForm frm = this.FindForm() as xinlongyuForm;
            //DecoderAssistant.CurrentControlList = frm._currentControlList;
            //DecoderAssistant.CurrentControlObjList = frm._currentControlObjList;

            //IControl control = DecoderAssistant.CurrentControlList.First(p => (p as Control).Name.Equals(controlId.Trim()));
            //string result = DecoderAssistant.GetValue(control, property.Trim()).ToString();
            //return result;
        }

        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="leftvalue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        private bool dealWithEqual(string leftvalue, string rightValue)
        {
            if (leftvalue.Equals(rightValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断不相等
        /// </summary>
        /// <param name="leftvalue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        private bool dealWithInEqual(string leftvalue, string rightValue)
        {

            if (!leftvalue.Equals(rightValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool dealWithUpperAndEqual(string leftvalue, string rightValue)
        {
            if (CommonConverter.StringToInt(leftvalue) >= CommonConverter.StringToInt(rightValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool dealWithUpper(string leftvalue, string rightValue)
        {
            if (CommonConverter.StringToInt(leftvalue) > CommonConverter.StringToInt(rightValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool dealWithLower(string leftvalue, string rightValue)
        {
            if (CommonConverter.StringToInt(leftvalue) < CommonConverter.StringToInt(rightValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool dealWithLowerAndEqual(string leftvalue, string rightValue)
        {
            if (CommonConverter.StringToInt(leftvalue) <= CommonConverter.StringToInt(rightValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
