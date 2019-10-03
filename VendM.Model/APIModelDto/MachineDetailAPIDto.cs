using System.ComponentModel.DataAnnotations;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModelDto;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineDetailAPIDto
    {
        /// <summary>
        /// 通道编号
        /// </summary>
        /// </summary>
        [Required]
        public int PassageNumber { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        public virtual ProductAPIDto Product { get; set; }
        /// <summary>
        /// 机器ID
        /// </summary>
        [Required]
        public int MachineId { get; set; }
        public virtual MachineAPIDto Machine { get; set; }
    }
}