using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 广高
    /// </summary>
    [Table("Advertisement")]
    public class Advertisement : BaseModel
    {
        /// <summary>
        /// 广告标题
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// 广告编号
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string AdvertisementNO { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 状态（上线，下线）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnable { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public virtual ICollection<Video> Videos { get; set; }
    }

}
