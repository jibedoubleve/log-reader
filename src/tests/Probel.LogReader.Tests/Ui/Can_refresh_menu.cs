﻿using NSubstitute;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Filters;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Plugins.Debug;
using Probel.LogReader.Tests.Constants;
using Probel.LogReader.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Probel.LogReader.Tests.Ui
{
    public class Can_refresh_menu
    {
        #region Fields

        private readonly ILogger _logger = Substitute.For<ILogger>();

        #endregion Fields

        #region Methods

        [Fact]
        public async Task From_config()
        {
            var id = Guid.NewGuid();
            var time = "15";

            var cfg = new AppSettings();
            cfg.Filters.Add(new FilterSettings()
            {
                Id = id,
                Expression = new List<FilterExpressionSettings>()
                {
                    new FilterExpressionSettings
                    {
                        Operation = FilterType.Time,
                        Operator = TimeFilterOperators.LessThan,
                        Operand = time,
                    }
                }
            });
            cfg.Repositories.Add(new RepositorySettings { PluginId = PluginType.Debug, });

            IPluginManager pm = new PluginManager(new DebugLoader(), _logger);
            IConfigurationManager cm = new ConfigurationManager(new MemorySettingsManager());

            await cm.SaveAsync(cfg);

            var fm = await cm.BuildFilterManagerAsync();

            var p = pm.Build(cfg.Repositories.ElementAt(0));
            var f = fm.Build(id);

            var day = p.GetDays().ElementAt(0);
            var logs = p.GetLogs(day);

            var filterd = f.Filter(logs);

            Assert.NotNull(filterd);
        }

        [Fact]
        public async Task From_debug_repository()
        {
            var names = new List<string>()
            {
                { Guid.NewGuid().ToString() },
                { Guid.NewGuid().ToString() },
                { Guid.NewGuid().ToString() },
                { Guid.NewGuid().ToString() },
                { Guid.NewGuid().ToString() },
            };

            var cm = new ConfigurationManager(new MemorySettingsManager());
            var stg = await cm.GetAsync();

            for (var i = 0; names.Count < 5; i++)
            {
                stg.Repositories.Add(new RepositorySettings { PluginId = PluginType.Debug, Name = i.ToString() }); ;
            }

            var pm = new PluginManager(new DebugLoader(), _logger);
            foreach (var repo in stg.Repositories)
            {
                var menuName = repo.Name;
                var plugin = pm.Build(repo);

                Assert.Contains(menuName, names);
                Assert.Equal(typeof(Plugin), plugin.GetType());
            }
        }

        [Fact]
        public void With_filters()
        {
            var settings = new RepositorySettings { PluginId = PluginType.Debug, Name = "debug" };
            var expression = new List<FilterExpressionSettings>()
            {
                new FilterExpressionSettings() { Operation = FilterType.Time , Operand = Rand.IntegerAsString, Operator = Rand.ComparisionOperator},
                new FilterExpressionSettings() { Operation = FilterType.Category, Operand = Rand.Text, Operator = Rand.EnsembleOperator},
                new FilterExpressionSettings() { Operation = FilterType.Level , Operand = Rand.Text, Operator = Rand.EnsembleOperator},
            };

            IPluginManager pm = new PluginManager(new DebugLoader(), _logger);
            IFilterManager fm = new FilterManager();

            var p = pm.Build(settings);
            var f = fm.Build(expression, "and");

            var day = p.GetDays().ElementAt(0);
            var logs = f.Filter(p.GetLogs(day));

            Assert.NotNull(logs);
        }

        #endregion Methods
    }
}