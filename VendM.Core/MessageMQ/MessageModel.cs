using System;
using System.Collections.Generic;

namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// 消息模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageModel<T>
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> data { get; set; }
    }
    /// <summary>
    /// 消息模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class MessageModel
    {
        /// <summary>
        /// 队列Id
        /// </summary>
        public int queueId { get; set; }
        /// <summary>
        /// API名称
        /// </summary>
        public string api { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
    }
}
