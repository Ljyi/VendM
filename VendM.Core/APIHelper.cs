using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace VendM.Core
{
    public class APIHelper
    {
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="paramDic"></param>
        /// <param name="SecretKey"></param>
        /// <returns></returns>
        public static string GetSign(string urlPath, SortedDictionary<string, string> paramDic, string SecretKey)
        {
            byte[] signatureKey = Encoding.UTF8.GetBytes(SecretKey);
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                list.Add(kv.Key + kv.Value);
            }
            list.Sort();
            string tmp = urlPath;
            foreach (string kvstr in list)
            {
                tmp = tmp + kvstr;
            }
            HMACSHA1 hmacsha1 = new HMACSHA1(signatureKey);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            byte[] hash = hmacsha1.Hash;
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="paramDic"></param>
        /// <param name="SecretKey"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static bool ValidateSign(string urlPath, Dictionary<string, Object> paramDic, string SecretKey, string sign)
        {
            byte[] signatureKey = Encoding.UTF8.GetBytes(SecretKey);
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, Object> kv in paramDic)
            {
                list.Add(kv.Key + kv.Value.ToString());
            }
            list.Sort();
            string tmp = urlPath;
            foreach (string kvstr in list)
            {
                tmp = tmp + kvstr;
            }
            HMACSHA1 hmacsha1 = new HMACSHA1(signatureKey);
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            byte[] hash = hmacsha1.Hash;
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper() == sign;
        }
    }
}
