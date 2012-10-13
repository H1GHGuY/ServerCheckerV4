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
using System.Text;
using System.Windows.Forms;

namespace SC.DefaultPlugins.GUI
{
    [SC.GUI.Utility.PluginPanel(typeof(SC.Interfaces.IScServerPluginClient))]
    public partial class ServerPluginPanel : SC.GUI.Utility.PluginPanelBase
    {
        private SC.Interfaces.IScServerPluginClient plugin = null;
        public ServerPluginPanel()
        {
            InitializeComponent();
        }
        public override void SetPlugin(SC.Interfaces.IScPluginClient plugin)
        {
            if (plugin != null && plugin is SC.Interfaces.IScServerPluginClient)
                this.plugin = plugin as SC.Interfaces.IScServerPluginClient;

            RefreshView();
        }
        private void RefreshView()
        {
            if (plugin.IsRunningStatus)
            {
                txtIsRunning.Text = "Running";
                txtIsRunning.BackColor = Color.DarkGreen;
            }
            else
            {
                txtIsRunning.Text = "Stopped";
                txtIsRunning.BackColor = Color.Red;
            }

            if (plugin.ShouldRunStatus)
            {
                txtShouldRun.Text = "Yes";
                txtShouldRun.BackColor = Color.DarkGreen;
            }
            else
            {
                txtShouldRun.Text = "No";
                txtShouldRun.BackColor = Color.Red;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }
    }
}
