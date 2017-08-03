using AppStoreService.Business.Services;
using AppStoreService.Core.Business;
using AppStoreService.Dal;
using Autofac;

namespace AppStoreService.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DalModule>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<EmailSendService>().As<IEmailSendService>();
        }
    }
}