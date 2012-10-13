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

namespace SC.DefaultPlugins
{
    [SC.Interfaces.License("DefaultPlugins")]
    [SC.Interfaces.ScPlugin("Manual Control")]
    public class ManualControl : SC.PluginBase.AbstractServerPlugin, SC.Interfaces.DefaultPlugins.IManualControl
    {
        private bool runServer = true;

        public ManualControl()
        {
        }

        protected override void SaveSettings()
        {
            // Nothing here
        }

        protected override void RestoreSettings()
        {
            // Nothing here
        }

        #region IScServerPluginBase Members


        public bool ShouldRun()
        {
            return runServer;
        }

        public bool IsRunning()
        {
            return true;
        }

        public bool IsRunningStatus
        {
            get
            {
                return true;
            }
        }
        public bool ShouldRunStatus
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("ShouldRunStatus");
                else
                    return runServer;
            }
        }

        #endregion

        public void Start()
        {
            if (InvokeRequired)
            {
                secProvider.DemandPermission();
                Invoke(new SC.Messaging.NDelegate(this.Start));
            }
            else
            {
                this.runServer = true;
            }
        }
        public void Stop()
        {
            if (InvokeRequired)
            {
                secProvider.DemandPermission();
                Invoke(new SC.Messaging.NDelegate(this.Stop));
            }
            else
            {
                this.runServer = false;
            }
        }
        public bool Running
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("Running");
                else
                    return runServer;
            }
        }
    }
}
