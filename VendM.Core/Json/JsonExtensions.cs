using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace VendM.Core.Json
{
    /// <summary>
    /// Json公共类
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        public static string DictionaryToJson<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";
            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }
        /// <summary>
        /// 将json字符串反序列化为字典类型
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>字典数据</returns>
        public static Dictionary<TKey, TValue> JsonToDictionary<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();
            Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);
            return jsonDict;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static SortedDictionary<TKey, TValue> JsonToSortedDictionary<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new SortedDictionary<TKey, TValue>();
            SortedDictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<SortedDictionary<TKey, TValue>>(jsonStr);
            return jsonDict;
        }
        /// <summary>
        /// 将JObject转化成字典
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this JToken json)
        {
            var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
            ProcessJObjectProperties(propertyValuePairs);
            ProcessJArrayProperties(propertyValuePairs);
            return propertyValuePairs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyValuePairs"></param>
        private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
        {
            var objectPropertyNames = (from property in propertyValuePairs
                                       let propertyName = property.Key
                                       let value = property.Value
                                       where value is JObject
                                       select propertyName).ToList();

            objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = propertyValuePairs[propertyName]);
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="propertyValuePairs"></param>
        private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
        {
            var arrayPropertyNames = (from property in propertyValuePairs
                                      let propertyName = property.Key
                                      let value = property.Value
                                      where value is JArray
                                      select propertyName).ToList();

            arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = propertyValuePairs[propertyName]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static object[] ToArray(this JArray array)
        {
            return array.ToObject<object[]>().Select(ProcessArrayEntry).ToArray();
        }

        private static object ProcessArrayEntry(object value)
        {
            if (value is JObject)
            {
                return ToDictionary((JObject)value);
            }
            if (value is JArray)
            {
                return ToArray((JArray)value);
            }
            return value;
        }
    }
}
