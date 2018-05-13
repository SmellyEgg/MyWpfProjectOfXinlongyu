
namespace xinlongyuOfWpf.Models.Return
{
    /// <summary>
    /// 表单提交返回实体
    /// </summary>
    public class FileUploadReturn
    {
        public string error_code = string.Empty;

        public dataObjForFileUploadReturnData data;

    }


    public class dataObjForFileUploadReturnData
    {
        public string path = string.Empty;

    }
}
