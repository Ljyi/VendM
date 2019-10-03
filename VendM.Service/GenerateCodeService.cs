using System;
using System.Linq;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModel.Product;

namespace VendM.Service
{
    /// <summary>
    /// 单例模式
    /// </summary>
    public class GenerateCodeService
    {
        private static readonly object locker = new object();
        private static GenerateCodeService generateCodeService;
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Product> productRepository;
        private IRepository<Store> storeRepository;
        private IRepository<Order> orderRepository;
        private IRepository<ProductCategory> productcategoryRepository;
        /// <summary>
        ///  防止创建类的实例
        /// </summary>
        private GenerateCodeService()
        {
            this.productRepository = new RepositoryBase<Product>();
            this.storeRepository = new RepositoryBase<Store>();
            this.orderRepository = new RepositoryBase<Order>();
            this.productcategoryRepository = new RepositoryBase<ProductCategory>();
        }
        public static GenerateCodeService SingleGenerateCodeService()
        {
            if (generateCodeService == null)
            {
                lock (locker)
                {
                    if (generateCodeService == null)
                    {
                        generateCodeService = new GenerateCodeService();
                    }
                }
            }
            return generateCodeService;
        }
        /// <summary>
        /// 自动生成产品编码
        /// </summary>
        /// 编码格式：0000001 
        /// 增量：1
        /// <returns></returns>
        public string GetNextProdcutCode(ref string productCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(productCode) && productCode.Length == 7)
                {
                    lock (locker)
                    {
                        string code = productCode;
                        if (productRepository.Entities.Any(p => p.ProductCode == code))
                        {
                            var product = productRepository.Entities.OrderByDescending(p => p.ProductCode).FirstOrDefault();
                            productCode = product.ProductCode;
                            int codeNum = int.Parse(productCode) + 1;
                            productCode = Math.Round((codeNum / 1000000m), 7).ToString("f6").Replace(".", "");
                        }
                    }
                    return productCode;
                }
                else
                {
                    productCode = "0000001";
                    return GetNextProdcutCode(ref productCode);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }
        /// <summary>
        /// 自动生成Market编码
        /// </summary>
        /// 编码格式：0001
        /// 增量：1
        /// <param name="productCode"></param>
        /// <param name="checkCode"></param>
        public string GetNextStoreCode(ref string storeCode)
        {
            //if (!string.IsNullOrEmpty(productCode) && productCode.Length == 7)
            //{
            //    lock (locker)
            //    {
            //        string code = productCode;
            //        if (productRepository.Entities.Any(p => p.ProductCode == code))
            //        {
            //            var product = productRepository.Entities.OrderByDescending(p => p.ProductCode).FirstOrDefault();
            //            productCode = product.ProductCode;
            //            int codeNum = int.Parse(productCode) + 1;
            //            productCode = Math.Round((codeNum / 1000000m), 7).ToString("f6").Replace(".", "");
            //        }
            //    }
            //    return productCode;
            //}
            //else
            //{
            //    productCode = "0000001";
            //    return GetNextProdcutCode(ref productCode);
            //}

            if (!string.IsNullOrEmpty(storeCode) && storeCode.Length == 4)
            {
                lock (locker)
                {
                    string code = storeCode;
                    if (storeRepository.Entities.Any(p => p.Code == code))
                    {
                        var store = storeRepository.Entities.OrderByDescending(p => p.Id).FirstOrDefault();
                        storeCode = store.Code;
                        int codeNum = int.Parse(storeCode) + 1;
                        storeCode = Math.Round((codeNum / 1000m), 7).ToString("f3").Replace(".", "");
                    }
                }
                return storeCode;
            }
            else
            {
                storeCode = "0001";
                return GetNextStoreCode(ref storeCode);
            }
        }
        /// <summary>
        /// 获取订单编号
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public string GetNextOrderCode(ref string orderCode)
        {
            string date = Common.GetShortDateString(DateTime.Now);
            if (!string.IsNullOrEmpty(orderCode) && orderCode.Length == 15)
            {
                lock (locker)
                {
                    string code = orderCode;
                    if (orderRepository.Entities.Where(zw => zw.OrderNo.Contains(date)).Any(p => p.OrderNo == code))
                    {
                        var order = orderRepository.Entities.OrderByDescending(p => p.Id).FirstOrDefault();
                        orderCode = order.OrderNo;
                        int codeNum = int.Parse(orderCode.Remove(0, 8)) + 1;
                        orderCode = date + (codeNum / 1000000m).ToString().Replace(".", "");
                    }
                }
                return orderCode;
            }
            else
            {
                orderCode = date + "0000001";
                return GetNextOrderCode(ref orderCode);
            }
        }
        /// <summary>
        /// 自动生成产品类别编码
        /// </summary>
        /// 编码格式：P0000001 
        /// 增量：1
        /// <returns></returns>
        public string GetNextProductCategoryCode(ref string productCategoryCode)
        {
            if (!string.IsNullOrEmpty(productCategoryCode) && productCategoryCode.Length == 8)
            {
                lock (locker)
                {
                    string code = productCategoryCode;
                    if (productcategoryRepository.Entities.Any(p => p.CategoryCode == code))
                    {
                        var product = productcategoryRepository.Entities.OrderByDescending(p => p.Id).FirstOrDefault();
                        productCategoryCode = product.CategoryCode;
                        int codeNum = int.Parse(productCategoryCode.Remove(0, 2)) + 1;
                        productCategoryCode = "PT" + Math.Round((codeNum / 100000m), 6).ToString("f5").Replace(".", "");
                    }
                }
                return productCategoryCode;
            }
            else
            {
                productCategoryCode = "PT000001";
                return GetNextProductCategoryCode(ref productCategoryCode);
            }
        }
    }
}
