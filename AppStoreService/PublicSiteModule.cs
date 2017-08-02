using AppStoreService.Core;
using AppStoreService.Dal;
using Autofac;
using Microsoft.Extensions.Options;

namespace AppStoreService
{
    public class PublicSiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<IOptions<Configuration>>().Value);
            builder.RegisterModule<DalModule>();
        }
    }
}