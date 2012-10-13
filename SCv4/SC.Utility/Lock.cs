/* 
ServerChecker v4 operates and manages various kinds of software on server systems.
Copyright (C) 2010 Stijn Devriendt

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Utility
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
        bool disposed = false;
#if DEBUG
        private System.Diagnostics.StackTrace callingfunction = null;
#endif

        public Lock(System.Threading.ReaderWriterLock rwl, LockType lt)
        {
            rwlock = rwl;
            if (type == LockType.ForReading)
                rwl.AcquireReaderLock(-1);
            else if (type == LockType.ForWriting)
                rwl.AcquireWriterLock(-1);

#if DEBUG
            callingfunction = new System.Diagnostics.StackTrace();
#endif
        }

        public void UpgradeToWriterLock()
        {
            System.Diagnostics.Debug.Assert(type == LockType.ForReading);

            if (type == LockType.ForReading)
            {
                cookie = rwlock.UpgradeToWriterLock(-1);
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
            if (disposed)
                return;

            System.Diagnostics.Debug.Assert(disposing);

            if (cookie.HasValue)
                DowngradeToReaderLock();

            if (type == LockType.ForReading)
                rwlock.ReleaseReaderLock();
            else if (type == LockType.ForWriting)
                rwlock.ReleaseWriterLock();

            disposed = true;
        }

        #endregion

        ~Lock()
        {
            Dispose(false);
        }
    }
}
