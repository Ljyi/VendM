using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Product;
using VendM.Model.DataModelDto.Product;

namespace VendM.Service
{

    /// <summary>
    /// 业务逻辑
    /// </summary>
    public class ProdcuctImgeService : BaseService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        private IRepository<ProductImage> prodcuctImgeRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ProdcuctImgeService()
        {
            this.prodcuctImgeRepository = new RepositoryBase<ProductImage>();
        }
        #region 增加/删除/修改
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Add(ProductImgeDto entity)
        {
            var prodcuctImgeEntity = Mapper.Map<ProductImgeDto, ProductImage>(entity);
            return prodcuctImgeRepository.Insert(prodcuctImgeEntity) > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(int id)
        {
            return prodcuctImgeRepository.Delete(id) > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">待删除的id列表</param>
        /// <returns>返回删除的数量</returns>
        public bool Delete(ProductImgeDto entity)
        {
            var prodcuctImgeEntity = Mapper.Map<ProductImgeDto, ProductImage>(entity);
            return prodcuctImgeRepository.Update(prodcuctImgeEntity, true, x => x.Url, x => x.UpdateTime, x => x.UpdateUser, x => x.IsDelete) > 0;
        }
		/// <summary>
		/// 删除图片
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public bool DeleteImg(string url)
		{
			if (string.IsNullOrEmpty(url))
				return false;

			Expression<Func<ProductImage, bool>> ex = t => true;
			ex = ex.And(t => !t.IsDelete);
			ex = ex.And(t => t.Url.Contains(url));

			var prodcuctImge = prodcuctImgeRepository.GetEntities(ex, false).FirstOrDefault();
			if (prodcuctImge == null)
				return false;

			prodcuctImge.IsDelete = true;
			return prodcuctImgeRepository.Update(prodcuctImge, true, x => x.Url, x => x.UpdateTime, x => x.UpdateUser, x => x.IsDelete) > 0;
		}
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新成功返回true，否则返回false</returns>
        public bool Update(ProductImgeDto entity)
        {
            var prodcuctImgeEntity = Mapper.Map<ProductImgeDto, ProductImage>(entity);
            return prodcuctImgeRepository.Update(prodcuctImgeEntity) > 0;
        }
        #endregion

        #region 单个查询/批量查询
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>返回单一实体</returns>
        public ProductImgeDto Get(int id)
        {
            var prodcuctImgeEntity = prodcuctImgeRepository.Find(id);
            return Mapper.Map<ProductImage, ProductImgeDto>(prodcuctImgeEntity);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>返回所有实体</returns>
        public List<ProductImgeDto> GetAll()
        {
            var prodcuctImgeEntitys = prodcuctImgeRepository.Entities.ToList();
            return Mapper.Map<List<ProductImage>, List<ProductImgeDto>>(prodcuctImgeEntitys);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页集合</returns>
        public List<ProductImgeDto> GetProdcuctImgeGrid(TablePageParameter tpg)
        {
            Expression<Func<ProductImage, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var prodcuctImgeList = prodcuctImgeRepository.GetEntities(ex, false);
            if (tpg == null)
            {
                return Mapper.Map<List<ProductImage>, List<ProductImgeDto>>(prodcuctImgeList.ToList());
            }
            else
            {
                return Mapper.Map<List<ProductImage>, List<ProductImgeDto>>(GetTablePagedList(prodcuctImgeList, tpg));
            }
        }
        #endregion
    }
}
