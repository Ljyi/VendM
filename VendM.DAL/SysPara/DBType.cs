using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendM.DAL.SysPara
{
    /// <summary>
    /// 数据库类型，主库（可读写）、备库（可读）
    /// </summary>
    public class DBType
    {   /// <summary>
        /// 主库，可读写
        /// </summary>
        public const string Main = "MAIN";
        /// <summary>
        /// 备库，只能读不能写
        /// </summary>
        public const string Read = "READ";
    }
}
