using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using VendM.Model.APIModelDto;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    /// <summary>
    /// 产品
    /// </summary>
    public class ProductController : ApiController
    {
        ProductService productService = null;
        MachineDetailService machineDetailService = null;
        MachineService machineService = null;
        MachineStockDetailService machineStockDetailService = null;
        static string httplink = ConfigurationManager.AppSettings["HttpUrl"].Trim();
        /// <summary>
        /// 构造
        /// </summary>
        private ProductController()
        {
            productService = new ProductService();
            machineDetailService = new MachineDetailService();
            machineStockDetailService = new MachineStockDetailService();
            machineService = new MachineService();
        }
        ///// <summary>
        ///// 获取机器详细信息
        ///// </summary>
        ///// <param name="machine_no">机器编号</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("api/product/multi-get")]
        //public ResultModels<ProductAPIDto> GetList(string machine_no)
        //{
        //    ResultModels<ProductAPIDto> resultModel = new ResultModels<ProductAPIDto>();
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(machine_no))
        //        {
        //            Machine machine = machineService.GetMachineByMachineNo(machine_no);
        //            List<MachineStockDetails> machineStockDetails = machineStockDetailService.GetMachineByNo(machine_no);
        //            if (machine != null && machine.MachineDetails.Count > 0)
        //            {
        //                List<MachineDetail> machineDetails = machine.MachineDetails.ToList();
        //                List<ProductAPIDto> products = machineDetails.Where(zw => !zw.Product.IsDelete).Select(p => new ProductAPIDto()
        //                {
        //                    Id = p.Product.Id,
        //                    ImageUrl = httplink + p.Product.ProductImages.Select(zw => zw.Url).FirstOrDefault(),
        //                    //  PassageNumber = p.PassageNumber,
        //                    //  InventoryQuantity = 0,
        //                    ProductCode = p.Product.ProductCode,
        //                    ProductDetails_CH = p.Product.ProductDetails_CH,
        //                    ProductDetails_EN = p.Product.ProductDetails_EN,
        //                    ProductName_CH = p.Product.ProductName_CH,
        //                    ProductName_EN = p.Product.ProductName_EN,
        //                    SalePriceType = productService.ConvertProductPriceDto(p.Product.ProductPrices)
        //                }).ToList();
        //                //查询售货机库存
        //                //products.ForEach(p =>
        //                //{
        //                //    var prod = machineStockDetails.FirstOrDefault(zw => zw.ProductId == p.Id && zw.PassageNumber == zw.PassageNumber);
        //                //    if (prod != null)
        //                //    {
        //                //        p.InventoryQuantity = prod.InventoryQuantity;
        //                //    }
        //                //});
        //                resultModel.data = products;
        //            }
        //            else
        //            {
        //                resultModel.message = "暂无商品";
        //            }
        //        }
        //        else
        //        {
        //            resultModel.success = false;
        //            resultModel.code = StatusCodeEnum.ParameterError.ToString();
        //            resultModel.message = "参数：machineNo不能为空";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        resultModel.success = false;
        //        resultModel.code = StatusCodeEnum.Error.ToString();
        //        resultModel.message = ex.Message;
        //    }
        //    return resultModel;
        //}

        /// <summary>
        /// 获取机器内产品
        /// </summary>
        /// <param name="machine_no">机器编号</param>
        /// <returns></returns>
        [Route("api/product/multi-get")]
        [HttpGet]
        public async Task<ResultModels<ProductAPIDto>> GetMachineProduct(string machine_no)
        {
            ResultModels<ProductAPIDto> resultModels = new ResultModels<ProductAPIDto>();
            try
            {
                resultModels.data = await machineService.GetMachineProduct(machine_no);
            }
            catch (Exception ex)
            {
                resultModels.success = false;
                resultModels.code = StatusCodeEnum.Error.ToString();
                resultModels.message = ex.Message;
            }
            return resultModels;
        }
        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="machine_no">机器编码</param>
        /// <param name="product_code">产品code</param>
        /// <returns></returns>
        [Route("api/product/get")]
        [HttpGet]
        public ResultModel<ProductAPIDto> GetProduct(string machine_no, string product_code)
        {
            ResultModel<ProductAPIDto> resultModel = new ResultModel<ProductAPIDto>();
            try
            {
                ProductAPIDto productAPIDto = new ProductAPIDto();
                var product = productService.GetByProductCode(product_code);
                if (product != null)
                {
                    productAPIDto.Id = product.Id;
                    if (product.ProductImages.Any())
                    {
                        productAPIDto.ImageUrl = product.ProductImages.FirstOrDefault().Url;
                    }
                    productAPIDto.ProductCode = product.ProductCode;
                    productAPIDto.ProductName_CH = product.ProductName_CH;
                    productAPIDto.ProductName_EN = product.ProductName_EN;
                    productAPIDto.ProductDetails_CH = product.ProductDetails_CH;
                    productAPIDto.ProductDetails_EN = product.ProductDetails_EN;
                    productAPIDto.SalePriceType = productService.ConvertProductPriceDto(product.ProductPrices);
                    resultModel.data = productAPIDto;
                }
                else
                {
                    resultModel.code = StatusCodeEnum.Error.ToString();
                    resultModel.success = false;
                    resultModel.message = "商品不存在";
                }
            }
            catch (Exception ex)
            {
                resultModel.code = StatusCodeEnum.Error.ToString();
                resultModel.success = false;
                resultModel.message = ex.Message;
            }
            return resultModel;
        }

    }
}
