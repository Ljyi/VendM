using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VendM.Core;

namespace VendM.Web.BaseApplication
{
    public class ServicedController<TService> : BaseController where TService : new()
    {
        private static readonly object locker = new object();
        public ServicedController()
        {
        }
        private TService _service;
        /// <summary>
        /// 用于业务端方法调用
        /// </summary>
        public TService Service
        {
            get
            {
                lock (locker)
                {
                    if (_service == null)
                    {
                        _service = new TService();
                    }
                }
                return _service;
            }
            set
            {
                _service = value;
            }
        }

        /// <summary>
        /// 根据枚举，把枚举自定义特性EnumDisplayNameAttribut的Display属性值撞到SelectListItem中
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static List<SelectListItem> GetSelectList(Type enumType, bool isAll = false)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (isAll)
            {
                selectList.Add(new SelectListItem() { Text = "-请选择-", Value = "-1" });
            }
            foreach (object e in Enum.GetValues(enumType))
            {
                selectList.Add(new SelectListItem() { Text = EnumExtension.GetDescription(e), Value = ((int)e).ToString() });
            }
            return selectList;
        }
    }
}