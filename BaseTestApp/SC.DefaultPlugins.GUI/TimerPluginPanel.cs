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
    [SC.GUI.Utility.PluginPanel(typeof(SC.Interfaces.DefaultPlugins.ITimerPlugin))]
    public partial class TimerPluginPanel : SC.GUI.Utility.PluginPanelBase
    {
        private SC.Interfaces.DefaultPlugins.ITimerPlugin plugin = null;

        public TimerPluginPanel()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            dtDate.Value = DateTime.Now;
            base.OnLoad(e);
        }
        public override void SetPlugin(SC.Interfaces.IScPluginClient plugin)
        {
            this.plugin = plugin as SC.Interfaces.DefaultPlugins.ITimerPlugin;
            RefreshView();
        }
        private void RefreshView()
        {
            if (plugin == null || !plugin.HavePermission())
                return;

            (plugin.DefaultActive ? radEnabled : radDisabled).Checked = true;
            lstActivations.Items.Clear();
            lstActivations.Items.AddRange(plugin.GetActivations().ToArray());

            radTrueDuration.Checked = true;
            tsDuration.Number = tsRepeat.Number = 0;
            tsDuration.Unit = tsRepeat.Unit = SC.Interfaces.DefaultPlugins.TimeUnit.Minute;
        }

        private void tsRepeat_ValueChanged(object sender, EventArgs e)
        {
            bool repetitive = (tsRepeat.Number > 0);
            if (repetitive)
            {
                radStopEarly.Enabled = true;
            }
            else
            {
                radStopEarly.Enabled = false;
                radTrueDuration.Checked = true;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (plugin.HavePermission())
            {
                if (radEnabled.Checked != plugin.DefaultActive)
                    plugin.DefaultActive = radEnabled.Checked;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (plugin.HavePermission())
            {
                DateTime startTime = dtDate.Value;
                SC.Interfaces.DefaultPlugins.DurationType durType = (radTrueDuration.Enabled ? SC.Interfaces.DefaultPlugins.DurationType.Duration : SC.Interfaces.DefaultPlugins.DurationType.StopEarly);
                plugin.AddActivation(startTime, durType, this.tsDuration.Number, this.tsDuration.Unit, this.tsRepeat.Number, this.tsRepeat.Unit);
                RefreshView();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            SC.Interfaces.DefaultPlugins.IActivation act = lstActivations.SelectedItem as SC.Interfaces.DefaultPlugins.IActivation;
            if (act != null && plugin.HavePermission())
            {
                plugin.RemoveActivation(act);
            }
        }

        private void lstActivations_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lstActivations.SelectedIndex != -1 && plugin.HavePermission());
        }
    }
}
