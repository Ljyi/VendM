using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Service;

namespace ConsoleApp
{
    //定义委托，它定义了可以代表的方法的类型
    public delegate void GreetingDelegate(string name);
    class Program
    {
        private static IRepository<InventoryChangeLog> inventoryChangeLogRepository = new RepositoryBase<InventoryChangeLog>();

        private static void EnglishGreeting(string name)
        {
            Console.WriteLine("Morning, " + name);
        }

        private static void ChineseGreeting(string name)
        {
            Console.WriteLine("早上好, " + name);
        }
        //注意此方法，它接受一个GreetingDelegate类型的方法作为参数
        private static void GreetPeople(string name, GreetingDelegate MakeGreeting)
        {
            MakeGreeting(name);
        }

        static void Main(string[] args)
        {
            MessageQueueService messageQueueService = new MessageQueueService();
            List<MessageQueue> list = new List<MessageQueue>() { new MessageQueue() {
                 APIName="Advertisement",
                 CreateUser="admin",
                 CredateTime=DateTime.Now,
                 Id=10,
                 Message="删除广告",
                 MachineNo="864687040323718",
                 QueueName="Advertisement864687040323718",
                 MQType="Topic"
            } };
            Console.WriteLine("Queue Message Start");
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Task<bool> t = Task<bool>.Run(async () =>
                    {
                        return await messageQueueService.SendQueueMessagesAsync(list);
                    });
                    t.Wait();
                    if (t.Result)
                    {
                        Console.WriteLine("Queue Message Success1");
                    }
                    else
                    {
                        Console.WriteLine("Queue Message Fail2");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail2" + ex.Message);
                }
            }

            try
            {
                //VendM.Application.Transaction transaction = new VendM.Application.Transaction();
                //IPOS.Model.ResponseModel.ResultModel<IPOS.Model.ResponseModel.TransactionRepsone> resultModel = new IPOS.Model.ResponseModel.ResultModel<IPOS.Model.ResponseModel.TransactionRepsone>();
                //Task t = Task.Run(async () =>
                //{
                //    resultModel = await transaction.TransactionAsync("", "", "");
                //    await transaction.TransactionComplete("", resultModel.Result.Data.redeemTransaction.id, true);
                //});
                //t.Wait();


                //  ManagerInitializer initializer = new ManagerInitializer();
                //   initializer.Seed();
                //ProductRedeem productRedeem = new ProductRedeem();
                //Task t = Task.Run(async () =>
                //{
                //    await productRedeem.ImportProductFormAPIAsync();
                //});
                //t.Wait();
                //var producer = new ActiveMQProducer();
                //producer.BrokerUri = @"tcp://10.105.0.239:61616/";
                //producer.UserName = "thhVendM";
                //producer.Password = "1624dc65a59ec1d6b835ab8d6c57532f";
                //producer.QueueName = "topic";
                //producer.MQMode = MQMode.Topic;
                //producer.OpenConnection();
                //MessageModel messageModel = new MessageModel() { message = "\"data\": [{\"Id\": 1,\"PaymentName\": \"sample string 2\", \"PaymentCode\": \"sample string 3\"},{\"Id\": 1,\"PaymentName\": \"sample string 2\",\"PaymentCode\": \"sample string 3\"}]\" " };
                //producer.Put(messageModel);

                ////发送到队列, Put对象类必须使用[Serializable]注解属性
                //for (int i = 0; i < 2000; i++)
                //{
                //    producer.Put(messageModel);
                //}



                //var consumer = new ActiveMQConsumer();
                //consumer.BrokerUri = @"tcp://10.105.0.239:61616/";
                //consumer.UserName = "admin";
                //consumer.Password = "admin";
                //consumer.QueueName = "869751024566174";
                //consumer.MQMode = MQMode.Topic;

                //consumer.OnMessageReceived = (msg) =>
                //{
                //    var bytesMessage = msg as ActiveMQBytesMessage;
                //    if (bytesMessage != null)
                //    {
                //        var buffer = new byte[bytesMessage.BodyLength];
                //        bytesMessage.WriteBytes(buffer);
                //        var result = buffer.ToObject<MessageModel>();
                //        Console.WriteLine(result.message);
                //       // Debug.WriteLine(result);
                //    }
                //};

                //consumer.OnDataCenterMessageReceived = (msg) =>
                //{
                //    Console.WriteLine(msg.message); ;
                //};

                //consumer.Open();
                //consumer.StartListen();

                Console.ReadKey();

                //ActiveMQTest activeMQTest = new ActiveMQTest();
                //activeMQTest.SendQueue();
                //activeMQTest.Accept();
                //GreetPeople("Jimmy Zhang", EnglishGreeting);
                //GreetPeople("张子阳", ChineseGreeting);
                //Console.ReadKey();

                //OrderService orderService = new OrderService();
                //APILog aPILog = new APILog() { APIName = "123" };
                //var sendEmailHandler = new OrderEventHandlerSendEmail();
                //EventBus.Instance.Subscribe(sendEmailHandler);
                //OrderEvent orderEvent = new OrderEvent { OrderId = Guid.NewGuid() };
                //EventBus.Instance.Publish(orderEvent, EventBusTest.CallBack);
                //System.Console.WriteLine("{0}注册成功", orderEvent.OrderId);
                //InventoryChangeLog log = new InventoryChangeLog();


                ////1、初始化鱼竿
                //var fishingRod = new FishingRod();
                ////2、声明垂钓者
                //var jeff = new FishingMan("圣杰");
                ////3.分配鱼竿
                //jeff.FishingRod = fishingRod;
                ////4、注册观察者
                //fishingRod.FishingEvent += jeff.Update;

                ////5、循环钓鱼
                //while (jeff.FishCount < 5)
                //{
                //    jeff.Fishing();
                //    Console.WriteLine("-------------------");
                //    //睡眠5s
                //    Thread.Sleep(5000);
                //}
                //log.ProductId = 1;
                //log.Quantity = 1;
                //log.IsDelete = false;
                //log.ChangeType = (int)ChangeLogType.Add;
                //log.Content = string.Format("Market编码：{0}，机器编码：{1}，货道号：{2}，扣减库存：{3}", "001","0001", 1, 1);
                //log.CreateUser = "system";
                //log.CredateTime = DateTime.Now.Date;
                //inventoryChangeLogRepository.Insert(log);
                ////ManagerInitializer initializer = new ManagerInitializer();
                //// initializer.Seed();
                //GenerateCodeService generateCodeService = GenerateCodeService.SingleGenerateCodeService();
                //string storeCode = "";
                //storeCode = generateCodeService.GetNextStoreCode(ref storeCode);
                //string orderCode = "201906030000001";
                //orderCode = generateCodeService.GetNextOrderCode(ref orderCode);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
    }
}
