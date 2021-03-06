﻿using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Probel.LogReader.Core.Plugins
{
    public class ConfigurationManager : IConfigurationManager
    {
        #region Fields

        private readonly ISettingFileManager _fileManager;

        #endregion Fields

        #region Constructors

        public ConfigurationManager(ISettingFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        #endregion Constructors

        #region Methods

        public IFilterManager BuildFilterManager()
        {
            var cfg = Get();
            var fm = new FilterManager(cfg.Filters);
            return fm;
        }

        public async Task<IFilterManager> BuildFilterManagerAsync()
        {
            var cfg = await GetAsync();
            var fm = new FilterManager(cfg.Filters);
            return fm;
        }

        public void Create(RepositorySettings repository)
        {
            var a = Get();
            a.Repositories.Add(repository);
            Save(a);
        }

        public async Task CreateAsync(RepositorySettings repository)
        {
            var a = await GetAsync();
            a.Repositories.Add(repository);
            await SaveAsync(a);
        }

        public IAppSettingsDecorator Decorate(AppSettings appSettings) => new AppSettingsDecorator(appSettings);

        public void Delete() => _fileManager.Delete();

        public void Delete(RepositorySettings repository)
        {
            var a = Get();
            var del = a.Repositories.Where(e => e.PluginId == repository.PluginId).FirstOrDefault();
            if (del != null)
            {
                a.Repositories.Remove(del);
                Save(a);
            }
        }

        public async Task DeleteAsync(RepositorySettings repository)
        {
            var a = await GetAsync();
            var del = a.Repositories.Where(e => e.PluginId == repository.PluginId).FirstOrDefault();
            if (del != null)
            {
                a.Repositories.Remove(del);
                await SaveAsync(a);
            }
        }

        public AppSettings Get()
        {
            var app = _fileManager.Get();
            if (app.Filters.Where(e => e.Id == FilterSettings.NoFilter.Id).Count() == 0)
            {
                app.Filters.Add(FilterSettings.NoFilter);
            }
            return app;
        }

        public async Task<AppSettings> GetAsync()
        {
            var app = await _fileManager.GetAsync();
            if (app.Filters.Where(e => e.Id == FilterSettings.NoFilter.Id).Count() == 0)
            {
                app.Filters.Add(FilterSettings.NoFilter);
            }
            return app;
        }

        public IAppSettingsDecorator GetDecorated() => Decorate(Get());

        public void Save(AppSettings settings) => _fileManager.Save(settings);

        public void Save(Action<AppSettings> update)
        {
            var input = Get();
            update(input);
            Save(input);
        }

        public async Task SaveAsync(AppSettings settings) => await _fileManager.SaveAsync(settings);

        public async Task SaveAsync(Action<AppSettings> update)
        {
            var input = await GetAsync();
            update(input);
            await SaveAsync(input);
        }

        public async Task UpdateAsync(RepositorySettings info)
        {
            var a = await GetAsync();
            var del = a.Repositories.Where(e => e.PluginId == info.PluginId).FirstOrDefault();
            if (del != null)
            {
                a.Repositories.Remove(del);
                a.Repositories.Add(info);
                await SaveAsync(a);
            }
        }

        #endregion Methods
    }
}