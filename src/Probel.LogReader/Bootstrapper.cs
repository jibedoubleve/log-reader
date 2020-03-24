using Caliburn.Micro;
using Notifications.Wpf;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Core.Plugins.Loaders;
using Probel.LogReader.Helpers;
using Probel.LogReader.Ui;
using Probel.LogReader.ViewModels;
using Probel.LogReader.ViewModels.Packs;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Injection;

namespace Probel.LogReader
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields

        private readonly IUnityContainer _container = new UnityContainer();

        #endregion Fields

        #region Constructors

        public Bootstrapper() { Initialize(); }

        #endregion Constructors

        #region Methods

        protected override void BuildUp(object instance) => _container.BuildUp(instance);

        protected override void Configure()
        {
            /* CALIBURN */
            _container.RegisterSingleton<IWindowManager, WindowManager>();
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();

            /* SERVICES */
            _container.RegisterType<IConfigurationManager, ConfigurationManager>();
            _container.RegisterType<ISettingFileManager, JsonSettingsManager>(new InjectionConstructor(@"%appdata%\probel\log-reader\data-settings.json"));
            _container.RegisterType<IPluginManager, PluginManager>();
            _container.RegisterType<IPluginLoader, PluginLoader>(); //Used to instanciate PluginManager
            _container.RegisterType<IFilterManager, FilterManager>();
            _container.RegisterType<IFilterTranslator, FilterTranslator>();
            _container.RegisterType<IPluginInfoManager, PluginManager>();
            _container.RegisterType<ILogger, NLogLogger>();

            /* UI */
            _container.RegisterType<IUserInteraction, UserInteraction>();

            _container.RegisterSingleton<INotificationManager, NotificationManager>();

            /* VIEWS */
            _container.RegisterType<MainViewModelPack>();
            _container.RegisterType<MainViewModel>();
            _container.RegisterType<LogsViewModel>();
            _container.RegisterType<ManageRepositoryViewModel>();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.ResolveAll(service);

        protected override object GetInstance(Type service, string key) => _container.Resolve(service, key);

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<MainViewModel>();

        #endregion Methods
    }
}