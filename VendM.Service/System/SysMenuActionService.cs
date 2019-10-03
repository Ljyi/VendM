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
    /// 菜单功能
    /// </summary>
    public class SysMenuActionService : BaseService
    {
        private IRepository<SysMenuAction> sysMenuActionRepository = null;
        public ButtonService buttonService = null;
        public MenuService menuService = null;
        public SysMenuActionService()
        {
            sysMenuActionRepository = new RepositoryBase<SysMenuAction>();
            buttonService = new ButtonService();
            menuService = new MenuService();
        }

        /// 获取菜单功能列表
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="sysMenuId"></param>
        /// <param name="buttonName"></param>
        /// <param name="buttonCode"></param>
        /// <returns></returns>
        public List<SysMenuActionGridDto> GeSysMenuActionGrid(TablePageParameter gp, int? menuId, string buttonName, string buttonCode)
        {
            Expression<Func<SysMenuAction, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (menuId.HasValue)
            {
                ex = ex.And(t => t.SysMenuId == menuId);
                if (!string.IsNullOrEmpty(buttonName))
                {
                    ex = ex.And(t => t.SysButton.ButtonName.Contains(buttonName));
                }
                if (!string.IsNullOrEmpty(buttonCode))
                {
                    ex = ex.And(t => t.SysButton.ButtonCode.Contains(buttonCode));
                }
                var sysMenuActionList = sysMenuActionRepository.GetEntities(ex);
                if (gp == null)
                {
                    return Mapper.Map<List<SysMenuAction>, List<SysMenuActionGridDto>>(sysMenuActionList.ToList());
                }
                else
                {
                    return Mapper.Map<List<SysMenuAction>, List<SysMenuActionGridDto>>(GetTablePagedList(sysMenuActionList, gp));
                }
            }
            else
            {
                return new List<SysMenuActionGridDto>();
            }

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysMenuActionDto GetSysMenuActionById(int id)
        {
            var sysMenuAction = sysMenuActionRepository.Find(id);
            return Mapper.Map<SysMenuAction, SysMenuActionDto>(sysMenuAction);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sysMenuActionDto"></param>
        /// <returns></returns>
        public bool SysMenuActionAdd(SysMenuActionDto sysMenuActionDto)
        {
            SysMenuAction sysMenuAction = Mapper.Map<SysMenuActionDto, SysMenuAction>(sysMenuActionDto);
            return sysMenuActionRepository.Insert(sysMenuAction) > 0;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysMenuActionDto"></param>
        /// <returns></returns>
        public bool SysMenuActionUpdate(SysMenuActionDto sysMenuActionDto)
        {
            SysMenuAction sysMenuAction = Mapper.Map<SysMenuActionDto, SysMenuAction>(sysMenuActionDto);
            Expression<Func<SysMenuAction, object>>[] properties = new Expression<Func<SysMenuAction, object>>[]
            {
                p=>p.ActionName,
                p=>p.AuthorizeCode,
                p=>p.ControlName,
                p =>p.SysButtonId,
                p=>p.SortNumber,
                p=>p.Status,
            };
            return sysMenuActionRepository.Update(sysMenuAction, true, properties) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool SysMenuActionDeletes(string ids)
        {
            int[] idInts = ids.StrToIntArray();
            return sysMenuActionRepository.Delete(idInts) > 0;
        }
        /// <summary>
        /// 验证功能是否存在
        /// </summary>
        /// <param name="controlName">控制器名称</param>
        /// <param name="actionName">方法</param>
        /// <returns></returns>
        public bool ValidateActionName(string controlName, string actionName, int? id)
        {
            Expression<Func<SysMenuAction, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (id.HasValue)
            {
                ex = ex.And(t => t.Id != id);
            }
            if (!string.IsNullOrEmpty(controlName))
            {
                ex = ex.And(t => t.ControlName == controlName);
            }
            if (!string.IsNullOrEmpty(actionName))
            {
                ex = ex.And(t => t.ActionName == actionName);
            }
            return sysMenuActionRepository.IsExist(ex);
        }
        /// <summary>
        /// 验证权限编码是否存在
        /// </summary>
        /// <param name="controlName">控制器名称</param>
        /// <param name="authorizeCode">权限编码</param>
        /// <returns></returns>
        public bool ValidateAuthorizeCode(string controlName, string authorizeCode, int? id)
        {
            Expression<Func<SysMenuAction, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (id.HasValue)
            {
                ex = ex.And(t => t.Id != id);
            }
            if (!string.IsNullOrEmpty(controlName))
            {
                ex = ex.And(t => t.ControlName == controlName);
            }
            if (!string.IsNullOrEmpty(authorizeCode))
            {
                ex = ex.And(t => t.AuthorizeCode == authorizeCode);
            }
            return sysMenuActionRepository.IsExist(ex);
        }
    }
}
