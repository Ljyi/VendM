using IPOS.API;
using IPOS.Model.Request;
using IPOS.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
using VendM.Service;

namespace VendM.Application
{
    /// <summary>
    /// 获取产品信息（通过API）
    /// </summary>
    public class ProductRedeem
    {
        private ProductService productService;
        private IRepository<ProductCategory> productCategoryRepository;
        private IRepository<Store> storeRepository;
        public ProductRedeem()
        {
            productService = new ProductService();
            this.productCategoryRepository = new RepositoryBase<ProductCategory>();
            this.storeRepository = new RepositoryBase<Store>();
        }
        /// <summary>
        /// 通过API导入产品
        /// </summary>
        /// <returns></returns>
        public async Task<string> ImportProductFormAPIAsync()
        {

            List<Model.DataModel.Product.Product> products = new List<Model.DataModel.Product.Product>();
            ProductAPI productAPI = new ProductAPI();
            //获取Market
            Expression<Func<Store, bool>> exs;
            exs = t => true;
            exs = exs.And(t => !t.IsDelete);
            var storeList = storeRepository.GetEntities(exs);
            if (storeList != null && storeList.Count() > 0)
            {
                //获取商品对应的分类ID
                Expression<Func<ProductCategory, bool>> ex;
                ex = t => true;
                ex = ex.And(t => !t.IsDelete);
                List<ProductCategory> productCategories = productCategoryRepository.GetEntities(ex).ToList();
                foreach (var store in storeList.ToList())
                {
                    //item.code
                    ProductSerchRequest productSerchRequest = new ProductSerchRequest() { MarketId = Convert.ToInt32(store.Code) };
                    ResultModel<ProductRepsone> resultModel = await productAPI.GetProducts(productSerchRequest);
                    if (resultModel.Success)
                    {
                        ProductRepsoneData productRepsoneData = resultModel.Result.Data;
                        foreach (var item in productRepsoneData.RedeemProductList)
                        {
                            ProductCategory productCategory = new ProductCategory();
                            Model.DataModel.Product.Product product = new Model.DataModel.Product.Product();
                            List<ProductPrice> productPrices = new List<ProductPrice>();
                            product.StoreId = Convert.ToInt32(store.Id);
                            product.ProductVId = item.Id;
                            product.ProductName_CH = item.Name;
                            product.CreateUser = "API";
                            product.CredateTime = DateTime.Now;
                            product.UpdateUser = "API";
                            product.UpdateTime = DateTime.Now;
                            product.IsDelete = false;
                            product.ProductCode = "";
                            product.ProductDetails_CH = "中文描述";
                            product.ProductDetails_EN = "英文描述";
                            product.ProductName_EN = item.Name;
                            try
                            {
                                int productCategoryId = Convert.ToInt32(item.ProductCategoryId);
                                var productCategorie = productCategories.Where(zw => zw.ProductCategoryVId == productCategoryId).FirstOrDefault();
                                if (productCategorie != null)
                                {
                                    product.ProductCategoryId = productCategorie.Id;
                                }
                            }
                            catch (Exception)
                            {
                                product.ProductCategoryId = 0;
                            }
                            foreach (var productWay in item.RedeemProductWay)
                            {
                                ProductPrice productPrice = new ProductPrice();
                                try
                                {
                                    productPrice.Price = productWay.Amount;
                                    productPrice.Point = Decimal.ToInt32(productWay.Point);
                                }
                                catch (Exception)
                                {
                                }
                                if (productPrice.Price > 0 && productPrice.Point > 0)
                                {
                                    productPrice.SaleType = 1;
                                }
                                else if (productPrice.Price > 0 && productPrice.Point <= 0)
                                {
                                    productPrice.SaleType = 0;
                                }
                                else
                                {
                                    productPrice.SaleType = 2;
                                }
                                productPrice.ProductWayVId = productWay.Id;
                                productPrice.IsDelete = false;
                                productPrice.CreateUser = "API";
                                productPrice.CredateTime = DateTime.Now;
                                productPrice.UpdateUser = "API";
                                productPrice.UpdateTime = DateTime.Now;
                                productPrices.Add(productPrice);
                            }
                            product.ProductPrices = productPrices;
                            products.Add(product);
                        }
                    }
                    //else
                    //{
                    //    throw new Exception(resultModel.Message);
                    //}
                }
            }
            if (products.Count > 0)
            {
                return productService.AddList(products);
            }
            else
            {
                throw new Exception("API中无商品数据!");
            }
        }

        //public (List<Model.DataModel.Product.Product> addProducts, List<Model.DataModel.Product.Product> upProducts) GetAddAndUpProducts(List<IPOS.Model.ResponseModel.Product> products)
        //{
        //    List<Model.DataModel.Product.Product> productList = productService.GetProductAll();
        //    List<string> productVIdList = products.Select(p => p.Id).ToList();
        //    //更新
        //    List<Model.DataModel.Product.Product> productUp = productList.Where(zw => productVIdList.Contains(zw.ProductVId)).ToList();

        //    productList.Where(zw => productVIdList.Contains(zw.ProductVId)).ToList().ForEach(p =>
        //    {

        //    });

        //    //添加
        //    List<Model.DataModel.Product.Product> productsAdd = new List<Model.DataModel.Product.Product>();
        //    products.Where(p =>
        //    {
        //        return productUp.Exists(zw => zw.ProductVId != p.Id);
        //    }
        //    ).ToList().ForEach(p =>
        //    {
        //        ProductCategory productCategory = new ProductCategory();
        //        Model.DataModel.Product.Product product = new Model.DataModel.Product.Product();
        //        List<ProductPrice> productPrices = new List<ProductPrice>();
        //        try
        //        {
        //            product.ProductVId = p.Id;
        //            product.ProductName_CH = p.Name;
        //            product.CreateUser = "API";
        //            product.CredateTime = DateTime.Now;
        //            product.IsDelete = false;
        //            product.ProductCode = "";
        //            product.ProductDetails_CH = "中文描述";
        //            product.ProductDetails_EN = "英文描述";
        //            product.ProductName_EN = p.Name;
        //            productsAdd.Add(product);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    });

        //    return (productsAdd, productUp);
        //}
    }
}
