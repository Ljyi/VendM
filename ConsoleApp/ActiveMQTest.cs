using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;

namespace ConsoleApp
{
    public class ActiveMQTest
    {
        IConnectionFactory connectionFactory = null;
        public ActiveMQTest()
        {
            connectionFactory = new ConnectionFactory("tcp://localhost:61616");
        }
        public void SendQueue()
        {
            try
            {
                //建立工厂连接
                using (IConnection connection = connectionFactory.CreateConnection())
                {
                    //通过工厂连接创建Session会话
                    using (ISession session = connection.CreateSession())
                    {
                        //通过会话创建生产者，方法里new出来MQ的Queue
                        IMessageProducer prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"));
                        //创建一个发送消息的对象
                        ITextMessage message = prod.CreateTextMessage();
                        message.Text = "测试数据"; //给这个消息对象赋实际的消息
                        //设置消息对象的属性，是Queue的过滤条件也是P2P的唯一指定属性
                        message.Properties.SetString("filter", "demo");
                        prod.Send(message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public void Accept()
        {
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                connection.ClientId = "testing listener1";
                connection.Start();
                //Create the Session  
                using (ISession session = connection.CreateSession())
                {
                    //Create the Consumer  
                    IMessageConsumer consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("testing"), "testing listener1", null, false);
                    consumer.Listener += new MessageListener(consumer_Listener);

                    Console.ReadLine();
                }
                connection.Stop();
                connection.Close();
            }
        }
        static void consumer_Listener(IMessage message)
        {
            try
            {
                ITextMessage msg = (ITextMessage)message;
                Console.WriteLine("Receive: " + msg.Text);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
