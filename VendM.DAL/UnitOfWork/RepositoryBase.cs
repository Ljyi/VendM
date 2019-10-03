using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core.DataException;
using VendM.Model.DataModel;
using Z.EntityFramework.Plus;

namespace VendM.DAL.UnitOfWork
{
    /// <summary>
    ///仓储操作基类
    /// </summary>
    /// <typeparam name="TEntity">动态实体类型</typeparam>
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        #region 属性
        /// <summary>
        ///获取 仓储上下文的实例
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }
        /// <summary>
        ///获取或设置的数据仓储上下文
        /// </summary>
        protected IUnitOfWorkContext EFContext
        {
            get
            {
                if (UnitOfWork == null)
                {
                    UnitOfWork = new UnitOfWorkContextBase();
                }
                if (UnitOfWork is IUnitOfWorkContext)
                {
                    return UnitOfWork as IUnitOfWorkContext;
                }
                throw new DataAccessException(string.Format("数据仓储上下文对象类型不正确，应为IRepositoryContext，实际为 {0}", UnitOfWork.GetType().Name));
            }
        }
        /// <summary>
        ///获取 当前实体的查询数据集
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get { return EFContext.Set<TEntity>(); }
        }
        /// <returns></returns>
        /// <summary>
        /// 获取 当前实体的查询数据集
        /// </summary>
        /// <param name="expression">lambda表达式</param>
        /// <param name="isTrack">是否跟踪</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> whereExpression, bool isNoTrack = true)
        {
            if (isNoTrack)
            {
                return EFContext.Set<TEntity>().AsExpandable().Where(whereExpression).AsNoTracking();

               
            }
            else
            {
                var Queryable = EFContext.Set<TEntity>().AsExpandable().Where(whereExpression);
                return Queryable;
            }
        }
        #endregion

        #region 公共方法

        /// <summary>
        ///插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(TEntity entity, bool isSave = true)
        {
            try
            {
                EFContext.RegisterNew(entity);
                return isSave ? EFContext.Commit() : 0;
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors).Select(m => m.ErrorMessage);
                var fullErrorMessage = string.Join(", ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " 验证异常消息是：", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        ///批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            try
            {
                EFContext.RegisterNew(entities);
                return isSave ? EFContext.Commit() : 0;
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors).Select(m => m.ErrorMessage);
                var fullErrorMessage = string.Join(", ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " 验证异常消息是：", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        ///删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(object id, bool isSave = true)
        {
            TEntity entity = EFContext.Set<TEntity>().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        /// <summary>
        ///删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int UpdateIsDelete(TEntity entity, bool isSave = true)
        {
            entity.IsDelete = true;
            EFContext.RegisterModified(entity);// RegisterDeleted(entity);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(TEntity entity, bool isSave = true)
        {
            EFContext.RegisterDeleted(entity);
            return isSave ? EFContext.Commit() : 0;
        }
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual int UpdateIsDelete(int[] id, bool isSave = true)
        {
            var entityList = EFContext.Set<TEntity>().Where(p => id.Contains(p.Id));
            foreach (var item in entityList)
            {
                item.IsDelete = true;
                EFContext.RegisterModified(item);
            }
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual int Delete(int[] id, bool isSave = true)
        {
            var entityList = EFContext.Set<TEntity>().Where(p => id.Contains(p.Id));
            foreach (var item in entityList)
            {
                EFContext.RegisterDeleted(item);
            }
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(IEnumerable<TEntity> entities, bool isSave = true)
        {
            EFContext.RegisterDeleted(entities);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true)
        {
            List<TEntity> entities = EFContext.Set<TEntity>().Where(predicate).ToList();
            return entities.Count > 0 ? Delete(entities, isSave) : 0;
        }

        /// <summary>
        ///更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Update(TEntity entity, bool isSave = true)
        {
            EFContext.RegisterModified(entity);
            return isSave ? EFContext.Commit() : 0;
        }
        public virtual int Update(TEntity entity, List<string> list, bool isSave = true)
        {
            EFContext.RegisterModified(entity, list);
            return isSave ? EFContext.Commit() : 0;
        }
        public virtual int Update(IEnumerable<TEntity> entityList, bool isSave = true)
        {
            EFContext.RegisterModified(entityList);
            return isSave ? EFContext.Commit() : 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity, bool isSave = true, params Expression<Func<TEntity, object>>[] properties)
        {
            try
            {
                EFContext.RegisterModified(entity, properties);
                return isSave ? EFContext.Commit() : 0;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception(msg);
            }
        }
        /// <summary>
        ///查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        public virtual TEntity Find(object key)
        {
            return EFContext.Set<TEntity>().Find(key);
        }
        /// <summary>
        /// 验证实体是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="isNoTrack"></param>
        /// <returns></returns>
        public virtual bool IsExist(Expression<Func<TEntity, bool>> whereExpression, bool isNoTrack = true)
        {
            try
            {
                return EFContext.Set<TEntity>().AsExpandable().Where(whereExpression).AsNoTracking().Any();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors).Select(m => m.ErrorMessage);
                var fullErrorMessage = string.Join(", ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " 验证异常消息是：", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        /// 异步查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(object key)
        {
            return await EFContext.Set<TEntity>().FindAsync(key);
        }
        #endregion
        /// <summary>
        /// 异步操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(TEntity entity)
        {
            try
            {
                int num = await EFContext.SaveChangesAsync<TEntity>(entity, EntityState.Added);
                return num;
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors).Select(m => m.ErrorMessage);
                var fullErrorMessage = string.Join(", ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " 验证异常消息是：", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
        /// <summary>
        /// 异步操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(TEntity entity)
        {
            try
            {
                return await EFContext.SaveChangesAsync(entity, EntityState.Deleted);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception(msg);
            }
        }
        /// <summary>
        /// 异步操作批量删除（物理删除）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            try
            {
                return await EFContext.Set<TEntity>().AsExpandable().Where(whereExpression).DeleteAsync();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception(msg);
            }
        }
        /// <summary>
        /// 批量更新(异步)
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public async Task<int> BulkUpdateAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> entity)
        {
            try
            {
                IQueryable<TEntity> query = EFContext.Set<TEntity>().AsExpandable().Where(whereExpression);
                return await query.UpdateAsync<TEntity>(entity);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception(msg);
            }
        }
        /// <summary>
        ///批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public int BulkInsert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public int BulkUpdate(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> entity)
        {
            try
            {
                IQueryable<TEntity> query = EFContext.Set<TEntity>().AsExpandable().Where(whereExpression);
                return query.Update(entity);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw new Exception(msg);
            }
        }
        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        public int BulkDalate(Expression<Func<TEntity, bool>> whereExpression)
        {
            return EFContext.Set<TEntity>().AsExpandable().Where(whereExpression).Delete();
        }
        /// <summary>
        /// sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetListBySQL(string sql, params object[] parameters)
        {
            return EFContext.GetListBySQL<TEntity>(sql, parameters);
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            EFContext.BeginTransaction();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void TransactionCommit()
        {
            EFContext.TransactionCommit();
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public void TransactionRollback()
        {
            EFContext.TransactionRollback();
        }
        /// <summary>
        /// 异常抛出
        /// </summary>
        /// <param name="ex"></param>
        private void ThrowException(DbEntityValidationException ex)
        {
            var errorMessages = ex.EntityValidationErrors.SelectMany(validationResult => validationResult.ValidationErrors).Select(m => m.ErrorMessage);
            var fullErrorMessage = string.Join(", ", errorMessages);
            var exceptionMessage = string.Concat(ex.Message, " 验证异常消息是：", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }
    }
}
