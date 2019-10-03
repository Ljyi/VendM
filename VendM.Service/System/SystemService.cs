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
    public class SystemService
    {
        private IRepository<SysMenu> sysMenuRepository = null;
        private UserRightsService userRightsService = null;
        public SystemService()
        {
            sysMenuRepository = new RepositoryBase<SysMenu>();
            userRightsService = new UserRightsService();
        }
        /// <summary>
        /// 获取当前用户菜单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<MenuTree> GetMenuByUserId(int userId, bool isAdmin = false)
        {
            Expression<Func<SysMenu, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            List<SysMenu> sysMenus = sysMenuRepository.GetEntities(ex).ToList();
            List<UserRights> userRights = userRightsService.GetUserRights(userId);
            if (!isAdmin)
            {
                sysMenus = userRights.Select(p => p.SysMenuAction.SysMenu).OrderBy(p => p.SortNumber).Distinct().ToList();
            }
            List<MenuTree> menuTreeList = new List<MenuTree>();
            foreach (var item in sysMenus.Where(zw => zw.ParentId == 0))
            {
                MenuTree menuTree = new MenuTree
                {
                    MenuCode = item.MenuCode,
                    MenuName = item.MenuName,
                    Icon = item.Icon,
                    MenuLevel = item.MenuLevel,
                    ParentId = item.ParentId,
                    SortNumber = item.SortNumber,
                    Url = item.Url
                };
                foreach (var itemc in sysMenus.Where(zw => zw.ParentId == item.Id))
                {
                    menuTree.menuTrees.Add(new MenuTree()
                    {
                        MenuCode = itemc.MenuCode,
                        MenuName = itemc.MenuName,
                        Icon = itemc.Icon,
                        MenuLevel = itemc.MenuLevel,
                        ParentId = itemc.ParentId,
                        SortNumber = itemc.SortNumber,
                        Url = itemc.Url
                    });
                }
                menuTreeList.Add(menuTree);
            }
            return menuTreeList;
        }
    }
}
