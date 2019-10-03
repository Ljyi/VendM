
using System;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static MachineDto New()
        {
            MachineDto machine = new MachineDto()
            {
            };
            return machine;
        }
        #region 需持久化属性



        /// 商店名称
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
		///	Name
		/// </summary>
        public string Name { get; set; }
        /// <summary>
		///	Code
		/// </summary>
        public string MachineNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
		///	Status
		/// </summary>
        public int Status { get; set; }

        /// <summary>
        ///	Status名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
		///	Address
		/// </summary>
        public string Address { get; set; }
        /// <summary>
		///	StoreId
		/// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// 故障类型
        /// </summary>
        public int FaultType { get; set; }
        /// <summary>
        /// 故障类型
        /// </summary>
        public string FaultTypeName { get; set; }
        /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime? FaultTime { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime { get; set; }
        /// <summary>
		///	Remark
		/// </summary>
        public string Remark { get; set; }
        #endregion
    }
}