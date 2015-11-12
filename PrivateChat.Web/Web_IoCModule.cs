using Autofac;
using Dal.Repositories;
using Dal.Repositories.IRepositories;
using Dal.Repositories.PrivatChat;
using Dal.Repositories.Repositories;
using Services.Providers;
using Services.Providers.IProviders;

namespace PrivateChat.Web
{
    public class Web_IoCModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>();
            builder.RegisterType<AuthServise>().As<IAuthServise>();

            base.Load(builder);
        }
    }
}
