using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Order
{
    /// <summary>
    /// 订单
    /// </summary>
    [Table("Order")]
    public class Order : BaseModel
    {
        /// <summary>
        /// 订单编号
        /// 订单号生成规则：20180529+0000001
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string OrderNo { get; set; }
        /// <summary>
        /// 交易Id
        /// </summary>
        public int? TransactionId { get; set; }
        /// <summary>
        /// Market名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string StoreNo { get; set; }
        /// <summary>
        /// Market名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string StoreName { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MachineNo { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        [Required]
        public int PayMent { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }
        /// <summary>
        /// 会有卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal? Points { get; set; }
        /// <summary>
        /// 订单状态(交易成功0，交易失败1)
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
