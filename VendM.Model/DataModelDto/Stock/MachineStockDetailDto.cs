using System;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 售货机库存实体
    /// </summary>
    public class MachineStockDetailDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static MachineStockDetailDto New()
        {
            MachineStockDetailDto machineStockDetail = new MachineStockDetailDto()
            {
            };
            return machineStockDetail;
        }
        #region 需持久化属性
        /// <summary>
		///	货道容量
		/// </summary>
        public int TotalQuantity { get; set; }
        /// <summary>
		///	货道库存数量
		/// </summary>
        public int InventoryQuantity { get; set; }
        /// <summary>
		///	货道
		/// </summary>
        public int PassageNumber { get; set; }
        /// <summary>
		///	产品Id
		/// </summary>
        public int ProductId { get; set; }
        /// <summary>
		///	机器库存Id
		/// </summary>
        public int MachineStockId { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProdcutCode { get; set; }
        /// <summary>
        /// 补货时间
        /// </summary>
        public DateTime? LastTime { get; set; }

        #endregion
    }
}