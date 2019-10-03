using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendM.Model.DataModel.Basics;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineStockDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static MachineStockDto New()
        {
            MachineStockDto machineStock = new MachineStockDto()
            {
            };
            return machineStock;
        }
        #region 需持久化属性
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
        /// <summary>
        /// 机器
        /// </summary>
        public virtual MachineDto MachineDto { get; set; }
        #endregion
    }
}