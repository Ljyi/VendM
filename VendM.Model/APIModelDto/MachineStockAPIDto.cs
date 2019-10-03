using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    ///补货库存
    /// </summary>
    public class MachineStockAPIDto
    {
        /// <summary>
        /// 机器编号
        /// </summary>
        public string MachineNo { get; set; }
        /// <summary>
        /// 库存明细
        /// </summary>
        [Required]
        public List<MachineStockDetailAPIDto> MachineStockDetails { get; set; }
    }
    /// <summary>
    /// 补货明细
    /// </summary>
    public class MachineStockDetailAPIDto
    {
        /// <summary>
        /// 通道编号
        /// </summary>
        [Required]
        public int PassageNumber { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 添加数量
        /// </summary>
        [Required]
        public int Quantity { get; set; }
    }
}
