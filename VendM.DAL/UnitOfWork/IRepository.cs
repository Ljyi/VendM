using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Model.DataModel;

namespace VendM.DAL.UnitOfWork
{
    /// <summary>
    ///  定义仓储模型中的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">动态实体类型</typeparam>
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        #region 属性
        /// <summary>
        /// 获取 当前实体的查询数据集
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// 获取 当前实体的查询数据集
        /// </summary>
        IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> expression, bool isNoTrack = true);
        /// <summary>
        /// 事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 事务提交
        /// </summary>
        void TransactionCommit();
        /// <summary>
        /// 事务回滚
        /// </summary>
        void TransactionRollback();
        #endregion

        #region 公共方法
        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Insert(TEntity entity, bool isSave = true);
        /// <summary>
        ///批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Insert(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        ///删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(object id, bool isSave = true);

        /// <summary>
        ///删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(int[] id, bool isSave = true);

        /// <summary>
        ///删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(TEntity entity, bool isSave = true);

        /// <summary>
        ///删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        ///删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true);

        /// <summary>
        ///更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Update(TEntity entity, bool isSave = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="list"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        int Update(TEntity entity, List<string> list, bool isSave = true);
        /// <summary>
        /// 根据字段更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        /// <param name="properties"></param>
        int Update(TEntity entity, bool isSave = true, params Expression<Func<TEntity, object>>[] properties);
        /// <summary>
        ///批量更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Update(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        int BulkInsert(IEnumerable<TEntity> entities, bool isSave = true);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        int BulkUpdate(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> entity);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        int BulkDalate(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        TEntity Find(object key);
        /// <summary>
        /// 验证实体是否存在
        /// </summary>
        /// <param name="whereExpression">条件</param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        bool IsExist(Expression<Func<TEntity, bool>> whereExpression, bool isNoTrack = true);
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetListBySQL(string sql, params object[] parameters);
        /// <summary>
        /// 异步查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(object key);
        /// <summary>
        /// 添加实体并提交到数据服务器
        /// </summary>
        /// <param name="item"></param>
        Task<int> InsertAsync(TEntity item);
        /// <summary>
        /// 批量更新（异步）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        Task<int> BulkUpdateAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> entity);
        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item">Item to delete</param>
        Task<int> DeleteAsync(TEntity item);
        /// <summary>
        /// 批量删除（异步）
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> whereExpression);
    }
}
