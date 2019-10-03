using System.Data.Entity;
using VendM.DAL.SysPara;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Order;
using VendM.Model.DataModel.Product;

namespace VendM.DAL
{
    public class VendMDbContext : DbContext
    {
        public VendMDbContext() : base("name=VendMContext")
        {
            //延迟加载
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.AutoDetectChangesEnabled = true;
            //Database.SetInitializer(new CreateDatabaseIfNotExists<VendMDbContext>());
            Database.SetInitializer<VendMDbContext>(null);
        }
        /// <summary>
        /// 读写分离
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        /// <param name="dbtype"></param>
        public VendMDbContext(string nameOrConnectionString, string dbtype) : base(dbtype == DBType.Read ? "name=VendMContextRead" : "name=VendMContext")
        {

        }
        public VendMDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbSet<User> User { get; set; }
        public DbSet<SysMenu> SysMenu { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRights> UserRights { get; set; }
        public DbSet<SysButton> SysButton { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<SysMenuAction> SysMenuAction { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Advertisement> Advertisement { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Machine> Machine { get; set; }
        public DbSet<PayMent> PayMent { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<MachineStock> MachineStock { get; set; }
        public DbSet<MachineStockDetails> MachineStockDetails { get; set; }
        public DbSet<InventoryChangeLog> InventoryChangeLog { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<MachineDetail> MachineDetail { get; set; }
        public DbSet<ReplenishmentUser> ReplenishmentUser { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        public DbSet<APILog> APILog { get; set; }
        public DbSet<Replenishment> Replenishment { get; set; }
        public DbSet<ReplenishmentDetail> ReplenishmentDetail { get; set; }
        public DbSet<MessageQueue> MessageQueue { get; set; }
        public DbSet<APP> APP { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            modelBuilder.Entity<SysMenu>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<UserRole>();
            modelBuilder.Entity<SysButton>();
            modelBuilder.Entity<SysMenuAction>();
            modelBuilder.Entity<UserRights>();
            modelBuilder.Entity<Log>();
            modelBuilder.Entity<Advertisement>();
            modelBuilder.Entity<Video>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<ProductCategory>();
            modelBuilder.Entity<ProductImage>();
            modelBuilder.Entity<Store>();
            modelBuilder.Entity<Machine>();
            modelBuilder.Entity<PayMent>();
            modelBuilder.Entity<Stock>();
            modelBuilder.Entity<MachineStock>();
            modelBuilder.Entity<MachineStockDetails>();
            modelBuilder.Entity<InventoryChangeLog>();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<OrderDetails>();
            modelBuilder.Entity<Transaction>();
            modelBuilder.Entity<MachineDetail>();
            modelBuilder.Entity<ReplenishmentUser>();
            modelBuilder.Entity<ProductPrice>();
            modelBuilder.Entity<APILog>();
            modelBuilder.Entity<Replenishment>();
            modelBuilder.Entity<ReplenishmentDetail>();
            modelBuilder.Entity<MessageQueue>();
            modelBuilder.Entity<APP>();
        }
    }
}
