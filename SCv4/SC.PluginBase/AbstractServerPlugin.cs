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
using System.Linq;
using System.Text;

namespace SC.PluginBase
{
    public abstract class AbstractServerPlugin : SC.Messaging.Invokeable, SC.Interfaces.IScServerPluginBase, SC.Interfaces.ILicensee
    {
        protected SC.Interfaces.ISecurityProvider secProvider = null;
        protected SC.Interfaces.ISettingsProvider settProvider = null;
        protected SC.Interfaces.ILicense license = null;
        protected SC.Interfaces.IPluginServer server = null;

        #region IScServerPluginBase Members

        public void SetProvider(SC.Interfaces.IProvider provider)
        {
            if (secProvider != null || settProvider != null)
                throw new InvalidOperationException("Cannot assign providers twice.");

            this.settProvider = provider.GetSettingsProvider();
            this.secProvider = provider.GetSecurityProvider();
        }

        public virtual void AttachServer(SC.Interfaces.IPluginServer server)
        {
            if (license == null)
                throw new InvalidOperationException("No license assigned.");

            license.AssertValid();
            this.server = server;
            RestoreSettings();
        }

        public virtual void DetachServer()
        {
            SaveSettings();

            this.server = null;
            if (secProvider != null)
                secProvider.Cleanup();
            if (license != null)
                license.Invalidate();
        }

        protected abstract void SaveSettings();
        protected abstract void RestoreSettings();

        public bool ShouldRun()
        {
            return true;
        }

        public bool IsRunning()
        {
            return true;
        }

        #endregion

        #region IScPluginBase Members

        public void Destroy()
        {
            if (settProvider != null)
                settProvider.Destroy();
            if (secProvider != null)
                secProvider.Destroy();
        }

        #endregion

        #region IScPluginClient Members

        public bool HavePermission()
        {
            if (secProvider != null)
                return secProvider.HavePermission();
            return false;
        }

        public bool HavePermission(string operation)
        {
            if (secProvider != null)
                return secProvider.HavePermission(operation);
            return false;
        }

        #endregion

        #region ILicensee Members

        public void AssignLicense(SC.Interfaces.ILicense lic)
        {
            this.license = lic;
        }

        public void AssertLicense()
        {
            license.AssertValid();
        }

        public bool IsLicensed()
        {
            return license.IsValid();
        }

        #endregion

        #region IScServerPluginClient Members

        public bool IsRunningStatus
        {
            get { return true; }
        }

        public bool ShouldRunStatus
        {
            get { return true; }
        }

        #endregion
    }
}
