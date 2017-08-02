using AppStoreService.Core;
using AppStoreService.Core.Entities;
using AppStoreService.Dal.Repositories;
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

            builder.RegisterType<UserRepository>().As<ICreate<User>>();
            builder.RegisterType<UserRepository>().As<IRead<User>>();
            builder.RegisterType<UserRepository>().As<IUpdate<User>>();
            builder.RegisterType<UserRepository>().As<IDelete<string>>();
        }
    }
}