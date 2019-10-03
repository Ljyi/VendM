using System;
using System.Collections.Generic;
using VendM.DAL;
using VendM.Model.DataModel;

namespace ConsoleApp
{
    public class ManagerInitializer
    {
        VendMDbContext context = null;
        public ManagerInitializer()
        {
            context = new VendMDbContext();
        }
        public void Seed()
        {
            try
            {
                //初始化用户
                InitializerUser(context);
                //初始化菜单
                InitializerSysMenu(context);
                //初始化角色
                InitializerRole(context);
                //初始化按钮
                InitializerSysButton(context);
                //初始化用户角色
                InitializerUserRole(context);
                //初始化功能
                InitializerSysFunction(context);
                //初始化用户权限
                InitializerUserRights(context);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }
        /// <summary>
        /// 初始用户
        /// </summary>
        /// <param name="context"></param>
        public void InitializerUser(VendMDbContext context)
        {
            List<User> userList = new List<User>() {
                new User()
                {
                    Email = "979671716@qq.com",
                    LogingName = "ljy",
                    Password = "123",
                    UserName = "李军毅",
                    CreateUser = "李军毅",
                    CredateTime = DateTime.Now
                },
                new User()
                {
                    Email = "979671716@qq.com",
                    LogingName = "Admin",
                    Password = "123456",
                    UserName = "管理员",
                    CreateUser = "李军毅",
                    CredateTime = DateTime.Now
                },
            };
            userList.ForEach(user =>
            {
                context.User.Add(user);
            });
            context.SaveChanges();
        }
        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="context"></param>
        public void InitializerSysMenu(VendMDbContext context)
        {
            List<SysMenu> sysMenuList = new List<SysMenu>();

            //系统管理
            SysMenu sysMenu = new SysMenu()
            {
                MenuName = "系统管理",
                MenuCode = "System",
                Url = "/SystemManager",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "icon wb-settings",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysUserMenu = new SysMenu()
            {
                MenuName = "用户管理",
                MenuCode = "S-001",
                Url = "/System/User/Index",
                ParentId = 1,
                MenuLevel = 1,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-cog fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysRoleMenu = new SysMenu()
            {
                MenuName = "角色管理",
                MenuCode = "S-002",
                Url = "/System/Role/Index",
                ParentId = 1,
                MenuLevel = 1,
                SortNumber = 2,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysFunctionMenu = new SysMenu()
            {
                MenuName = "功能管理",
                MenuCode = "S-003",
                Url = "/System/Function/Index",
                ParentId = 1,
                MenuLevel = 1,
                SortNumber = 3,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysButtonMenu = new SysMenu()
            {
                MenuName = "按钮管理",
                MenuCode = "S-004",
                Url = "/System/Button/Index",
                ParentId = 1,
                MenuLevel = 1,
                SortNumber = 4,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysMenuMenu = new SysMenu()
            {
                MenuName = "菜单管理",
                MenuCode = "S-005",
                Url = "/System/Menu/Index",
                ParentId = 1,
                MenuLevel = 1,
                SortNumber = 5,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysMenu);
            sysMenuList.Add(sysUserMenu);
            sysMenuList.Add(sysRoleMenu);
            sysMenuList.Add(sysFunctionMenu);
            sysMenuList.Add(sysButtonMenu);
            sysMenuList.Add(sysMenuMenu);
            //基础管理
            SysMenu sysBasicsMenu = new SysMenu()
            {
                MenuName = "基础管理",
                MenuCode = "Basics",
                Url = "/Basics",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-building fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysStoreMenu = new SysMenu()
            {
                MenuName = "门店管理",
                MenuCode = "B-001",
                Url = "Basics/Store/Index",
                ParentId = 7,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysPaymentMenu = new SysMenu()
            {
                MenuName = "支付管理",
                MenuCode = "B-002",
                Url = "/Basics/Payment/Index",
                ParentId = 7,
                MenuLevel = 0,
                SortNumber = 2,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysMachineMenu = new SysMenu()
            {
                MenuName = "设备管理",
                MenuCode = "B-003",
                Url = "/Basics/Machine/Index",
                ParentId = 7,
                MenuLevel = 0,
                SortNumber = 3,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysMachineDetailMenu = new SysMenu()
            {
                MenuName = "设备通道",
                MenuCode = "B-004",
                Url = "/Basics/MachineDetail/Index",
                ParentId = 7,
                MenuLevel = 0,
                SortNumber = 4,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysReplenishmentUserMenu = new SysMenu()
            {
                MenuName = "补货员管理",
                MenuCode = "B-005",
                Url = "/Basics/ReplenishmentUser/Index",
                ParentId = 7,
                MenuLevel = 0,
                SortNumber = 5,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysBasicsMenu);
            sysMenuList.Add(sysStoreMenu);
            sysMenuList.Add(sysPaymentMenu);
            sysMenuList.Add(sysMachineMenu);
            sysMenuList.Add(sysMachineDetailMenu);
            sysMenuList.Add(sysReplenishmentUserMenu);
            //商品管理
            SysMenu sysProductMenu = new SysMenu()
            {
                MenuName = "商品管理",
                MenuCode = "Product",
                Url = "/Product",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-product-hunt fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysProductCategoryMenu = new SysMenu()
            {
                MenuName = "商品分类",
                MenuCode = "P-001",
                Url = "/Product/ProductCategory/Index",
                ParentId = 13,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysProductProductMenu = new SysMenu()
            {
                MenuName = "商品列表",
                MenuCode = "P-002",
                Url = "/Product/Product/Index",
                ParentId = 13,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-cog fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysProductMenu);
            sysMenuList.Add(sysProductCategoryMenu);
            sysMenuList.Add(sysProductProductMenu);
            //订单管理
            SysMenu sysOrderMenu = new SysMenu()
            {
                MenuName = "订单管理",
                MenuCode = "Order",
                Url = "/Order",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-table fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysOrderOrderMenu = new SysMenu()
            {
                MenuName = "订单列表",
                MenuCode = "O-001",
                Url = "/Order/Order/Index",
                ParentId = 16,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysStoreOrderMenu = new SysMenu()
            {
                MenuName = "订单记录",
                MenuCode = "O-002",
                Url = "/Order/Order/StoreOrder",
                ParentId = 16,
                MenuLevel = 0,
                SortNumber = 2,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysOrderMenu);
            sysMenuList.Add(sysOrderOrderMenu);
            sysMenuList.Add(sysStoreOrderMenu);
            //库存管理
            SysMenu sysStockMenu = new SysMenu()
            {
                MenuName = "库存管理",
                MenuCode = "Stock",
                Url = "/Stock",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-desktop fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysMachineStockMenu = new SysMenu()
            {
                MenuName = "库存列表",
                MenuCode = "S-001",
                Url = "/Stock/MachineStock/Index",
                ParentId = 19,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysStockMenu);
            sysMenuList.Add(sysMachineStockMenu);
            //广告管理
            SysMenu sysAdvertiseMentMenu = new SysMenu()
            {
                MenuName = "广告管理",
                MenuCode = "AdvertiseMent",
                Url = "/AdvertiseMent",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-film fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysAdvertiseMentListMenu = new SysMenu()
            {
                MenuName = "广告列表",
                MenuCode = "A-001",
                Url = "/AdvertiseMent/AdvertiseMent/Index",
                ParentId = 21,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysAdvertiseMentMenu);
            sysMenuList.Add(sysAdvertiseMentListMenu);
            //日志管理
            SysMenu sysLogMenu = new SysMenu()
            {
                MenuName = "日志管理",
                MenuCode = "Log",
                Url = "/Log",
                ParentId = 0,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-archive fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            SysMenu sysInventoryChangeLogMenu = new SysMenu()
            {
                MenuName = "日志管理",
                MenuCode = "L-001",
                Url = "/Log/InventoryChangeLog/Index",
                ParentId = 23,
                MenuLevel = 0,
                SortNumber = 1,
                Status = 1,
                Icon = "fa fa-archive fa-lg",
                IsDelete = false,
                CreateUser = "李军毅",
                CredateTime = DateTime.Now,
            };
            sysMenuList.Add(sysLogMenu);
            sysMenuList.Add(sysInventoryChangeLogMenu);

            sysMenuList.ForEach(p =>
            {
                context.SysMenu.Add(p);
            });
            context.SaveChanges();
        }
        /// <summary>
        /// 初始化角色
        /// </summary>
        /// <param name="context"></param>
        public void InitializerRole(VendMDbContext context)
        {
            List<Role> roleList = new List<Role>() {
                 new Role()
                 {
                    RoleName ="普通员工",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new Role()
                 {
                    RoleName ="超级管理员",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                }
            };
            roleList.ForEach(p =>
            {
                context.Role.Add(p);
            });
            context.SaveChanges();
        }
        /// <summary>
        /// 初始化按钮
        /// </summary>
        /// <param name="context"></param>
        public void InitializerSysButton(VendMDbContext context)
        {
            List<SysButton> SysButtonList = new List<SysButton>() {
                 new SysButton()
                 {
                    ButtonCode ="Menu",
                    ButtonName="菜单",
                    InputType="",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                new SysButton()
                 {
                    ButtonCode ="List",
                    ButtonName="列表",
                    InputType="",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Add",
                    ButtonName="新增",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Edit",
                    ButtonName="编辑",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Browse",
                    ButtonName="浏览",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Delete",
                    ButtonName="删除",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Submit ",
                    ButtonName="提交",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Confirm ",
                    ButtonName="确认",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Approval ",
                    ButtonName="审核",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                },
                 new SysButton()
                 {
                    ButtonCode ="Reject ",
                    ButtonName="拒绝",
                    InputType="Input",
                    ButtonStyle="",
                    Status="N",
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                }
            };
            SysButtonList.ForEach(p =>
            {
                context.SysButton.Add(p);
            });
            context.SaveChanges();
        }
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <param name="context"></param>
        public void InitializerUserRole(VendMDbContext context)
        {
            List<UserRole> userRoleList = new List<UserRole>() {
                 new UserRole()
                 {
                    RoleId =1,
                    UserId=1,
                    CreateUser="李军毅",
                    CredateTime = DateTime.Now
                }
            };
            userRoleList.ForEach(p =>
            {
                context.UserRole.Add(p);
            });
            context.SaveChanges();
        }
        /// <summary>
        /// 功能
        /// </summary>
        /// <param name="context"></param>
        public void InitializerSysFunction(VendMDbContext context)
        {
            //List<SysFunction> sysFunctionList = new List<SysFunction>() {
            //     new SysFunction()
            //     {
            //        SysMenuId =1,
            //        //SysButtonId =1,
            //        CreateUser="李军毅",
            //        CredateTime = DateTime.Now
            //    }
            //};
            //sysFunctionList.ForEach(p =>
            //{
            //    context.SysFunction.Add(p);
            //});
            //context.SaveChanges();
        }
        /// <summary>
        /// 用户权限
        /// </summary>
        /// <param name="context"></param>
        public void InitializerUserRights(VendMDbContext context)
        {
            //List<UserRights> userRightsList = new List<UserRights>() {
            //     new UserRights()
            //     {
            //         UserId=1,
            //         SysFunctionId=1,
            //         CreateUser="李军毅",
            //         CredateTime = DateTime.Now
            //    }
            //};
            //userRightsList.ForEach(p =>
            //{
            //    context.UserRights.Add(p);
            //});
            //context.SaveChanges();
        }
    }
}
