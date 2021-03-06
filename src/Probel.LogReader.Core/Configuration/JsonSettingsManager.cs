﻿using Newtonsoft.Json;
using Probel.LogReader.Core.Helpers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Probel.LogReader.Core.Configuration
{
    public class JsonSettingsManager : ISettingFileManager
    {
        #region Fields

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        #endregion Fields

        #region Constructors

        public JsonSettingsManager(string path)
        {
            FileName = path.Expand();
        }

        #endregion Constructors

        #region Properties

        private string FileName { get; }

        #endregion Properties

        #region Methods

        public void Delete()
        {
            _semaphore.Wait();
            try
            {
                var file = Environment.ExpandEnvironmentVariables(FileName);

                if (File.Exists(file)) { File.Delete(file); }
            }
            finally { _semaphore.Release(); }
        }

        public AppSettings Get()
        {
            _semaphore.Wait();
            try
            {
                AppSettings result;

                if (File.Exists(FileName))
                {
                    var json = File.ReadAllText(FileName);
                    result = JsonConvert.DeserializeObject<AppSettings>(json);
                }
                else
                {
                    CreateDefaultConfig();
                    result = new AppSettings();
                }

                return result == null
                    ? new AppSettings() //throw new InvalidOperationException($"The json property file '{FileName}' is empty!")
                    : result;
            }
            finally { _semaphore.Release(); }
        }

        public async Task<AppSettings> GetAsync()
        {
            _semaphore.Wait();
            try
            {
                AppSettings result;

                if (File.Exists(FileName))
                {
                    var json = File.ReadAllText(FileName);
                    result = JsonConvert.DeserializeObject<AppSettings>(json);
                }
                else
                {
                    await CreateDefaultConfigAsync();
                    result = new AppSettings();
                }

                return result == null
                    ? new AppSettings() //throw new InvalidOperationException($"The json property file '{FileName}' is empty!")
                    : result;
            }
            finally { _semaphore.Release(); }
        }

        public void Save(AppSettings settings)
        {
            _semaphore.Wait();
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                using (var stream = new FileStream(FileName, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                    writer.Flush();
                }
            }
            finally { _semaphore.Release(); }
        }

        public async Task SaveAsync(AppSettings settings)
        {
            _semaphore.Wait();
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                using (var stream = new FileStream(FileName, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(json);
                    await writer.FlushAsync();
                }
            }
            finally { _semaphore.Release(); }
        }

        private void CreateDefaultConfig()
        {
            var json = JsonConvert.SerializeObject(new AppSettings(), Formatting.Indented);
            using (var stream = File.CreateText(FileName))
            {
                stream.Write(json);
                stream.Flush();
            }
        }

        private async Task CreateDefaultConfigAsync()
        {
            var json = JsonConvert.SerializeObject(new AppSettings(), Formatting.Indented);
            using (var stream = File.CreateText(FileName))
            {
                await stream.WriteAsync(json);
                await stream.FlushAsync();
            }
        }

        #endregion Methods
    }
}