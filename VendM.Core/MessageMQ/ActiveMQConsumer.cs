using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// ActiveMQ消费者,打开连接,监听队列,接收到数据之后触发回调
    /// </summary>
    public class ActiveMQConsumer : ActiveMQ, IMessageQueue, IDisposable
    {
        /// <summary>
        /// 接收到数据回调,ActiveMQ原生IMessage类型
        /// </summary>
        public Action<IMessage> OnMessageReceived { get; set; }

        /// <summary>
        /// 接收到消息回调(业务数据对象, 根据自己的业务灵活替换)
        /// </summary>
        public Action<MessageModel> OnDataCenterMessageReceived { get; set; }
        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenConnection()
        {
            if (string.IsNullOrWhiteSpace(this.BrokerUri))
                throw new MemberAccessException("未指定BrokerUri");
            if (string.IsNullOrWhiteSpace(this.QueueName))
                throw new MemberAccessException("未指定QueueName");

            var factory = new ConnectionFactory(this.BrokerUri);
            if (string.IsNullOrWhiteSpace(this.UserName) && string.IsNullOrWhiteSpace(this.Password))
                connection = factory.CreateConnection();
            else
                connection = factory.CreateConnection(this.UserName, this.Password);
            connection.Start();
            session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

            switch (MQMode)
            {
                case MQMode.Queue:
                    {
                        consumer = session.CreateConsumer(new ActiveMQQueue(this.QueueName));
                        break;
                    }
                case MQMode.Topic:
                    {
                        consumer = session.CreateConsumer(new ActiveMQTopic(this.QueueName));
                        break;
                    }
                default:
                    {
                        throw new Exception(string.Format("无法识别的MQMode类型:{0}", MQMode.ToString()));
                    }
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            consumer?.Close();
            session?.Close();
            connection?.Close();
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void StartListen()
        {
            if (consumer == null)
            {
                OpenConnection();
            }
            consumer.Listener += new MessageListener(msg =>
            {
                if (OnMessageReceived != null)
                    OnMessageReceived(msg);

                //转换为业务需要的数据对象
                if (OnDataCenterMessageReceived != null)
                {
                    var objectMessage = msg as ActiveMQObjectMessage;
                    if (objectMessage != null)
                    {
                        var dataCenterMsg = objectMessage.Body as MessageModel;
                        if (dataCenterMsg != null)
                        {
                            OnDataCenterMessageReceived(dataCenterMsg);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            this.CloseConnection();
        }
    }
}
