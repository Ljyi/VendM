using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModelDto;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class MachineService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Machine> machineRepository;
        private IRepository<MachineStock> machinestockRepository;
        private IRepository<MachineDetail> machinedetailRepository;
        private IRepository<MachineStockDetails> machinestockdetaisRepository;
        private IRepository<InventoryChangeLog> inventoryChangeLogRepository;
        private MachineStockDetailService machineStockDetailService;
        private MachineStockService machineStockService;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public MachineService()
        {
            this.machineRepository = new RepositoryBase<Machine>();
            this.machinestockRepository = new RepositoryBase<MachineStock>();
            this.machinedetailRepository = new RepositoryBase<MachineDetail>();
            this.machinestockdetaisRepository = new RepositoryBase<MachineStockDetails>();
            this.machineStockDetailService = new MachineStockDetailService();
            this.machineStockService = new MachineStockService();
            this.inventoryChangeLogRepository = new RepositoryBase<InventoryChangeLog>();
        }

        /// <summary>
        /// 获取机器ID
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        public Machine GetMachineIdByMachineNo(string MachineNo)
        {
            var machine = machineRepository.Entities;
            return machine.Where(p => p.MachineNo == MachineNo && !p.IsDelete).FirstOrDefault();
        }
        /// <summary>
        /// 根据No获取机器信息
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public Machine GetMachineByMachineNo(string machineNo)
        {
            Expression<Func<Machine, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.MachineNo == machineNo);
            }
            var machine = machineRepository.GetEntities(ex, false);
            return machine.FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineNo">机器编号</param>
        /// <param name="passageNumbers">货道号</param>
        /// <returns></returns>
        public List<MachineDetail> GetMachineByMachineNo(string machineNo, List<int> passageNumbers)
        {
            Expression<Func<MachineDetail, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.Machine.MachineNo == machineNo);
            }
            if (passageNumbers != null && passageNumbers.Count > 0)
            {
                ex = ex.And(t => passageNumbers.Contains(t.PassageNumber));
            }
            var machinedetails = machinedetailRepository.GetEntities(ex, false);
            return machinedetails.ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineID">机器库存ID</param>
        /// <param name="passageNumbers">货道号</param>
        /// <returns></returns>
        public List<MachineStockDetails> GetMachineStockDetailsByMachineStockID(int machineStockID, List<int> passageNumbers)
        {
            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            ex = ex.And(t => t.MachineStockId == machineStockID);
            if (passageNumbers != null && passageNumbers.Count > 0)
            {
                ex = ex.And(t => passageNumbers.Contains(t.PassageNumber));
            }
            var machineStockDetails = machinestockdetaisRepository.GetEntities(ex, false);
            return machineStockDetails.ToList();
        }
        //public Tuple<MachineDetail, Machine, MachineStockDetails> GeProductById(int productId)
        //{
        //    Tuple<MachineDetail, Machine, MachineStockDetails> tupleEntitys;
        //    //查询货道产品信息
        //    Expression<Func<MachineDetail, bool>> exWhere = t => true;
        //    exWhere = exWhere.And(t => !t.IsDelete);
        //    exWhere = exWhere.And(t => t.ProductId == productId);
        //    exWhere = exWhere.And(t => t.Machine.MachineNo == machineNo);
        //    var machinedetail = machinedetailRepository.GetEntities(exWhere).FirstOrDefault();


        //    return tupleEntitys;
        //}
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(MachineDto entity)
        {
            var machineEntity = Mapper.Map<MachineDto, Machine>(entity);
            return machineRepository.Insert(machineEntity) > 0;
        }

        /// <summary>
        /// 添加机器
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool AddMachine(MachineDto entity, string user)
        {
            var machine = Mapper.Map<MachineDto, Machine>(entity);
            machine.FaultType = -1;
            machine.CreateUser = user;
            machine.CredateTime = DateTime.Now;
            machine.UpdateUser = user;
            machine.UpdateTime = DateTime.Now;
            bool Insertresult = true;
            Insertresult = machineRepository.Insert(machine) > 0;
            if (Insertresult)
            {
                //添加完机器后添加一条机器初始化库存
                MachineStock macs = new MachineStock();
                macs.ThresholdPercent = 0;
                macs.TotalQuantity = 100;
                macs.MachineId = machine.Id;
                macs.CreateUser = user;
                macs.CredateTime = DateTime.Now;
                macs.UpdateUser = user;
                macs.UpdateTime = DateTime.Now;
                Insertresult = machinestockRepository.Insert(macs) > 0;
            }
            return Insertresult;
        }

        /// <summary>
        /// 添加机器库存
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool AddMachinestockd(MachineStockDto entity)
        {
            var machinestockEntity = Mapper.Map<MachineStockDto, MachineStock>(entity);
            return machinestockRepository.Insert(machinestockEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return machineRepository.Delete(id) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var made = machinedetailRepository.Entities.Where(p => !p.IsDelete && ida.Contains(p.MachineId));
            if (made.Count() > 0)
            {
                return false;
            }
            var ma = machineRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var machine in ma)
            {
                machine.IsDelete = true;
                machine.UpdateTime = DateTime.Now;
                machine.UpdateUser = currentuser;
            }
            return machineRepository.Update(ma) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return machineRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(MachineDto Machinedto)
        {
            var menu = Mapper.Map<MachineDto, Machine>(Machinedto);
            List<string> list = new List<string>() { "Name", "MachineNo", "Password", "Status", "Address", "StoreId", "FaultType", "FaultTime", "HandleTime", "UpdateUser", "UpdateTime" };
            return machineRepository.Update(menu, list) > 0;
        }

        /// <summary>
        /// 更新故障类型
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="replenishmentUser"></param>
        /// <returns></returns>
        public bool SetFault(int ids, int FaultType, DateTime? faulttime, DateTime? handletime)
        {
            Expression<Func<Machine, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (ids >= 0)
            {
                ex = ex.And(t => t.Id == ids);
            }
            var machineList = machineRepository.GetEntities(ex).ToList();
            machineList.ForEach(p =>
            {
                if (FaultType == -1)
                {
                    p.FaultType = FaultType;
                    p.HandleTime = handletime;
                    p.Status = 1;
                }
                else
                {
                    p.FaultType = FaultType;
                    p.FaultTime = faulttime;
                    p.Status = 0;
                }
            });
            return machineRepository.Update(machineList) > 0;
        }

        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public MachineDto Get(int id)
        {
            var machineEntity = machineRepository.Find(id);
            return Mapper.Map<Machine, MachineDto>(machineEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<MachineDto> GetAll()
        {
            var machineEntitys = machineRepository.Entities.ToList();
            return Mapper.Map<List<Machine>, List<MachineDto>>(machineEntitys);
        }
        public List<Machine> GetMachineAll()
        {
            Expression<Func<Machine, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var machineList = machineRepository.GetEntities(ex);
            return machineList.ToList();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MachineDto> GetMachineGrid(TablePageParameter tpg = null, string name = "", string code = "", string status = "", string fault = "")
        {
            Expression<Func<Machine, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                ex = ex.And(t => t.MachineNo.Contains(code));
            }
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                ex = ex.And(t => t.Status.ToString() == status);
            }
            if (!string.IsNullOrEmpty(fault) && fault != "-1")
            {
                ex = ex.And(t => t.FaultType.ToString() == fault);
            }
            var machineList = machineRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<Machine>, List<MachineDto>>(machineList.ToList());
            }
            else
            {
                return Mapper.Map<List<Machine>, List<MachineDto>>(GetTablePagedList(machineList, tpg));
            }
        }


        #endregion
    }
}