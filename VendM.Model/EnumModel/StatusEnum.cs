using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 设备损坏类型
    /// </summary>
    public enum StatusEnum
    {
        [Description("異常")]
        Abnormal = 0,
        [Description("正常")]
        Normal = 1,
        [Description("其他")]
        Other = 2
    }

    /// <summary>
    /// 支付状态类型
    /// </summary>
    public enum PaymentEnum
    {
        [Description("启用")]
        Enable = 1,
        [Description("禁用")]
        Unable = 0
    }

    /// <summary>
    /// ProductPrice类型
    /// </summary>
    public enum ProPriceStatusEnum
    {
        [Description("禁用")]
        NotEnabled = 0,
        [Description("启用")]
        Enabled = 1
    }
}
