using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 广告实体
    /// </summary>
    public class AdvertisementAPIDto
    {
        #region 需持久化属性
        /// <summary>
        /// 自增Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///	广告名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public virtual ICollection<VideoAPIDto> Videos { get; set; }
        #endregion
    }
}
