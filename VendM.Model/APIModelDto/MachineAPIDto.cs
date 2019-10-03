
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineAPIDto
    {
        #region 需持久化属性
        /// <summary>
        /// 自增Id
        /// </summary>
        public int Id { get; set; }

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
		///	Status
		/// </summary>
        public int Status { get; set; }
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

        /// <summary>
        /// 机器明细
        /// </summary>
        public virtual ICollection<MachineDetailAPIDto> MachineDetails { get; set; }

        #endregion
    }

    /// <summary>
    /// 货道
    /// </summary>
    public class MachinePassageAPIDto
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public string MachineNo { get; set; }
        /// <summary>
        /// 货道Id
        /// </summary>
        [Required]
        public int PassagesId { get; set; }
        /// <summary>
        /// 货道容量
        /// </summary>
        [Required]
        public int Quantity { get; set; }
    }
    /// <summary>
    /// 货道(删除货道)
    /// </summary>
    public class MachinePassageDelAPIDto
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public string MachineNo { get; set; }
        /// <summary>
        /// 货道Id
        /// </summary>
        [Required]
        public string PassagesIds { get; set; }
    }

    /// <summary>
    /// 货道产品
    /// </summary>
    public class MachinePassageProductAPIDto
    {
        /// <summary>
        /// 货道Id
        /// </summary>
        public int PassageNumber { get; set; }
        /// <summary>
        /// 货道容量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 产品信息
        /// </summary>
        public string ProductInfo { get; set; }
    }
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineInfoAPIDto
    {
        public MachineInfoAPIDto()
        {
            MachineDetails = new List<MachineDetailInfoAPIDto>();
        }
        #region 需持久化属性
        /// <summary>
        /// 机器Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
		/// 机器名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
		///	机器编码
		/// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 机器明细
        /// </summary>
        public List<MachineDetailInfoAPIDto> MachineDetails { get; set; }
        #endregion
    }
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineDetailInfoAPIDto
    {
        public MachineDetailInfoAPIDto()
        {
            Product = new ProductAPIDto();
        }
        /// <summary>
        /// 通道编号
        ///</summary>
        public int PassageNumber { get; set; }
        /// <summary>
        /// 货道容量
        /// </summary>
        [Required]
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        [Required]
        public int InventoryQuantity { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        [Required]
        public ProductAPIDto Product { get; set; }
    }
}