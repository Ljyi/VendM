using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 邮箱发送状态
    /// </summary>
    public enum SendEmilTypeEnum
    {
        /// <summary>
        /// 待发送
        /// </summary>
        [Description("待发送")]
        SendWait = 0,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("发送成功")]
        SendSuccess = 1,
        /// <summary>
        /// 发送失败
        /// </summary>
        [Description("发送失败")]
        SendFail = 2,
    }
}
