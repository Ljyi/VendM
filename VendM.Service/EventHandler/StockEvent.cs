using System.Threading.Tasks;
using VendM.Model.DataModel;

namespace VendM.Service.EventHandler
{
    public class StockEvent
    {
        public delegate void DelegateStock(MachineStock machineStock);
        //声明委托
        public event DelegateStock CheckStockEvent;
        /// <summary>
        /// 检查库存
        /// </summary>
        /// <param name="log"></param>
        public void CheckStock(MachineStock machineStock)
        {
            if (CheckStockEvent != null)
            {
                CheckStockEvent(machineStock);
            }
        }
    }
}
