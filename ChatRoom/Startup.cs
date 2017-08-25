using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Owin;
using ChatRoom;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace ChatRoom
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(a => a.FullName.Contains("ChatRoom")).ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => t.Name.EndsWith("Buiness"))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => t.Name.EndsWith("Helper"))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => t.Name.EndsWith("Handler"))
                .AsImplementedInterfaces();
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            var hubConfig = new HubConfiguration
            {
                Resolver = new AutofacDependencyResolver(container),
                EnableDetailedErrors = true
            };
            //todo:以下代码来自各种官网，然而并没有什么暖用！
            //
            //var hubPipeline = hubConfig.Resolver.Resolve<IHubPipeline>();
            //hubPipeline.AddModule(new CustomerHubGropModule());
            //GlobalHost.HubPipeline.AddModule(new CustomerHubGropModule());
            app.UseAutofacMiddleware(container);
            //GlobalHost.HubPipeline.AddModule(new CustomerHubGropModule());
            //这个UerIdProvider注册了并没有用。可能是因为使用了AutoFac容器的原因。
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new DbUserIdProvider());
            app.MapSignalR("/signalr", hubConfig);
        }
    }
}
