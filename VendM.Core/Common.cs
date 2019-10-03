using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VendM.Core
{
    /// <summary>
    /// 常用方法
    /// </summary>
    public static class Common
    {
        public static bool YorNToBool(string value)
        {
            return Defaults.YesorNo_True.Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }
        public static string BoolToYorN(bool value)
        {
            return (value ? Defaults.YesorNo_True : Defaults.YesorNo_False);
        }
        /// <summary>
        /// 字符串true/false 转化成 Y/N
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrueorFalseToYorN(string value)
        {
            return Defaults.TrueorFalse_True.Equals(value, StringComparison.CurrentCultureIgnoreCase) ? Defaults.YesorNo_True : Defaults.YesorNo_False;
        }

        public static string YesOrNoString(string value)
        {
            return Defaults.YesorNo_True.Equals(value, StringComparison.CurrentCultureIgnoreCase) ? "是" : "否";
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pageSize"></param>
        /// <param name="singlePageList"></param>
        public static void ToPagingProcess<T>(this IEnumerable<T> item, int pageSize, Action<IEnumerable<T>> singlePageList) where T : class
        {
            if (item != null && item.Count() > 0)
            {
                var cnt = item.Count();
                var totalPages = item.Count() / pageSize;
                if (cnt % pageSize > 0) totalPages += 1;

                for (int pageIndex = 1; pageIndex <= totalPages; pageIndex++)
                {
                    var currentPageItems = item.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    singlePageList(currentPageItems);
                }
            }
        }

        /// <summary>
        /// 把输入的字符串转化为数据库存储的Y或N。
        /// </summary>
        /// <param name="isValidityCheck">如果不为Y时是否都为N</param>
        public static string ConvertToYorN(string value, bool isValidityCheck = false)
        {
            if (value != null) value = value.Trim();
            if ("Y".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "True".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "T".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "是" == value || "1" == value)
                return Defaults.YesorNo_True;
            else if (!isValidityCheck || "N".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "False".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "F".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "否" == value || "0" == value)
                return Defaults.YesorNo_False;
            else throw new Exception("不能转换为Y或N");
        }

        /// <summary>
        /// 判断 非负浮点数（整数、浮点）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsFloat(string value)
        {
            string pattern = @"^\d+(\.\d+)?$";
            bool IsSuccess = true;
            if (!string.IsNullOrEmpty(value))
            {
                Match m = Regex.Match(value, pattern);
                if (!m.Success)
                {
                    IsSuccess = false;
                }
            }
            return IsSuccess;
        }

        /// <summary>
        /// 判断是否为整数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isIncludeZero">是否包括零 默认true:包括</param>
        /// <returns></returns>
        public static bool IsNumeric(string number, bool isIncludeZero = true)
        {
            string patternNO = @"^[0-9]\d*$";
            if (!isIncludeZero)
                patternNO = @"^[1-9]\d*$";
            bool IsSuccess = true;
            if (!string.IsNullOrEmpty(number))
            {
                Match m = Regex.Match(number, patternNO);
                if (!m.Success)
                {
                    IsSuccess = false;
                }
            }
            return IsSuccess;
        }

        #region 日期
        /// <summary>
        /// 获得原日期的最小值  [小时:分钟:秒 为  00:00:00]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static DateTime GetCurrentMinDate(DateTime sourceDate)
        {
            return sourceDate.Date;
        }
        /// <summary>
        /// 获得原日期的最大值  [小时:分钟:秒 为  23:59:59]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static DateTime GetCurrentMaxDate(DateTime sourceDate)
        {
            return sourceDate.Date.AddDays(1).AddSeconds(-1);
        }

        /// <summary>
        /// 获得原日期 这个月的第一天
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static DateTime GetCurrentMonthFirstDay(DateTime sourceDate)
        {
            //sourceDate = sourceDate.AddMonths(-1);
            return new DateTime(sourceDate.Year, sourceDate.Month, 1);
        }
        /// <summary>
        /// 获得原日期 这个月的最后一天
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static DateTime GetCurrentMonthLastDay(DateTime sourceDate)
        {
            sourceDate = new DateTime(sourceDate.Year, sourceDate.Month, 1);
            return sourceDate.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获得原日期 上一个月的第一天
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static DateTime GetLastMonthFirstDay(DateTime sourceDate)
        {
            sourceDate = sourceDate.AddMonths(-1);
            return new DateTime(sourceDate.Year, sourceDate.Month, 1);
        }
        /// <summary>
        /// 获得原日期 上一个月的最后一天
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static DateTime GetLastMonthLastDay(DateTime sourceDate)
        {
            return new DateTime(sourceDate.Year, sourceDate.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// 获得当前日期格式[yyyy-MM-dd]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static string GetShortDateTimeString(DateTime? sourceDate)
        {
            return sourceDate.HasValue ? sourceDate.Value.ToString(Defaults.DateFormat) : string.Empty;
        }
        /// <summary>
        /// 获得当前日期格式[yyyyMMdd]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static string GetShortDateString(DateTime? sourceDate)
        {
            return sourceDate.HasValue ? sourceDate.Value.ToString("yyyyMMdd") : string.Empty;
        }
        /// <summary>
        /// 获得当前日期格式[yyyy-MM-dd HH:mm:ss]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static string GetLongDateTimeString(DateTime? sourceDate)
        {
            return sourceDate.HasValue ? sourceDate.Value.ToString(Defaults.DateTimeFormat) : string.Empty;
        }

        /// <summary>
        /// 获得当前日期格式[HH:mm:ss]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static string GetTimeDateTimeString(DateTime? sourceDate)
        {
            return sourceDate.HasValue ? sourceDate.Value.ToString(Defaults.DateFormatTime) : string.Empty;
        }

        /// <summary>
        /// 获得当前日期格式的月份[yyyy-MM]
        /// </summary>
        /// <param name="sourceDate">原日期</param>
        /// <returns></returns>
        public static string GetDateMonthString(DateTime? sourceDate)
        {
            return sourceDate.HasValue ? sourceDate.Value.ToString(Defaults.DateMonthFormat) : string.Empty;
        }

        /// <summary>
        /// 获取指定时间属于哪个季度
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static int GetCurQuarter(DateTime dt)
        {
            return ((int)(dt.Month - 1) / 3) + 1;
        }

        /// <summary>
        /// 系统最小时间
        /// </summary>
        public static DateTime MinDateTime
        {
            get { return Convert.ToDateTime("1900-1-1 0:00:00"); }
        }
        /// <summary>
        /// 是否为null或无效
        /// </summary>
        public static bool IsNullDateTime(DateTime? value)
        {
            if (value == null || value == DateTime.MinValue || value <= Common.MinDateTime)
                return true;
            return false;
        }
        public static DateTime? ConvertToNullableDateTime(DateTime? value)
        {
            if (IsNullDateTime(value)) return null;
            return value;
        }
        public static DateTime? ConvertToNullableDateTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return ConvertToNullableDateTime(Convert.ToDateTime(value.Trim()));
        }
        /// <summary>  
        /// 获取时间戳Timestamp    
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string GetTimeStamp(DateTime dt)
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((dt - dateStart).TotalSeconds).ToString();
        }
        /// <summary>
        /// //获取相差时间
        /// </summary>
        /// <param name="Interval">相差的时间类别</param>
        /// <param name="StartDate">开始时间</param>
        /// <param name="EndDate">截止时间</param>
        /// <returns></returns>
        public static long GetDateDiff(DateInterval Interval, DateTime StartDate, DateTime EndDate)
        {
            long lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long)(TS.Days / 30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }
        /// <summary>返回当前日期的星期名称</summary>  
        /// <param name="dt">日期</param>  
        /// <returns>星期名称</returns>  
        public static string GetWeekNameOfDay(DateTime idt)
        {
            string dt, week = "";

            dt = idt.DayOfWeek.ToString();
            switch (dt)
            {
                case "Monday":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期六";
                    break;
                case "Sunday":
                    week = "星期日";
                    break;

            }
            return week;
        }
        #endregion

        /// <summary>
        /// 获得数据的百分比
        /// </summary>
        /// <param name="molecular">分子</param>
        /// <param name="denominator">分母</param>
        /// <returns></returns>
        public static string GetDataPercentage(decimal molecular, decimal denominator)
        {
            if (molecular == 0M && denominator == 0M)   //分子与分母都为0
            {
                return "0%";
            }
            else if (molecular == 0M && denominator != 0M)      //分子为0 分母不为0
            {
                return "0%";
            }
            else if (molecular != 0M && denominator == 0M)      //分子不为0 分母为0
            {
                return "100%";
            }
            else
            {
                return Convert.ToDecimal((molecular / denominator) * 100).ToString("0.00") + "%";
            }
        }

        /// <summary>
        /// 小数转换成百分比
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ConvertDecimalToPercentage(decimal? d)
        {
            string result = string.Empty;
            if (d.HasValue)
            {
                result = Math.Round(d.Value * 100, 2) + "%";
            }
            return result;
        }

        /// <summary>
        /// 将百分比转换成小数
        /// </summary>
        /// <param name="perc">百分比值，可纯为数值，或都加上%号的表示，
        /// 如：65|65%</param>
        /// <returns></returns>
        public static decimal PerctangleToDecimal(string perc)
        {
            try
            {
                var sdf = perc.Substring(0, perc.Length - 1);
                var d = Decimal.Parse(sdf);
                decimal percNum = Decimal.Parse(perc.Substring(0, perc.Length - 1));
                return percNum / (decimal)100;
            }
            catch
            {
                return 1;
            }
        }


        /// <summary>
        /// 获得数据类型字符串
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string GetDataTypeString(string dataType)
        {
            switch (dataType)
            {
                case "1":
                    return "字符串";
                case "2":
                    return "整数";
                case "3":
                    return "小数";
                case "4":
                    return "布尔值";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 将值转化为decimal类型，报错时返回0
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>value</returns>
        public static decimal ConvertToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }


        ///   <summary>
        ///   将指定字符串按指定长度进行剪切，
        ///   </summary>
        ///   <param   name= "oldStr "> 需要截断的字符串 </param>
        ///   <param   name= "maxLength "> 字符串的最大长度 </param>
        ///   <param   name= "endWith "> 超过长度的后缀 </param>
        ///   <returns> 如果超过长度，返回截断后的新字符串加上后缀，否则，返回原字符串 </returns>
        public static string SubString(string oldStr, int maxLength, string endWith)
        {
            if (string.IsNullOrEmpty(oldStr))
                return oldStr + endWith;
            if (maxLength < 1)
                throw new Exception("返回的字符串长度必须大于[0] ");
            if (oldStr.Length > maxLength)
            {
                string strTmp = oldStr.Substring(0, maxLength);
                if (string.IsNullOrEmpty(endWith))
                    return strTmp;
                else
                    return strTmp + endWith;
            }
            return oldStr;
        }

        /// <summary>
        /// 保留2位小数
        /// </summary>
        /// <param name="dec">数字</param>
        /// <param name="n">小数的位置</param>
        /// <returns></returns>
        public static decimal ToRound(this Decimal dec)
        {
            return decimal.Parse(string.Format("{0:F2}", dec));
        }
        /// <summary>
        /// 保留2位小数
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static decimal ToRound(this Decimal? dec)
        {
            if (dec.HasValue)
            {
                return decimal.Parse(string.Format("{0:F2}", dec.Value));
            }

            return 0.00M;
        }
    }

    public enum DateInterval
    {
        Second, Minute, Hour, Day, Week, Month, Quarter, Year
    }
}
