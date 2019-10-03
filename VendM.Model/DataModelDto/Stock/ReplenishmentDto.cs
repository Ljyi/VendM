using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendM.Model.DataModel.Basics;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 补货清单
    /// </summary>
    public class ReplenishmentDto : BaseDto
    {
        /// <summary>
        /// 补货员名称
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 补货员邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// 补货状态（0未发送，1发送成功，2发送失败）
        /// </summary>
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// 机器ID
        /// </summary>
        [Required]
        public int MachineId { get; set; }
        /// <summary>
        /// 补货单明细
        /// </summary>
        public virtual ICollection<ReplenishmentDetailDto> ReplenishmentDetails { get; set; }
    }
}
