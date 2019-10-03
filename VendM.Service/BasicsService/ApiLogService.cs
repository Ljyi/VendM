using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;

namespace VendM.Service.BasicsService
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class ApiLogService:BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<APILog> apiLogRepository;
        private IUnitOfWork unitOfWork;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ApiLogService()
        {
            this.apiLogRepository = new RepositoryBase<APILog>();
            this.unitOfWork = new UnitOfWorkContextBase();
        }
        #region 增加/删除/修改

        #endregion
    }
}
