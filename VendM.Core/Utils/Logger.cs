using System;
using System.Text;

namespace VendM.Core.Utils
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info = 1,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 2,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3,
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 4,
    }

    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="info">内容</param>
        /// <param name="level">等级</param>
        /// <param name="ex">异常</param>
        public static void WriteLog(string info, LogLevel level, Exception ex)
        {
            var log = NLog.LogManager.GetCurrentClassLogger();
            switch (level)
            {
                case LogLevel.Info:
                    log.Info(ex, info);
                    break;
                case LogLevel.Error:
                    if (ex == null)
                    {
                        log.Error(info);
                        break;
                    }
                    var sb = new StringBuilder();
                    sb.Append("\r\n\t" + info);
                    sb.Append("\r\n\t错误信息：" + ex.Message);
                    sb.Append("\r\n\t错误源：" + ex.Source);
                    sb.Append("\r\n\t异常方法：" + ex.TargetSite);
                    sb.Append("\r\n\t堆栈信息：" + ex.StackTrace);

                    log.Error(sb.ToString());
                    break;
                case LogLevel.Warn:
                    log.Warn(ex, info);
                    break;
                case LogLevel.Debug:
                    log.Debug(ex, info);
                    break;
                default:
                    log.Error(ex, info);
                    return;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="p">错误信息</param>
        public static void Error(string p)
        {
            WriteLog(p, LogLevel.Error, null);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="p">错误信息</param>
        /// <param name="e">异常信息</param>
        public static void Error(string p, Exception e)
        {
            WriteLog(p, LogLevel.Error, e);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="p">错误信息</param>
        /// <param name="args">格式化参数</param>
        public static void Error(string p, params object[] args)
        {
            WriteLog(string.Format(p, args), LogLevel.Error, null);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="p">信息</param>
        public static void Info(string p)
        {
            WriteLog(p, LogLevel.Info, null);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="p">信息</param>
        /// <param name="args">格式化数据</param>
        public static void Info(string p, params object[] args)
        {
            WriteLog(string.Format(p, args), LogLevel.Info, null);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="p">警告信息</param>
        public static void Warn(string p)
        {
            WriteLog(p, LogLevel.Warn, null);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="p">警告信息</param>
        /// <param name="e">异常信息</param>
        public static void Warn(string p, Exception e)
        {
            WriteLog(p, LogLevel.Warn, e);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="p">警告信息</param>
        /// <param name="args">格式化数据</param>
        public static void Warn(string p, params object[] args)
        {
            WriteLog(string.Format(p, args), LogLevel.Warn, null);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="p">Debug信息</param>
        public static void Debug(string p)
        {
            WriteLog(p, LogLevel.Debug, null);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="p">Debug信息</param>
        /// <param name="args">格式化数据</param>
        public static void Debug(string p, params object[] args)
        {
            WriteLog(string.Format(p, args), LogLevel.Debug, null);
        }
    }

    /// <summary>
    /// 日志对象
    /// </summary>
    class LogData
    {
        /// <summary>
        /// 日志信息
        /// </summary>
        public string Info
        {
            get;
            set;
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level
        {
            get;
            set;
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// 日志记录时间
        /// </summary>
        public DateTime CreateDateTime
        {
            get;
            set;
        }
    }
}
