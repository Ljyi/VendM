using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;
using VendM.Service.EventHandler;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class AdvertisementService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Advertisement> advertisementRepository;
        private IRepository<Video> videoRepository;
        private IUnitOfWork unitOfWork;
        private MessageQueueService messageQueueService;
        private ActiveMQEvent activeMQEvent;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public AdvertisementService()
        {
            this.advertisementRepository = new RepositoryBase<Advertisement>();
            this.videoRepository = new RepositoryBase<Video>();
            unitOfWork = new UnitOfWorkContextBase();
            messageQueueService = new MessageQueueService();
            activeMQEvent = new ActiveMQEvent();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(AdvertisementDto entity, ref string errorMsg)
        {
            try
            {
                unitOfWork.BeginTransaction();
                List<Video> videos = new List<Video>();
                List<string> videoUrlList = entity.VideoUrl.ToStrList();
                var advertisementEntity = Mapper.Map<AdvertisementDto, Advertisement>(entity);
                string advertisementNO = "100001";
                var advertisement = advertisementRepository.Entities.ToList().OrderBy(p => p.Id).LastOrDefault();
                if (advertisement != null && !string.IsNullOrEmpty(advertisement.AdvertisementNO))
                {
                    advertisementNO = (int.Parse(advertisement.AdvertisementNO) + 1).ToString();
                }
                advertisementEntity.AdvertisementNO = advertisementNO;
                int num = advertisementRepository.Insert(advertisementEntity);
                if (num > 0)
                {

                    foreach (var item in videoUrlList)
                    {
                        videos.Add(new Video()
                        {
                            VideoUrl = item,
                            AdvertisementId = advertisementEntity.Id,
                            CreateUser = entity.CreateUser,
                            CredateTime = entity.CredateTime,
                            UpdateUser = entity.CreateUser,
                            UpdateTime = entity.CredateTime
                        });
                    }
                    num = videoRepository.Insert(videos);
                }
                if (num > 0)
                {
                    List<MessageQueue> list = messageQueueService.Add("Advertisement", "添加广告");
                    activeMQEvent.SendMQMessageEvent += ActiveMQEvent_SendMQMessageEvent;
                    activeMQEvent.SendMQMessage(list);
                    unitOfWork.TransactionCommit();
                    return true;
                }
                else
                {
                    unitOfWork.TransactionRollback();
                    return true;
                }
            }
            catch (Exception ex)
            {
                unitOfWork.TransactionRollback();
                errorMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return advertisementRepository.Delete(id) > 0;
        }

        public bool DeleteReadio(int id, string filename)
        {
            List<Video> videos = advertisementRepository.Find(id).Videos.ToList();
            videos = videos.Where(zw => zw.VideoUrl == filename).ToList();
            return videoRepository.Delete(videos) > 0;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public bool DeleteAsync(string ids)
        {
            unitOfWork.BeginTransaction();
            try
            {
                int num = 0;
                List<MessageQueue> list = messageQueueService.Add("Advertisement", "删除广告");
                num = advertisementRepository.Delete(ids.ToIntArray());
                activeMQEvent.SendMQMessageEvent += ActiveMQEvent_SendMQMessageEvent;
                activeMQEvent.SendMQMessage(list);
                if (num > 0)
                {
                    unitOfWork.TransactionCommit();
                    return true;
                }
                else
                {
                    unitOfWork.TransactionRollback();
                    return false;
                }
            }
            catch (Exception)
            {
                unitOfWork.TransactionRollback();
                return false;
            }
        }

        private Task<bool> ActiveMQEvent_SendMQMessageEvent(List<MessageQueue> list)
        {
            return Task.Run(async () =>
             {
                 return await messageQueueService.SendQueueMessagesAsync(list);
             });
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(AdvertisementDto entity)
        {
            unitOfWork.BeginTransaction();
            List<Video> videos = new List<Video>();
            List<string> videoUrlList = entity.VideoUrl.ToStrList().Distinct().ToList();
            var advertisementEntity = Mapper.Map<AdvertisementDto, Advertisement>(entity);
            int num = advertisementRepository.Update(advertisementEntity, true, x => x.EndTime, x => x.Name, x => x.StartTime, x => x.IsEnable);
            var videosList = videoRepository.Entities.Where(zw => zw.AdvertisementId == entity.Id).ToList();
            foreach (var item in videoUrlList)
            {
                if (!videosList.Any(zw => zw.VideoUrl == item))
                {
                    videos.Add(new Video()
                    {
                        VideoUrl = item,
                        AdvertisementId = advertisementEntity.Id,
                        CreateUser = entity.CreateUser,
                        CredateTime = entity.CredateTime,
                        UpdateUser = entity.CreateUser,
                        UpdateTime = entity.CredateTime
                    });
                }
            }
            if (num > 0)
            {
                if (videos.Count > 0)
                {
                    num = videoRepository.Insert(videos);
                }
            }
            if (num > 0)
            {
                unitOfWork.TransactionCommit();
                return true;
            }
            else
            {
                unitOfWork.TransactionRollback();
                return true;
            }
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public AdvertisementDto Get(int id)
        {
            var advertisementEntity = advertisementRepository.Find(id);
            return Mapper.Map<Advertisement, AdvertisementDto>(advertisementEntity);
        }
        /// <summary>
        /// 验证广告名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateName(string name, int? id)
        {
            Expression<Func<Advertisement, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.Name == name);
            }
            if (id.HasValue)
            {
                ex = ex.And(t => t.Id != id.Value);
            }
            return advertisementRepository.IsExist(ex);
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<AdvertisementDto> GetAll()
        {
            var advertisementEntitys = advertisementRepository.Entities.ToList();
            return Mapper.Map<List<Advertisement>, List<AdvertisementDto>>(advertisementEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<AdvertisementDto> GetAdvertisementGrid(TablePageParameter tpg, string name)
        {
            Expression<Func<Advertisement, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.Name.Contains(name));
            }
            var advertisementList = advertisementRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<Advertisement>, List<AdvertisementDto>>(advertisementList.ToList());
            }
            else
            {
                return Mapper.Map<List<Advertisement>, List<AdvertisementDto>>(GetTablePagedList(advertisementList, tpg));
            }
        }
        #endregion
    }
}
