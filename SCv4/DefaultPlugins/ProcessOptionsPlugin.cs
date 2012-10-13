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

namespace SC.Core.DefaultPlugins
{
    public class ProcessOptionSettings
    {
        public System.Diagnostics.ProcessPriorityClass Priority = System.Diagnostics.ProcessPriorityClass.Normal;
        [System.Xml.Serialization.XmlIgnore]
        public IntPtr _Affinity = ProcessOptionsPlugin.GetMaxAffinity();
        public Int64 Affinity
        {
            get
            {
                return _Affinity.ToInt64();
            }
            set
            {
                _Affinity = new IntPtr(value & ProcessOptionsPlugin.GetMaxAffinity().ToInt64());
            }
        }

        public ProcessOptionSettings()
        {
        }
        public ProcessOptionSettings(System.Diagnostics.ProcessPriorityClass priority, IntPtr affinity)
        {
            this.Priority = priority;
            this._Affinity = affinity;
        }
    }

    [SC.Interfaces.License("DefaultPlugins")]
    [SC.Interfaces.ScPlugin("Process Options")]
    public class ProcessOptionsPlugin : SC.PluginBase.AbstractServerPlugin, SC.Interfaces.DefaultPlugins.IProcessOptionsPlugin
    {
        public static IntPtr GetMaxAffinity()
        {
            Int64 a = 0;
            for (int i = 0; i < GetProcessorCount(); ++i)
            {
                a <<= 1;
                a |= 1;
            }
            return new IntPtr(a);
        }
        [System.Security.Permissions.EnvironmentPermission(System.Security.Permissions.SecurityAction.Assert, Read = "NUMBER_OF_PROCESSORS")]
        public static int GetProcessorCount()
        {
            return Environment.ProcessorCount;
        }

        private System.Diagnostics.ProcessPriorityClass priority = System.Diagnostics.ProcessPriorityClass.Normal;
        private IntPtr affinity = GetMaxAffinity();
        private bool destroyed = false;
        private bool doSet = true;

        #region IProcessOptionsPlugin Members

        public System.Diagnostics.ProcessPriorityClass Priority
        {
            get
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    return InvokeGet<System.Diagnostics.ProcessPriorityClass>("Priority");
                }
                else
                    return priority;
            }
            set
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    InvokeSet<System.Diagnostics.ProcessPriorityClass>("Priority", value);
                }
                else
                {
                    priority = value;
                    doSet = true;
                }
            }
        }
        public IntPtr Affinity
        {
            get
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    return InvokeGet<IntPtr>("Affinity");
                }
                else
                    return affinity;
            }
            set
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    InvokeSet<IntPtr>("Affinity", value);
                }
                else
                {
                    affinity = value;
                    doSet = true;
                }
            }
        }

        public int ProcessorCount
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<int>("ProcessorCount");
                else
                    return GetProcessorCount();
            }
        }

        #endregion

        private void SetOptions()
        {
            SC.Interfaces.IProcess process = server.GetProcess();

            if (process != null)
            {
                process.ProcessorAffinity = affinity;
                process.PriorityClass = priority;
                doSet = false;
            }
        }

        #region IScServerPluginBase Members

        protected override void RestoreSettings()
        {
            ProcessOptionSettings settings = settProvider.RestoreSettings(typeof(ProcessOptionSettings)) as ProcessOptionSettings;
            if (settings != null)
            {
                this.Settings = settings;
            }
        }

        protected override void SaveSettings()
        {
            settProvider.SaveSettings(Settings);
        }

        public override void AttachServer(SC.Interfaces.IPluginServer server)
        {
            base.AttachServer(server);

            server.ProcessChanged += new SC.Interfaces.ProcessChangedEvent(server_ProcessChanged);
        }

        void server_ProcessChanged(object sender, SC.Interfaces.ProcessChangedEventArgs args)
        {
            doSet = true;
        }

        public bool ShouldRun()
        {
            return true;
        }

        public bool IsRunning()
        {
            if (doSet)
            {
                SetOptions();
            }
            return true;
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

        private ProcessOptionSettings Settings
        {
            get
            {
                return new ProcessOptionSettings(priority, affinity);
            }
            set
            {
                this.priority = value.Priority;
                this.affinity = value._Affinity;
                doSet = true;
            }
        }
    }
}
