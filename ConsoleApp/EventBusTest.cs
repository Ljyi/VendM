using System;
using VendM.Core.EventBus;

namespace ConsoleApp
{
    public static class EventBusTest
    {
        public static void CallBack(OrderEvent orderGeneratorEvent, bool result, Exception ex)
        {
            if (result)
            {
                System.Console.WriteLine("用户下单订阅事件执行成功");
            }
            else
            {
                System.Console.WriteLine("用户下单订阅事件执行失败");
            }
        }
    }
    public class OrderEvent : IEvent
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public Guid OrderId { get; set; }
    }
    /// <summary>
    /// send email
    /// </summary>
    public class OrderEventHandlerSendEmail : IEventHandler<OrderEvent>
    {

        public void Handle(OrderEvent tEvent)
        {
            System.Console.WriteLine(string.Format("{0}的订单已发送", tEvent.OrderId));
        }
    }


    /// <summary>
    /// 鱼的品类枚举
    /// </summary>
    public enum FishType
    {
        鲫鱼,
        鲤鱼,
        黑鱼,
        青鱼,
        草鱼,
        鲈鱼
    }
    /// <summary>
    ///     鱼竿（被观察者）
    /// </summary>
    public class FishingRod
    {
        public delegate void FishingHandler(FishType type); //声明委托
        public event FishingHandler FishingEvent; //声明事件
        public void ThrowHook(FishingMan man)
        {
            Console.WriteLine("开始下钩！");
            //用随机数模拟鱼咬钩，若随机数为偶数，则为鱼咬钩
            if (new Random().Next() % 2 == 0)
            {
                var type = (FishType)new Random().Next(0, 5);
                Console.WriteLine("铃铛：叮叮叮，鱼儿咬钩了");
                if (FishingEvent != null)
                    FishingEvent(type);
            }
        }
    }
    /// <summary>
    ///垂钓者（观察者）
    /// </summary>
    public class FishingMan
    {
        public FishingMan(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public int FishCount { get; set; }

        /// <summary>
        /// 垂钓者自然要有鱼竿啊
        /// </summary>
        public FishingRod FishingRod { get; set; }
        public void Fishing()
        {
            this.FishingRod.ThrowHook(this);
        }
        public void Update(FishType type)
        {
            FishCount++;
            Console.WriteLine("{0}：钓到一条[{2}]，已经钓到{1}条鱼了！", Name, FishCount, type);
        }
    }
}
