using System.ComponentModel.DataAnnotations;
using VendM.Model.DataModel.Basics;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MachineDetailDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static MachineDetailDto New()
        {
            MachineDetailDto machinedetail = new MachineDetailDto()
            {
            };
            return machinedetail;
        }
        #region 需持久化属性

        /// <summary>
        /// 所属Market
        /// </summary>
        public string StoreName { get; set; }

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
        /// 机器ID
        /// </summary>
        public int MachineId { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductCode { get; set; }

        public string MachineName { get; set; }


        public string MachineNo { get; set; }

        public string ProductName_CH { get; set; }
        
        #endregion
    }
}