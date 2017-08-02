using AppStoreService.Core;
using Autofac;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppStoreService
{
    public class PublicSiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<IOptions<Configuration>>().Value);
        }
    }
}