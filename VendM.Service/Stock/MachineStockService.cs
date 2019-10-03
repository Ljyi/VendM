using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;
using VendM.Service.BasicsService;
using VendM.Service.Stock;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class MachineStockService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<MachineStock> machineStockRepository;
        private IRepository<MachineStockDetails> machineStockDetailsRepository;
        private IRepository<ReplenishmentUser> replenishmentUserRepository;
        private ReplenishmentService replenishmentService;
        private ReplenishmentUserService replenishmentUserService;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public MachineStockService()
        {
            this.machineStockRepository = new RepositoryBase<MachineStock>();
            this.machineStockDetailsRepository = new RepositoryBase<MachineStockDetails>();
            this.replenishmentUserRepository = new RepositoryBase<ReplenishmentUser>();
            this.replenishmentService = new ReplenishmentService();
            this.replenishmentUserService = new ReplenishmentUserService();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(MachineStockDto entity)
        {
            var machineStockEntity = Mapper.Map<MachineStockDto, MachineStock>(entity);
            return machineStockRepository.Insert(machineStockEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return machineStockRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return machineStockRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(MachineStockDto entity)
        {
            var machineStockEntity = Mapper.Map<MachineStockDto, MachineStock>(entity);
            return machineStockRepository.Update(machineStockEntity) > 0;
        }
        /// <summary>
        /// 补货
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public async void CreateReplenishAsync(MachineStock machineStock)
        {
            if (machineStock.ThresholdPercent * machineStock.TotalQuantity <= machineStock.RealStockQuantity)
            {
                List<ReplenishmentDetail> replenishmentDetails = new List<ReplenishmentDetail>();
                Replenishment replenishment = replenishmentService.GetByMachineId(machineStock.MachineId);
                if (replenishment != null)
                {
                    replenishmentDetails = replenishmentService.GetReplenishmentDetails(replenishment.Id);
                    var machineStockDetails = machineStock.MachineStockDetails.ToList();
                    replenishmentDetails.ForEach(p =>
                    {
                        var machineStockDetail = machineStockDetails.FirstOrDefault(o => o.ProductId == p.ProductId && o.PassageNumber == p.PassageNumber);
                        if (machineStockDetail != null)
                        {
                            p.InventoryQuantity = machineStockDetail.InventoryQuantity;
                            p.UpdateUser = "system";
                            p.UpdateTime = DateTime.Now;
                        }
                    });
                }
                else
                {
                    replenishment.MachineId = machineStock.MachineId;
                    replenishment.UserName = machineStock.ReplenishmentUser;
                    replenishment.Email = replenishmentUserService.GetEmailBuyUserName(machineStock.ReplenishmentUser);//Email
                    replenishmentDetails = machineStock.MachineStockDetails.Select(p => new ReplenishmentDetail()
                    {
                        InventoryQuantity = p.InventoryQuantity,
                        ProductName = p.Product.ProductDetails_CH,
                        ProductNo = p.Product.ProductCode,
                        PassageNumber = p.PassageNumber,
                        ProductId = p.ProductId,
                        Status = 0,
                        TotalQuantity = p.TotalQuantity,
                        IsDelete = false,
                        CreateUser = "system",
                        CredateTime = DateTime.Now,
                        UpdateUser = "system",
                        UpdateTime = DateTime.Now,
                    }).ToList();
                }
                await replenishmentService.AddAndEit(replenishment, replenishmentDetails);
            }
        }
		#endregion

		#region 单个查询/批量查询
		/// <summary>
		/// 获取单个实体
		/// </summary>
		/// <param name="machineId">机器id</param>
		/// <returns></returns>
		public MachineStock GetMachineStockByMachineId(int machineId) {
			Expression<Func<MachineStock, bool>> ex = t => true;
			ex = ex.And(t => !t.IsDelete);
			ex = ex.And(t => t.MachineId == machineId);
			var machineStock = machineStockRepository.GetEntities(ex).FirstOrDefault();
			return machineStock;
		}
		/// <summary>
		/// 获取单一实体
		/// </summary>
		/// <param name="id">id</param>
		/// <returns>返回单一实体</returns>
		public MachineStockDto Get(int id)
        {
            var machineStockEntity = machineStockRepository.Find(id);
            return Mapper.Map<MachineStock, MachineStockDto>(machineStockEntity);
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<MachineStockDto> GetAll()
        {
            var machineStockEntitys = machineStockRepository.Entities.ToList();
            return Mapper.Map<List<MachineStock>, List<MachineStockDto>>(machineStockEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MachineStockGridDto> GetMachineStockGrid(TablePageParameter tpg)
        {
            Expression<Func<MachineStock, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var machineStockList = machineStockRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<MachineStock>, List<MachineStockGridDto>>(machineStockList.ToList());
            }
            else
            {
                return Mapper.Map<List<MachineStock>, List<MachineStockGridDto>>(GetTablePagedList(machineStockList, tpg));
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MachineStockDetailDto> GetMachineStockDetailGrid(TablePageParameter tpg, string machineNo)
        {
            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.MachineStock.Machine.MachineNo == machineNo);
            }
            var machineStockList = machineStockDetailsRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<MachineStockDetails>, List<MachineStockDetailDto>>(machineStockList.ToList());
            }
            else
            {
                return Mapper.Map<List<MachineStockDetails>, List<MachineStockDetailDto>>(GetTablePagedList(machineStockList, tpg));
            }
        }
        /// <summary>
        /// 获取分配员
        /// </summary>
        /// <returns></returns>
        public List<ReplenishmentUserDto> GetReplenishmentUserList()
        {
            Expression<Func<ReplenishmentUser, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var replenishmentUserDtos = replenishmentUserRepository.GetEntities(ex);
            return Mapper.Map<List<ReplenishmentUser>, List<ReplenishmentUserDto>>(replenishmentUserDtos.ToList());
        }
        /// <summary>
        /// 更新补货员
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="replenishmentUser"></param>
        /// <returns></returns>
        public bool SetReplenishmentUser(string ids, string replenishmentUser)
        {
            List<int> idList = ids.SplitDefaultToIntList();
            Expression<Func<MachineStock, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ids))
            {
                ex = ex.And(t => idList.Contains(t.Id));
            }
            return machineStockRepository.BulkUpdate(ex, p => new MachineStock() { ReplenishmentUser = replenishmentUser }) > 0;
        }
        /// <summary>
        /// 设置警戒库存
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public bool SetPercent(string ids, string percent)
        {
            List<int> idList = ids.SplitDefaultToIntList();
            Expression<Func<MachineStock, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ids))
            {
                ex = ex.And(t => idList.Contains(t.Id));
            }
            decimal percentStock = decimal.Parse(percent) / 100;
            var machineStockList = machineStockRepository.GetEntities(ex).ToList();
            machineStockList.ForEach(p =>
            {
                p.ThresholdPercent = percentStock;
            });
            return machineStockRepository.Update(machineStockList) > 0;
        }
        #endregion
    }
}
