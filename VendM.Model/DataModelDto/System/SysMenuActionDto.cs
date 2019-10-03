using System.ComponentModel.DataAnnotations;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 菜单功能Dto
    /// </summary>
    public class SysMenuActionDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static SysMenuActionDto New()
        {
            SysMenuActionDto sysMenuActionDto = new SysMenuActionDto()
            {
            };
            return sysMenuActionDto;
        }
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
        [Required]
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
        /// <summary>
        /// 按钮样式
        /// </summary>
        public int SysButtonId { get; set; }
    }

    public class SysMenuActionGridDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static SysMenuActionGridDto New()
        {
            SysMenuActionGridDto sysMenuActionDto = new SysMenuActionGridDto()
            {
            };
            return sysMenuActionDto;
        }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string ButtonName { get; set; }
        /// <summary>
        /// 按钮编码
        /// </summary>
        public string ButtonCode { get; set; }
        /// <summary>
        /// 控制器方法名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 按钮权限编码 
        /// 菜单Code+按钮Code
        /// </summary>
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
        public int SysMenuId { get; set; }
        /// <summary>
        /// 按钮样式
        /// </summary>
        public int SysButtonId { get; set; }
    }
}
