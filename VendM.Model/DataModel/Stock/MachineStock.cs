using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendM.Model.DataModel.Basics;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 机器库存
    /// </summary>
    [Table("MachineStock")]
    public class MachineStock : BaseModel
    {
        /// <summary>
        /// 预警库存百分比
        /// </summary>
        [Required]
        public decimal ThresholdPercent { get; set; }
        /// <summary>
        /// 库存容量
        /// </summary>
        [Required]
        public int TotalQuantity { get; set; }
        /// <summary>
        /// 实际库存数量
        /// </summary>
        [Required]
        public int RealStockQuantity { get; set; }
        /// <summary>
        /// 补货员工
        /// </summary>
        [MaxLength(50)]
        public string ReplenishmentUser { get; set; }
        /// <summary>
        /// 补货时间
        /// </summary>
        public DateTime? LastTime { get; set; }
        /// <summary>
        /// 机器ID
        /// </summary>
        [Required]
        public int MachineId { get; set; }
        public virtual Machine Machine { get; set; }
        /// <summary>
        /// 库存明细
        /// </summary>
        public virtual ICollection<MachineStockDetails> MachineStockDetails { get; set; }
    }
}
