using IPOS.API;
using IPOS.Model.Request;
using IPOS.Model.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
using VendM.Service;

namespace VendM.Application
{
    public class ProductCategoryRedeem
    {
        private ProductCategoryService productCategoryService;
        private IRepository<Store> storeRepository;
        public ProductCategoryRedeem()
        {
            this.productCategoryService = new ProductCategoryService();
            this.storeRepository = new RepositoryBase<Store>();
        }
        /// <summary>
        /// 通过API导入产品
        /// </summary>
        /// <returns></returns>
        public async Task<string> ImportProductCategoryFormAPIAsync()
        {
            List<Model.DataModel.Product.ProductCategory> productCategoryList = new List<Model.DataModel.Product.ProductCategory>();
            ProductCatagoryAPI productCatagoryAPI = new ProductCatagoryAPI();

            //获取Market
            Expression<Func<Store, bool>> exs;
            exs = t => true;
            exs = exs.And(t => !t.IsDelete);
            var storeList = storeRepository.GetEntities(exs);
            if (storeList != null && storeList.Count() > 0)
            {
                foreach (var store in storeList.ToList())
                {
                    ProductCatagoryRequest productCatagoryRequest = new ProductCatagoryRequest() { MarketId = Convert.ToInt32(store.Code) };
                    ResultModel<ProductCatagoryRespsone> resultModel = await productCatagoryAPI.GetProductCatagory(productCatagoryRequest);
                    if (resultModel.Success)
                    {
                        RedeemPageApiModel productCatagoryRepsoneData = resultModel.Result.Data;
                        if (productCatagoryRepsoneData.ProductCategorieList != null && productCatagoryRepsoneData.ProductCategorieList.Count > 0)
                        {
                            //拿到分类数据，进行简单初始化
                            productCategoryList.AddRange(productCatagoryRepsoneData.ProductCategorieList.Select(p => new ProductCategory
                            {
                                CategoryName_CN = p.Name,
                                CategoryName_EN = p.Name,
                                CredateTime = DateTime.Now,
                                CreateUser = "API",
                                IsDelete = false,
                                ProductCategoryVId = Convert.ToInt32(p.Id)

                            }));
                        }
                        //if (productCategoryList.Count > 0)
                        //{
                        //    return productCategoryService.AddList(productCategoryList);
                        //}
                        //else
                        //{
                        //    throw new Exception("API中无商品分类数据!");
                        //}
                    }
                }
            }
            //else
            //{
            //    throw new Exception(resultModel.Message);
            //}
            if (productCategoryList.Count > 0)
            {
                return productCategoryService.AddList(productCategoryList);
            }
            else
            {
                throw new Exception("API中无商品分类数据!");
            }
        }
    }
}

