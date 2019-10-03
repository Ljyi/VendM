using VendM.Model.DataModelDto;

namespace VendM.Model.DataModel.Order
{
    public class StoreOrderDetailDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static StoreOrderDetailDto New()
        {
            StoreOrderDetailDto storeOrderDetail = new StoreOrderDetailDto()
            {
            };
            return storeOrderDetail;
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
        public int SaleType { get; set; }
        /// <summary>
        /// Market名称
        /// </summary>
        public string StoreNo { get; set; }
        /// <summary>
        /// Market名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public int PayMent { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ProdcuctImge { get; set; }
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
