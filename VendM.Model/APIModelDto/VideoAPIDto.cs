using System.ComponentModel.DataAnnotations;

namespace VendM.Model.APIModelDto
{
    /// <summary>
    /// 视频
    /// </summary>
    public class VideoAPIDto
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 视频链接
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string VideoUrl { get; set; }
    }
}
