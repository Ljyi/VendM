using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendM.Model.DataModel
{
    /// <summary>
    /// 消息队列
    /// </summary>
    [Table("MessageQueue")]
    public class MessageQueue : BaseModel
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        [Required]
        public string QueueName { get; set; }
        /// <summary>
        /// 机器编号
        /// </summary>
        [Required]
        public string MachineNo { get; set; }
        /// <summary>
        /// API名称
        /// </summary>
        [Required]
        public string APIName { get; set; }
        /// <summary>
        /// 队列类型
        /// </summary>
        [Required]
        public string MQType { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 消息状态（0：待推送，1：推送，2：接收成功，3：接收失败）
        /// </summary>
        public int Status { get; set; }
    }
}
