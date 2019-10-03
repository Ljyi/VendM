namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 库存实体
    /// </summary>
    public class StockDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static StockDto New()
        {
            StockDto stock = new StockDto()
            {
            };
            return stock;
        }
        #region 需持久化属性
        /// <summary>
		///	库存数量
		/// </summary>
        public int TotalQuantity { get; set; }
        /// <summary>
		///	产品Id
		/// </summary>
        public int ProductId { get; set; }
        #endregion
    }
}
