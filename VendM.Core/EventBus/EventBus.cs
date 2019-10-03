using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VendM.Core.EventBus
{
    public class EventBus
    {
        /// <summary>
        /// 事件总线对象
        /// </summary>
        private static EventBus eventBus = null;

        /// <summary>
        /// 领域模型事件句柄字典，用于存储领域模型的句柄
        /// </summary>
        private static ConcurrentDictionary<Type, List<object>> dicEventHandler=new ConcurrentDictionary<Type, List<object>>();

        /// <summary>
        /// 附加领域模型处理句柄时，锁住
        /// </summary>
        private readonly object syncObject = new object();

        /// <summary>
        /// 单例事件总线
        /// </summary>
        public static EventBus Instance
        {
            get
            {
                return eventBus ?? (eventBus = new EventBus());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, object, bool> eventHandlerEquals = (o1, o2) =>
        {
            var o1Type = o1.GetType();
            var o2Type = o2.GetType();
            return o1Type == o2Type;
        };
        #region 订阅事件

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            //同步锁
            lock (syncObject)
            {
                //获取领域模型的类型
                var eventType = typeof(TEvent);
                //如果此领域类型在事件总线中已注册过
                if (dicEventHandler.ContainsKey(eventType))
                {
                    var handlers = dicEventHandler[eventType];
                    if (handlers != null)
                    {
                        handlers.Add(eventHandler);
                    }
                    else
                    {
                        handlers = new List<object>
                        {
                            eventHandler
                        };
                    }
                }
                else
                {
                    dicEventHandler.TryAdd(eventType, new List<object> { eventHandler });
                }
            }
        }

        /// <summary>
        /// 订阅事件实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subTypeList"></param>
        public void Subscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : IEvent
        {
            Subscribe<TEvent>(new EventHandler<TEvent>(eventHandlerFunc));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventHandlers"></param>
        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : IEvent
        {
            foreach (var eventHandler in eventHandlers)
            {
                Subscribe<TEvent>(eventHandler);
            }
        }

        #endregion

        #region 发布事件

        public void Publish<TEvent>(TEvent tEvent, Action<TEvent, bool, Exception> callback) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (dicEventHandler.ContainsKey(eventType) && dicEventHandler[eventType] != null &&
                dicEventHandler[eventType].Count > 0)
            {
                var handlers = dicEventHandler[eventType];
                try
                {
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        eventHandler.Handle(tEvent);
                        callback(tEvent, true, null);
                    }
                }
                catch (Exception ex)
                {
                    callback(tEvent, false, ex);
                }
            }
            else
            {
                callback(tEvent, false, null);
            }
        }

        #endregion

        #region 取消订阅
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            lock (syncObject)
            {
                var eventType = typeof(TEvent);
                if (dicEventHandler.ContainsKey(eventType))
                {
                    var handlers = dicEventHandler[eventType];
                    if (handlers != null
                        && handlers.Exists(deh => eventHandlerEquals(deh, eventHandler)))
                    {
                        var handlerToRemove = handlers.First(deh => eventHandlerEquals(deh, eventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }
        #endregion

    }

}
