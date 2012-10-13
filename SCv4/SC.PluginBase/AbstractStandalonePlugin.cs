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
    public abstract class AbstractStandalonePlugin : SC.Messaging.InvokeableTask, SC.Interfaces.IScStandalonePluginBase
    {
        protected SC.Interfaces.ISecurityProvider secProvider = null;
        protected SC.Interfaces.ISettingsProvider settProvider = null;
        protected SC.Interfaces.ILicense license = null;
        protected SC.Interfaces.IRoot root = null;

        public AbstractStandalonePlugin(string name) : base(name)
        {
        }

        public override void Start()
        {
            license.AssertValid();
            base.Start();
        }
        public void Stop()
        {
            base.Stop(true);
            secProvider.Cleanup();
            license.Invalidate();
        }

        protected override void Initialize()
        {
            RestoreSettings();
        }

        protected override void Cleanup()
        {
            SaveSettings();
        }

        #region IScStandalonePluginBase Members

        public void SetProvider(SC.Interfaces.IStandaloneProvider provider)
        {
            if (secProvider != null || settProvider != null)
                throw new InvalidOperationException("Cannot assign providers twice.");

            this.secProvider = provider.GetSecurityProvider();
            this.settProvider = provider.GetSettingsProvider();
            this.root = provider.GetRoot();
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

        protected abstract void SaveSettings();
        protected abstract void RestoreSettings();

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
    }
}
