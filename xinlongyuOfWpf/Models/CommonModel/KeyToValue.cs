

namespace xinlongyuOfWpf.Models.CommonModel
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class KeyToValue
    {
        public KeyToValue()
        {

        }
        public KeyToValue(string newkey, string newvalue)
        {
            key = newkey;
            value = newvalue;
        }
        private string key = string.Empty;

        private string value = string.Empty;

        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
