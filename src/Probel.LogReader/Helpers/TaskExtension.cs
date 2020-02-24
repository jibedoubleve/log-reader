using Probel.LogReader.Ui;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Probel.LogReader.Helpers
{
    public static class TaskExtension
    {
        #region Methods

        public static void OnErrorHandle(this Task t, IUserInteraction ui, TaskScheduler scheduler = null) => OnErrorHandle(t, ui, CancellationToken.None, scheduler);

        public static void OnErrorHandle(this Task t, IUserInteraction ui, CancellationToken token, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Current;

            t.ContinueWith(r =>
            {
                ui.Logger.Error(r.Exception);
                ui.HandleError(r.Exception);
            }, token, TaskContinuationOptions.OnlyOnFaulted, s);
        }

        public static void OnErrorHandleWith(this Task t, Action<Task> handler, TaskScheduler scheduler = null) => OnErrorHandleWith(t, handler, CancellationToken.None, scheduler);

        public static void OnErrorHandleWith(this Task t, Action<Task> handler, CancellationToken token, TaskScheduler scheduler = null)
        {
            var s = scheduler ?? TaskScheduler.Current;

            t.ContinueWith(r => handler(r), token, TaskContinuationOptions.OnlyOnFaulted, s);
        }

        #endregion Methods
    }
}