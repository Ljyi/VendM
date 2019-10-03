using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;

namespace VendM.Service.BasicsService
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class ReplenishmentUserService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<ReplenishmentUser> replenishmentuserRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ReplenishmentUserService()
        {
            this.replenishmentuserRepository = new RepositoryBase<ReplenishmentUser>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(ReplenishmentUserDto entity)
        {
            var replenishmentuserEntity = Mapper.Map<ReplenishmentUserDto, ReplenishmentUser>(entity);
            return replenishmentuserRepository.Insert(replenishmentuserEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return replenishmentuserRepository.Delete(id) > 0;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var replen = replenishmentuserRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var replenishmentuser in replen)
            {
                replenishmentuser.IsDelete = true;
                replenishmentuser.UpdateTime = DateTime.Now;
                replenishmentuser.UpdateUser = currentuser;
            }
            return replenishmentuserRepository.Update(replen) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return replenishmentuserRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(ReplenishmentUserDto ReplenishmentUserdto)
        {
            var menu = Mapper.Map<ReplenishmentUserDto, ReplenishmentUser>(ReplenishmentUserdto);
            List<string> list = new List<string>() { "Name", "Email", "Status", "UpdateUser", "UpdateTime" };
            return replenishmentuserRepository.Update(menu, list) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public ReplenishmentUserDto Get(int id)
        {
            var replenishmentuserEntity = replenishmentuserRepository.Find(id);
            return Mapper.Map<ReplenishmentUser, ReplenishmentUserDto>(replenishmentuserEntity);
        }

        public string GetEmailBuyUserName(string userName)
        {
            Expression<Func<ReplenishmentUser, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(userName))
            {
                ex = ex.And(t => t.Name == userName);
            }
            var replenishmentuser = replenishmentuserRepository.GetEntities(ex).FirstOrDefault();
            return replenishmentuser.Email;
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<ReplenishmentUserDto> GetAll()
        {
            var replenishmentuserEntitys = replenishmentuserRepository.Entities.ToList();
            return Mapper.Map<List<ReplenishmentUser>, List<ReplenishmentUserDto>>(replenishmentuserEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<ReplenishmentUserDto> GetReplenishmentUserGrid(TablePageParameter tpg = null, string name = "", string email = "", string status = "")
        {
            Expression<Func<ReplenishmentUser, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(email))
            {
                ex = ex.And(t => t.Email.Contains(email));
            }
            if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                ex = ex.And(t => t.Status.ToString() == status);
            }
            var replenishmentuserList = replenishmentuserRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<ReplenishmentUser>, List<ReplenishmentUserDto>>(replenishmentuserList.ToList());
            }
            else
            {
                return Mapper.Map<List<ReplenishmentUser>, List<ReplenishmentUserDto>>(GetTablePagedList(replenishmentuserList, tpg));
            }
        }


        /// <summary>
        /// 验证補貨員編號是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        public bool IsExistReplenishmentUserNo(string replenishmentUserNo)
        {
            Expression<Func<ReplenishmentUser, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(replenishmentUserNo))
            {
                ex = ex.And(t => t.ReplenishmentUserNo == replenishmentUserNo);
            }
            return replenishmentuserRepository.IsExist(ex);
        }
        #endregion
    }
}
