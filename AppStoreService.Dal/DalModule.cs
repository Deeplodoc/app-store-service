using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using AppStoreService.Dal.Creators;
using AppStoreService.Dal.Finders;
using AppStoreService.Dal.Readers;
using AppStoreService.Dal.Removers;
using AppStoreService.Dal.Senders;
using AppStoreService.Dal.Updaters;
using Autofac;
using MongoDB.Driver;

namespace AppStoreService.Dal
{
    public class DalModule : Module
    {
        static DalModule()
        {
            MongoConventions.BindConventions();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.Register(c => new MongoClient(c.Resolve<Configuration>().MongoConnectionStringWithProject))
                .As<IMongoClient>().SingleInstance();
            builder.Register(c => c.Resolve<IMongoClient>()
                    .GetDatabase(MongoUrl.Create(c.Resolve<Configuration>().MongoConnectionStringWithProject)
                        .DatabaseName))
                .As<IMongoDatabase>().InstancePerLifetimeScope();

            builder.RegisterType<UserCreator>().As<ICreate<User>>();
            builder.RegisterType<UserReader>().As<IRead<User>>();
            builder.RegisterType<UserUpdater>().As<IUpdate<User>>();
            builder.RegisterType<UserRemover>().As<IDelete<string>>();

            builder.RegisterType<EmailCreator>().As<ICreate<Email>>();

            builder.RegisterType<YandexEmailSender>().As<ISend<Email>>();

            builder.RegisterType<UserLoginFinder>().As<IFilter<UserLoginFindModel, User>>();
            builder.RegisterType<UserConfirmFinder>().As<IFilter<UserConfirmModel, User>>();
        }
    }
}