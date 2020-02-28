using System;
using System.Windows;
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

        public static void EndWaiting() => Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);

        public void Dispose() => EndWaiting();

        public void StartWaiting() => Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);

        #endregion Methods
    }
}