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
    [SC.GUI.Utility.PluginPanel(typeof(SC.Interfaces.DefaultPlugins.IManualControl))]
    public partial class ManualControlPanel : SC.GUI.Utility.PluginPanelBase
    {
        private SC.Interfaces.DefaultPlugins.IManualControl plugin = null;

        public ManualControlPanel()
        {
            InitializeComponent();
        }

        public override void SetPlugin(SC.Interfaces.IScPluginClient plugin)
        {
            if (plugin == null)
                throw new ArgumentNullException("plugin");
            else if (!(plugin is SC.Interfaces.DefaultPlugins.IManualControl))
                throw new ArgumentException("plugin");

            this.plugin = plugin as SC.Interfaces.DefaultPlugins.IManualControl;
            RefreshView();
        }

        private void RefreshView()
        {
            if (plugin != null)
            {
                bool Running = plugin.Running;
                txtRunning.Text = (Running ? "" : "Halting Server...");
                txtRunning.BackColor = (Running ? Color.DarkGreen : Color.Red);

                btnStop.Enabled = Running;
                btnStart.Enabled = !Running;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (plugin != null)
                plugin.Start();
            RefreshView();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (plugin != null)
                plugin.Stop();
            RefreshView();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }
    }
}
