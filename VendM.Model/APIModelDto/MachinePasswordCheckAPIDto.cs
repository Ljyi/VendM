using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    public class MachinePasswordCheckAPIDto
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public string MachineNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
