using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Order
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [Table("OrderDetails")]
    public class OrderDetails : BaseModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string OrderNo { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }
        /// <summary>
        /// 销售类型(扩展字段)
        /// 积分支付,钱支付，积分+钱支付
        /// </summary>
        public int SaleType { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal? Points { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public int? ProductId { get; set; }
        public virtual Product.Product Product { get; set; }
    }
}
