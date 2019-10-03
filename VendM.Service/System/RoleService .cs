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
    public class RoleService : BaseService
    {
        private IRepository<Role> roleRepository = null;
        public bool isSave = true;
        public RoleService()
        {
            roleRepository = new RepositoryBase<Role>();
        }
        public List<RoleDto> GetAllRoles()
        {
            var role = roleRepository.Entities.ToList();
            return Mapper.Map<List<Role>, List<RoleDto>>(role);
        }
        public Role GetRole(int id)
        {
            return roleRepository.Find(id);
        }
        public RoleDto Find(int id)
        {
            Role role = roleRepository.Find(id);
            return Mapper.Map<Role, RoleDto>(role);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="roleName"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public List<RoleDto> GetRoleGrid(TablePageParameter gp = null, string roleName = "")
        {
            Expression<Func<Role, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(roleName))
            {
                ex = ex.And(t => t.RoleName.Contains(roleName));
            }
            var RoleList = roleRepository.GetEntities(ex);
            if (gp == null)
            {
                return Mapper.Map<List<Role>, List<RoleDto>>(RoleList.ToList());
            }
            else
            {
                return Mapper.Map<List<Role>, List<RoleDto>>(GetTablePagedList(RoleList, gp));
            }
        }
       
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        public bool Add(RoleDto roleDto)
        {
            var role = Mapper.Map<RoleDto, Role>(roleDto);
            return roleRepository.Insert(role) > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        public bool Update(RoleDto roleDto)
        {
            var role = Mapper.Map<RoleDto, Role>(roleDto);
            List<string> list = new List<string>() { "RoleName", "Status",  "UpdateUser", "UpdateTime" };
            return roleRepository.Update(role, list) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids,string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var ro = roleRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var role in ro)
            {
                role.IsDelete = true;
                role.UpdateTime = DateTime.Now;
                role.UpdateUser = currentuser;
            }
            return roleRepository.Update(ro) > 0;
        }
    }
}
