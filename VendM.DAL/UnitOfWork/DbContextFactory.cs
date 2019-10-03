using System.Runtime.Remoting.Messaging;

namespace VendM.DAL.UnitOfWork
{
    public class DbContextFactory
    {
        public static VendMDbContext GetCurrentContext()
        {
            VendMDbContext baseDbContext = CallContext.GetData("VendMContext") as VendMDbContext;
            if (baseDbContext == null)
            {
                baseDbContext = new VendMDbContext();
                CallContext.SetData("VendMContext", baseDbContext);
            }
            return baseDbContext;
        }
    }
}
