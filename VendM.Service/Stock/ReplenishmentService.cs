using LinqKit;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core.Utils;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.EnumModel;
using VendM.Model.TemplateModel;
using VendM.Service.Log;

namespace VendM.Service.Stock
{
    public class ReplenishmentService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Replenishment> replenishmentRepository;
        private IRepository<ReplenishmentDetail> replenishmentDetailsRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ReplenishmentService()
        {
            this.replenishmentRepository = new RepositoryBase<Replenishment>();
            this.replenishmentDetailsRepository = new RepositoryBase<ReplenishmentDetail>();
        }
        /// <summary>
        /// 添加修改
        /// </summary>
        /// <returns></returns>
        public async Task AddAndEit(Replenishment replenishment, List<ReplenishmentDetail> replenishmentDetails)
        {
            await Task.Run(() =>
            {
                try
                {
                    //更新
                    if (replenishment.Id > 0)
                    {
                        replenishmentDetailsRepository.Update(replenishmentDetails);
                    }
                    else
                    {
                        //添加
                        replenishmentRepository.Insert(replenishment);
                        replenishmentDetails.ForEach(p =>
                        {
                            p.ReplenishmentId = replenishment.Id;
                        });
                        replenishmentDetailsRepository.Insert(replenishmentDetails);
                    }
                    return true;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }

        /// <summary>
        /// 获取补货清单
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public Replenishment GetByMachineId(int machineId)
        {
            Expression<Func<Replenishment, bool>> expression = t => true;
            expression = expression.And(t => !t.IsDelete);
            expression = expression.And(t => t.MachineId == machineId);
            var machineStockList = replenishmentRepository.GetEntities(expression, false);
            return machineStockList.FirstOrDefault();
        }


        /// <summary>
        /// 获取补货清单
        /// </summary>
        /// <returns></returns>
        public List<Replenishment> GetAll()
        {
            Expression<Func<Replenishment, bool>> expression = t => true;
            expression = expression.And(t => !t.IsDelete);
            expression = expression.And(t => t.Status != (int)SendEmilTypeEnum.SendSuccess);
            var replenishmentList = replenishmentRepository.GetEntities(expression, false).ToList();
            return replenishmentList;
        }
        /// <summary>
        /// 获取补货明细
        /// </summary>
        /// <param name="replenishmentId"></param>
        /// <returns></returns>
        public List<ReplenishmentDetail> GetReplenishmentDetails(int replenishmentId)
        {
            Expression<Func<ReplenishmentDetail, bool>> expression = t => true;
            expression = expression.And(t => !t.IsDelete);
            expression = expression.And(t => t.ReplenishmentId == replenishmentId);
            return replenishmentDetailsRepository.GetEntities(expression, false).ToList();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(List<Replenishment> lis)
        {
            return replenishmentRepository.Update(lis) > 0;
        }

        public void ReplenishmentEmail()
        {
            try
            {
                var lis = GetAll();
                if (!lis.Any())
                    return;

                var userList = lis.GroupBy(it => new { it.Email, it.UserName });
                foreach (var item in userList)
                {
                    if (item.Any())
                        SendEmail(item.ToList());
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="lis"></param>
        public async void SendEmail(List<Replenishment> lis)
        {
            try
            {
                var groupLis = lis.GroupBy(it => new { it.Machine.Store.Name, it.Machine.Store.Code, it.Machine.MachineNo, it.Machine.Store.Address });
                var sendELis = new List<SendEmail>();
                foreach (var item in groupLis)
                {
                    if (!item.Any())
                        continue;
                    sendELis.Add(new SendEmail
                    {
                        Name = item.Key.Name,
                        Code = item.Key.Code,
                        MachineNo = item.Key.MachineNo,
                        Address = item.Key.Address,
                        ReplenishmentLis = item.ToList()
                    });
                }

                if (sendELis.Count() <= 0)
                    return;

                string resultHtml;
                var config = new TemplateServiceConfiguration();
                using (var service = RazorEngineService.Create(config))
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/Views/Template/EmailTemplate.cshtml";
                    string index = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
                    resultHtml = service.RunCompile(index, string.Empty, null, sendELis);
                }
                var isSend = false;

                await Task.Run(async () =>
                 {
                     var mailToArray = lis.FirstOrDefault().Email;//GetValue("MailToArray").Split(',');
                     MailHelper mail = new MailHelper();
                     mail.mailSubject = "";
                     mail.mailBody = resultHtml;
                     //接收人
                     mail.mailToArray = new string[] { mailToArray };
                     //抄送人
                     //mail.mailCcArray 
                     mail.mailSubject = "售貨機補貨單";
                     mail.isbodyHtml = true;
                     mail.host = GetValue("MailHost");
                     mail.mailFrom = GetValue("MailUserName");
                     mail.mailPwd = GetValue("MailPassword");
                     isSend = mail.Send();
                     //更新状态
                     var updateLis = lis.Select(it => new Replenishment
                     {
                         Id = it.Id,
                         CreateUser = it.CreateUser,
                         CredateTime = it.CredateTime,
                         UserName = it.UserName,
                         Email = it.Email,
                         MachineId = it.MachineId,
                         UpdateTime = DateTime.Now,
                         UpdateUser = "SendEmailJob",
                         Status = isSend ? (int)SendEmilTypeEnum.SendSuccess : (int)SendEmilTypeEnum.SendFail
                     }).ToList();

                     var updateRes = Update(updateLis);
                     //记录日志
                     await new LogService().Add(new Model.DataModelDto.LogDto
                     {
                         LogType = isSend ? SendEmilTypeEnum.SendSuccess.ToString() : SendEmilTypeEnum.SendFail.ToString(),
                         LogContent = resultHtml,
                         CreateDate = DateTime.Now,
                         CredateTime = DateTime.Now,
                         UpdateTime = DateTime.Now,
                         UpdateUser = "SendEmailJob",
                         CreateUser = "SendEmailJob"
                     });
                 });

            }
            catch (Exception ex)
            {
                //记录日志
                await new LogService().Add(new Model.DataModelDto.LogDto
                {
                    LogType = SendEmilTypeEnum.SendFail.ToString(),
                    LogContent = ex.Message,
                    CreateDate = DateTime.Now,
                    CreateUser = "Sys"
                });
            }
        }
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].Trim();
        }
    }
}
