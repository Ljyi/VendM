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
    public class ButtonService : BaseService
    {
        private IRepository<SysButton> buttonRepository = null;
        public bool isSave = true;
        public ButtonService()
        {
            buttonRepository = new RepositoryBase<SysButton>();
        }
        public List<ButtonDto> GetAllButtons()
        {
            var button = buttonRepository.Entities.ToList();
            return Mapper.Map<List<SysButton>, List<ButtonDto>>(button);
        }
        public SysButton GetButton(int id)
        {
            return buttonRepository.Find(id);
        }
        public ButtonDto Find(int id)
        {
            SysButton button = buttonRepository.Find(id);
            return Mapper.Map<SysButton, ButtonDto>(button);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="buttonName"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public List<ButtonDto> GetButtonGrid(TablePageParameter gp = null,  string buttonName = "")
        {
            Expression<Func<SysButton, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(buttonName))
            {
                ex = ex.And(t => t.ButtonName.Contains(buttonName));
            }
            var ButtonList = buttonRepository.GetEntities(ex);
            if (gp == null)
            {
                return Mapper.Map<List<SysButton>, List<ButtonDto>>(ButtonList.ToList());
            }
            else
            {
                return Mapper.Map<List<SysButton>, List<ButtonDto>>(GetTablePagedList(ButtonList, gp));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emptyKey"></param>
        /// <param name="emptyValue"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetButtonList(string emptyKey = null, string emptyValue = null)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(emptyKey))
            {
                result.Add(new KeyValuePair<string, string>(emptyKey, emptyValue));
            }
            Expression<Func<SysButton, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var sysMenuList = buttonRepository.GetEntities(ex).ToList();
            if (sysMenuList != null && sysMenuList.Count > 0)
            {
                foreach (var item in sysMenuList)
                {
                    result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.ButtonName));
                }
            }
            return result;
        }

        ///// <summary>
        ///// 父级菜单名称
        ///// </summary>
        ///// <param name="emptyKey"></param>
        ///// <param name="emptyValue"></param>
        ///// <param name="isAll"></param>
        ///// <returns></returns>
        //public List<ButtonDto> GetParentList()
        //{
        //    Expression<Func<SysButton, bool>> ex = t => true;
        //    ex = ex.And(t => !t.IsDelete && t.ButtonLevel==0);
        //    var parentList = buttonRepository.GetEntities(ex);
        //    List<ButtonDto> lists = Mapper.Map<List<SysButton>, List<ButtonDto>>(parentList.ToList());
        //    return lists;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="emptyKey"></param>
        ///// <param name="emptyValue"></param>
        ///// <returns></returns>
        //public List<KeyValuePair<string, string>> GetParentList(string emptyKey = null, string emptyValue = null)
        //{
        //    List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
        //    if (!string.IsNullOrEmpty(emptyKey))
        //    {
        //        result.Add(new KeyValuePair<string, string>(emptyKey, emptyValue));
        //    }
        //    Expression<Func<SysButton, bool>> ex = t => true;
        //    ex = ex.And(t => !t.IsDelete);
        //    var parentList = buttonRepository.GetEntities(ex);
        //    List<ButtonDto> lists = Mapper.Map<List<SysButton>, List<ButtonDto>>(parentList.ToList());
        //    if (lists != null && lists.Count > 0)
        //    {
        //        foreach (var item in lists)
        //        {
        //            result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.ButtonName));
        //        }
        //    }
        //    return result;
        //}


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="buttonDto"></param>
        /// <returns></returns>
        public bool Add(ButtonDto buttonDto)
        {
            var button = Mapper.Map<ButtonDto, SysButton>(buttonDto);
            return buttonRepository.Insert(button) > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="buttonDto"></param>
        /// <returns></returns>
        public bool Update(ButtonDto buttonDto)
        {
            var button = Mapper.Map<ButtonDto, SysButton>(buttonDto);
            List<string> list = new List<string>() { "ButtonName", "Status",  "UpdateUser", "UpdateTime" };
            return buttonRepository.Update(button, list) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids,string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var bu = buttonRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var button in bu)
            {
                button.IsDelete = true;
                button.UpdateTime = DateTime.Now;
                button.UpdateUser = currentuser;
            }
            return buttonRepository.Update(bu) > 0;
        }
    }
}
