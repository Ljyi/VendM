using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class MachineStockDetailService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<MachineStockDetails> machineStockDetailRepository;
        private IRepository<MachineStock> machineStockRepository;
        private IRepository<InventoryChangeLog> inventoryChangeLogRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public MachineStockDetailService()
        {
            this.machineStockDetailRepository = new RepositoryBase<MachineStockDetails>();
            machineStockRepository = new RepositoryBase<MachineStock>();
            inventoryChangeLogRepository = new RepositoryBase<InventoryChangeLog>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(MachineStockDetailDto entity)
        {
            var machineStockDetailEntity = Mapper.Map<MachineStockDetailDto, MachineStockDetails>(entity);
            return machineStockDetailRepository.Insert(machineStockDetailEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return machineStockDetailRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return machineStockDetailRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(MachineStockDetailDto entity)
        {
            var machineStockDetailEntity = Mapper.Map<MachineStockDetailDto, MachineStockDetails>(entity);
            return machineStockDetailRepository.Update(machineStockDetailEntity) > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(MachineStockDetails entity)
        {
            return machineStockDetailRepository.Update(entity) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public MachineStockDetailDto Get(int id)
        {
            var machineStockDetailEntity = machineStockDetailRepository.Find(id);
            return Mapper.Map<MachineStockDetails, MachineStockDetailDto>(machineStockDetailEntity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineNo"></param>
        /// <param name="productId"></param>
        /// <param name="passageNumber"></param>
        /// <returns></returns>
        public MachineStockDetails GetMachineStockDetailByNo(string machineNo, int? productId, int? passageNumber)
        {
            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.MachineStock.Machine.MachineNo == machineNo);
            }
            if (productId.HasValue)
            {
                ex = ex.And(t => t.ProductId == productId.Value);
            }
            if (passageNumber.HasValue)
            {
                ex = ex.And(t => t.PassageNumber == passageNumber.Value);
            }
            var machine = machineStockDetailRepository.GetEntities(ex);
            return machine.FirstOrDefault();
        }

        public MachineStockDetails GetMachineStockDetailByNo(string machineNo,  int? passageNumber)
        {
            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.MachineStock.Machine.MachineNo == machineNo);
            }
            if (passageNumber.HasValue)
            {
                ex = ex.And(t => t.PassageNumber == passageNumber.Value);
            }
            var machine = machineStockDetailRepository.GetEntities(ex);
            return machine.FirstOrDefault();
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<MachineStockDetailDto> GetAll()
        {
            var machineStockDetailEntitys = machineStockDetailRepository.Entities.ToList();
            return Mapper.Map<List<MachineStockDetails>, List<MachineStockDetailDto>>(machineStockDetailEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MachineStockDetailDto> GetMachineStockDetailGrid(TablePageParameter tpg)
        {

            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var machineStockDetailList = machineStockDetailRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<MachineStockDetails>, List<MachineStockDetailDto>>(machineStockDetailList.ToList());
            }
            else
            {
                return Mapper.Map<List<MachineStockDetails>, List<MachineStockDetailDto>>(GetTablePagedList(machineStockDetailList, tpg));
            }
        }
        #endregion
    }
}
