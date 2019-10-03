using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;

namespace VendM.Service
{
    /// <summary>
    /// 交易业务逻辑
    /// </summary>
    public partial class TransactionService : BaseService
    {
        private IRepository<Transaction> transactionRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public TransactionService()
        {
            this.transactionRepository = new RepositoryBase<Transaction>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(TransactionDto entity)
        {
            var orderEntity = Mapper.Map<TransactionDto, Transaction>(entity);
            return transactionRepository.Insert(orderEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return transactionRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return transactionRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(TransactionDto entity)
        {
            var orderEntity = Mapper.Map<TransactionDto, Transaction>(entity);
            return transactionRepository.Update(orderEntity) > 0;
        }
        #endregion
    }
}
