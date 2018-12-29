using LitJson;

namespace Titan.AppService.BeeCloud
{
    /// <summary>
    /// LitJson.Json扩展类
    /// </summary>
    public static class JsonDataExtension
    {
        /// <summary>
        /// 通过key获取值
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryGet(this JsonData jsonData, string key, out JsonData data)
        {
            try
            {
                data = jsonData[key];
                return true;
            }
            catch
            {
                data = null;
                return false;
            }
        }
    }
}
