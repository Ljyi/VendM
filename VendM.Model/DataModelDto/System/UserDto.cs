
namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class UserDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static UserDto New()
        {
            UserDto user = new UserDto()
            {
            };
            return user;
        }
        //#region 需持久化属性
        /// <summary>
		///	用户名称
		/// </summary>
        public string UserName { get; set; }
        /// <summary>
		///	登录名称
		/// </summary>
        public string LogingName { get; set; }
        /// <summary>
		///	密码
		/// </summary>
        public string Password { get; set; }
        /// <summary>
		///	邮箱
		/// </summary>
        public string Email { get; set; }
        /// <summary>
		///	状态
		/// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        //#endregion
    }
}