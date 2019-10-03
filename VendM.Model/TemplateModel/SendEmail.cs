using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendM.Model.DataModel;
using VendM.Model.DataModelDto;

namespace VendM.Model.TemplateModel
{
     public class SendEmail
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string MachineNo { get; set; }
        public string Address { get; set; }
        public List<Replenishment> ReplenishmentLis { get; set; }
    }
}
