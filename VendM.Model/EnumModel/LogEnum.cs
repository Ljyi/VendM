using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogEnum
    {
        [Description("邮件发送成功")]
        EmailSuccess = 0,
        [Description("邮件发送失败")]
        EmailError = 1
    }
}
