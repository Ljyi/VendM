
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModelDto;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineDetailAPIOutput 
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
        public string MachineNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(500)]
        public string ProductDetails { get; set; }

        /// <summary>
        /// 商品库存
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string ProductInventoryQuantity { get; set; }

        [Required]
        public List<ProductImageAPIDto> ProductImage { get; set; }
    }
}