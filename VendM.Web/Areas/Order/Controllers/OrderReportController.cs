using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using VendM.Core;
using VendM.Model.DataModelDto.Order;
using VendM.Service;
using VendM.Web.BaseApplication;
using VendM.Web.Models;

namespace VendM.Web.Areas.Order.Controllers
{
    public class OrderReportController : ServicedController<OrderService>
    {
        // GET: Order/OrderReport
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 主页报表
        /// </summary>
        /// <returns></returns>
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult MainReportList(string startdate="", string enddate="", string storeno = "", string machineno = "")
        {

            var resData =new  ResultJsonInfo();
            resData.Success = false;
            try
            {
                //Common.GetLongDateTimeString(Common.GetCurrentMinDate(startdate))
                DateTime startD = Common.GetCurrentMinDate(DateTime.Now.AddDays(-7));
                DateTime endD = Common.GetCurrentMinDate(DateTime.Now);
                var isMonth = false;
                List<object> rankingList = null;
                //x轴是否是月份
                if (!string.IsNullOrEmpty(startdate) )
                   startD = DateTime.Parse(startdate);
                    
                    
                if (!string.IsNullOrEmpty(enddate))
                    endD = DateTime.Parse(enddate);

                //isMonth = Common.GetDateDiff(DateInterval.Day, startD, endD) >= 30;

                //獲取排行榜
                rankingList = RankingList(startD, endD).ToList();


                var list = Service.GetOrderMainViews(storeno, machineno, Common.GetLongDateTimeString(startD), Common.GetLongDateTimeString(endD));
                if (list==null || list.Count <=0)
                    return Json(resData, JsonRequestBehavior.AllowGet);

                //List<OrderMainView> newList = list;

                //if (isMonth)
                //{
                //    newList = list.GroupBy(x => new { x.Year, x.Month }).Select(it => new OrderMainView
                //    {
                //        Year = it.Key.Year,
                //        Month = it.Key.Month,
                //        Amount = it.Sum(s => s.Amount),
                //        OrderCount = it.Count()
                //    }).ToList();
                    
                //}


                var  data = DateOrMonthList(startD, endD, list);

                object jsondata = new
                {
                    categories = data.Keys.ToList(),
                    series = data.Values.ToList(),
                    monthamount =  list.Sum(it => it.Amount),
                    monthquantity = list.Sum(it => it.OrderCount),
                    todayquantity = list.Where(it => it.Day == DateTime.Now.Day).Sum(it => it.OrderCount),
                    todayamount = list.Where(it => it.Day == DateTime.Now.Day).Sum(it => it.Amount),
                    rankingList = rankingList.Count <=0 ? new List<object>() : rankingList

                };
                resData.Success = true;
                resData.Data = JsonConvert.SerializeObject(jsondata);

                return Json(resData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                resData.Success = false;
                resData.ErrorMsg = "出現異常";
                return Json(resData, JsonRequestBehavior.AllowGet);
            }

        }

        public IEnumerable<object> RankingList(DateTime? startDate, DateTime? endDate)
        {
            var list = Service.GetStoreOrderViewGrid(null, "", "", "", startDate, endDate);
            var resData = list.GroupBy(it => new {it.StoreNo, it.StoreName}).OrderBy(it=>it.Sum(o => o.TotalAmount)).Select(it => new
            {
                it.Key.StoreName,
                it.Key.StoreNo,
                Amount = it.Sum(o=>o.TotalAmount).ToRound()
            });
            return resData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, decimal> DateOrMonthList(DateTime startD, DateTime endD, List<OrderMainView> ovlis, bool ismonth = false)
        {
            SortedDictionary<string, decimal> dic = new SortedDictionary<string, decimal>();
            if (ismonth)
            {
                for (DateTime i = startD; i < endD; i.AddMonths(1))
                {
                    dic.Add(startD.ToString("yyyy-M"), 0);
                }

                foreach (var it in ovlis)
                {
                    dic[it.Year + "-" + it.Month] =  it.Amount.ToRound();
                }

            }
            else
            {
                for (DateTime i = startD; i < endD; i=i.AddDays(1))
                {
                    dic.Add(i.ToString("yyyy-MM-dd"), 0);
                }

                foreach (var it in ovlis)
                {
                    dic[it.CredateTime.ToString("yyyy-MM-dd")] = it.Amount.ToRound();
                }
            }
            
            return dic;
        }

    }
}