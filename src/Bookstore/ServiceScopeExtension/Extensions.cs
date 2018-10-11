using Akka.Actor;
using Bookstore.ServiceScopeExtension;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extensions
    {
        public static void AddServiceScopeFactory(this ActorSystem system, IServiceScopeFactory serviceScopeFactory)
        {
            system.RegisterExtension(ServiceScopeExtensionIdProvider.Instance);
            ServiceScopeExtensionIdProvider.Instance.Get(system).Initialize(serviceScopeFactory);
        }

        public static IServiceScope CreateScope(this ActorSystem system)
        {
            return ServiceScopeExtensionIdProvider.Instance.Get(system).CreateScope();
        }
    }
}