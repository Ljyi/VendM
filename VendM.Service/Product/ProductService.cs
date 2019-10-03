using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;
using VendM.Model.EnumModel;
using VendM.Service.EventHandler;

namespace VendM.Service
{
    /// <summary>
    /// 业务逻辑
    /// </summary>
    public partial class ProductService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<Product> productRepository;
        private IRepository<ProductCategory> productCategoryRepository;
        private IRepository<ProductImage> prodcuctimgeRepository;
        private IRepository<MachineDetail> machinedetailRepository;
        private IRepository<Machine> machineRepository;
        private IRepository<MachineStockDetails> machineStockDetailsRepository;
        private IRepository<ProductPrice> productPriceRepository;
        private IRepository<Store> storeRepository;
        private GenerateCodeService generateCodeService;
        private MessageQueueService messageQueueService;
        private ActiveMQEvent activeMQEvent;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ProductService()
        {
            this.productRepository = new RepositoryBase<Product>();
            this.productCategoryRepository = new RepositoryBase<ProductCategory>();
            this.prodcuctimgeRepository = new RepositoryBase<ProductImage>();
            this.machinedetailRepository = new RepositoryBase<MachineDetail>();
            this.machineRepository = new RepositoryBase<Machine>();
            this.machineStockDetailsRepository = new RepositoryBase<MachineStockDetails>();
            this.productPriceRepository = new RepositoryBase<ProductPrice>();
            this.storeRepository = new RepositoryBase<Store>();
            generateCodeService = GenerateCodeService.SingleGenerateCodeService();
            messageQueueService = new MessageQueueService();
            activeMQEvent = new ActiveMQEvent();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(ProductDto entity, ref string errMsg)
        {
            bool addRes;
            try
            {
                productRepository.BeginTransaction();
                List<ProductImage> imgs = new List<ProductImage>();
                List<string> imgsList = entity.PhotoUrl.ToStrList();
                var productEntity = Mapper.Map<ProductDto, Product>(entity);
                //获取最新产品code
                string productCode = productEntity.ProductCode;
                generateCodeService.GetNextProdcutCode(ref productCode);
                productEntity.ProductCode = productCode;
                addRes = productRepository.Insert(productEntity) > 0;
                foreach (var item in imgsList)
                {
                    imgs.Add(new ProductImage
                    {
                        Url = item,
                        ProductId = productEntity.Id,
                        CreateUser = entity.CreateUser,
                        CredateTime = entity.CredateTime,
                        UpdateUser = entity.CreateUser,
                        UpdateTime = entity.CredateTime
                    });
                }
                if (addRes)
                {
                    entity.Id = productEntity.Id;
                    addRes = prodcuctimgeRepository.Insert(imgs, true) > 0;
                }
                else
                {
                    productRepository.TransactionRollback();
                }
                if (addRes)
                {
                    productRepository.TransactionCommit();
                }
                return addRes;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                productRepository.TransactionRollback();
                return false;
            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public string AddList(List<Product> products)
        {
            //所有的产品
            List<Product> productList = productRepository.Entities.Where(s => s.IsDelete != true).ToList();
            var productPricesList = productPriceRepository.Entities.Where(s => s.IsDelete != true).ToList();
            int count = 0;
            foreach (var item in products)
            {
                //ProductVId为空的不导入
                if (!string.IsNullOrEmpty(item.ProductVId))
                {
                    string productCode = item.ProductCode;

                    //根据ProductVId查找是更新还是新增商品
                    var product = productList.Where(p => p.ProductVId == item.ProductVId && p.StoreId == item.StoreId).FirstOrDefault();
                    if (product != null)
                    {
                        product.ProductVId = item.ProductVId;
                        product.ProductName_CH = item.ProductName_CH;
                        product.ProductCategoryId = item.ProductCategoryId;
                        product.UpdateUser = "API";
                        product.UpdateTime = DateTime.Now;
                        if (productRepository.Update(product) > 0)
                        {
                            count++;
                            productPriceRepository.BeginTransaction();
                            try
                            {
                                productPriceRepository.Delete(p => p.ProductId == product.Id);
                                var productPrices = item.ProductPrices.ToList();
                                //商品价格
                                if (productPrices != null && productPrices.Count > 0)
                                {
                                    productPrices.ForEach(p =>
                                    {
                                        p.Product = product;
                                    });
                                    int result = productPriceRepository.Insert(productPrices);

                                    //for (int i = 0; i < productPrices.Count; i++)
                                    //{
                                    //    var itemPri = productPrices[i];
                                    //var prices = productPricesList.Where(p => p.ProductWayVId == itemPri.ProductWayVId).FirstOrDefault();
                                    ////根据ProductWayVId查找是更新还是新增商品价格
                                    //if (prices != null)
                                    //{
                                    //    prices.Price = itemPri.Price;
                                    //    prices.Point = itemPri.Point;
                                    //    prices.SaleType = itemPri.SaleType;
                                    //    prices.UpdateUser = itemPri.UpdateUser;
                                    //    prices.UpdateTime = itemPri.UpdateTime;
                                    //    prices.Product = product;
                                    //    if (i + 1 == productPrices.Count)
                                    //    {
                                    //        productPriceRepository.Update(prices, true);
                                    //    }
                                    //    else
                                    //    {
                                    //        productPriceRepository.Update(prices, false);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //itemPri.Product = product;
                                    //if (i + 1 == productPrices.Count)
                                    //{
                                    //    productPriceRepository.Insert(itemPri, true);
                                    //}
                                    //else
                                    //{
                                    //    productPriceRepository.Insert(itemPri, false);
                                    //}
                                    //}
                                    //    }
                                }
                                productPriceRepository.TransactionCommit();
                            }
                            catch (Exception ex)
                            {
                                productPriceRepository.TransactionRollback();
                            }
                        }
                    }
                    else
                    {
                        generateCodeService.GetNextProdcutCode(ref productCode);
                        item.ProductCode = productCode;
                        if (productRepository.Insert(item) > 0)
                        {
                            count++;
                        }
                    }
                }
            }
            if (count <= 0)
            {
                throw new Exception("导入0条数据!");
            }
            else if (products.Count > count)
            {
                var result = new { Data = "", ErrorMsg = $"（{products.Count - count}条数据失败!）", Success = true };
                string resultStr = JsonConvert.SerializeObject(result);
                return resultStr;
            }
            else
            {
                List<MessageQueue> list = messageQueueService.Add("Product", "更新产品");
                activeMQEvent.SendMQMessageEvent += ActiveMQEvent_SendMQMessageEvent;
                activeMQEvent.SendMQMessage(list);
                return "true";
            }
        }
        /// <summary>
        /// 根据产品ID查询
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<ProductPriceDto> GetProductPrices(int productId)
        {
            Expression<Func<ProductPrice, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            ex = ex.And(t => t.ProductId == productId);
            var productPrices = productPriceRepository.GetEntities(ex).ToList();
            return Mapper.Map<List<ProductPrice>, List<ProductPriceDto>>(productPrices);
        }

        /// <summary>
        /// 设置商品价格
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="replenishmentUser"></param>
        /// <returns></returns>
        public bool SetProductPrice(ProductPriceDto entity)
        {
            bool IsAdd;
            List<ProductPrice> prices = new List<ProductPrice>();
            var proprice = new ProductPrice();
            List<ProductPrice> productPricesList = productPriceRepository.Entities.Where(p => p.ProductId == entity.Id).AsNoTracking().ToList();
            if (productPricesList.Count != 3)
                IsAdd = true;
            else
                IsAdd = false;
            if (entity.Point >= 0 || entity.Point == null)
            {
                proprice = new ProductPrice();
                proprice.Id = IsAdd ? entity.Id : productPricesList.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.Point).Id;
                proprice.ProductId = entity.Id;
                proprice.Point = entity.Point;
                proprice.Price = 0;
                proprice.SaleType = (int)SaleTypeEnum.Point;
                proprice.Status = entity.Point > 0 ? (int)ProPriceStatusEnum.Enabled : (int)ProPriceStatusEnum.NotEnabled;
                prices.Add(proprice);
            }
            if (entity.Price >= 0 || entity.Price == null)
            {
                proprice = new ProductPrice();
                proprice.Id = IsAdd ? proprice.Id : productPricesList.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.Money).Id;
                proprice.ProductId = entity.Id;
                proprice.Price = entity.Price;
                proprice.Point = 0;
                proprice.SaleType = (int)SaleTypeEnum.Money;
                proprice.Status = entity.Price > 0 ? (int)ProPriceStatusEnum.Enabled : (int)ProPriceStatusEnum.NotEnabled;
                prices.Add(proprice);
            }
            if (entity.PartPrice >= 0 || entity.PartPrice == null)
            {
                proprice = new ProductPrice();
                proprice.Id = IsAdd ? proprice.Id : productPricesList.FirstOrDefault(it => it.SaleType == (int)SaleTypeEnum.MoneyAndPoint).Id;
                proprice.ProductId = entity.Id;
                proprice.Price = entity.PartPrice;
                proprice.Point = entity.PartPoint;
                proprice.SaleType = (int)SaleTypeEnum.MoneyAndPoint;
                proprice.Status = entity.PartPoint > 0 ? (int)ProPriceStatusEnum.Enabled : (int)ProPriceStatusEnum.NotEnabled;
                prices.Add(proprice);
            }
            if (!IsAdd)
            {
                prices.ForEach(it =>
                {
                    it.UpdateUser = entity.UpdateUser;
                    it.UpdateTime = DateTime.Now;
                });
                bool updateRes = false;
                foreach (var item in prices)
                {
                    updateRes = productPriceRepository.Update(item, true, it => it.Point, it => it.Price, it => it.SaleType, it => it.Status, it => it.UpdateTime, it => it.UpdateUser) > 0;
                }
                return updateRes;
            }

            prices.ForEach(it =>
            {
                it.CreateUser = entity.UpdateUser;
                it.CredateTime = entity.UpdateTime;
            });
            return productPriceRepository.Insert(prices) > 0;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return productRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return productRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids, string currentuser)
        {
            int[] ida = ids.StrToIntArray();
            var ma = productRepository.Entities.Where(p => ida.Contains(p.Id));
            foreach (var prod in ma)
            {
                prod.IsDelete = true;
                prod.UpdateTime = DateTime.Now;
                prod.UpdateUser = currentuser;
            }
            bool isSuccess = productRepository.Update(ma) > 0;
            if (isSuccess)
            {
                List<MessageQueue> list = messageQueueService.Add("Product", "删除产品");
                activeMQEvent.SendMQMessageEvent += ActiveMQEvent_SendMQMessageEvent;
                activeMQEvent.SendMQMessage(list);
            }
            return isSuccess;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(ProductDto entity)
        {
            bool updateRes;
            try
            {
                productRepository.BeginTransaction();
                List<ProductImage> imgs = new List<ProductImage>();
                var productEntity = Mapper.Map<ProductDto, Product>(entity);
                if (entity.PhotoUrl != null)
                {
                    List<string> imgsList = entity.PhotoUrl.ToStrList();
                    foreach (var item in imgsList)
                    {
                        imgs.Add(new ProductImage
                        {
                            Url = item,
                            ProductId = productEntity.Id,
                            CreateUser = entity.UpdateUser,
                            CredateTime = entity.UpdateTime,
                            UpdateUser = entity.UpdateUser,
                            UpdateTime = entity.UpdateTime
                        });
                    }
                }
                updateRes = productRepository.Update(productEntity, true, x => x.ProductDetails_EN, x => x.ProductDetails_CH, x => x.Specification_CH, x => x.Specification_EN, x => x.ProductName_CH
                           , it => it.ProductName_EN, it => it.ProductCategoryId, it => it.ProductCode, it => it.UpdateTime, it => it.UpdateUser) > 0;
                if (updateRes)
                {
                    entity.Id = productEntity.Id;
                    //把之前的图片移除
                    var prodcuctImgList = prodcuctimgeRepository.GetEntities(x => x.ProductId == entity.Id && x.IsDelete == false).ToList();
                    if (prodcuctImgList != null && prodcuctImgList.Count != 0)
                    {
                        foreach (var item in prodcuctImgList)
                        {
                            item.IsDelete = true;
                        }
                        prodcuctimgeRepository.Update(prodcuctImgList, true);
                    }
                    //增加新的图片
                    if (imgs.Count > 0)
                    {
                        updateRes = prodcuctimgeRepository.Insert(imgs, true) > 0;
                    }
                }
                if (updateRes)
                {
                    List<MessageQueue> list = messageQueueService.Add("Product", "更新产品");
                    activeMQEvent.SendMQMessageEvent += ActiveMQEvent_SendMQMessageEvent;
                    activeMQEvent.SendMQMessage(list);
                    productRepository.TransactionCommit();
                }
                else
                {
                    productRepository.TransactionRollback();
                }
                return updateRes;
            }
            catch (Exception)
            {
                productRepository.TransactionRollback();
                return false;
            }
        }

        private Task<bool> ActiveMQEvent_SendMQMessageEvent(List<MessageQueue> list)
        {
            return Task.Run(async () =>
            {
                return await messageQueueService.SendQueueMessagesAsync(list);
            });
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public ProductDto Get(int id)
        {
            var productEntity = productRepository.Find(id);
            productEntity.ProductImages = productEntity.ProductImages.Where(p => !p.IsDelete).ToList();
            return Mapper.Map<Product, ProductDto>(productEntity);
        }
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public Product GetByProductCode(string productCode)
        {
            Expression<Func<Product, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete).And(it => it.ProductCode == productCode);
            var productEntity = productRepository.GetEntities(ex).FirstOrDefault();
            //  productEntity.ProductImages = productEntity.ProductImages.Where(p => !p.IsDelete).ToList();
            return productEntity;
        }
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<ProductDto> GetAll()
        {
            var productEntitys = productRepository.Entities.ToList();
            return Mapper.Map<List<Product>, List<ProductDto>>(productEntitys);
        }
        public List<Product> GetProductAll()
        {
            Expression<Func<Product, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var productList = productRepository.GetEntities(ex);
            return productList.ToList();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<ProductDto> GetProductGrid(TablePageParameter tpg = null, string name = "", DateTime? dateFrom = null, DateTime? dateTo = null, int typeId = 0, string productCode = "")
        {
            Expression<Func<Product, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
                ex = ex.And(t => t.ProductName_CH.Contains(name) || t.ProductName_EN.Contains(name) || t.ProductCode.Contains(name));
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
            if (!string.IsNullOrEmpty(productCode))
                ex = ex.And(t => t.ProductCode == productCode);
            if (typeId > 0)
                ex = ex.And(t => t.ProductCategoryId == typeId);

            var productList = productRepository.GetEntities(ex);
            var storeList = storeRepository.GetEntities(p => !p.IsDelete);
            List<ProductDto> result = new List<ProductDto>();
            if (tpg == null)
            {
                 result = Mapper.Map<List<Product>, List<ProductDto>>(productList.ToList());
            }
            else
            {
                 result = Mapper.Map<List<Product>, List<ProductDto>>(GetTablePagedList(productList, tpg));
                //if (result != null && result.Count > 0 && storeList != null && storeList.Count() > 0)
                //{
                //    var storeLists = storeList.ToList();
                //    result.ForEach(p =>
                //    {
                //        p.StoreName = storeLists.Where(s => s.Id == p.StoreId).FirstOrDefault() == null ? "" : storeLists.Where(s => s.Id == p.StoreId).FirstOrDefault().Name;
                //    });
                //    return result;
                //}
                //return result;
            }
            if (result != null && result.Count > 0 && storeList != null && storeList.Count() > 0)
            {
                var storeLists = storeList.ToList();
                result.ForEach(p =>
                {
                    p.StoreName = storeLists.Where(s => s.Id == p.StoreId).FirstOrDefault() == null ? "" : storeLists.Where(s => s.Id == p.StoreId).FirstOrDefault().Name;
                });
                return result;
            }
            return result;
        }

        /// <summary>
        /// 验证实体是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExist(string name, int? id)
        {
            Expression<Func<Product, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
            {
                ex = ex.And(t => t.ProductCode == name);
            }
            if (id.HasValue)
            {
                ex = ex.And(t => t.Id != id.Value);
            }
            return productRepository.IsExist(ex);
        }

        public bool InportProduct()
        {
            //ProductRedeem a = new ProductRedeem();
            return true;
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 产品价格转换
        /// </summary>
        /// <param name="productPriceDtos"></param>
        /// <returns></returns>
        public List<ProductPriceAPIDto> ConvertProductPriceDto(IEnumerable<ProductPriceDto> productPriceDtos)
        {
            return productPriceDtos.Select(zw =>
             {
                 ProductPriceAPIDto productPriceAPIDto = new ProductPriceAPIDto() { Point = zw.Point.HasValue ? zw.Point : 0, Price = zw.Price.HasValue ? zw.Price : 0, SaleType = zw.SaleType };
                 if (zw.SaleType == (int)SaleTypeEnum.Money)
                 {
                     if (zw.Price.HasValue && zw.Price.Value > 0)
                     {
                         productPriceAPIDto.IsEffective = true;
                     }
                     else
                     {
                         productPriceAPIDto.IsEffective = false;
                     }
                 }
                 else if (zw.SaleType == (int)SaleTypeEnum.MoneyAndPoint)
                 {
                     if (zw.Price.HasValue && zw.Point.HasValue && zw.Price.Value > 0 && zw.Point.Value > 0)
                     {
                         productPriceAPIDto.IsEffective = true;
                     }
                     else
                     {
                         productPriceAPIDto.IsEffective = false;
                     }
                 }
                 else if (zw.SaleType == (int)SaleTypeEnum.Point)
                 {
                     if (zw.Point.HasValue && zw.Point.Value > 0)
                     {
                         productPriceAPIDto.IsEffective = true;
                     }
                     else
                     {
                         productPriceAPIDto.IsEffective = false;
                     }
                 }
                 return productPriceAPIDto;
             }).ToList();
        } /// <summary>
          /// 产品价格转换
          /// </summary>
          /// <param name="productPriceDtos"></param>
          /// <returns></returns>
        public List<ProductPriceAPIDto> ConvertProductPriceDto(IEnumerable<ProductPrice> productPriceDtos)
        {
            List<ProductPriceAPIDto> productPriceAPIDtos = new List<ProductPriceAPIDto>();

            productPriceDtos.ToList().ForEach(zw =>
           {
               ProductPriceAPIDto productPriceAPIDto = new ProductPriceAPIDto() { Point = zw.Point.HasValue ? zw.Point : 0, Price = zw.Price.HasValue ? zw.Price : 0, SaleType = zw.SaleType, ProductWayVId = zw.ProductWayVId };
               if (zw.SaleType == (int)SaleTypeEnum.Money)
               {
                   if (zw.Price.HasValue && zw.Price.Value > 0)
                   {
                       productPriceAPIDto.IsEffective = true;
                   }
                   else
                   {
                       productPriceAPIDto.IsEffective = false;
                   }
               }
               else if (zw.SaleType == (int)SaleTypeEnum.MoneyAndPoint)
               {
                   if (zw.Price.HasValue && zw.Point.HasValue && zw.Price.Value > 0 && zw.Point.Value > 0)
                   {
                       productPriceAPIDto.IsEffective = true;
                   }
                   else
                   {
                       productPriceAPIDto.IsEffective = false;
                   }
               }
               else if (zw.SaleType == (int)SaleTypeEnum.Point)
               {
                   if (zw.Point.HasValue && zw.Point.Value > 0)
                   {
                       productPriceAPIDto.IsEffective = true;
                   }
                   else
                   {
                       productPriceAPIDto.IsEffective = false;
                   }
               }
               productPriceAPIDtos.Add(productPriceAPIDto);
               //   return productPriceAPIDto;
           });
            if (productPriceAPIDtos.Count < 3)
            {
                List<int> saleTypeList = new List<int>();

                foreach (object e in Enum.GetValues(typeof(SaleTypeEnum)))
                {
                    var productPrice = productPriceAPIDtos.Where(p => p.SaleType == (int)e);
                    if (!(productPrice != null && productPrice.Count() > 0))
                    {
                        saleTypeList.Add((int)e);
                    }
                }
                if (saleTypeList.Count() > 0)
                {
                    foreach (var item in saleTypeList)
                    {
                        productPriceAPIDtos.Add(new ProductPriceAPIDto
                        {
                            Price = 0,
                            Point = 0,
                            SaleType = item,
                            IsEffective = false,
                            ProductWayVId = ""
                        });
                    }
                }
            }
            return productPriceAPIDtos;
        }
        #endregion
    }
}
