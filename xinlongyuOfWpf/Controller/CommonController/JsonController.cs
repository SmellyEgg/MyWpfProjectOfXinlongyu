
namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// json管理类
    /// </summary>
    public class JsonController
    {
        /// <summary>
        /// 将类转换成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T objectToSerialize) where T : class
        {
            if (objectToSerialize == null)
            {
                return string.Empty;
            }
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize);
            return output;
        }

        /// <summary>
        /// 将json字符串转换为类实体
        /// </summary>
        /// <param name="jsontext"></param>
        /// <returns></returns>
        public static object DeSerializeToObject(string jsontext)
        {
            if (string.IsNullOrEmpty(jsontext))
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsontext);
        }

        /// <summary>
        /// json字符串转对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeSerializeToClass<T>(string json)
        {
            T m = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return m;
        }

    }
}
