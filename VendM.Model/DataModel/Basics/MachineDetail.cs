using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendM.Model.DataModel.Basics;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 机器详情
    /// </summary>
    [Table("MachineDetail")]
    public class MachineDetail : BaseModel
    {
        /// <summary>
        /// 通道编号
        /// </summary>
        [Required]
        public int PassageNumber { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        //[Required]
        public int? ProductId { get; set; }
        public virtual Product.Product Product { get; set; }
        /// <summary>
        /// 机器ID
        /// </summary>
        [Required]
        public int MachineId { get; set; }

        public virtual Machine Machine { get; set; }


        //public virtual Store Store { get; set; }
    }
}
