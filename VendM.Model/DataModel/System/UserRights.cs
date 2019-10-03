using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 用户权限
    /// </summary>
    [Table("UserRights")]
    public class UserRights : BaseModel
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public override int Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        //   [Required]
        //   public int SysMenuId { get; set; }
        //    public virtual SysMenu SysMenu { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        [Required]
        public int SysMenuActionId { get; set; }
        public virtual SysMenuAction SysMenuAction { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
