using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 销售类型
    /// </summary>
    public enum SaleTypeEnum
    {
        [Description("钱")]
        Money = 0,
        [Description("积分+钱")]
        MoneyAndPoint = 1,
        [Description("积分")]
        Point = 2
    }
}
