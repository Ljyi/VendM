using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Core;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 机器
    /// </summary>
    public class MachineController : ApiController
    {
        ProductService productService = null;
        MachineDetailService machineDetailService = null;
        MachineService machineService = null;
        MachineStockDetailService machineStockDetailService = null;
        static string httplink = ConfigurationManager.AppSettings["HttpUrl"].Trim();
        private MachineController()
        {

            productService = new ProductService();
            machineDetailService = new MachineDetailService();
            machineStockDetailService = new MachineStockDetailService();
            machineService = new MachineService();
        }
        // GET api/values

        /// <summary>
        /// 验证机器密码
        /// </summary>
        /// <returns></returns>
        [Route("api/machine/validatepassword")]
        [HttpPost]
        public ResultModel CheckMachinePassword(MachinePasswordCheckAPIDto mpcad)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                if (ModelState.IsValid)
                {
                    if (!machineService.Checkmachinepassword(mpcad))
                    {
                        resultModel.message = "账号或密码错误";
                        resultModel.success = false;
                    }
                }
                else
                {
                    List<string> errorMsg = new List<string>();
                    foreach (var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if (modelstate.Errors.Any())
                        {
                            errorMsg.Add(modelstate.Errors.FirstOrDefault().ErrorMessage);
                        }
                    }
                    resultModel.code = StatusCodeEnum.Error.ToString();
                    resultModel.success = false;
                    resultModel.message = errorMsg.ToSeparateString();
                }
            }
            catch (Exception ex)
            {
                resultModel.success = false;
                resultModel.code = StatusCodeEnum.Error.ToString();
                resultModel.message = ex.Message;
            }
            return resultModel;
        }
        /// <summary>
        /// 获取机器信息
        /// </summary>
        /// <param name="machine_no"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Machine/get")]
        public ResultModel<MachineInfoAPIDto> GetMachineInfo(string machine_no)
        {
            ResultModel<MachineInfoAPIDto> resultModel = new ResultModel<MachineInfoAPIDto>();
            try
            {
                if (!string.IsNullOrEmpty(machine_no))
                {
                    MachineInfoAPIDto machineInfoAPIDto = new MachineInfoAPIDto();
                    Machine machine = machineService.GetMachineByMachineNo(machine_no);
                    List<MachineStockDetails> machineStockDetails = machineStockDetailService.GetMachineByNo(machine_no);
                    if (machine != null && machine.MachineDetails.Count > 0)
                    {
                        machineInfoAPIDto.Id = machine.Id;
                        machineInfoAPIDto.MachineNo = machine.MachineNo;
                        machineInfoAPIDto.Name = machine.Name;
                        machineInfoAPIDto.MachineDetails = new List<MachineDetailInfoAPIDto>();
                        List<MachineDetail> machineDetails = machine.MachineDetails.Where(p => !p.IsDelete).ToList();
                        foreach (var item in machineDetails)
                        {
                            MachineDetailInfoAPIDto machineDetail = new MachineDetailInfoAPIDto()
                            {
                                PassageNumber = item.PassageNumber
                            };
                            machineDetail.Product = null;
                            if (item.ProductId.HasValue)
                            {
                                machineDetail.Product = new ProductAPIDto()
                                {
                                    Id = item.Product.Id,
                                    ImageUrl = httplink + item.Product.ProductImages.Select(zw => zw.Url).FirstOrDefault(),
                                    ProductCode = item.Product.ProductCode,
                                    ProductDetails_CH = item.Product.ProductDetails_CH,
                                    ProductDetails_EN = item.Product.ProductDetails_EN,
                                    ProductName_CH = item.Product.ProductName_CH,
                                    ProductName_EN = item.Product.ProductName_EN,
                                    ProductVId = item.Product.ProductVId,
                                    Specification_CH = item.Product.Specification_CH,
                                    Specification_EN = item.Product.Specification_EN,
                                    SalePriceType = productService.ConvertProductPriceDto(item.Product.ProductPrices)
                                };
                            }
                            machineInfoAPIDto.MachineDetails.Add(machineDetail);
                        }
                        //查询售货机库存
                        machineInfoAPIDto.MachineDetails.ForEach(p =>
                        {
                            var prod = machineStockDetails.FirstOrDefault(zw => zw.PassageNumber == p.PassageNumber && !zw.IsDelete);
                            if (prod != null)
                            {
                                p.InventoryQuantity = prod.InventoryQuantity;
                                p.TotalQuantity = prod.TotalQuantity;
                            }
                        });
                        resultModel.data = machineInfoAPIDto;
                    }
                    else
                    {
                        resultModel.message = "暂无商品";
                    }
                }
                else
                {
                    resultModel.success = false;
                    resultModel.code = StatusCodeEnum.ParameterError.ToString();
                    resultModel.message = "参数：machineNo不能为空";
                }
            }
            catch (Exception ex)
            {
                resultModel.success = false;
                resultModel.code = StatusCodeEnum.Error.ToString();
                resultModel.message = ex.Message;
            }
            return resultModel;
        }

        /// <summary>
        /// 更新货道产品
        /// </summary>
        /// <param name="machine_prodcut">机器产品</param>
        /// <returns></returns>
        [Route("api/machine/machinedetail-update")]
        [HttpPost]
        public async Task<ResultModel<bool>> MachineProductUpdate(MachineProdcutDto machine_prodcut)
        {
            ResultModel<bool> resultModel = new ResultModel<bool>();
            try
            {
                if (ModelState.IsValid)
                {
                    await Task.Run(() =>
                    {
                        resultModel.data = machineService.MachineDetailUpdate(machine_prodcut.MachineNo, machine_prodcut.ProductId, machine_prodcut.PassagesId, machine_prodcut.InventoryQuantity);
                    });
                    if (!resultModel.data)
                    {
                        resultModel.code = StatusCodeEnum.Error.ToString();
                        resultModel.success = false;
                        resultModel.message = "更新失败";
                    }
                }
                else
                {
                    List<string> errorMsg = new List<string>();
                    foreach (var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if (modelstate.Errors.Any())
                        {
                            errorMsg.Add(modelstate.Errors.FirstOrDefault().ErrorMessage);
                        }
                    }
                    resultModel.code = StatusCodeEnum.Error.ToString();
                    resultModel.success = false;
                    resultModel.message = errorMsg.ToSeparateString();
                }
            }
            catch (Exception ex)
            {
                resultModel.success = false;
                resultModel.code = StatusCodeEnum.Error.ToString();
                resultModel.message = ex.Message;
            }
            return resultModel;
        }

        /// <summary>
        /// 删除货道内产品
        /// </summary>
        /// <param name="machine_prodcut"></param>
        /// <returns></returns>
        [Route("api/machine/machinedetail-product-delete")]
        [HttpPost]
        public async Task<ResultModel<bool>> MachineDetailDelete(MachineProdcutDelDto machine_prodcut)
        {
            ResultModel<bool> resultModel = new ResultModel<bool>();
            try
            {
                if (ModelState.IsValid)
                {
                    await Task.Run(() =>
                    {
                        resultModel.data = machineService.MachineDetailProductDelete(machine_prodcut);
                    });
                }
                else
                {
                    List<string> errorMsg = new List<string>();
                    foreach (var key in ModelState.Keys)
                    {
                        var modelstate = ModelState[key];
                        if (modelstate.Errors.Any())
                        {
                            errorMsg.Add(modelstate.Errors.FirstOrDefault().ErrorMessage);
                        }
                    }
                    resultModel.code = StatusCodeEnum.Error.ToString();
                    resultModel.success = false;
                    resultModel.message = errorMsg.ToSeparateString();
                }
            }
            catch (Exception ex)
            {
                resultModel.success = false;
                resultModel.code = StatusCodeEnum.Error.ToString();
                resultModel.message = ex.Message;
            }
            return resultModel;
        }

    }
}
