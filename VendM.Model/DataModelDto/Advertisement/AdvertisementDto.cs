using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 实体
    /// </summary>
    public class AdvertisementDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static AdvertisementDto New()
        {
            AdvertisementDto advertisement = new AdvertisementDto()
            {
            };
            return advertisement;
        }
        #region 需持久化属性
        /// <summary>
        ///	Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 广告编号
        /// </summary>
        public string AdvertisementNO { get; set; }
        /// <summary>
        ///	StaerTime
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
        /// <summary>
        ///	EndTime
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 视频链接
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string VideoUrl { get; set; }
        /// <summary>
		///	Status
		/// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 中文状态
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        ///	是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public virtual ICollection<VideoDto> Videos { get; set; }
        #endregion
    }
}