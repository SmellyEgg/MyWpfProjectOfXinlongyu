
namespace xinlongyuOfWpf.Models.DecodeModel
{
    /// <summary>
    /// 控件解析类实体
    /// </summary>
    public class DecoderOfControl
    {
        private int tarGetctrlId;

        private string type = string.Empty;

        private string leftCtrlProperty = string.Empty;

        private object rightDirectValue;

        private string patameter = string.Empty;

        /// <summary>
        /// 控件ID
        /// </summary>
        public int CtrlId
        {
            get
            {
                return tarGetctrlId;
            }

            set
            {
                tarGetctrlId = value;
            }
        }

        /// <summary>
        /// 左侧控件属性
        /// </summary>
        public string LeftCtrlProperty
        {
            get
            {
                return leftCtrlProperty;
            }

            set
            {
                leftCtrlProperty = value;
            }
        }

        /// <summary>
        /// 右半部值部分
        /// </summary>
        public object RightDirectValue
        {
            get
            {
                return rightDirectValue;
            }

            set
            {
                rightDirectValue = value;
            }
        }

        /// <summary>
        /// 类型，包括sql与点语法
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Patameter
        {
            get
            {
                return patameter;
            }

            set
            {
                patameter = value;
            }
        }
    }
}
