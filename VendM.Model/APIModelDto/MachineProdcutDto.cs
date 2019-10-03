using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 机器产品信息
    /// </summary>
    public class MachineProdcutDto
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public string MachineNo { get; set; }
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 货道Id
        /// </summary>
        [Required]
        public int PassagesId { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        [Required]
        public int InventoryQuantity { get; set; }
    }
    /// <summary>
    /// 机器产品信息
    /// </summary>
    public class MachineProdcutDelDto
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public string MachineNo { get; set; }
        /// <summary>
        /// 多个货道Id以逗号分隔
        /// 23,23,4,5
        /// </summary>
        [Required]
        public string PassagesIds { get; set; }
    }
}
