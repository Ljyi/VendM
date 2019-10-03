using System.Collections.Generic;
using VendM.Model.DataModel;

namespace VendM.Model.DataModelDto.Order
{
    /// <summary>
    /// 首页订单模型实体
    /// </summary>
    public class OrderMainView : BaseModel
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Points { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }
    }
}
