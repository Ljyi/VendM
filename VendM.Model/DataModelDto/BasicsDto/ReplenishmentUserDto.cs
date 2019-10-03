using System.ComponentModel.DataAnnotations;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class ReplenishmentUserDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static ReplenishmentUserDto New()
        {
            ReplenishmentUserDto replenishmentuser = new ReplenishmentUserDto()
            {
            };
            return replenishmentuser;
        }
        #region 需持久化属性

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

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        #endregion
    }
}