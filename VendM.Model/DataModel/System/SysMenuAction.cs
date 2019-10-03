using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 控制器方法
    /// </summary>
    [Table("SysMenuAction")]
    public class SysMenuAction : BaseModel
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ControlName { get; set; }
        /// <summary>
        /// 控制器方法名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ActionName { get; set; }
        /// <summary>
        /// 按钮权限编码 
        /// 菜单Code+按钮Code
        /// </summary>
        [MaxLength(20)]
        public string AuthorizeCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? SortNumber { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required]
        public int SysMenuId { get; set; }
        public virtual SysMenu SysMenu { get; set; }
        /// <summary>
        /// 按钮样式
        /// </summary>
        public int? SysButtonId { get; set; }
        public virtual SysButton SysButton { get; set; }
    }
}
