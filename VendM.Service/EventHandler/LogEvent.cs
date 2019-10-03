using System.Threading.Tasks;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto.Log;

namespace VendM.Service.EventHandler
{
    public delegate Task<bool> DelegateAddLog(APILogDto log);
    /// <summary>
    /// 日志
    /// </summary>
    public class LogEvent
    {
        //声明委托
        public event DelegateAddLog AddLogEvent;
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log"></param>
        public void AddLog(APILogDto log)
        {
            if (AddLogEvent != null)
            {
                AddLogEvent(log);
            }
        }
    }
}
