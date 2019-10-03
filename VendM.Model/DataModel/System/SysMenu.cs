using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenu : BaseModel
    {
        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public override int Id { get; set; }
        /// <summary>
        /// 菜单编码
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string MenuCode { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单级别
        /// </summary>
        [Required]
        public int MenuLevel { get; set; }
        /// <summary>
        /// 父节点Id
        /// </summary>
        [Required]
        public int ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        [MaxLength(100)]
        public string Icon { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 控制器方法
        /// </summary>
        public virtual ICollection<SysMenuAction> SysMenuAction { get; set; }
    }
}
