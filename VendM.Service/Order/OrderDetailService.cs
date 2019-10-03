using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModelDto.Order;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class OrderDetailService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<OrderDetails> orderDetailRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        OrderDetailService()
        {
            this.orderDetailRepository = new RepositoryBase<OrderDetails>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(OrderDetailDto entity)
        {
            var orderDetailEntity = Mapper.Map<OrderDetailDto, OrderDetails>(entity);
            return orderDetailRepository.Insert(orderDetailEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return orderDetailRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return orderDetailRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(OrderDetailDto entity)
        {
            var orderDetailEntity = Mapper.Map<OrderDetailDto, OrderDetails>(entity);
            return orderDetailRepository.Update(orderDetailEntity) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public OrderDetailDto Get(int id)
        {
            var orderDetailEntity = orderDetailRepository.Find(id);
            return Mapper.Map<OrderDetails, OrderDetailDto>(orderDetailEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<OrderDetailDto> GetAll()
        {
            var orderDetailEntitys = orderDetailRepository.Entities.ToList();
            return Mapper.Map<List<OrderDetails>, List<OrderDetailDto>>(orderDetailEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<OrderDetailDto> GetOrderDetailGrid(TablePageParameter tpg)
        {
            Expression<Func<OrderDetails, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var orderDetailList = orderDetailRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<OrderDetails>, List<OrderDetailDto>>(orderDetailList.ToList());
            }
            else
            {
                return Mapper.Map<List<OrderDetails>, List<OrderDetailDto>>(GetTablePagedList(orderDetailList, tpg));
            }
        }
        #endregion
    }
}
