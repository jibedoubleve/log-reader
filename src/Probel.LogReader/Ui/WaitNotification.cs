using System;
using System.Windows.Input;

namespace Probel.LogReader.Ui
{
    public sealed class WaitNotification : IDisposable
    {
        #region Methods

        public void Dispose() => EndWaiting();

        private void EndWaiting() => Mouse.OverrideCursor = null;

        public void StartWaiting() => Mouse.OverrideCursor = Cursors.Wait;

        #endregion Methods
    }
}