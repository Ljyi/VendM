using Autofac;
using System.Reflection;

namespace VendM.WebAPI.App_Start
{
    public class AutofacConfig
    {
        public static void AutofacRegister()
        {
            var builder = new ContainerBuilder();
            var assemblysServices = Assembly.Load("VendM.Service");//要记得!!!这个注入的是实现类层，不是接口层！不是 IServices
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();//指定已扫描程序集中的类型注册为提供所有其实现的接口。
                                                                                       // builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepository<>)).PropertiesAutowired();
                                                                                       //   var assemblysRepository = Assembly.Load("Blog.Core.Repository");//模式是 Load(解决方案名)
                                                                                       //    builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            //将services填充到Autofac容器生成器中

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();
        }
    }
}