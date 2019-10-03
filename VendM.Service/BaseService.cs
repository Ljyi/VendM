using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using VendM.Core;

namespace VendM.Service
{
    public class BaseService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="gp"></param>
        /// <returns></returns>
        public List<T> GetTablePagedList<T>(IQueryable<T> entitys, TablePageParameter gp)
        {
            gp.PageParameterInit(entitys);
            if (!gp.NotSort)
            {
                if (string.IsNullOrEmpty(gp.SortName))
                {
                    entitys = entitys.OrderBy("Id");
                }
                else
                {
                    try
                    {
                        entitys = entitys.OrderBy(gp.SortName + " " + gp.SortOrder); //排序
                    }
                    catch (Exception)
                    {
                        entitys = entitys.OrderBy("Id");
                    }
                }
            }
            entitys = entitys.Skip(gp.Offset).Take(gp.PageSize); //分页
            return entitys.ToList();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="gp"></param>
        /// <returns></returns>
        public IEnumerable<T> GetTablePagedList<T>(IEnumerable<T> entitys, TablePageParameter gp)
        {
            gp.PageParameterInit(entitys);
            if (!gp.NotSort)
            {
                if (string.IsNullOrEmpty(gp.SortName))
                {
                    entitys = entitys.OrderBy("Id");
                }
                else
                {
                    try
                    {
                        entitys = entitys.OrderBy(gp.SortName + " " + gp.SortOrder); //排序
                    }
                    catch (Exception)
                    {
                        entitys = entitys.OrderBy("Id");
                    }
                }
            }
            entitys = entitys.Skip(gp.Offset).Take(gp.PageSize); //分页
            return entitys.ToList();
        }

        /// <summary>
        /// 分页处理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <param name="pageSize"></param>
        /// <param name="singlePageList"></param>
        public void ToPagingProcess<TEntity>(IEnumerable<TEntity> item, int pageSize, Action<IEnumerable<TEntity>> singlePageList)
        {
            if (item != null && item.Count() > 0)
            {
                var count = item.Count();
                var pages = item.Count() / pageSize;
                if (count % pageSize > 0)
                {
                    pages += 1;
                }
                for (int i = 1; i <= pages; i++)
                {
                    var currentPageItem = item.Skip((i - 1) * pageSize).Take(pageSize);
                    singlePageList(currentPageItem);
                }
            }
        }
    }
}
