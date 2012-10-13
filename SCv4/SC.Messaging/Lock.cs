using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Messaging
{
    public class Lock : IDisposable
    {
        private System.Threading.ReaderWriterLock rwlock;
        public enum LockType
        {
            ForReading,
            ForWriting,
        }
        LockType type;
        System.Nullable<System.Threading.LockCookie> cookie = null;

        public Lock(System.Threading.ReaderWriterLock rwl, LockType lt)
        {
            rwlock = rwl;
            if (type == LockType.ForReading)
                rwl.AcquireReaderLock(-1);
            else if (type == LockType.ForWriting)
                rwl.AcquireWriterLock(-1);
        }

        public void UpgradeToWriterLock()
        {
            System.Diagnostics.Debug.Assert(type == LockType.ForReading);

            if (type == LockType.ForReading)
            {
                rwlock.UpgradeToWriterLock(-1);
                type = LockType.ForWriting;
            }
        }

        public void DowngradeToReaderLock()
        {
            System.Diagnostics.Debug.Assert(type == LockType.ForWriting);
            if (type == LockType.ForWriting)
            {
                System.Threading.LockCookie c = cookie.Value;
                rwlock.DowngradeFromWriterLock(ref c);
                cookie = null;
                type = LockType.ForReading;
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.Assert(disposing);

            if (cookie.HasValue)
                DowngradeToReaderLock();

            if (type == LockType.ForReading)
                rwlock.ReleaseReaderLock();
            else if (type == LockType.ForWriting)
                rwlock.ReleaseWriterLock();
        }

        #endregion

        ~Lock()
        {
            Dispose(false);
        }
    }
}
