using Autofac;


namespace Core
{
    public class IoCModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
//            builder.RegisterType<HashHelper>().As<IHashHelper>();
//            builder.RegisterType<CryptoHelper>().As<ICryptoHelper>();

            //
            base.Load(builder);
        }
    }//end class
}
