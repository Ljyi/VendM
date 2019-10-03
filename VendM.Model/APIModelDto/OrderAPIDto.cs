using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 主订单
    /// </summary>
    public class OrderAPIDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MachineNo { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 销售类型(扩展字段)
        /// 积分支付,钱支付，积分+钱支付
        /// </summary>
        public int SaleType { get; set; }
        /// <summary>
        /// 货道号
        /// </summary>
        public int PassageNumber { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal? Points { get; set; }
        /// <summary>
        /// 0：客户端交易成功，出货成功.
        /// 1：客户端交易成功，出货失败.
        /// </summary>
        [Required]
        public int ClientOrderStatus { get; set; }
        /// <summary>
        /// 交易信息.
        /// </summary>
        public string TransactionInfo { get; set; }
        //[Required]
        //public List<OrderDetailAPIDto> OrderDetail { get; set; }
    }
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OrderDetailAPIDto
    {
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
        /// 货道号
        /// </summary>
        public int PassageNumber { get; set; }
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
        /// 商品ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }
    }
}
