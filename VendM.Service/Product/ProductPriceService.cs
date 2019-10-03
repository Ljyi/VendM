using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendM.DAL.UnitOfWork;
using VendM.Model.DataModel.Product;

namespace VendM.Service
{
    public class ProductPriceService : BaseService
    { /// <summary>
      /// 数据仓储
      /// </summary>
        private IRepository<ProductPrice> productPriceRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="repository">数据访问仓储</param>
        public ProductPriceService()
        {
            this.productPriceRepository = new RepositoryBase<ProductPrice>();
        }
        /// <summary>
        /// 根据产品ID查询
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<ProductPrice> GetProductPrices(int productId)
        {
            Expression<Func<ProductPrice, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            ex = ex.And(t => t.ProductId == productId);
            return productPriceRepository.GetEntities(ex).ToList();
        }
    }
}
