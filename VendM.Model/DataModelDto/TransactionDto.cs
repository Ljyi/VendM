namespace VendM.Model.DataModelDto
{
    /// <summary>
    /// 交易实体
    /// </summary>
    public class TransactionDto : BaseDto
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns>返回新建的实体</returns>
        public static TransactionDto New()
        {
            TransactionDto transactionDto = new TransactionDto()
            {
            };
            return transactionDto;
        }
        #region 需持久化属性
        /// <summary>
		///	订单号
		/// </summary>
        public string OrderNo { get; set; }
        /// <summary>
		///	交易信息
		/// </summary>
        public string TransactionInfo { get; set; }
        #endregion
    }
}
