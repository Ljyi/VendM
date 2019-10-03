namespace VendM.Model.DataModelDto.Order
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class OrderDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static OrderDto New()
        {
            OrderDto order = new OrderDto()
            {
            };
            return order;
        }
        #region 需持久化属性
        /// <summary>
		///	订单号
		/// </summary>
        public string OrderNo { get; set; }
        /// <summary>
		///	交易ID
		/// </summary>
        public int? TransactionId { get; set; }
        /// <summary>
		///	Market名称
		/// </summary>
        public string StoreName { get; set; }
        /// <summary>
		///	机器编号
		/// </summary>
        public string MachineNo { get; set; }
        /// <summary>
		///	支付类型
		/// </summary>
        public int PayMent { get; set; }
        /// <summary>
		///	订单数量
		/// </summary>
        public int Quantity { get; set; }
        /// <summary>
		///	订单金额
		/// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
		///	积分
		/// </summary>
        public decimal? Points { get; set; }
        /// <summary>
        /// 会有卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
		///	订单状态
		/// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        ///	商品名称
        /// </summary>
        public string ProductName { get; set; }

        #endregion
    }
}
