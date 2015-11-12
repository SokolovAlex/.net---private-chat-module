using Autofac;

namespace Core.Components
{
    public static class IoC
    {
        #region private

        private static IContainer _container;

        private static readonly object _sync = new object();

        #endregion

        #region public
        public static IContainer Instance
        {
            get
            {
                if (_container != null)
                {
                    return _container;
                }

                lock (_sync)
                {
                    if (_container == null)
                    {
                        // ReSharper disable once PossibleMultipleWriteAccessInDoubleCheckLocking
                        _container = new ContainerBuilder().Build();
                    }
                }

                return _container;
            }
        }

        #endregion

        public static void Initialize(params Module[] modules)
        {
            var builder = new ContainerBuilder();
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
            lock (_sync)
            {
                _container = builder.Build();
            }
        }

    }
}
