using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModelDto;
using VendM.Service;

namespace VendM.Service.BasicsService
{

    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class StoreService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Store> storeRepository;
        private IRepository<Machine> machineRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public StoreService()
        {
            this.storeRepository = new RepositoryBase<Store>();
            this.machineRepository = new RepositoryBase<Machine>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(StoreDto entity)
        {
            if (!storeRepository.Entities.Any(p => p.Code == entity.Code && !p.IsDelete))
            {
                var storeEntity = Mapper.Map<StoreDto, Store>(entity);
                return storeRepository.Insert(storeEntity) > 0;
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var ma = machineRepository.Entities.Where(p => !p.IsDelete && ida.Contains(p.StoreId));
            if (ma.Count() > 0)
            {
                return false;
            }
            var sto = storeRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var store in sto)
            {
                store.IsDelete = true;
                store.UpdateTime = DateTime.Now;
                store.UpdateUser = currentuser;
            }
            return storeRepository.Update(sto) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return storeRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(StoreDto storedto)
        {
            var menu = Mapper.Map<StoreDto, Store>(storedto);
            List<string> list = new List<string>() { "Name", "Code", "Status", "Address", "UpdateUser", "UpdateTime" };
            return storeRepository.Update(menu, list) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public StoreDto Get(int id)
        {
            var storeEntity = storeRepository.Find(id);
            return Mapper.Map<Store, StoreDto>(storeEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<StoreDto> GetAll()
        {
            var storeEntitys = storeRepository.Entities.ToList();
            return Mapper.Map<List<Store>, List<StoreDto>>(storeEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<StoreDto> GetStoreGrid(TablePageParameter tpg = null, string name = "", string status = "")
        {
            Expression<Func<Store, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                ex = ex.And(t => t.Status.ToString() == status);
            }
            var storeList = storeRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<Store>, List<StoreDto>>(storeList.ToList());
            }
            else
            {
                return Mapper.Map<List<Store>, List<StoreDto>>(GetTablePagedList(storeList, tpg));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetStoreList(string emptyKey = null, string emptyValue = null)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(emptyKey))
            {
                result.Add(new KeyValuePair<string, string>(emptyKey, emptyValue));
            }
            Expression<Func<Store, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var StoreList = storeRepository.GetEntities(ex);
            List<StoreDto> lists = Mapper.Map<List<Store>, List<StoreDto>>(StoreList.ToList());
            if (lists != null && lists.Count > 0)
            {
                foreach (var item in lists)
                {
                    result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.Name));
                }
            }
            return result;
        }

        #endregion
    }
}
