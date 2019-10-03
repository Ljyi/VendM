using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VendM.Core.MessageMQ
{
    /// <summary>
    /// ActiveMQ生产者,打开连接,向指定队列中发送数据
    /// </summary>
    public class ActiveMQProducer : ActiveMQ, IMessageQueue, IDisposable
    {
        /// <summary>
        /// 队列缓存字典
        /// </summary>
        private ConcurrentDictionary<string, IMessageProducer> concrtProcuder = new ConcurrentDictionary<string, IMessageProducer>();

        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenConnection()
        {
            if (string.IsNullOrWhiteSpace(this.BrokerUri))
            {
                throw new MemberAccessException("未指定BrokerUri");
            }
            //if (string.IsNullOrWhiteSpace(this.QueueName))
            //{
            //    throw new MemberAccessException("未指定QueueName");
            //}
            var factory = new ConnectionFactory(this.BrokerUri);
            if (string.IsNullOrWhiteSpace(this.UserName) && string.IsNullOrWhiteSpace(this.Password))
            {
                connection = factory.CreateConnection();
            }
            else
            {
                connection = factory.CreateConnection(this.UserName, this.Password);
            }
            connection.Start();
            session = connection.CreateSession();
           // CreateProducer(this.QueueName);
        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            IMessageProducer imProducer = null;
            foreach (var p in this.concrtProcuder)
            {
                if (this.concrtProcuder.TryGetValue(p.Key, out imProducer))
                {
                    imProducer?.Close();
                }
            }
            this.concrtProcuder.Clear();
            session?.Close();
            connection?.Close();
        }
        /// <summary>
        /// 批量发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        public void Put<T>(List<T> body)
        {
            foreach (var item in body)
            {
                Send(this.QueueName, item);
            }
        }
        /// <summary>
        /// 向队列发送数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="body">数据</param>
        public void Put<T>(T body)
        {
            Send(this.QueueName, body);
        }

        /// <summary>
        /// 向指定队列发送数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="body">数据</param>
        /// <param name="queueName">指定队列名</param>
        public void Put<T>(T body, string queueName)
        {
            Send(queueName, body);
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="queueName"></param>
        private IMessageProducer CreateProducer(string queueName)
        {
            if (session == null)
            {
                OpenConnection();
            }
            //创建新生产者
            Func<string, IMessageProducer> CreateNewProducter = (name) =>
            {
                IMessageProducer newProducer = null;
                switch (MQMode)
                {
                    case MQMode.Queue:
                        {
                            newProducer = session.CreateProducer(new ActiveMQQueue(name));
                            break;
                        }
                    case MQMode.Topic:
                        {
                            newProducer = session.CreateProducer(new ActiveMQTopic(name));
                            break;
                        }
                    default:
                        {
                            throw new Exception(string.Format("无法识别的MQMode类型:{0}", MQMode.ToString()));
                        }
                }
                return newProducer;
            };
            return this.concrtProcuder.GetOrAdd(queueName, CreateNewProducter);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="body">数据</param>
        private void Send<T>(string queueName, T body)
        {
            try
            {
                var producer = CreateProducer(queueName);
                Apache.NMS.IMessage msg;
                if (body is byte[])
                {
                    msg = producer.CreateBytesMessage(body as byte[]);
                }
                else if (body is string)
                {
                    msg = producer.CreateTextMessage(body as string);
                }
                else
                {
                    msg = producer.CreateObjectMessage(body);
                }
                if (msg != null)
                {
                    producer.Send(msg, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
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
