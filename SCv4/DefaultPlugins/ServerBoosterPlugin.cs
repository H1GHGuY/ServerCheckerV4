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
    [Serializable]
    public class ServerBoosterPluginSettings
    {
        public bool Enabled = true;
        public uint Period = 10;

        public ServerBoosterPluginSettings()
        {
        }
        public ServerBoosterPluginSettings(bool enabled, uint period)
        {
            this.Enabled = enabled;
            this.Period = period;
        }
    }

    [SC.Interfaces.License("DefaultPlugins")]
    [SC.Interfaces.ScPlugin("Server Booster")]
    public class ServerBoosterPlugin : SC.PluginBase.AbstractStandalonePlugin, SC.Interfaces.DefaultPlugins.IServerBoosterPlugin
    {
        private ServerBoosterPluginSettings settings = new ServerBoosterPluginSettings();
        
        public ServerBoosterPlugin() : base("Server Booster")
        {
        }

        [System.Runtime.InteropServices.DllImport("winmm.dll", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern UInt32 timeBeginPeriod(UInt32 period);

        [System.Runtime.InteropServices.DllImport("winmm.dll", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern UInt32 timeEndPeriod(UInt32 period);

        #region IScStandalonePluginBase Members

        protected override void Initialize()
        {
            base.Initialize();
            if (settings.Enabled)
                timeBeginPeriod(settings.Period);
        }

        protected override int SingleIteration()
        {
            return 30000;
        }

        protected override void Cleanup()
        {
            if (settings.Enabled)
                timeEndPeriod(settings.Period);
            base.Cleanup();
        }

        #endregion

        protected override void SaveSettings()
        {
            settProvider.SaveSettings(settings);
        }

        protected override void RestoreSettings()
        {
            ServerBoosterPluginSettings newSettings = (ServerBoosterPluginSettings)settProvider.RestoreSettings(typeof(ServerBoosterPluginSettings));
            if (newSettings != null)
                settings = newSettings;
        }

        public bool Enabled
        {
            get
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    return (bool)InvokeGet<bool>("Enabled");
                }
                else
                {
                    return settings.Enabled;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    InvokeSet<bool>("Enabled", value);
                }
                else
                {
                    if (settings.Enabled != value)
                    {
                        if (value)
                            timeBeginPeriod(settings.Period);
                        else
                            timeEndPeriod(settings.Period);

                        settings.Enabled = value;
                    }
                }
            }
        }
        public UInt32 Period
        {
            get
            {
                if (InvokeRequired)
                    return (UInt32)InvokeGet<UInt32>("Period");
                else
                {
                    return settings.Period;
                }
            }
            set
            {
                if (InvokeRequired)
                {
                    secProvider.DemandPermission();
                    InvokeSet<UInt32>("Period", value);
                }
                else
                {
                    if (settings.Period != value)
                    {
                        if (settings.Enabled)
                        {
                            timeEndPeriod(settings.Period);
                            timeBeginPeriod(value);
                        }
                        settings.Period = value;
                    }
                }
            }
        }
    }
}
