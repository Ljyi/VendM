using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 设备损坏类型
    /// </summary>
    public enum FaultEnum
    {

        [Description("正常")]
        Fine = -1,
        [Description("網絡異常")]
        NetworkAnomaly = 0,
        [Description("貨道不出物")]
        OutAnomaly = 1,
        [Description("其他")]
        Other = 2
    }
}
