using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;


namespace VendM.Service
{
    public class MenuService : BaseService
    {
        private IRepository<SysMenu> menuRepository = null;
        private IRepository<SysButton> buttonRepository = null;
        public MenuService()
        {
            menuRepository = new RepositoryBase<SysMenu>();
            buttonRepository = new RepositoryBase<SysButton>();
        }
        public List<SysMenu> GetAllMenus()
        {
            var menu = menuRepository.Entities.ToList();
            return menu;
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysMenu GetMenu(int id)
        {
            return menuRepository.Find(id);
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysMenuDto Find(int id)
        {
            SysMenu menu = menuRepository.Find(id);
            return Mapper.Map<SysMenu, SysMenuDto>(menu);
        }
        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool IsHasSubNode(int menuId)
        {
            Expression<Func<SysMenu, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            ex = ex.And(t => t.ParentId == menuId);
            return menuRepository.GetEntities(ex).Any();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="menuName"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public List<SysMenuDto> GetMenuGrid(TablePageParameter gp, string menucode, string menuName, int? parentId)
        {
            Expression<Func<SysMenu, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(menucode))
            {
                ex = ex.And(t => t.MenuCode.Contains(menucode));
            }
            if (!string.IsNullOrEmpty(menuName))
            {
                ex = ex.And(t => t.MenuName.Contains(menuName));
            }
            if (parentId.HasValue)
            {
                ex = ex.And(t => t.ParentId == parentId.Value);
            }
            var MenuList = menuRepository.GetEntities(ex);
            if (gp == null)
            {
                return Mapper.Map<List<SysMenu>, List<SysMenuDto>>(MenuList.ToList());
            }
            else
            {
                return Mapper.Map<List<SysMenu>, List<SysMenuDto>>(GetTablePagedList(MenuList, gp));
            }
        }


        /// <summary>
        /// 父级菜单名称
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public List<SysMenu> GetParentList()
        {
            Expression<Func<SysMenu, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete && t.MenuLevel == 0);
            var parentList = menuRepository.GetEntities(ex).ToList();
            return parentList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetParentList(string emptyKey = null, string emptyValue = null)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(emptyKey))
            {
                result.Add(new KeyValuePair<string, string>(emptyKey, emptyValue));
            }
            Expression<Func<SysMenu, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var sysMenuList = menuRepository.GetEntities(ex).ToList();
            if (sysMenuList != null && sysMenuList.Count > 0)
            {
                foreach (var item in sysMenuList)
                {
                    result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.MenuName));
                }
            }
            return result;
        }


        /// <summary>
        /// 获取 菜单功能 树
        /// </summary>
        /// <returns></returns>
        public List<MenuZTree> GetMenuZTree()
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

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        public bool Add(SysMenu sysMenu)
        {
            return menuRepository.Insert(sysMenu) > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        public bool Update(SysMenuDto sysMenuDto)
        {
            var sysMenu = Mapper.Map<SysMenuDto, SysMenu>(sysMenuDto);
            Expression<Func<SysMenu, object>>[] expressions = new Expression<Func<SysMenu, object>>[]
            {
                x => x.Icon, x => x.MenuCode, x => x.MenuLevel, x => x.MenuName, x => x.ParentId, x => x.SortNumber, x => x.Url, x => x.UpdateTime, x => x.UpdateUser
            };
            return menuRepository.Update(sysMenu, true, expressions) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var me = menuRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var menu in me)
            {
                menu.IsDelete = true;
                menu.UpdateTime = DateTime.Now;
                menu.UpdateUser = currentuser;
            }
            return menuRepository.Update(me) > 0;
        }
    }
}
