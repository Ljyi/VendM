using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model;
using VendM.Model.DataModel;

namespace VendM.Service
{
    public class UserRightsService
    {
        private IRepository<UserRights> userRightsRepository = null;
        private IRepository<SysMenu> menuRepository = null;
        private IRepository<SysButton> buttonRepository = null;
        public UserRightsService()
        {
            userRightsRepository = new RepositoryBase<UserRights>();
            menuRepository = new RepositoryBase<SysMenu>();
            buttonRepository = new RepositoryBase<SysButton>();
        }
        public List<UserRights> GetUserRights(int userId)
        {
            return userRightsRepository.Entities.ToList();
        }
        /// <summary>
        /// 获取用户请求Url
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRights> GetListByUrl(string url)
        {
            Expression<Func<UserRights, bool>> ex = t => true;
            ex = ex.And(t => t.SysMenuAction.SysMenu.Url == url);
            return userRightsRepository.GetEntities(ex, true).ToList();
        }
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuZTree> GetUserMenuZTree(int? userId)
        {
            var list = new List<MenuZTree>();
            Expression<Func<SysMenu, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var sysMenu = menuRepository.GetEntities(ex).ToList();
            foreach (var item in sysMenu.Where(zw => zw.ParentId == 0))
            {
                MenuZTree menuTree = new MenuZTree
                {
                    id = item.Id,
                    pId = item.ParentId,
                    name = item.MenuName,
                    isChecked = true,
                    chkDisabled = true
                };
                list.Add(menuTree);
                foreach (var itemc in sysMenu.Where(zw => zw.ParentId == item.Id))
                {
                    list.Add(new MenuZTree()
                    {
                        id = itemc.Id,
                        pId = itemc.ParentId,
                        name = itemc.MenuName,
                        isChecked = true,
                        chkDisabled = true
                    });
                }
            }
            return list;
        }
    }
}
