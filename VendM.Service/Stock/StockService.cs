using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModelDto;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class StockService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Model.DataModel.Stock> stockRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public StockService()
        {
            this.stockRepository = new RepositoryBase<Model.DataModel.Stock>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(StockDto entity)
        {
            var stockEntity = Mapper.Map<StockDto, Model.DataModel.Stock>(entity);
            return stockRepository.Insert(stockEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return stockRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return stockRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(StockDto entity)
        {
            var stockEntity = Mapper.Map<StockDto, Model.DataModel.Stock>(entity);
            return stockRepository.Update(stockEntity) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public StockDto Get(int id)
        {
            var stockEntity = stockRepository.Find(id);
            return Mapper.Map<Model.DataModel.Stock, StockDto>(stockEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<StockDto> GetAll()
        {
            var stockEntitys = stockRepository.Entities.ToList();
            return Mapper.Map<List<Model.DataModel.Stock>, List<StockDto>>(stockEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<StockDto> GetStockGrid(TablePageParameter tpg)
        {
            Expression<Func<Model.DataModel.Stock, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var stockList = stockRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<Model.DataModel.Stock>, List<StockDto>>(stockList.ToList());
            }
            else
            {
                return Mapper.Map<List<Model.DataModel.Stock>, List<StockDto>>(GetTablePagedList(stockList, tpg));
            }
        }
        #endregion
    }
}
