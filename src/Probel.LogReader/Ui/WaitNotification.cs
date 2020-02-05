using System;
using System.Windows.Input;

namespace Probel.LogReader.Ui
{
    /// <summary>
    /// The sole purpose of this object is to change the mouse cursor.
    /// On dispose the cursor is reset to its default state
    /// 
    /// </summary>
    public sealed class WaitNotification : IDisposable
    {
        #region Methods

        public void Dispose() => EndWaiting();

        private void EndWaiting() => Mouse.OverrideCursor = null;

        public void StartWaiting() => Mouse.OverrideCursor = Cursors.Wait;

        #endregion Methods
    }
}