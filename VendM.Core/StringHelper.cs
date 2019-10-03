using System;
using System.Collections.Generic;
using System.Linq;

namespace VendM.Core
{
    public static class StringHelper
    {
        #region
        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="strLength">要保留的字符串长度</param>
        /// <returns></returns>
        public static string CutStrLength(this string str, int strLength)
        {
            var strNew = str;
            if (string.IsNullOrEmpty(strNew)) return strNew;
            var strOriginalLength = strNew.Length;
            if (strOriginalLength > strLength)
            {
                strNew = strNew.Substring(0, strLength) + "...";
            }
            return strNew;
        }
        /// <summary>
        /// 字符串转换为数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static int[] StrToIntArray(this string str, char separator = ',')
        {
            int[] array = str.Split(separator).Select(p => Convert.ToInt32(p)).ToArray();
            return array;
        }
        /// <summary>
        /// 字符串转化成整型数组
        /// </summary>
        /// <param name="separator"></param>
        public static int[] ToIntArray(this string str, char separator = ',')
        {
            string[] strArray = str.Split(separator);
            return Array.ConvertAll<string, int>(strArray, delegate (string s) { return int.Parse(s); });
        }
        /// <summary>
        /// 字符串转化成list
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> ToStrList(this string str, char separator = ',')
        {
            List<string> strList = str.Split(separator).ToList();
            return strList;
        }

        #endregion

        #region
        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="strLength">要保留的字符串长度</param>
        /// <param name="endWithEllipsis">是或以省略号(...)结束</param>
        /// <returns></returns>
        public static string CutStrLength(string str, int strLength, bool endWithEllipsis)
        {
            string strNew = str;
            if (!strNew.Equals(""))
            {
                int strOriginalLength = strNew.Length;
                if (strOriginalLength > strLength)
                {
                    strNew = strNew.Substring(0, strLength);
                    if (endWithEllipsis)
                    {
                        strNew += "...";
                    }
                }
            }
            return strNew;
        }

        /// <summary>
        /// 去除字符串字符串数组中 重复的字符串
        /// 输入："qw,de,we,qw"
        /// 输出："qw,de,we"
        /// </summary>
        /// <param name="str">字符串数组</param>
        /// <returns></returns>
        public static string ToRemoveRepeat(this string str)
        {
            return string.Join(",", str.Split(',').Distinct().ToArray());
        }
        /// <summary>  
        /// 字符串替换方法  
        /// </summary>  
        /// <param name="myStr">需要替换的字符串</param>  
        /// <param name="replaceStr">需要替换的字符</param>  
        /// <param name="replaceWord">将替换为</param>  
        /// <returns></returns>  
        public static string ToReplace(this string str, string replaceStr, string replaceWord = "")
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(replaceStr))
            {
                return str;
            }
            var StrList = replaceStr.Split(',').ToList();
            foreach (var item in StrList)
            {
                str.Replace(item, replaceWord);
            }
            return str;
        }
        /// <summary>
        /// 查找字符串中重复出现最多的字符
        /// </summary>
        /// <param name="str">字符串 例："AS,SD,DE,DE,AS"</param>
        /// <returns>返回出现最多的字符串</returns>
        public static string MaxNumString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            var StrList = str.Split(',').ToList();
            Dictionary<string, int> strDictionary = new Dictionary<string, int>();
            string max = StrList[0];
            foreach (var item in StrList)
            {
                if (strDictionary.ContainsKey(item))
                {
                    strDictionary[item]++;
                }
                else
                {
                    strDictionary.Add(item, 1);
                }
                if (strDictionary[max] < strDictionary[item]) max = item;
            }
            return max;

        }
        #endregion

        #region 截断字符串(可保留完整单词)
        /// <summary>
        /// 截断字符串(可保留完整单词)
        /// </summary>
        /// <param name="valueToTruncate">需处理的字符串</param>
        /// <param name="maxLength">字符数</param>
        /// <param name="options">截断选项</param>
        /// <returns></returns>
        public static string TruncateString(this string valueToTruncate, int maxLength, TruncateOptions options)
        {
            if (valueToTruncate == null)
            {
                return "";
            }

            if (valueToTruncate.Length <= maxLength)
            {
                return valueToTruncate;
            }

            var includeEllipsis = (options & TruncateOptions.IncludeEllipsis) ==
                    TruncateOptions.IncludeEllipsis;
            var finishWord = (options & TruncateOptions.FinishWord) ==
                    TruncateOptions.FinishWord;
            var allowLastWordOverflow =
              (options & TruncateOptions.AllowLastWordToGoOverMaxLength) ==
              TruncateOptions.AllowLastWordToGoOverMaxLength;

            var retValue = valueToTruncate;

            if (includeEllipsis)
            {
                maxLength -= 1;
            }

            var lastSpaceIndex = retValue.LastIndexOf(" ",
              maxLength, StringComparison.CurrentCultureIgnoreCase);

            if (!finishWord)
            {
                retValue = retValue.Remove(maxLength);
            }
            else if (allowLastWordOverflow)
            {
                var spaceIndex = retValue.IndexOf(" ",
                  maxLength, StringComparison.CurrentCultureIgnoreCase);
                if (spaceIndex != -1)
                {
                    retValue = retValue.Remove(spaceIndex);
                }
            }
            else if (lastSpaceIndex > -1)
            {
                retValue = retValue.Remove(lastSpaceIndex);
            }

            if (includeEllipsis && retValue.Length < valueToTruncate.Length)
            {
                retValue += "...";
            }
            return retValue;
        }

        #endregion
        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        /// <param name="addSingleQuotes">是否添加单引号</param>
        public static string ToSeparateString(this IEnumerable<string> list, bool isAddSingleQuotes, char separator = ',')
        {
            if (list == null || list.Count() == 0) return string.Empty;
            string result = "";
            if (isAddSingleQuotes)
            {
                foreach (var item in list)
                {
                    result += string.Format("'{0}'{1}", item, separator);
                }
            }
            else
            {
                foreach (var item in list)
                {
                    result += string.Format("{0}{1}", item, separator);
                }
            }
            return result.TrimEnd(separator);
        }
        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        /// <param name="addSingleQuotes">是否添加单引号</param>
        public static string ToSeparateString(this IEnumerable<int> list, bool isAddSingleQuotes, char separator = ',')
        {
            return ToSeparateString(list.Select(p => p.ToString()), isAddSingleQuotes, separator);
        }

        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        public static string ToSeparateString(this IEnumerable<string> list, char separator = ',')
        {
            return ToSeparateString(list, false, separator);
        }
        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        public static string ToSeparateString(this IEnumerable<int> list, char separator = ',')
        {
            return ToSeparateString(list.Select(p => p.ToString()), separator);
        }
        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        public static string ToSeparateString(this IEnumerable<decimal> list, char separator = ',')
        {
            return ToSeparateString(list.Select(p => p.ToString()), separator);
        }
        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        public static string ToSeparateString(this IEnumerable<float> list, char separator = ',')
        {
            return ToSeparateString(list.Select(p => p.ToString()), separator);
        }
        /// <summary>
        /// 用指定符号串连数组列表（默认为逗号）
        /// </summary>
        /// <param name="separator">分隔符号</param>
        public static string ToSeparateString(this IEnumerable<double> list, char separator = ',')
        {
            return ToSeparateString(list.Select(p => p.ToString()), separator);
        }

        /// <summary>
        /// 将字符串转为 Int32 的扩展方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            return int.Parse(str);
        }
        /// <summary>
        /// 字符串分割
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string[] SplitDefault(this string str, string chars = ",")
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Split(new string[] { chars }, StringSplitOptions.RemoveEmptyEntries);
            }
            return null;
        }
        /// <summary>
        /// 字符串分割
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static List<string> SplitDefaultToList(this string str, string chars = ",")
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Split(new string[] { chars }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return new List<string>();
        }
        /// <summary>
        /// 字符串转换为Int集合
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static List<int> SplitDefaultToIntList(this string str, string chars = ",")
        {

            if (!string.IsNullOrEmpty(str))
            {
                if (str.IndexOf(chars) > 0)
                {
                    return str.Split(new string[] { chars }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                }
                else
                {
                    return new List<int>() { int.Parse(str) };
                }
            }
            return new List<int>();
        }
    }

    #region 截断字符串用的枚举
    /// <summary>
    /// 截断字符串用的枚举
    /// </summary>
    [Flags]
    public enum TruncateOptions
    {
        /// <summary>
        /// 不作任何处理
        /// </summary>
        None = 0x0,
        /// <summary>
        /// 保留完整单词
        /// </summary>
        FinishWord = 0x1,
        /// <summary>
        /// 允许最后一个单词超过最大长度限制
        /// </summary>
        AllowLastWordToGoOverMaxLength = 0x2,
        /// <summary>
        /// 字符串最后跟省略号
        /// </summary>
        IncludeEllipsis = 0x4
    }
    #endregion
}