using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using VendM.Model;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Log;
using VendM.Model.EnumModel;
using VendM.Service.EventHandler;

namespace VendM.Service
{
    public partial class OrderService
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="orderApi"></param>
        /// <returns></returns>
        public async Task<Result> AddAsync(OrderAPIDto orderApi)
        {
            Result result = new Result() { Success = true };
            Machine machine = new Machine();
            try
            {
                //开启事务
                orderRepository.BeginTransaction();
                machine = machineService.GetMachineIdByMachineNo(orderApi.MachineNo);
                if (machine != null)
                {
                    string orderNo = "";
                    orderNo = generateCodeService.GetNextOrderCode(ref orderNo);
                    Order order = new Order();
                    order.OrderNo = orderNo;
                    order.Amount = orderApi.Amount;
                    order.Points = orderApi.Points;
                    order.Quantity = orderApi.Quantity;
                    order.StoreNo = machine.Store.Code;
                    order.MachineNo = orderApi.MachineNo;
                    order.StoreName = machine.Store.Name;
                    order.OrderStatus = orderApi.ClientOrderStatus;
                    order.IsDelete = false;
                    order.CreateUser = "系统";
                    order.CredateTime = DateTime.Now;
                    order.UpdateUser= "系统";
                    order.UpdateTime= DateTime.Now;
                    orderRepository.Insert(order);
                    Product product = productRepository.Find(orderApi.ProductId);
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        OrderId = order.Id,
                        OrderNo = order.OrderNo,
                        Amount = order.Amount,
                        Points = order.Points,
                        Quantity = order.Quantity,
                        ProductCode = product.ProductCode,
                        ProductId = orderApi.ProductId,
                        ProductName = product.ProductName_EN,
                        IsDelete = false,
                        CreateUser = "系统",
                        CredateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        UpdateUser = "系统"
                    };
                    orderDetailsRepository.Insert(orderDetails);
                    if (order.OrderStatus == 0)
                    {
                        //添加明细
                        // Result resultOrderDetails = await AddOrderDetails(orderDetails);
                        //扣减库存
                        Result resultStock = await DeductionStock(order.StoreNo, order.MachineNo, orderApi.Quantity, orderApi.ProductId, orderApi.PassageNumber);

                        // 添加交易信息
                        Result resultTransaction = await transactionService.AddTransaction(order.OrderNo, orderApi.TransactionInfo);

                        //扣减库存日志
                        Result resultLog = await inventoryChangeLogService.AddLog(order.StoreNo, order.MachineNo, orderApi.Quantity, orderApi.ProductId, orderApi.PassageNumber, ChangeLogType.Reduce);
                        if (!resultLog.Success || !resultStock.Success || !resultTransaction.Success)
                        {
                            //回滚
                            orderRepository.TransactionRollback();
                            result.Success = false;
                            result.ErrorMsg = "异常信息：" + resultStock.ErrorMsg + "，" + resultLog.ErrorMsg + "，" + resultTransaction.ErrorMsg;
                            APILogDto log = new APILogDto()
                            {
                                APIName = "Order/CreateOrderAsync",
                                CreateUser = "system",
                                CredateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                IsDelete = false,
                                LogContent = result.ErrorMsg,
                                MachineNo = machine.MachineNo,
                                RequestData = JsonConvert.SerializeObject(orderApi),
                                Source = (int)RequstSourceEnum.Server
                            };
                            logEvent.AddLogEvent += new DelegateAddLog(logApiService.Add);
                            logEvent.AddLog(log);
                        }
                        else
                        {
                            //提交
                            orderRepository.TransactionCommit();
                        }
                    }
                    else
                    {
                        //提交
                        orderRepository.TransactionCommit();
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorMsg = "暂无相关数据";
                    orderRepository.TransactionRollback();
                }
            }
            catch (Exception ex)
            {
                orderRepository.TransactionRollback();
                result.Success = false;
                result.ErrorMsg = ex.Message;
                APILogDto log = new APILogDto()
                {
                    APIName = "Order/CreateOrderAsync",
                    CreateUser = "system",
                    CredateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    IsDelete = false,
                    LogContent = ex.Message,
                    MachineNo = machine.MachineNo,
                    RequestData = JsonConvert.SerializeObject(orderApi),
                    Source = (int)RequstSourceEnum.Server
                };
                logEvent.AddLogEvent += new DelegateAddLog(logApiService.Add);
                logEvent.AddLog(log);
            }
            return result;
        }

        /// <summary>
        /// 添加订单明细
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        public async Task<Result> AddOrderDetails(OrderDetails orderDetails)
        {
            return await Task.Run(() =>
            {
                Result result = new Result() { Success = true };
                try
                {
                    result.Success = orderDetailsRepository.Insert(orderDetails) > 0;
                }
                catch (Exception ex)
                {
                    result.ErrorMsg = ex.Message;
                    result.Success = false;
                }
                return result;
            });
        }
        /// <summary>
        /// 扣减库存
        /// </summary>
        /// <param name="storeNo"></param>
        /// <param name="machineNo"></param>
        /// <param name="quantity"></param>
        /// <param name="productId"></param>
        /// <param name="passageNumber"></param>
        /// <returns></returns>
        public async Task<Result> DeductionStock(string storeNo, string machineNo, int quantity, int productId, int passageNumber)
        {
            return await Task.Run(() =>
            {
                Result result = new Result() { Success = true };
                try
                {
                    var machine = machineStockDetailService.GetMachineStockDetailByNo(machineNo, productId, passageNumber);
                    machine.InventoryQuantity -= quantity;
                    machine.MachineStock.RealStockQuantity -= quantity;
                    machine.UpdateTime = DateTime.Now;
                    machine.UpdateUser = "system";
                    result.Success = machineStockDetailService.Update(machine);
                    stockEvent.CheckStockEvent += new StockEvent.DelegateStock(machineStockService.CreateReplenishAsync);
                    stockEvent.CheckStock(machine.MachineStock);
                }
                catch (Exception ex)
                {
                    result.ErrorMsg = ex.Message;
                    result.Success = false;
                }
                return result;
            });
        }
    }
}
