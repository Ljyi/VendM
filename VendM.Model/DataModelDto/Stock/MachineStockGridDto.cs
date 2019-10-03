using System;

namespace VendM.Model.DataModelDto
{
    public class MachineStockGridDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static MachineStockGridDto New()
        {
            MachineStockGridDto machineStockGridDto = new MachineStockGridDto()
            {
            };
            return machineStockGridDto;
        }
        #region 需持久化属性
        /// <summary>
        /// Market
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 机器编号
        /// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 机器地址
        /// </summary>
        public string MachineAddress { get; set; }
        /// <summary>
        ///	库存警戒
        /// </summary>
        public decimal ThresholdPercent { get; set; }
        /// <summary>
		///	貨道容量
		/// </summary>
        public int TotalQuantity { get; set; }
        /// <summary>
		///	补货人
		/// </summary>
        public string ReplenishmentUser { get; set; }
        /// <summary>
		///	补货时间
		/// </summary>
        public DateTime? LastTime { get; set; }
        /// <summary>
		///	机器ID
		/// </summary>
        public int MachineId { get; set; }
        #endregion
    }
}
