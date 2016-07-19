using System.Web.Http;
using Microsoft.Practices.Unity;
using WebAPI.Library.DependencyInjection;
using WebAPI.Repositories;

namespace WebAPI.App_Start
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IMessageRepository, MessageRepository>(new ContainerControlledLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}