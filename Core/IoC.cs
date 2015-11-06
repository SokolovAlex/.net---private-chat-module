using Autofac;

namespace Core
{
    public static class IoC
    {
        #region Static Fields

        #region private

        /// <summary>
        ///     An instance of container.
        /// </summary>
        private static IContainer _container;

        /// <summary>
        ///     The sync object.
        /// </summary>
        private static readonly object _sync = new object();

        #endregion

        #endregion

        #region Static Properties

        #region public

        /// <summary>
        ///     Gets the IoC-container instance.
        /// </summary>
        /// <value>
        ///     The IoC-container instance.
        /// </value>
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

        #endregion


        /// <summary>
        ///     Initializes a new IoC-container by the specified modules.
        /// </summary>
        /// <param name="modules">
        ///     The modules.
        /// </param>
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
