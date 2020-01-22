﻿using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Win32;
using Probel.LogReader.Ui;
using Probel.LogReader.ViewModels;
using Probel.LogReader.ViewModels.Packs;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace Probel.LogReader
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields

        private IUnityContainer _container = new UnityContainer();

        #endregion Fields

        #region Constructors

        public Bootstrapper() => Initialize();

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
            _container.RegisterType<ISettingFileManager, JsonSettingsManager>();
            _container.RegisterType<IPluginManager, PluginManager>();
            _container.RegisterType<IFilterManager, FilterManager>();
            _container.RegisterType<IFilterTranslator, FilterTranslator>();
            _container.RegisterType<IPluginInfoManager, PluginManager>();

            /* UI */
            _container.RegisterType<IUserInteraction, UserInteraction>();

            /* VIEWS */
            _container.RegisterType<MainViewModelPack>();
            _container.RegisterType<MainViewModel>();
            _container.RegisterType<DaysViewModel>();
            _container.RegisterType<LogsViewModel>();
            _container.RegisterType<ManageRepositoryViewModel>();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.ResolveAll(service);

        protected override object GetInstance(Type service, string key) => _container.Resolve(service, key);

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<MainViewModel>();

        #endregion Methods
    }
}