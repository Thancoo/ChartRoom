using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using ChatRoom.Common;
using ChatRoom.Hubs.Module;
using Microsoft.AspNet.SignalR;

namespace ChatRoom
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
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
            var container = builder.Build();
            //注册所有Hub
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            //注册Api
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            ChatRoomEnv.Container = container;
        }
    }
}
