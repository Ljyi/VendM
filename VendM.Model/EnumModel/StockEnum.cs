using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    /// <summary>
    /// 百分比
    /// </summary>
    public enum PercentEnum
    {
        /// <summary>
        /// 百分之10
        /// </summary>
        [Description("10%")]
        Ten = 10,
        /// <summary>
        /// 百分之20
        /// </summary>
        [Description("20%")]
        Twenty = 20,
        /// <summary>
        /// 百分之30
        /// </summary>
        [Description("30%")]
        Thirty = 30,
        /// <summary>
        /// 百分之40
        /// </summary>
        [Description("40%")]
        Forty = 40,
        /// <summary>
        /// 百分之50
        /// </summary>
        [Description("50%")]
        Fifty = 50,
        /// <summary>
        /// 百分之60
        /// </summary>
        [Description("60%")]
        Sixty = 60,
        /// <summary>
        /// 百分之70
        /// </summary>
        [Description("70%")]
        Seventy = 70,
        /// <summary>
        /// 百分之80
        /// </summary>
        [Description("80%")]
        Eighty = 80,
        /// <summary>
        /// 百分之90
        /// </summary>
        [Description("90%")]
        Ninety = 90
    }

    public enum ChangeLogType
    {
        /// <summary>
        /// 扣减库存
        /// </summary>
        [Description("扣减库存")]
        Reduce = 0,
        /// <summary>
        /// 添加库存
        /// </summary>
        [Description("添加库存")]
        Add = 1,
    }
}
