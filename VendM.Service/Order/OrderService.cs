using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto;
using VendM.Model.DataModelDto.Order;
using VendM.Model.ExportModel;
using VendM.Service.BasicsService;
using VendM.Service.EventHandler;
using VendM.Service.Log;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class OrderService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Order> orderRepository;
        private IRepository<OrderDetails> orderDetailsRepository;
        private IRepository<Machine> machineRepository;
        private IRepository<Store> storeRepository;
        private IRepository<StoreOrderView> storeOrderViewRepository;
        private IRepository<Product> productRepository;
        private IRepository<OrderMainView> orderMainViewRepository;
        private GenerateCodeService generateCodeService = null;
        private StoreService storeService = null;
        private MachineService machineService = null;
        MachineStockDetailService machineStockDetailService = null;
        MachineStockService machineStockService = null;
        InventoryChangeLogService inventoryChangeLogService = null;
        LogEvent logEvent = null;
        StockEvent stockEvent = null;
        LogApiService logApiService = null;
        TransactionService transactionService = null;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public OrderService()
        {
            this.orderRepository = new RepositoryBase<Order>();
            this.orderDetailsRepository = new RepositoryBase<OrderDetails>();
            this.machineRepository = new RepositoryBase<Machine>();
            this.storeRepository = new RepositoryBase<Store>();
            this.storeOrderViewRepository = new RepositoryBase<StoreOrderView>();
            this.orderMainViewRepository = new RepositoryBase<OrderMainView>();
            productRepository = new RepositoryBase<Product>();
            generateCodeService = GenerateCodeService.SingleGenerateCodeService();
            storeService = new StoreService();
            machineService = new MachineService();
            inventoryChangeLogService = new InventoryChangeLogService();
            machineStockDetailService = new MachineStockDetailService();
            logEvent = new LogEvent();
            stockEvent = new StockEvent();
            logApiService = new LogApiService();
            machineStockService = new MachineStockService();
            transactionService = new TransactionService();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(OrderDto entity)
        {
            var orderEntity = Mapper.Map<OrderDto, Order>(entity);
            return orderRepository.Insert(orderEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return orderRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return orderRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(OrderDto entity)
        {
            var orderEntity = Mapper.Map<OrderDto, Order>(entity);
            return orderRepository.Update(orderEntity) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public OrderDto Get(int id)
        {
            var orderEntity = orderRepository.Find(id);
            return Mapper.Map<Order, OrderDto>(orderEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<OrderDto> GetAll()
        {
            var orderEntitys = orderRepository.Entities.ToList();
            return Mapper.Map<List<Order>, List<OrderDto>>(orderEntitys);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tpg">分页参数</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="storeNo">Market号</param>
        /// <param name="machineNo">机器号</param>
        /// <param name="dateFrom">创建时间</param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<OrderDto> GetOrderGrid(TablePageParameter tpg, string orderNo, string storeNo, string machineNo, DateTime? dateFrom, DateTime? dateTo)
        {
            Expression<Func<Order, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(orderNo))
            {
                ex = ex.And(t => t.OrderNo == orderNo);
            }
            if (!string.IsNullOrEmpty(storeNo))
            {
                ex = ex.And(t => t.StoreNo == storeNo);
            }
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.MachineNo == machineNo);
            }
            if (dateFrom.HasValue)
            {
                dateFrom = Convert.ToDateTime((Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd HH:mm")));
                ex = ex.And(t => t.CredateTime >= dateFrom);

            }
            if (dateTo.HasValue)
            {

                dateTo = Convert.ToDateTime((Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd HH:mm"))).AddMinutes(1).AddSeconds(-1);
                ex = ex.And(t => t.CredateTime <= dateTo);
            }
            var orderList = orderRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<Order>, List<OrderDto>>(orderList.ToList());
            }
            else
            {
                return Mapper.Map<List<Order>, List<OrderDto>>(GetTablePagedList(orderList, tpg));
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="orderNo"></param>
        /// <param name="storeNo"></param>
        /// <param name="machineNo"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<OrderExport> OrderExportGrid(string ids, string orderNo, string storeNo, string machineNo, DateTime? dateFrom, DateTime? dateTo)
        {
            Expression<Func<Order, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(ids))
            {
                int[] idArray = ids.ToIntArray();
                ex = ex.And(t => idArray.Contains(t.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(orderNo))
                {
                    ex = ex.And(t => t.OrderNo == orderNo);
                }
                if (!string.IsNullOrEmpty(storeNo))
                {
                    ex = ex.And(t => t.StoreNo == storeNo);
                }
                if (!string.IsNullOrEmpty(machineNo))
                {
                    ex = ex.And(t => t.MachineNo == machineNo);
                }
                if (dateFrom.HasValue)
                {
                    DateTime dateTimeStart = Common.GetCurrentMinDate(dateFrom.Value);
                    DateTime dateTimeEnd = Common.GetCurrentMaxDate(dateFrom.Value);
                    ex = ex.And(t => t.CredateTime >= dateTimeStart && t.CredateTime <= dateTimeEnd);
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    int[] idArray = ids.ToIntArray();
                    ex = ex.And(t => idArray.Contains(t.Id));
                }
            }
            var orderList = orderRepository.GetEntities(ex);
            return Mapper.Map<List<Order>, List<OrderExport>>(orderList.ToList());
        }
        /// <summary>
        /// 订单明细查询列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<OrderDetailDto> GetOrderDetailsGrid(TablePageParameter tpg, int? orderId)
        {
            Expression<Func<OrderDetails, bool>> where = t => true;
            where = where.And(t => !t.IsDelete);
            if (orderId.HasValue)
            {
                where = where.And(t => t.OrderId == orderId.Value);
            }
            var orderDetailsList = orderDetailsRepository.GetEntities(where);
            if (tpg == null)
            {
                return Mapper.Map<List<OrderDetails>, List<OrderDetailDto>>(orderDetailsList.ToList());
            }
            else
            {
                return Mapper.Map<List<OrderDetails>, List<OrderDetailDto>>(GetTablePagedList(orderDetailsList, tpg));
            }
        }
        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderView GetOrderDetail(int orderId)
        {
            var orderMain = orderRepository.Find(orderId);
            return Mapper.Map<Order, OrderView>(orderMain);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tpg">分页参数</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="storeNo">Market号</param>
        /// <param name="machineNo">机器号</param>
        /// <param name="dateFrom">创建时间</param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<StoreOrderView> GetStoreOrderViewGrid(TablePageParameter pg, string productName, string storeNo, string machineNo, DateTime? dateFrom, DateTime? dateTo)
        {
            string sql = @"select * from StoreOrder_View  where 1={0}";

            if (!string.IsNullOrEmpty(productName))
            {
                sql += " and ProductName='" + productName + "'";
            }
            if (!string.IsNullOrEmpty(storeNo))
            {
                sql += " and StoreNo='" + storeNo + "'";
            }
            if (!string.IsNullOrEmpty(machineNo))
            {
                sql += " and MachineNo='" + machineNo + "'";
            }
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                dateFrom = Convert.ToDateTime((Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd HH:mm")));
                dateTo = Convert.ToDateTime((Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd HH:mm"))).AddMinutes(1).AddSeconds(-1);

                //DateTime dateTimeStart = Common.GetCurrentMinDate(dateFrom.Value);
                //DateTime dateTimeEnd = Common.GetCurrentMaxDate(dateTo.Value);
                sql += " and OrderTime>='" + dateFrom + "' and OrderTime<='" + dateTo + "'";
            }
            var transactionReportViewList = storeOrderViewRepository.GetListBySQL(sql, 1);
            if (pg == null)
                return transactionReportViewList.ToList();

            return GetTablePagedList(transactionReportViewList, pg).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pg"></param>
        /// <param name="storeNo"></param>
        /// <param name="machineNo"></param>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        public List<StoreOrderDetailDto> GetStoreOrderDetailGrid(TablePageParameter pg, string productCode, string machineNo, DateTime? dateFrom)
        {
            try
            {
                Expression<Func<OrderDetails, bool>> where = t => true;
                where = where.And(t => !t.IsDelete);
                if (!string.IsNullOrEmpty(productCode))
                {
                    where = where.And(t => t.ProductCode == productCode);
                }
                if (!string.IsNullOrEmpty(machineNo))
                {
                    where = where.And(t => t.Order.MachineNo == machineNo);
                }
                if (dateFrom.HasValue)
                {
                    where = where.And(t => t.CredateTime >= Common.GetCurrentMinDate(dateFrom.Value) && t.CredateTime <= Common.GetCurrentMaxDate(dateFrom.Value));
                }
                var orderDetailsList = orderDetailsRepository.GetEntities(where);
                if (pg == null)
                {
                    return Mapper.Map<List<OrderDetails>, List<StoreOrderDetailDto>>(orderDetailsList.ToList());
                }
                else
                {
                    return Mapper.Map<List<OrderDetails>, List<StoreOrderDetailDto>>(GetTablePagedList(orderDetailsList, pg));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获器Market列表
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetStorelist()
        {
            List<KeyValue> keyValues = new List<KeyValue>();
            Expression<Func<Store, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var machineList = storeRepository.GetEntities(ex);
            var machineDtoList = Mapper.Map<List<Store>, List<StoreDto>>(machineList.ToList());
            keyValues = machineDtoList.Select(p => new KeyValue
            {
                Key = p.Code,
                Value = p.Name
            }).ToList();
            return keyValues;
        }
        /// <summary>
        /// 获取取机列表
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetMachinelist(string StoreCode)
        {
            List<KeyValue> keyValues = new List<KeyValue>();
            Expression<Func<Machine, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            ex = ex.And(t => t.Store.Code == StoreCode);
            var machineList = machineRepository.GetEntities(ex);
            var machineDtoList = Mapper.Map<List<Machine>, List<MachineDto>>(machineList.ToList());
            keyValues = machineDtoList.Select(p => new KeyValue
            {
                Key = p.MachineNo,
                Value = p.Name
            }).ToList();
            return keyValues;
        }

        /// <summary>
        /// 查询主页报表
        /// </summary>
        /// <param name="storeno"></param>
        /// <param name="machineno"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public List<OrderMainView> GetOrderMainViews(string storeno, string machineno, string startdate, string enddate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"SELECT *
                                INTO #tab
                                FROM
                                (
                                    SELECT O.*,
                                OD.ProductName,
                                OD.ProductCode
                                    FROM dbo.[Order] O
                                    LEFT JOIN dbo.OrderDetails OD
                                ON O.Id = OD.OrderId
                                AND O.IsDelete = OD.IsDelete
                                WHERE O.IsDelete = 0
                                  AND O.StoreNo = ISNULL({(string.IsNullOrEmpty(storeno) ? "null" : '\'' + storeno + '\'')},O.StoreNo)
                                  AND O.MachineNo = ISNULL({(string.IsNullOrEmpty(machineno) ? "null" : '\'' + machineno + '\'')},MachineNo)
                                  AND O.CredateTime
                                  BETWEEN {'\'' + startdate + '\'' } 
                                  AND {'\'' + enddate + '\'' }               
                                ) tab;

                               
                                SELECT YEAR(CredateTime) AS [Year],
                                       MONTH(CredateTime) AS [Month],
                                       DAY(CredateTime) AS [Day],
                                       MAX(CredateTime) AS [CredateTime],
                                       SUM(Amount) AS [Amount],
                                       SUM(Points) AS Points,
                                       COUNT(*) AS OrderCount
                                FROM #tab
                                GROUP BY YEAR(CredateTime),
                                         MONTH(CredateTime),
                                         DAY(CredateTime);


                                DROP TABLE #tab"
            );

            var reportList = orderMainViewRepository.GetListBySQL(sb.ToString(), 1);

            return reportList.ToList();

        }

        #endregion
    }
}
