using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;
using VendM.Model.EnumModel;

namespace VendM.Service.BasicsService
{

    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class InventoryChangeLogService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<InventoryChangeLog> inventoryChangeLogRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public InventoryChangeLogService()
        {
            this.inventoryChangeLogRepository = new RepositoryBase<InventoryChangeLog>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(InventoryChangeLogDto entity)
        {
            var inventoryChangeLogEntity = Mapper.Map<InventoryChangeLogDto, InventoryChangeLog>(entity);
            return inventoryChangeLogRepository.Insert(inventoryChangeLogEntity) > 0;
        }
        /// <summary>
        ///库存日志
        /// </summary>
        /// <param name="storeNo"></param>
        /// <param name="machineNo"></param>
        /// <param name="quantity"></param>
        /// <param name="productId"></param>
        /// <param name="passageNumber"></param>
        /// <returns></returns>
        public async Task<Result> AddLog(string storeNo, string machineNo, int quantity, int productId, int passageNumber, ChangeLogType changeLogType)
        {
            return await Task.Run(() =>
            {
                Result result = new Result() { Success = true };
                try
                {
                    InventoryChangeLog log = new InventoryChangeLog();
                    log.ProductId = productId;
                    log.Quantity = quantity;
                    log.IsDelete = false;
                    log.ChangeType = (int)changeLogType;
                    log.Content = string.Format("Market编码：{0}，机器编码：{1}，货道号：{2}，扣减库存：{3}", storeNo, machineNo, passageNumber, quantity);
                    log.CreateUser = "system";
                    log.CredateTime = DateTime.Now.Date;
                    log.UpdateUser = "system";
                    log.UpdateTime = DateTime.Now.Date;
                    result.Success = inventoryChangeLogRepository.Insert(log) > 0;
                    return result;
                }
                catch (Exception ex)
                {
                    result.ErrorMsg = "扣减库存日志添加异常" + ex.Message;
                    result.Success = false;
                }
                return result;
            });
        }
        /// <summary>
        /// 批量添加日志
        /// </summary>
        /// <param name="inventoryChangeLogs"></param>
        /// <param name="changeLogType"></param>
        /// <returns></returns>
        public async Task<Result> AddLogBulk(List<InventoryChangeLog> inventoryChangeLogs, ChangeLogType changeLogType)
        {
            return await Task.Run(() =>
            {
                Result result = new Result() { Success = true };
                try
                {
                    result.Success = inventoryChangeLogRepository.Insert(inventoryChangeLogs) > 0;
                    return result;
                }
                catch (Exception ex)
                {
                    result.ErrorMsg = ex.Message;
                    result.Success = false;
                }
                return result;
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var icl = inventoryChangeLogRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var inventoryChangeLog in icl)
            {
                inventoryChangeLog.IsDelete = true;
                inventoryChangeLog.UpdateTime = DateTime.Now;
                inventoryChangeLog.UpdateUser = currentuser;
            }
            return inventoryChangeLogRepository.Update(icl) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return inventoryChangeLogRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(InventoryChangeLogDto inventoryChangeLogdto)
        {
            var menu = Mapper.Map<InventoryChangeLogDto, InventoryChangeLog>(inventoryChangeLogdto);
            List<string> list = new List<string>() { "ChangeType", "Quantity", "Content", "UpdateUser", "UpdateTime" };
            return inventoryChangeLogRepository.Update(menu, list) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public InventoryChangeLogDto Get(int id)
        {
            var inventoryChangeLogEntity = inventoryChangeLogRepository.Find(id);
            return Mapper.Map<InventoryChangeLog, InventoryChangeLogDto>(inventoryChangeLogEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<InventoryChangeLogDto> GetAll()
        {
            var inventoryChangeLogEntitys = inventoryChangeLogRepository.Entities.ToList();
            return Mapper.Map<List<InventoryChangeLog>, List<InventoryChangeLogDto>>(inventoryChangeLogEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<InventoryChangeLogDto> GetInventoryChangeLogGrid(TablePageParameter tpg, string name, DateTime? dateFrom, DateTime? dateTo)
        {
            Expression<Func<InventoryChangeLog, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.Product.ProductName_CH.Contains(name));
            }
            if (dateFrom.HasValue)
            {
                dateFrom = Convert.ToDateTime((Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd HH:mm")));
                ex = ex.And(t => t.CredateTime >= dateFrom);

            }
            if (dateTo.HasValue)
            {

                dateTo = Convert.ToDateTime((Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd HH:mm"))).AddMinutes(1).AddSeconds(-1);
                ex = ex.And(t => t.CredateTime <= dateTo);
            }
            var inventoryChangeLogList = inventoryChangeLogRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<InventoryChangeLog>, List<InventoryChangeLogDto>>(inventoryChangeLogList.ToList());
            }
            else
            {
                return Mapper.Map<List<InventoryChangeLog>, List<InventoryChangeLogDto>>(GetTablePagedList(inventoryChangeLogList, tpg));
            }
        }

        #endregion
    }
}
