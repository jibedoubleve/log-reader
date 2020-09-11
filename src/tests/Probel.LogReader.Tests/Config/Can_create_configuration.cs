using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Constants;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Probel.LogReader.Tests.Config
{
    public sealed class Can_create_configuration : IDisposable
    {
        #region Fields

        private readonly string FilePath = @"%appdata%\probel\log-reader\test_data-settings.json";

        #endregion Fields

        #region Constructors

        public Can_create_configuration()
        {
            ((ConfigurationManager)NewConfigurationManager()).Delete();
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            if (File.Exists(FilePath.Expand())) { File.Delete(FilePath.Expand()); }
        }

        [Fact]
        public async Task With_multiple_filters()
        {
            var cm = NewConfigurationManager();
            var cfg = await cm.GetAsync();

            var time = "15";
            cfg.Filters.Add(new FilterSettings
            {
                Name = Guid.NewGuid().ToString(),
                Expression = new List<FilterExpressionSettings>()
                {
                        new FilterExpressionSettings()
                        {
                            Operation = FilterType.Time,
                            Operator = TimeFilterOperators.LessThan,
                            Operand = time,
                        }
                    }
            });
            cfg.Filters.Add(new FilterSettings
            {
                Name = Guid.NewGuid().ToString(),
                Expression = new List<FilterExpressionSettings>()
                {
                        new FilterExpressionSettings()
                        {
                            Operation = FilterType.Time,
                            Operator = TimeFilterOperators.LessThan,
                            Operand = time,
                        }
                    }
            });

            await cm.SaveAsync(cfg);
            cfg = await cm.GetAsync();

            Assert.Equal(2 + 1, cfg.Filters.Count);
        }

        [Fact]
        public async Task With_multiple_repository()
        {
            var cm = NewConfigurationManager();
            var cfg = await cm.GetAsync();

            var name = Guid.NewGuid().ToString();
            var connectionString = Guid.NewGuid().ToString();
            var queryDay = Guid.NewGuid().ToString();
            var queryLog = Guid.NewGuid().ToString();

            cfg.Repositories.Add(new RepositorySettings()
            {
                Name = name,
                PluginId = Guid.NewGuid(),
                ConnectionString = connectionString,
                QueryDay = queryDay,
                QueryLog = queryLog,
            });
            cfg.Repositories.Add(new RepositorySettings()
            {
                Name = name,
                PluginId = Guid.NewGuid(),
                ConnectionString = connectionString,
                QueryDay = queryDay,
                QueryLog = queryLog,
            });

            await cm.SaveAsync(cfg);
            cfg = await cm.GetAsync();

            Assert.Equal(2, cfg.Repositories.Count);
        }

        [Fact]
        public async Task With_one_filter()
        {
            var cm = NewConfigurationManager();
            var cfg = await cm.GetAsync();

            var id = Guid.NewGuid();
            var time = "15";
            cfg.Filters.Add(new FilterSettings()
            {
                Id = id,
                Name = Guid.NewGuid().ToString(),
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

            await cm.SaveAsync(cfg);
            cfg = await cm.GetAsync();

            Assert.Equal(1 + 1, cfg.Filters.Count);

            var f = (from fi in cfg.Filters
                     where fi.Id == id
                     select fi.Expression.Single()).Single();

            Assert.Equal(FilterType.Time, f.Operation);
            Assert.Equal(TimeFilterOperators.LessThan, f.Operator);
            Assert.Equal(time, f.Operand);
        }

        [Fact]
        public async Task With_one_repository()
        {
            var cm = NewConfigurationManager();
            var cfg = await cm.GetAsync();

            var name = Guid.NewGuid().ToString();
            var connectionString = Guid.NewGuid().ToString();
            var queryDay = Guid.NewGuid().ToString();
            var queryLog = Guid.NewGuid().ToString();

            cfg.Repositories.Add(new RepositorySettings()
            {
                Name = name,
                PluginId = Guid.NewGuid(),
                ConnectionString = connectionString,
                QueryDay = queryDay,
                QueryLog = queryLog,
            });

            await cm.SaveAsync(cfg);
            cfg = await cm.GetAsync();

            Assert.Equal(1, cfg.Repositories.Count);

            var r = cfg.Repositories[0];
            Assert.Equal(name, r.Name);
            Assert.Equal(connectionString, r.ConnectionString);
            Assert.Equal(queryDay, r.QueryDay);
            Assert.Equal(queryLog, r.QueryLog);
        }

        private IConfigurationManager NewConfigurationManager() => new ConfigurationManager(new JsonSettingsManager(FilePath));

        #endregion Methods
    }
}