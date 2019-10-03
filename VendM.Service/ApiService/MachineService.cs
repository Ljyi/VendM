using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.Model;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
using VendM.Model.EnumModel;

namespace VendM.Service
{
    public partial class MachineService
    {
        #region 机器
        /// <summary>
        /// 验证机器后台登录
        /// </summary>
        /// <param name="mpcad"></param>
        /// <returns></returns>
        public bool Checkmachinepassword(MachinePasswordCheckAPIDto mpcad)
        {
            var machine = machineRepository.Entities.Where(p => p.MachineNo == mpcad.MachineNo && p.Password == mpcad.Password && !p.IsDelete).FirstOrDefault();
            if (machine == null)
            {
                return false;
            }
            return true;
        }
        #endregion 

        #region 货道
        /// <summary>
        /// 获取货道信息
        /// </summary>
        /// <param name="machineNo"></param>
        /// <returns></returns>
        public async Task<List<MachinePassageProductAPIDto>> GetPassageNumber(string machineNo)
        {
            return await Task.Run(() =>
            {
                try
                {
                    List<MachinePassageProductAPIDto> list = new List<MachinePassageProductAPIDto>();
                    Expression<Func<MachineDetail, bool>> ex = t => true;
                    ex = ex.And(t => !t.IsDelete);
                    if (!string.IsNullOrEmpty(machineNo))
                    {
                        ex = ex.And(t => t.Machine.MachineNo == machineNo);
                    }
                    //查询所有货道号
                    var machineDetails = machinedetailRepository.GetEntities(ex);
                    machineDetails.ToList().ForEach(p =>
                    {
                        var machineStockDetail = machineStockDetailService.GetMachineStockDetailByNo(machineNo, null, p.PassageNumber);
                        list.Add(new MachinePassageProductAPIDto()
                        {
                            Quantity = machineStockDetail != null ? machineStockDetail.TotalQuantity : 0,
                            PassageNumber = p.PassageNumber,
                            ProductInfo = p.Product != null ? "货道号 " + p.PassageNumber.ToString() + "(货道已存在商品:" + p.Product.ProductName_CH + ")" : "货道号 " + p.PassageNumber.ToString(),
                        });
                    });


                    return list;
                }
                catch (Exception ex)
                {
                    throw new Exception("查询货道信息异常" + ex.Message, ex);
                }
            });
        }

        /// <summary>
        /// 更新货道信息
        /// </summary>
        /// <param name="machineNo">机器No</param>
        /// <param name="productId">产品Id</param>
        /// <param name="passagesId">货道Id</param>
        /// <returns></returns>
        public bool MachineDetailUpdate(string machineNo, int productId, int passagesId, int InventoryQuantity)
        {
            try
            {
                machinedetailRepository.BeginTransaction();
                //机器
                Machine machine = GetMachineByMachineNo(machineNo);
                if (machine != null)
                {
                    MachineDetail machineDetail = machine.MachineDetails.FirstOrDefault(p => p.PassageNumber == passagesId);

                    if (machineDetail != null)
                    {
                        //货道
                        machineDetail.ProductId = productId;
                        machineDetail.UpdateTime = DateTime.Now;
                        machineDetail.UpdateUser = RequstSourceEnum.Client.ToString();
                        bool success = machinedetailRepository.Update(machineDetail) > 0;

                        //此机器所有货道库存
                        List<MachineStockDetails> machineStockDetails = machineStockDetailService.GetMachineByNo(machineNo);
                        List<MachineStockDetails> machineStockDetailsNew = new List<MachineStockDetails>();
                        List<InventoryChangeLog> inventoryChangeLogs = new List<InventoryChangeLog>();

                        var machineStock = machineStockDetails.FirstOrDefault().MachineStock;
                        string storeNo = machineStock.Machine.MachineNo;
                        //货道库存
                        var machineStockDetail = machineStockDetails.Where(zw => zw.PassageNumber == passagesId).FirstOrDefault();
                        if (machineStockDetail != null)
                        {
                            machineStockDetail.ProductId = productId;
                            machineStockDetail.InventoryQuantity = InventoryQuantity;
                            machineStockDetail.UpdateTime = DateTime.Now;
                            machineStockDetail.UpdateUser = machineStock.ReplenishmentUser;
                            machineStockDetailsNew.Add(machineStockDetail);
                            inventoryChangeLogs.Add(new InventoryChangeLog()
                            {
                                ChangeType = (int)ChangeLogType.Add,
                                IsDelete = false,
                                ProductId = productId,
                                Quantity = InventoryQuantity,
                                Content = string.Format("Market编码：{0}，机器编码：{1}，货道号：{2}，扣减库存：{3}", storeNo, machineNo, passagesId, InventoryQuantity),
                                CreateUser = machineStock.ReplenishmentUser,
                                CredateTime = DateTime.Now
                            });
                            //更新货道库存
                            success = machinestockdetaisRepository.Update(machineStockDetailsNew) > 0;
                        }
                        else
                        {
                            success = false;
                        }

                        if (success)
                        {
                            if (inventoryChangeLogs.Count > 0)
                            {
                                machineStock.UpdateTime = DateTime.Now;
                                machineStock.UpdateUser = machineStock.ReplenishmentUser;
                                int realStockQuantity = machineStock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.InventoryQuantity);
                                machineStock.RealStockQuantity = realStockQuantity;
                                success = machinestockRepository.Update(machineStock) > 0;
                            }
                        }
                        if (success)
                        {
                            //扣减库存日志
                            if (inventoryChangeLogs.Count > 0)
                            {
                                success = inventoryChangeLogRepository.Insert(inventoryChangeLogs) > 0;
                            }
                        }
                        if (success)
                        {
                            machinedetailRepository.TransactionCommit();
                            return true;
                        }
                        else
                        {
                            machinedetailRepository.TransactionRollback();
                            return false;
                        }
                    }
                    else
                    {
                        throw new Exception("获取不到此货道号的数据!");
                    }
                }
                else
                {
                    throw new Exception("获取不到此机器编号的数据!");
                }
            }
            catch (Exception ex)
            {
                machinedetailRepository.TransactionRollback();
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除货道产品
        /// 清除货道内商品
        /// </summary>
        /// <param name="machineProdcut"></param>
        /// <returns></returns>
        public bool MachineDetailProductDelete(MachineProdcutDelDto machineProdcut)
        {
            machinedetailRepository.BeginTransaction();
            bool reflg = true;
            try
            {
                List<int> passagesIds = machineProdcut.PassagesIds.SplitDefaultToIntList();
                if (passagesIds.Count > 0)
                {
                    //货道
                    List<MachineDetail> machineDetails = GetMachineByMachineNo(machineProdcut.MachineNo, passagesIds);
                    List<int> passageNumbers = new List<int>();
                    int machineID = 0;
                    machineDetails.ForEach(p =>
                    {
                        p.ProductId = null;
                        p.Product = null;
                        p.UpdateTime = DateTime.Now;
                        p.UpdateUser = RequstSourceEnum.Client.ToString();

                        machineID = p.MachineId;
                        passageNumbers.Add(p.PassageNumber);

                    });
                    if (!(machinedetailRepository.Update(machineDetails) > 0))
                    {
                        throw new Exception("提交货道失败!");
                    }
                    //机器库存
                    Expression<Func<MachineStock, bool>> ex = t => true;
                    ex = ex.And(t => !t.IsDelete);
                    ex = ex.And(t => t.MachineId == machineID);

                    MachineStock machineStock = machinestockRepository.GetEntities(ex).FirstOrDefault();
                    int machineStockID = machineStock.Id;
                    if (passageNumbers.Count > 0)
                    {
                        //货道库存
                        List<MachineStockDetails> machineStockDetails = GetMachineStockDetailsByMachineStockID(machineStockID, passageNumbers);
                        machineStockDetails.ForEach(p =>
                        {
                            p.ProductId = null;
                            p.Product = null;
                            p.InventoryQuantity = 0;
                            p.UpdateTime = DateTime.Now;
                            p.UpdateUser = RequstSourceEnum.Client.ToString();
                        });
                        if (!(machinestockdetaisRepository.Update(machineStockDetails) > 0))
                        {
                            throw new Exception("提交货道库存失败!");
                        };

                        Expression<Func<MachineStockDetails, bool>> exs = t => true;
                        exs = exs.And(t => !t.IsDelete);
                        exs = exs.And(t => t.MachineStockId == machineStockID);
                        var totalQuantity = machinestockdetaisRepository.GetEntities(exs).Sum(p => p.InventoryQuantity);

                        machineStock.RealStockQuantity = totalQuantity;
                        machineStock.UpdateTime = DateTime.Now;
                        machineStock.UpdateUser = RequstSourceEnum.Client.ToString();
                        if (!(machinestockRepository.Update(machineStock) > 0))
                        {
                            throw new Exception("提交机器库存失败!");
                        };
                        reflg = true;
                    }
                    else
                    {
                        reflg = false;
                    }
                }
                else
                {
                    reflg = false;
                }
                machinedetailRepository.TransactionCommit();
                return reflg;
            }
            catch (Exception ex)
            {
                machinedetailRepository.TransactionRollback();
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除货道
        /// </summary>
        /// <param name="machineProdcut"></param>
        /// <returns></returns>
        public bool MachineDetailDelete(MachinePassageDelAPIDto machinePassage)
        {
            machinedetailRepository.BeginTransaction();
            try
            {
                bool isNumber = true;
                //机器
                Machine machine = GetMachineByMachineNo(machinePassage.MachineNo);
                if (machine != null)
                {
                    //机器总库存
                    MachineStock machineStock = machineStockService.GetMachineStockByMachineId(machine.Id);
                    if (machineStock != null)
                    {
                        List<int> passagesIds = machinePassage.PassagesIds.SplitDefaultToIntList();

                        //货道库存
                        List<MachineStockDetails> machineStockDetails = machineStock.MachineStockDetails.Where(p => passagesIds.Contains(p.PassageNumber)).ToList();
                        //货道
                        List<MachineDetail> machineDetails = machine.MachineDetails.Where(p => passagesIds.Contains(p.PassageNumber)).ToList();
                        if (machineStockDetails != null && machineDetails != null)
                        {
                            //货道详细信息标记删除
                            machineDetails.ForEach(p =>
                            {
                                p.ProductId = null;
                                p.IsDelete = true;
                                p.UpdateTime = DateTime.Now;
                                p.UpdateUser = RequstSourceEnum.Client.ToString();

                            });
                            isNumber = machinedetailRepository.Update(machineDetails) > 0;
                            if (!isNumber)
                            {
                                throw new Exception("删除货道失败!");
                            }
                            //货道库存信息标记删除
                            machineStockDetails.ForEach(s =>
                            {
                                s.ProductId = null;
                                s.TotalQuantity = 0;
                                s.InventoryQuantity = 0;
                                s.IsDelete = true;
                                s.UpdateTime = DateTime.Now;
                                s.UpdateUser = RequstSourceEnum.Client.ToString();
                            });
                            isNumber = machinestockdetaisRepository.Update(machineStockDetails) > 0;
                            if (!isNumber)
                            {
                                throw new Exception("删除货道失败!");
                            }
                        }
                        else
                        {
                            throw new Exception("货道或货道库存数据有误!");
                        }
                        //更新机器库存容量
                        var totalQuantity = machineStock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.TotalQuantity);
                        int realStockQuantity = machineStock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.InventoryQuantity);
                        machineStock.RealStockQuantity = realStockQuantity;
                        machineStock.TotalQuantity = totalQuantity;
                        machineStock.UpdateTime = DateTime.Now;
                        machineStock.UpdateUser = RequstSourceEnum.Client.ToString();
                        isNumber = machinestockRepository.Update(machineStock) > 0;
                    }
                    else
                    {
                        throw new Exception("机器库存不存在!");
                    }
                }
                else
                {
                    throw new Exception("无法放获取到对应的机器!");
                }
                if (!isNumber)
                {
                    throw new Exception("删除货道失败!");
                }
                else
                {
                    machinedetailRepository.TransactionCommit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                machinedetailRepository.TransactionRollback();
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        ///添加货道
        /// </summary>
        /// <param name="machinePassage"></param>
        /// <returns></returns>
        public bool MachineDetailAdd(MachinePassageAPIDto machinePassage)
        {
            machinedetailRepository.BeginTransaction();
            try
            {
                Machine machine = GetMachineByMachineNo(machinePassage.MachineNo);
                bool isSuccess = true;
                if (machine != null)
                {
                    MachineStock machineStock = machineStockService.GetMachineStockByMachineId(machine.Id);
                    if (machineStock != null)
                    {
                        MachineDetail machineDetail = machine.MachineDetails.FirstOrDefault(p => p.PassageNumber == machinePassage.PassagesId);
                        MachineStockDetails machineStockDetails = machineStockDetailService.GetMachineStockDetailByNo(machinePassage.MachineNo, machinePassage.PassagesId);
                        ////货道
                        if (machineDetail == null)
                        {
                            machineDetail = new MachineDetail()
                            {
                                MachineId = machine.Id,
                                PassageNumber = machinePassage.PassagesId,
                                IsDelete = false,
                                CreateUser = RequstSourceEnum.Client.ToString(),
                                CredateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                UpdateUser = RequstSourceEnum.Client.ToString()
                            };
                            isSuccess = machinedetailRepository.Insert(machineDetail) > 0;
                            if (!isSuccess)
                            {
                                throw new Exception("保存失败!");
                            }
                        }
                        else
                        {
                            machineDetail.IsDelete = false;
                            machineDetail.UpdateTime = DateTime.Now;
                            machineDetail.UpdateUser = RequstSourceEnum.Client.ToString();
                            isSuccess = machinedetailRepository.Update(machineDetail) > 0;
                            if (!isSuccess)
                            {
                                throw new Exception("保存失败!");
                            }
                        }

                        //货道库存
                        if (machineStockDetails == null)
                        {
                            machineStockDetails = new MachineStockDetails()
                            {
                                TotalQuantity = machinePassage.Quantity,
                                InventoryQuantity = 0,
                                PassageNumber = machinePassage.PassagesId,
                                MachineStockId = machineStock.Id,
                                IsDelete = false,
                                CreateUser = RequstSourceEnum.Client.ToString(),
                                CredateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                UpdateUser = RequstSourceEnum.Client.ToString()
                            };
                            isSuccess = machinestockdetaisRepository.Insert(machineStockDetails) > 0;
                            if (!isSuccess)
                            {
                                throw new Exception("保存失败!");
                            }
                        }
                        else
                        {
                            machineStockDetails.IsDelete = false;
                            if (machineStockDetails.InventoryQuantity > machinePassage.Quantity)
                            {
                                throw new Exception("货道容量不能小于库存数量!");
                            }
                            machineStockDetails.TotalQuantity = machinePassage.Quantity;
                            machineStockDetails.UpdateTime = DateTime.Now;
                            machineStockDetails.UpdateUser = RequstSourceEnum.Client.ToString();
                            isSuccess = machineStockDetailService.Update(machineStockDetails);
                            if (!isSuccess)
                            {
                                throw new Exception("保存失败!");
                            }
                        }
                        //更新机器库存容量
                        var totalQuantity = machineStock.MachineStockDetails.Where(s => s.IsDelete.Equals(false)).Sum(p => p.TotalQuantity);
                        machineStock.TotalQuantity = totalQuantity;
                        machineStock.UpdateTime = DateTime.Now;
                        machineStock.UpdateUser = RequstSourceEnum.Client.ToString();
                        isSuccess = machinestockRepository.Update(machineStock) > 0;
                        if (!isSuccess)
                        {
                            throw new Exception("保存失败!");
                        }
                    }
                    else
                    {
                        throw new Exception("获取机器库存信息失败!");
                    }
                }
                else
                {
                    throw new Exception("获取对应的机器信息失败!");
                }
                machinedetailRepository.TransactionCommit();
                return true;
            }
            catch (Exception ex)
            {
                machinedetailRepository.TransactionRollback();
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 机器产品
        /// <summary>
        /// 获取机器内产品（机器内商品管理）
        /// </summary>
        /// <param name="machineNo">机器编码</param>
        /// <returns></returns>
        public async Task<List<ProductAPIDto>> GetMachineProduct(string machineNo)
        {
            return await Task.Run(() =>
            {
                try
                {
                    List<ProductAPIDto> productAPIDtos = new List<ProductAPIDto>();
                    Machine machine = GetMachineByMachineNo(machineNo);
                    if (machine != null)
                    {
                        List<Product> products = machine.MachineDetails.Where(p => !p.IsDelete).Select(p => p.Product).ToList();
                        return Mapper.Map<List<Product>, List<ProductAPIDto>>(products);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            });

        }
        #endregion
    }
}
