using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModelDto;
using VendM.Service.EventHandler;

namespace VendM.Service
{
    public partial class PayMentService : BaseService
    {
        private IRepository<PayMent> paymentRepository = null;
        private ActiveMQEvent activeMQEvent;
        private MessageQueueService messageQueueService = null;
        public PayMentService()
        {
            this.paymentRepository = new RepositoryBase<PayMent>();
            activeMQEvent = new ActiveMQEvent();
            messageQueueService = new MessageQueueService();
        }
        /// <summary>
        /// 获取Machine列表
        /// </summary>
        /// <param name="gp"></param>
        /// <returns></returns>
        public List<PayMentDto> GetPayMentGrid(TablePageParameter gp, string paymentcode, string paymentname)
        {
            Expression<Func<PayMent, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(paymentcode))
            {
                ex = ex.And(t => t.PaymentCode.Contains(paymentcode));
            }
            if (!string.IsNullOrEmpty(paymentname))
            {
                ex = ex.And(t => t.PaymentName.Contains(paymentname));
            }
            var paymentList = paymentRepository.GetEntities(ex);
            if (gp == null)
            {
                return Mapper.Map<List<PayMent>, List<PayMentDto>>(paymentList.ToList());
            }
            else
            {
                return Mapper.Map<List<PayMent>, List<PayMentDto>>(GetTablePagedList(paymentList, gp));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PayMentDto Find(int id)
        {
            PayMent payment = paymentRepository.Find(id);
            return Mapper.Map<PayMent, PayMentDto>(payment);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="paymentDto"></param>
        /// <returns></returns>
        public bool Add(PayMentDto paymentDto)
        {
            try
            {
                var payment = Mapper.Map<PayMentDto, PayMent>(paymentDto);
                payment.UpdateTime = DateTime.Now;
                var res = paymentRepository.Insert(payment) > 0;
                if (res)
                {
                    List<MessageQueue> list = messageQueueService.Add("PayMent", "新增支付方式");
                    Task.Run(() =>
                    {
                        activeMQEvent.SendMQMessageEvent += messageQueueService.SendQueueMessagesAsync;
                        activeMQEvent.SendMQMessage(list);
                    });
                };
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="paymentDto"></param>
        /// <returns></returns>
        public bool Update(PayMentDto paymentDto)
        {
            var payment = Mapper.Map<PayMentDto, PayMent>(paymentDto);
            List<string> list = new List<string>() { "PaymentName", "PaymentCode", "Charge", "Fee", "Status" };
            return paymentRepository.Update(payment, list) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var pay = paymentRepository.Entities.Where(p => ida.Contains(p.Id));

            foreach (var payment in pay)
            {
                payment.IsDelete = true;
                payment.UpdateTime = DateTime.Now;
                payment.UpdateUser = currentuser;
            }
            var res = paymentRepository.Update(pay) > 0;
            if (res)
            {
                List<MessageQueue> list = messageQueueService.Add("PayMent", "删除支付方式");
                Task.Run(() =>
                {
                    activeMQEvent.SendMQMessageEvent += messageQueueService.SendQueueMessagesAsync;
                    activeMQEvent.SendMQMessage(list);
                });
            };
            return res;
        }

        /// <summary>
        /// 验证实体是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        public bool IsExist(string name)
        {
            Expression<Func<PayMent, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.PaymentName == name);
            }
            return paymentRepository.IsExist(ex);
        }
    }
}