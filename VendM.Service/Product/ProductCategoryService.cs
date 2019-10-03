using AutoMapper;
using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;

namespace VendM.Service
{

    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class ProductCategoryService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<ProductCategory> productCategoryRepository;
        private GenerateCodeService generateCodeService;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ProductCategoryService()
        {
            this.productCategoryRepository = new RepositoryBase<ProductCategory>();
            this.generateCodeService = GenerateCodeService.SingleGenerateCodeService();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(ProductCategoryDto entity, ref string errMsg)
        {
            try
            {
                var productCategoryEntity = Mapper.Map<ProductCategoryDto, ProductCategory>(entity);
                return productCategoryRepository.Insert(productCategoryEntity) > 0;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return productCategoryRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public int Delete(IEnumerable<int> ids)
        {
            return productCategoryRepository.Delete(ids.ToArray());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(ProductCategoryDto entity)
        {
            var productCategoryEntity = Mapper.Map<ProductCategoryDto, ProductCategory>(entity);
            List<string> update = new List<string>{"CategoryCode",
                "CategoryName_CN", "CategoryName_EN", "UpdateTime", "UpdateUser"};
            return productCategoryRepository.Update(productCategoryEntity, update) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public ProductCategoryDto Get(int id)
        {
            var productCategoryEntity = productCategoryRepository.Find(id);
            return Mapper.Map<ProductCategory, ProductCategoryDto>(productCategoryEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<ProductCategoryDto> GetAll()
        {
            var productCategoryEntitys = productCategoryRepository.Entities.ToList();
            return Mapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategoryEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<ProductCategoryDto> GetProductCategoryGrid(Core.TablePageParameter tpg = null, string name = "")
        {
            Expression<Func<ProductCategory, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(name))
                ex = ex.And(t => t.CategoryName_CN.Contains(name) || t.CategoryName_EN.Contains(name));
            var productCategoryList = productCategoryRepository.GetEntities(ex);
            if (tpg == null)
            {
                return Mapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(productCategoryList.ToList());
            }
            else
            {
                return Mapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(GetTablePagedList(productCategoryList, tpg));
            }
        }

        /// <summary>
        /// 验证实体是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExist(string code, int? id)
        {
            Expression<Func<ProductCategory, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            if (!string.IsNullOrEmpty(code))
            {
                ex = ex.And(t => t.CategoryCode == code);
            }
            if (id.HasValue)
            {
                ex = ex.And(t => t.Id != id.Value);
            }
            return productCategoryRepository.IsExist(ex);
        }

        /// <summary>
        /// 批量插入分类
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public string AddList(List<ProductCategory> productCategoryList)
        {
            List<ProductCategory> productCategoryVIdList = productCategoryRepository.Entities.Where(s => s.IsDelete != true && s.ProductCategoryVId.HasValue).ToList();
            int count = 0;
            foreach (var item in productCategoryList)
            {
                //根据API的分类ID查找有没有重复的数据
                if (productCategoryVIdList == null ? true : !productCategoryVIdList.Any(p => p.ProductCategoryVId == item.ProductCategoryVId.Value))
                {
                    string categoryCode = item.CategoryCode;
                    generateCodeService.GetNextProductCategoryCode(ref categoryCode);
                    item.CategoryCode = categoryCode;
                    if (productCategoryRepository.Insert(item) > 0)
                    {
                        count++;
                    };
                }
                else
                {
                    var productCategory = productCategoryVIdList.Where(p => p.ProductCategoryVId == item.ProductCategoryVId.Value).FirstOrDefault();
                    productCategory.UpdateUser = "API";
                    productCategory.UpdateTime = DateTime.Now;
                    productCategory.CategoryName_CN = item.CategoryName_CN;
                    productCategory.CategoryName_EN = item.CategoryName_EN;
                    if (productCategoryRepository.Update(productCategory) > 0)
                    {
                        count++;
                    };
                }
            }
            if (count <= 0)
            {
                throw new Exception("导入0条数据!");
            }
            else if (productCategoryList.Count > count)
            {
                var result = new { Data = "", ErrorMsg = $"（{productCategoryList.Count - count}条数据失败!）", Success = true };
                string resultStr = JsonConvert.SerializeObject(result);
                return resultStr;
            }
            else
            {
                return "true";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(int[] ids, string currentuser)
        {
            var ma = productCategoryRepository.Entities.Where(p => ids.Contains(p.Id));
            foreach (var prod in ma)
            {
                prod.IsDelete = true;
                prod.UpdateTime = DateTime.Now;
                prod.UpdateUser = currentuser;
            }
            return productCategoryRepository.Update(ma) > 0;
        }
        #endregion
    }
}
