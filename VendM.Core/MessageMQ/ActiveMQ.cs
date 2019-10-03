using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// ActiveMQ
    /// </summary>
    public abstract class ActiveMQ
    {
        #region 监听连接对象
        protected IConnection connection;
        protected ISession session;
        protected IMessageConsumer consumer;
        #endregion

        /// <summary>
        /// 连接地址
        /// </summary>
        public string BrokerUri { get; set; }

        /// <summary>
        /// 用于登录的用户名,必须和密码同时指定
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用于登录的密码,必须和用户名同时指定
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 指定使用队列的模式
        /// </summary>
        public MQMode MQMode { get; set; }
    }
}
