using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto;
using VendM.Model.DataModelDto.Product;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class MachineDetailService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<MachineDetail> machinedetailRepository = null;
        private IRepository<Machine> machineRepository = null;
        private IRepository<Product> productRepository = null;
        private IRepository<MachineStock> machinestockRepository = null;
        private IRepository<MachineStockDetails> machinestockdetailsRepository = null;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public MachineDetailService()
        {
            this.machinedetailRepository = new RepositoryBase<MachineDetail>();
            this.machineRepository = new RepositoryBase<Machine>();
            this.productRepository = new RepositoryBase<Product>();
            this.machinestockRepository = new RepositoryBase<MachineStock>();
            this.machinestockdetailsRepository = new RepositoryBase<MachineStockDetails>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(MachineDetailDto entity)
        {
            var machinedetailEntity = Mapper.Map<MachineDetailDto, MachineDetail>(entity);
            return machinedetailRepository.Insert(machinedetailEntity) > 0;
        }

        /// <summary>
        /// 添加通道，并且添加通道庫存初始化
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool AddMachineDetail(MachineDetailDto entity, string username)
        {
            bool AddResult = true;
            machinedetailRepository.BeginTransaction();
            try
            {
                var machinedetailEntity = Mapper.Map<MachineDetailDto, MachineDetail>(entity);
                machinedetailEntity.CreateUser = username;
                machinedetailEntity.CredateTime = DateTime.Now;
                machinedetailEntity.UpdateUser = username;
                machinedetailEntity.UpdateTime = DateTime.Now;
                AddResult = machinedetailRepository.Insert(machinedetailEntity) > 0;
                if (AddResult)
                {
                    MachineStockDetails MachinestockDetails = new MachineStockDetails();
                    MachinestockDetails.TotalQuantity = 0;
                    MachinestockDetails.InventoryQuantity = 0;
                    MachinestockDetails.PassageNumber = machinedetailEntity.PassageNumber;
                    MachinestockDetails.ProductId = machinedetailEntity.ProductId;
                    var machinestock = GetMachineStockIdByMachineId(machinedetailEntity.MachineId);
                    MachinestockDetails.MachineStockId = machinestock.Id;
                    MachinestockDetails.CreateUser = username;
                    MachinestockDetails.CredateTime = DateTime.Now;
                    MachinestockDetails.UpdateUser = username;
                    MachinestockDetails.UpdateTime = DateTime.Now;
                    AddResult = machinestockdetailsRepository.Insert(MachinestockDetails) > 0;
                }
                if (AddResult)
                {
                    machinedetailRepository.TransactionCommit();
                }
                else
                {
                    machinedetailRepository.TransactionRollback();
                }
            }
            catch (Exception ex)
            {

                AddResult = false;
                machinedetailRepository.TransactionRollback();
            }
            return AddResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return machinedetailRepository.Delete(id) > 0;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            machinedetailRepository.BeginTransaction();
            try
            {
                bool flg = false;
                int i = 0;
                int[] ida = ids.StrToIntArray();
                int[] machineId = new int[ida.Length];
                int[] machinestockId = new int[ida.Length];
                int[] passagenumber = new int[ida.Length];
                var machinedet = machinedetailRepository.Entities.Where(p => ida.Contains(p.Id) && !p.IsDelete);
                foreach (var mac in machinedet)
                {
                    if (!machineId.Contains(mac.MachineId))
                    {
                        machineId[i] = mac.MachineId;
                        passagenumber[i] = mac.PassageNumber;
                    }
                }
                i = 0;
                var machinestock = machinestockRepository.Entities.Where(p => !p.IsDelete && machineId.Contains(p.MachineId)).FirstOrDefault();
                if (machinestock != null)
                {
                    machinestockId[i] = machinestock.Id;
                }
                var msc = machinestockdetailsRepository.Entities.Where(p => !p.IsDelete && machinestockId.Contains(p.MachineStockId) && passagenumber.Contains(p.PassageNumber));
                foreach (var item in msc)
                {
                    item.ProductId = null;
                    item.TotalQuantity = 0;
                    item.InventoryQuantity = 0;
                    item.IsDelete = true;
                    item.UpdateTime = DateTime.Now;
                    item.UpdateUser = currentuser;
                }
                flg = machinestockdetailsRepository.Update(msc) > 0;
                if (!flg)
                {
                    throw new Exception("删除货道库存失败!");
                }
                var ma = machinedetailRepository.Entities.Where(p => ida.Contains(p.Id));
                foreach (var machinedetail in ma)
                {
                    machinedetail.ProductId = null;
                    machinedetail.IsDelete = true;
                    machinedetail.UpdateTime = DateTime.Now;
                    machinedetail.UpdateUser = currentuser;
                }
                flg = machinedetailRepository.Update(ma) > 0;
                if (!flg)
                {
                    throw new Exception("删除货道库存失败!");
                }
                //更新机器库存容量
                var totalQuantity = machinestock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.TotalQuantity);
                int realStockQuantity = machinestock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.InventoryQuantity);
                machinestock.RealStockQuantity = realStockQuantity;
                machinestock.TotalQuantity = totalQuantity;
                machinestock.UpdateTime = DateTime.Now;
                machinestock.UpdateUser = currentuser;
                flg = machinestockRepository.Update(machinestock) > 0;
                if (!flg)
                {
                    throw new Exception("更新机器库存失败!");
                }
                machinedetailRepository.TransactionCommit();
                return flg;
            }
            catch (Exception ex)
            {
                machinedetailRepository.TransactionRollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return machinedetailRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(MachineDetailDto MachineDetaildto, ref string errorMsg)
        {
            bool flg = false;
            machinedetailRepository.BeginTransaction();

            try
            {
                //拿到修改之前的货道
                var machinedetail = machinedetailRepository.Find(MachineDetaildto.Id);
                int oldMachineId = machinedetail.MachineId;
                int oldPassageNumber = machinedetail.PassageNumber;
                if (MachineDetaildto.MachineId != machinedetail.MachineId || MachineDetaildto.PassageNumber != machinedetail.PassageNumber)
                {
                    if (CheckChannel(MachineDetaildto.MachineId, MachineDetaildto.PassageNumber))
                    {
                        errorMsg = "該機器的通道已存在!";
                        throw new Exception("該機器的通道已存在!");
                    }
                }
                machinedetail.PassageNumber = MachineDetaildto.PassageNumber;
                machinedetail.ProductId = MachineDetaildto.ProductId;
                machinedetail.UpdateUser = MachineDetaildto.UpdateUser;
                machinedetail.UpdateTime = MachineDetaildto.UpdateTime;
                //var menu = Mapper.Map<MachineDetailDto, MachineDetail>(MachineDetaildto);
                //List<string> list = new List<string>() { "PassageNumber", "ProductId", "UpdateUser", "UpdateTime" };
                flg = machinedetailRepository.Update(machinedetail) > 0;
                if (!flg)
                {
                    errorMsg = "执行修改货道失败!";
                    throw new Exception("执行修改货道失败!");
                }
                var machineStock = machinestockRepository.GetEntities(p => p.MachineId == oldMachineId && p.IsDelete != true).FirstOrDefault();
                if (machineStock != null)
                {
                    var machineStockDetails = machineStock.MachineStockDetails.Where(p => p.PassageNumber == oldPassageNumber && p.IsDelete != true).FirstOrDefault();
                    if (machineStockDetails != null)
                    {
                        machineStockDetails.PassageNumber = machinedetail.PassageNumber;
                        machineStockDetails.ProductId = machinedetail.ProductId;
                        machineStockDetails.UpdateUser = machinedetail.UpdateUser;
                        machineStockDetails.UpdateTime = machinedetail.UpdateTime;
                        flg = machinestockdetailsRepository.Update(machineStockDetails) > 0;
                    }
                }
                if (!flg)
                {
                    errorMsg = "执行修改货道库存失败!";
                    throw new Exception("执行修改货道库存失败!");
                }
                machinedetailRepository.TransactionCommit();
            }
            catch (Exception ex)
            {
                flg = false;
                machinedetailRepository.TransactionRollback();
            }
            return flg;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public MachineDetailDto Get(int id)
        {
            var machinedetailEntity = machinedetailRepository.Find(id);
            return Mapper.Map<MachineDetail, MachineDetailDto>(machinedetailEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<MachineDetailDto> GetAll()
        {
            var machinedetailEntitys = machinedetailRepository.Entities.ToList();
            return Mapper.Map<List<MachineDetail>, List<MachineDetailDto>>(machinedetailEntitys);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MachineDetailDto> GetMachineDetailGrid(TablePageParameter tpg, string machinemame = "", string machinenumber = "", string productid = "")
        {
            Expression<Func<MachineDetail, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(machinemame))
            {
                ex = ex.And(t => t.Machine.Name.Contains(machinemame));
            }
            if (!string.IsNullOrEmpty(machinenumber))
            {
                ex = ex.And(t => t.Machine.MachineNo.Contains(machinenumber));
            }
            if (!string.IsNullOrEmpty(productid) && productid != "-1")
            {
                ex = ex.And(t => t.Product.Id.ToString() == productid);
            }
            var machinedetailList = machinedetailRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<MachineDetail>, List<MachineDetailDto>>(machinedetailList.ToList());
            }
            else
            {
                return Mapper.Map<List<MachineDetail>, List<MachineDetailDto>>(GetTablePagedList(machinedetailList, tpg));
            }
        }



        ///// <summary>
        ///// 获取机器ID
        ///// </summary>
        ///// <param name = "whereExpression" ></ param >
        ///// < param name="isNoTrack"></param>
        ///// <returns></returns>
        //public Machine GetMachineIdByMachineDetailId(int[] ida)
        //{
        //    return machineRepository.Entities.Where(p => ida.Contains(p.Id) && !p.IsDelete).FirstOrDefault();

        //}

        /// <summary>
        /// 获取库存ID
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        public MachineStock GetMachineStockIdByMachineId(int MachineId)
        {
            var machinestock = machinestockRepository.Entities;
            return machinestock.Where(p => p.MachineId == MachineId && !p.IsDelete).First();
        }

        public bool CheckChannel(int machineId, int passageNumber)
        {
            Expression<Func<MachineDetail, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete && t.MachineId == machineId && t.PassageNumber == passageNumber);
            return machinedetailRepository.IsExist(ex);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<MachineDetailDto> GetMachineDetailGrid(TablePageParameter tpg)
        {
            Expression<Func<MachineDetail, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var machineDetailList = machinedetailRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<MachineDetail>, List<MachineDetailDto>>(machineDetailList.ToList());
            }
            else
            {
                return Mapper.Map<List<MachineDetail>, List<MachineDetailDto>>(GetTablePagedList(machineDetailList, tpg));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetProductList(string emptyKey = null, string emptyValue = null)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(emptyKey))
            {
                result.Add(new KeyValuePair<string, string>(emptyKey, emptyValue));
            }
            Expression<Func<Product, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var ProductList = productRepository.GetEntities(ex);
            List<ProductDto> lists = Mapper.Map<List<Product>, List<ProductDto>>(ProductList.ToList());
            if (lists != null && lists.Count > 0)
            {
                foreach (var item in lists)
                {
                    result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.ProductName_CH));
                }
            }
            return result;
        }

        public List<ProductDto> GetProductGrid()
        {
            return Mapper.Map<List<Product>, List<ProductDto>>(productRepository.GetEntities(p=>!p.IsDelete).ToList());
        }

        #endregion
    }
}
