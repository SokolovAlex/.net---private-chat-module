using Autofac;
using Dal.Repositories;
using Dal.Repositories.IRepositories;
using Dal.Repositories.PrivatChat;
using Dal.Repositories.Repositories;
using Services.Providers;
using Services.Providers.IProviders;

namespace PrivateChat.Web.App_Start
{
    public class IoCConfig
    {
        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем споставление типов
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>();
            builder.RegisterType<AuthServise>().As<IAuthServise>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();
        }
    }
}