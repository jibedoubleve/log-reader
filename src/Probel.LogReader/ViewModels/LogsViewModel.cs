using Caliburn.Micro;
using Probel.LogReader.Core.Configuration;
using Probel.LogReader.Core.Plugins;
using Probel.LogReader.Helpers;
using Probel.LogReader.Ui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Probel.LogReader.ViewModels
{
    public class LogsViewModel : Screen
    {
        #region Fields

        private readonly IConfigurationManager _configManager;
        private readonly IEventAggregator _eventAggregator;
        private IEnumerable<LogRow> _cachedLogs;
        private bool _canListen;
        private int _changeCount;
        private DateTime _date;
        private string _filePath;
        private bool _isDebugVisible = true;
        private bool _isErrorVisible = true;
        private bool _isFatalVisible = true;
        private bool _isFile;
        private bool _isInfoVisible = true;
        private bool _isListeningFile;
        private bool _isLoggerVisible = true;
        private bool _isThreadIdVisible;
        private bool _isTraceVisible = true;
        private bool _isWarnVisible = true;
        private ObservableCollection<LogRow> _logs;

        private string _repositoryName;

        #endregion Fields

        #region Constructors

        public LogsViewModel(IConfigurationManager configManager, IEventAggregator eventAggregator)
        {
            FilterCommand = new RelayCommand(Filter);
            _eventAggregator = eventAggregator;
            _configManager = configManager;
        }

        #endregion Constructors

        #region Properties

        public bool CanListen
        {
            get => _canListen;
            set => Set(ref _canListen, value, nameof(CanListen));
        }

        public int ChangeCount
        {
            get => _changeCount;
            set => Set(ref _changeCount, value, nameof(ChangeCount));
        }

        public DateTime Date
        {
            get => _date;
            set => Set(ref _date, value, nameof(Date));
        }

        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value, nameof(FilePath));
        }

        public ICommand FilterCommand { get; set; }

        public System.Action GoBack { get; internal set; }

        public bool IsDebugVisible
        {
            get => _isDebugVisible;
            set => Set(ref _isDebugVisible, value, nameof(IsDebugVisible));
        }

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => Set(ref _isErrorVisible, value, nameof(IsErrorVisible));
        }

        public bool IsFatalVisible
        {
            get => _isFatalVisible;
            set => Set(ref _isFatalVisible, value, nameof(IsFatalVisible));
        }

        public bool IsFile
        {
            get => _isFile;
            set => Set(ref _isFile, value, nameof(IsFile));
        }

        public bool IsInfoVisible
        {
            get => _isInfoVisible;
            set => Set(ref _isInfoVisible, value, nameof(IsInfoVisible));
        }

        public bool IsListeningFile
        {
            get => _isListeningFile;
            set => Set(ref _isListeningFile, value, nameof(IsListeningFile));
        }

        public bool IsLoggerVisible
        {
            get => _isLoggerVisible;
            set => Set(ref _isLoggerVisible, value, nameof(IsLoggerVisible));
        }

        public bool IsThreadIdVisible
        {
            get => _isThreadIdVisible;
            set => Set(ref _isThreadIdVisible, value, nameof(IsThreadIdVisible));
        }

        public bool IsTraceVisible
        {
            get => _isTraceVisible;
            set => Set(ref _isTraceVisible, value, nameof(IsTraceVisible));
        }

        public bool IsWarnVisible
        {
            get => _isWarnVisible;
            set => Set(ref _isWarnVisible, value, nameof(IsWarnVisible));
        }

        public IDataListener Listener { get; set; }

        public ObservableCollection<LogRow> Logs
        {
            get => _logs;
            set
            {
                if (Set(ref _logs, value, nameof(Logs)))
                {
                    NotifyOfPropertyChange(nameof(LogsCount));
                }
            }
        }

        public int LogsCount => Logs.Count();

        public System.Action RefreshData { get; set; }

        public string RepositoryName
        {
            get => _repositoryName;
            set => Set(ref _repositoryName, value, nameof(RepositoryName));
        }

        #endregion Properties

        #region Methods

        public void Cache(IEnumerable<LogRow> logs)
        {
            _cachedLogs = logs;
            Date = logs.Select(e => e.Time.Date).Distinct().FirstOrDefault();
        }

        public void ClearCache() => _cachedLogs = null;

        public void LoadDays() => GoBack?.Invoke();

        public void ResetCache()
        {
            Logs = (_cachedLogs == null)
                ? new ObservableCollection<LogRow>()
                : new ObservableCollection<LogRow>(_cachedLogs);
        }

        protected override void OnActivate()
        {
            _eventAggregator.PublishOnUIThread(UiEvent.ShowMenuFilter(true));
            IsTraceVisible
                = IsDebugVisible
                = IsInfoVisible
                = IsWarnVisible
                = IsErrorVisible
                = IsFatalVisible
                = true;

            if (CanListen)
            {
                if (Listener != null)
                {
                    Listener.DataChanged += OnDataChanged;
                    Listener.StartListening(Date);
                }
                else { Trace.TraceWarning("Plugin can listen but no listener is configured!"); }
            }
        }

        //TODO: Error handling
        protected override async void OnDeactivate(bool close)
        {
            _eventAggregator.PublishOnUIThread(UiEvent.ShowMenuFilter(false));
            var app = await _configManager.GetAsync();
            app.Ui.ShowLogger = IsLoggerVisible;
            app.Ui.ShowThreadId = IsThreadIdVisible;
            await _configManager.SaveAsync(app);
        }

        private void Filter()
        {
            var levels = GetLevels();
            var logs = (from l in _cachedLogs
                        where levels.Contains(l.Level.ToLower())
                        select l).ToList();
            Logs = new ObservableCollection<LogRow>(logs);
        }

        private IEnumerable<string> GetLevels()
        {
            var levels = new List<string>();
            if (IsTraceVisible) { levels.Add("trace"); }
            if (IsDebugVisible) { levels.Add("debug"); }
            if (IsInfoVisible) { levels.Add("info"); }
            if (IsWarnVisible) { levels.Add("warn"); }
            if (IsErrorVisible) { levels.Add("error"); }
            if (IsFatalVisible) { levels.Add("fatal"); }
            return levels;
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            if (IsListeningFile)
            {
                ChangeCount++;
                RefreshData?.Invoke();
            }
        }

        #endregion Methods
    }
}