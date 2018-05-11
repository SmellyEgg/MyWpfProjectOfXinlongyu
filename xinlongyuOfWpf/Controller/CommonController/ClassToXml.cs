using System.IO;
using System.Xml.Serialization;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// 类与xml文件转换类
    /// </summary>
    public class ClassToXml
    {
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, false);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (System.Exception ex)
            {
                string error = ex.Message;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            catch (System.Exception ex)
            {
                string error = ex.Message;
                return default(T);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
