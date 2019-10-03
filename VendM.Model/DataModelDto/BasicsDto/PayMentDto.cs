using System;
using System.ComponentModel.DataAnnotations;
using VendM.Model.DataModelDto;

namespace VendM.Model.DataModelDto
{
    public class PayMentDto : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string MID { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }
        /// <summary>
        /// 支付Code
        /// </summary>
        public string PaymentCode { get; set; }
        /// <summary>
        /// 费率（银行）
        /// </summary>
        public string Charge { get; set; }
        /// <summary>
        /// 手续费率（商家）
        /// </summary>
        public string Fee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }
        ///// <summary>
        ///// 结算时间
        ///// </summary>
        //[Required]
        //public DateTime SettlingTime { get; set; }
    }
}
