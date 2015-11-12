using Autofac;
using Dal.Repositories;
using Dal.Repositories.IRepositories;
using Dal.Repositories.PrivatChat;
using Dal.Repositories.Repositories;

namespace Dal
{
    public class IoCModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>();

            base.Load(builder);
        }
    }
}
