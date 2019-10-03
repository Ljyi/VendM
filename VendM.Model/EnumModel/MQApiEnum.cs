using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MQApiEnum
    {
        /// <summary>
        /// 广告
        /// </summary>
        [Description("广告")]
        Advertisement,
        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        PayMent,
    }
    /// <summary>
    /// 消息状态枚举类型
    /// 消息状态（0：待推送，1：推送，2：接收成功，3：接收失败）
    /// </summary>
    public enum MQMessageStatusEnum
    {
        /// <summary>
        /// 待发送
        /// </summary>
        [Description("待推送")]
        SendWait = 0,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("推送")]
        Sended = 1,
        /// <summary>
        /// 推送失败
        /// </summary>
        [Description("推送失败")]
        SendFail = 2,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("接收成功")]
        AcceptSuccess = 3,
        /// <summary>
        /// 发送失败
        /// </summary>
        [Description("接收失败")]
        AcceptFail = 4,
    }
}
