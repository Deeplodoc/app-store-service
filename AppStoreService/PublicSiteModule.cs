using AppStoreService.Business;
using AppStoreService.Core;
using Autofac;
using Microsoft.Extensions.Options;

namespace AppStoreService
{
    public class PublicSiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<IOptions<Configuration>>().Value);
            builder.RegisterModule<BusinessModule>();
        }
    }
}