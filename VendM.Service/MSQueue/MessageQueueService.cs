using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.Core.MessageMQ;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto.ActiveMQ;
using VendM.Model.EnumModel;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class MessageQueueService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<MessageQueue> messageQueueRepository;
        MachineService machineService;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public MessageQueueService()
        {
            this.messageQueueRepository = new RepositoryBase<MessageQueue>();
            machineService = new MachineService();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(MessageQueueDto entity)
        {
            var messageQueueEntity = Mapper.Map<MessageQueueDto, MessageQueue>(entity);
            return messageQueueRepository.Insert(messageQueueEntity) > 0;
        }
        /// <summary>
        /// 添加队列消息
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="message"></param>
        /// <param name="mQMode"></param>
        /// <param name="isAll"></param>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public List<MessageQueue> Add(string apiName, string message, MQMode mQMode = MQMode.Topic, bool isAll = true, string machineNo = "")
        {
            List<MessageQueue> list = new List<MessageQueue>();
            if (isAll)
            {
                var machineList = machineService.GetMachineAll();
                foreach (var item in machineList)
                {
                    MessageQueue messageQueueDto = new MessageQueue()
                    {
                        QueueName = apiName + item.MachineNo,
                        MachineNo = item.MachineNo,
                        APIName = apiName,
                        Message = message,
                        MQType = mQMode.ToString(),
                        IsDelete = false,
                        CreateUser = "System",
                        CredateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        UpdateUser = "System",
                        Status = (int)MQMessageStatusEnum.SendWait
                    };
                    list.Add(messageQueueDto);
                }
            }
            else
            {
                MessageQueue messageQueueDto = new MessageQueue()
                {
                    QueueName = machineNo + apiName,
                    MachineNo = machineNo,
                    APIName = apiName,
                    Message = message,
                    MQType = mQMode.ToString(),
                    IsDelete = false,
                    CreateUser = "System",
                    CredateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    UpdateUser = "System",
                    Status = (int)MQMessageStatusEnum.SendWait
                };
                list.Add(messageQueueDto);
            }
            messageQueueRepository.Insert(list);
            return list;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return messageQueueRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return messageQueueRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(MessageQueueDto entity)
        {
            var messageQueueEntity = Mapper.Map<MessageQueueDto, MessageQueue>(entity);
            return messageQueueRepository.Update(messageQueueEntity) > 0;
        }
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="messageStatusEnum"></param>
        /// <returns></returns>
        public bool UpdateAsync(MessageQueue messageQueue, MQMessageStatusEnum statusEnum)
        {
            messageQueue.Status = (int)statusEnum;
            messageQueue.UpdateUser = "system";
            messageQueue.UpdateTime = DateTime.Now;
            return messageQueueRepository.Update(messageQueue) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public MessageQueueDto Get(int id)
        {
            var messageQueueEntity = messageQueueRepository.Find(id);
            return Mapper.Map<MessageQueue, MessageQueueDto>(messageQueueEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<MessageQueueDto> GetAll()
        {
            var messageQueueEntitys = messageQueueRepository.Entities.ToList();
            return Mapper.Map<List<MessageQueue>, List<MessageQueueDto>>(messageQueueEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MessageQueueDto> GetMessageQueueGrid(TablePageParameter tpg)
        {
            Expression<Func<MessageQueue, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var messageQueueList = messageQueueRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<MessageQueue>, List<MessageQueueDto>>(messageQueueList.ToList());
            }
            else
            {
                return Mapper.Map<List<MessageQueue>, List<MessageQueueDto>>(GetTablePagedList(messageQueueList, tpg));
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="mQApiEnum"></param>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public async Task<bool> SendQueueMessagesAsync(List<MessageQueue> list)
        {
            string tcpIp = ConfigurationManager.AppSettings["MQUri"].Trim();
            string userName = ConfigurationManager.AppSettings["MQUserName"].Trim();
            string password = ConfigurationManager.AppSettings["MQPassword"].Trim();
            return await Task<bool>.Run(() =>
            {
                var producer = new ActiveMQProducer();
                producer.BrokerUri = tcpIp;
                producer.UserName = userName;
                producer.Password = password;
                try
                {
                    producer.OpenConnection();
                    foreach (var item in list)
                    {
                        try
                        {
                            producer.QueueName = item.QueueName;
                            producer.MQMode = MQMode.Topic;
                            MessageModel messageModel = new MessageModel() { message = item.Message, api = item.APIName, queueId = item.Id };
                            string json = JsonConvert.SerializeObject(messageModel);
                            producer.Put(json);
                            Console.WriteLine("Queue Message Success");
                            UpdateAsync(item, MQMessageStatusEnum.Sended);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Queue Message Fail");
                            UpdateAsync(item, MQMessageStatusEnum.SendFail);
                        }
                    }
                    return true;
                }
                catch (System.Exception ex)
                {
                    string msg = ex.Message;
                    return false;
                }
                finally
                {
                    producer.CloseConnection();
                }
            });
        }
        #endregion
    }
}
