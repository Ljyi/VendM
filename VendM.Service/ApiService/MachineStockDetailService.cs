using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Model;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.EnumModel;

namespace VendM.Service
{
    /// <summary>
    /// API
    /// </summary>
    public partial class MachineStockDetailService
    {
        /// <summary>
        /// 根据No获取机器库存
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public List<MachineStockDetails> GetMachineByNo(string machineNo)
        {
            //获取根据机器编号获取机器信息
            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(machineNo))
            {
                ex = ex.And(t => t.MachineStock.Machine.MachineNo == machineNo);
            }
            var machineStockDetails = machineStockDetailRepository.GetEntities(ex);
            return machineStockDetails.ToList();
        }
        /// <summary>
        /// 补货
        /// </summary>
        /// <param name="machineStockDetails"></param>
        /// <returns></returns>
        public async Task<Result> UpdataMachineStock(MachineStockAPIDto machineStockApi)
        {
            Result result = new Result() { Success = true, ErrorMsg = "" };
            machineStockDetailRepository.BeginTransaction();

            return await Task.Run(() =>
            {
                try
                {
                    List<MachineStockDetails> machineStockDetails = GetMachineByNo(machineStockApi.MachineNo);
                    if (machineStockDetails != null && machineStockDetails.Count > 0)
                    {
                        List<MachineStockDetails> machineStockDetailsNew = new List<MachineStockDetails>();
                        List<InventoryChangeLog> inventoryChangeLogs = new List<InventoryChangeLog>();
                        var machineStock = machineStockDetails.FirstOrDefault().MachineStock;
                        string storeNo = machineStock.Machine.MachineNo;
                        string numStr = "";
                        string emptyStr = "";
                        Dictionary<int, int> dicNumCount = new Dictionary<int, int>();
                        Dictionary<int, int> dicEmptyCount = new Dictionary<int, int>();
                        machineStockApi.MachineStockDetails.ForEach(p =>
                        {
                            var machineStockDetail = machineStockDetails.Where(zw => zw.PassageNumber == p.PassageNumber && zw.ProductId == p.ProductId).FirstOrDefault();
                            if (machineStockDetail != null)
                            {
                                if (machineStockDetail.TotalQuantity >= p.Quantity && p.Quantity >= 0)
                                {
                                    machineStockDetail.InventoryQuantity = p.Quantity;
                                    machineStockDetail.UpdateUser = RequstSourceEnum.Client.ToString();
                                    machineStockDetail.UpdateTime = DateTime.Now;
                                    machineStockDetail.UpdateUser = machineStock.ReplenishmentUser;
                                    machineStockDetailsNew.Add(machineStockDetail);
                                    inventoryChangeLogs.Add(new InventoryChangeLog()
                                    {
                                        ChangeType = (int)ChangeLogType.Add,
                                        IsDelete = false,
                                        ProductId = p.ProductId,
                                        Quantity = p.Quantity,
                                        Content = string.Format("Market编码：{0}，机器编码：{1}，货道号：{2}，扣减库存：{3}", storeNo, machineStockApi.MachineNo, p.PassageNumber, p.Quantity),
                                        CreateUser = machineStock.ReplenishmentUser,
                                        CredateTime = DateTime.Now
                                    });
                                }
                                else
                                {
                                    if (dicNumCount.Count() > 0)
                                    {
                                        if (!dicNumCount.ContainsKey(p.PassageNumber))
                                        {
                                            numStr = numStr.Replace("!", "," + p.PassageNumber + "!");
                                        }
                                    }
                                    else
                                    {
                                        numStr = $"货道号：{p.PassageNumber}!数量要大于等于0且要小于货道容量；";
                                    }
                                    if (!dicNumCount.ContainsKey(p.PassageNumber))
                                    {
                                        dicNumCount.Add(p.PassageNumber, p.PassageNumber);
                                    }
                                }
                            }
                            else
                            {
                                if (dicEmptyCount.Count() > 0)
                                {
                                    if (!dicEmptyCount.ContainsKey(p.PassageNumber))
                                    {
                                        emptyStr = emptyStr.Replace("!", "," + p.PassageNumber + "!");
                                    }
                                }
                                else
                                {
                                    emptyStr = $"货道号：{p.PassageNumber}!与数据库货道库存数据不匹配；";
                                }
                                if (!dicEmptyCount.ContainsKey(p.PassageNumber))
                                {
                                    dicEmptyCount.Add(p.PassageNumber, p.PassageNumber);
                                }
                            }
                        });
                        if (dicEmptyCount.Count() > 0)
                        {
                            emptyStr = emptyStr.Replace("!", "");
                            throw new Exception(emptyStr);
                        }
                        if (dicNumCount.Count() > 0)
                        {
                            numStr = numStr.Replace("!", "");
                            throw new Exception(numStr);
                        }
                        result.Success = machineStockDetailRepository.Update(machineStockDetailsNew) > 0;
                        //更新
                        if (result.Success)
                        {
                            machineStock.UpdateTime = DateTime.Now;
                            machineStock.UpdateUser = machineStock.ReplenishmentUser;
                            int realStockQuantity = machineStock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.InventoryQuantity);
                            machineStock.RealStockQuantity = realStockQuantity;
                            result.Success = machineStockRepository.Update(machineStock) > 0;
                        }
                        else
                        {
                            throw new Exception("补货失败!");
                        }
                        //更新
                        if (result.Success)
                        {
                            //扣减库存日志
                            result.Success = inventoryChangeLogRepository.Insert(inventoryChangeLogs) > 0;
                            if (result.Success)
                            {
                                result.Success = true;
                                machineStockDetailRepository.TransactionCommit();
                            }
                            else
                            {
                                throw new Exception("添加扣减库存日志失败!");
                            }
                        }
                        else
                        {
                            throw new Exception("修改机器总库存失败!");
                        }
                    }
                    else
                    {
                        throw new Exception("根据机器编号查询不到货道库存信息!");
                    }
                }
                catch (Exception ex)
                {
                    machineStockDetailRepository.TransactionRollback();
                    result.ErrorMsg = ex.Message;
                    result.Success = false;
                }
                return result;
            });

        }
        /// <summary>
        /// 更新货道信息
        /// </summary>
        /// <param name="machineNo"></param>
        /// <param name="passageNumber"></param>
        /// <param name="productId"></param>
        /// <param name="inventoryQuantity"></param>
        /// <returns></returns>
        public async Task<Result> UpdataMachineStock(string machineNo, int passageNumber, int productId, int inventoryQuantity)
        {
            Result result = new Result() { Success = true };
            return await Task.Run(() =>
            {
                try
                {
                    List<MachineStockDetails> machineStockDetails = GetMachineByNo(machineNo);
                    List<MachineStockDetails> machineStockDetailsNew = new List<MachineStockDetails>();
                    List<InventoryChangeLog> inventoryChangeLogs = new List<InventoryChangeLog>();

                    var machineStock = machineStockDetails.FirstOrDefault().MachineStock;
                    string storeNo = machineStock.Machine.MachineNo;
                    var machineStockDetail = machineStockDetails.Where(zw => zw.PassageNumber == passageNumber).FirstOrDefault();
                    if (machineStockDetail != null)
                    {
                        machineStockDetail.ProductId = productId;
                        machineStockDetail.InventoryQuantity = inventoryQuantity;
                        machineStockDetail.UpdateTime = DateTime.Now;
                        machineStockDetail.UpdateUser = machineStock.ReplenishmentUser;
                        machineStockDetailsNew.Add(machineStockDetail);
                        inventoryChangeLogs.Add(new InventoryChangeLog()
                        {
                            ChangeType = (int)ChangeLogType.Add,
                            IsDelete = false,
                            ProductId = productId,
                            Quantity = inventoryQuantity,
                            Content = string.Format("Market编码：{0}，机器编码：{1}，货道号：{2}，扣减库存：{3}", storeNo, machineNo, passageNumber, inventoryQuantity),
                            CreateUser = machineStock.ReplenishmentUser,
                            CredateTime = DateTime.Now
                        });
                    }
                    if (machineStockDetail != null)
                    {
                        result.Success = machineStockDetailRepository.Update(machineStockDetailsNew) > 0;
                    }
                    //更新
                    if (result.Success)
                    {
                        if (inventoryChangeLogs.Count > 0)
                        {
                            machineStock.UpdateTime = DateTime.Now;
                            machineStock.UpdateUser = machineStock.ReplenishmentUser;
                            int realStockQuantity = machineStock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.InventoryQuantity);
                            machineStock.RealStockQuantity = realStockQuantity;
                            result.Success = machineStockRepository.Update(machineStock) > 0;
                        }
                    }
                    if (result.Success)
                    {
                        //扣减库存日志
                        if (inventoryChangeLogs.Count > 0)
                        {
                            result.Success = inventoryChangeLogRepository.Insert(inventoryChangeLogs) > 0;
                        }
                    }

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
