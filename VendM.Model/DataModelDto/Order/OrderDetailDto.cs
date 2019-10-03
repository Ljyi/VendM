namespace VendM.Model.DataModelDto.Order
{
    /// <summary>
    /// 订单明细实体
    /// </summary>
    public class OrderDetailDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static OrderDetailDto New()
        {
            OrderDetailDto orderDetail = new OrderDetailDto()
            {
            };
            return orderDetail;
        }
        #region 需持久化属性
        /// <summary>
		///	订单号
		/// </summary>
        public string OrderNo { get; set; }
        /// <summary>
		///	产品编号
		/// </summary>
        public string ProductCode { get; set; }
        /// <summary>
		///	产品名称
		/// </summary>
        public string ProductName { get; set; }
        /// <summary>
		///	价格类型
		/// </summary>
        public int PriceType { get; set; }
        /// <summary>
		///	金额
		/// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
		///	积分
		/// </summary>
        public decimal? Points { get; set; }
        /// <summary>
		///	数量
		/// </summary>
        public int Quantity { get; set; }
        /// <summary>
		///	订单ID
		/// </summary>
        public int OrderId { get; set; }
        /// <summary>
		///	产品Id
		/// </summary>
        public int ProductId { get; set; }
        #endregion
    }
}