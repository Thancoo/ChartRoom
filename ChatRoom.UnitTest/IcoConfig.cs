using System;
using System.Linq;
using System.Reflection;
using Autofac;
using ChatRoom.Common;

namespace ChatRoom.UnitTest
{
    public static class IcoConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var assemblys = Assembly.GetCallingAssembly().GetReferencedAssemblies().Where(a => a.FullName.Contains("ChatRoom")).Select(Assembly.Load);
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => t.Name.EndsWith("Bll"))
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
            ChatRoomEnv.Container = container;
        }
    }
}
