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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SC.DefaultPlugins.GUI
{
    [SC.GUI.Utility.PluginPanel(typeof(SC.Interfaces.DefaultPlugins.IServerBoosterPlugin))]
    public partial class ServerBoosterPanel : SC.GUI.Utility.PluginPanelBase
    {
        private SC.Interfaces.DefaultPlugins.IServerBoosterPlugin plugin = null;
        
        public ServerBoosterPanel()
        {
            InitializeComponent();
        }

        public override void SetPlugin(SC.Interfaces.IScPluginClient plugin)
        {
            if (plugin == null)
                throw new ArgumentException("plugin");
            else if (!(plugin is SC.Interfaces.DefaultPlugins.IServerBoosterPlugin))
                throw new ArgumentException("plugin");

            this.plugin = plugin as SC.Interfaces.DefaultPlugins.IServerBoosterPlugin;
            RefreshView();
        }

        private void RefreshView()
        {
            if (plugin != null)
            {
                chkEnabled.Checked = plugin.Enabled;
                numPeriod.Value = plugin.Period;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (plugin != null)
            {
                plugin.Period = Convert.ToUInt32(numPeriod.Value);
                plugin.Enabled = chkEnabled.Checked;
            }
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        private void numPeriod_ValueChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }
    }
}
