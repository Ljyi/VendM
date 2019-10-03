using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel.Basics
{
    [Table("Machine")]
    public class Machine : BaseModel
    {
        /// <summary>
        /// 机器名称
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 机器编号
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string MachineNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 机器状态
        /// </summary>
        public int Status { get; set; }
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
        /// 地址
        /// </summary>
        [MaxLength(500)]
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 市场
        /// </summary>
        [Required]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        /// <summary>
        /// 机器明细
        /// </summary>
        public virtual ICollection<MachineDetail> MachineDetails { get; set; }

    }
}
