using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class ExtendMethods
    {
        /// <summary>
        /// 将对象转换为bytes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bytes</returns>
        public static byte[] ToBytes<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 将bytes转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] bytes) where T : class
        {
            if (bytes == null)
                return default(T);
            using (var ms = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(ms) as T;
            }
        }
    }
}
