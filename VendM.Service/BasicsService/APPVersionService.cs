using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Service.EventHandler;

namespace VendM.Service.BasicsService
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class APPVersionService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<APP> appVersionRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <parm name="repository"">数据访问仓储</param>
        public APPVersionService()
        {
            this.appVersionRepository = new RepositoryBase<APP>();
        }
        /// <summary>
        /// app版本列表
        /// </summary>
        /// <param name="tpg"></param>
        /// <param name="version"></param>
        /// <returns></returns>
		public List<APP> GetAPPVersionGrid(TablePageParameter tpg = null, string version = "")
        {
            Expression<Func<APP, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(version))
            {
                ex = ex.And(t => t.Version.Contains(version));
            }
            var appList = appVersionRepository.GetEntities(ex);
            if (tpg == null)
            {
                return appList.ToList();
            }
            else
            {
                return GetTablePagedList(appList, tpg);
            }
        }

        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(APP entity, ref string errorMsg)
        {
            try
            {
                return appVersionRepository.Insert(entity) > 0;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Update(APP entity, ref string errorMsg)
        {
            try
            {
                return appVersionRepository.Update(entity, true, x => x.Url, x => x.Version, it => it.UpdateTime, it => it.UpdateUser) > 0;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool DeleteAsync(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var aPPs = appVersionRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var prod in aPPs)
            {
                prod.IsDelete = true;
                prod.UpdateTime = DateTime.Now;
                prod.UpdateUser = currentuser;
            }
            return appVersionRepository.Update(aPPs) > 0;
        }

        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public APP Get(int id)
        {
            var advertisementEntity = appVersionRepository.Find(id);
            return advertisementEntity;
        }
        /// <summary>
        /// 清除url
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filename"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool DeleteFiles(int id, string filename, ref string errorMsg)
        {
            try
            {
                var aPPs = appVersionRepository.Find(id);
                aPPs.Url = "";
                return appVersionRepository.Update(aPPs) > 0;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 获取ID最大APP版本
        /// </summary>
        /// <returns>APP实体</returns>
        public AppDto GetAppVersionFirst()
        {
            var appVersion = appVersionRepository.Entities.OrderByDescending(P => P.Id).First();
            return Mapper.Map<APP, AppDto>(appVersion);
        }
    }
}
#endregion