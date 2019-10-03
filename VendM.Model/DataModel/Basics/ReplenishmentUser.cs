using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 补货员
    /// </summary>
    [Table("ReplenishmentUser")]
    public class ReplenishmentUser : BaseModel
    {
        /// <summary>
        /// 补货员编号
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string ReplenishmentUserNo { get; set; }
        /// <summary>
        /// 补货员名称
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 补货员邮箱
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
