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
    [SC.GUI.Utility.PluginPanel(typeof(SC.Interfaces.DefaultPlugins.IServerCheckPlugin))]
    public partial class ServerCheckPanel : SC.GUI.Utility.PluginPanelBase
    {
        private SC.Interfaces.DefaultPlugins.IServerCheckPlugin plugin;
        
        public ServerCheckPanel()
        {
            InitializeComponent();

            foreach (SC.Interfaces.DefaultPlugins.eGameType type in Enum.GetValues(typeof(SC.Interfaces.DefaultPlugins.eGameType)))
            {
                cmbGameType.Items.Add(type);
            }
        }

        public override void SetPlugin(SC.Interfaces.IScPluginClient plugin)
        {
            if (plugin == null)
                throw new ArgumentNullException("plugin");
            else if (!(plugin is SC.Interfaces.DefaultPlugins.IServerCheckPlugin))
                throw new ArgumentException("plugin");

            this.plugin = plugin as SC.Interfaces.DefaultPlugins.IServerCheckPlugin;
            RefreshView();
        }

        private string ConvertToString(byte[] bytes)
        {
            System.Text.StringBuilder builder = new StringBuilder(bytes.Length * 4);
            foreach (byte b in bytes)
            {
                builder.Append("\\x");
                builder.Append(b.ToString("X"));
            }
            return builder.ToString();
        }

        private byte[] ConvertToBytes(string txt)
        {
            List<byte> bytes = new List<byte>(txt.Length / 3);
            string[] allHex = txt.Split(new char[] { '\\', 'x' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string hex in allHex)
            {
                bytes.Add(Convert.ToByte(hex, 16));
            }
            return bytes.ToArray();
        }

        private void RefreshView()
        {
            if (plugin != null)
            {
                txtTimeout.Text = plugin.Timeout.ToString();
                SC.Interfaces.DefaultPlugins.eGameType type;
                cmbGameType.SelectedItem = type = plugin.GameType;

                if ((txtCustomCommand.Enabled = (type == SC.Interfaces.DefaultPlugins.eGameType.Custom)))
                {
                    txtCustomCommand.Text = ConvertToString(plugin.CustomCommand);
                }
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
                int timeout = Convert.ToInt32(txtTimeout.Text);
                SC.Interfaces.DefaultPlugins.eGameType type = (SC.Interfaces.DefaultPlugins.eGameType)cmbGameType.SelectedItem;

                byte[] CustomCommand = ConvertToBytes(txtCustomCommand.Text);

                plugin.Timeout = timeout;
                plugin.CustomCommand = CustomCommand;
                plugin.GameType = type;
            }
        }

    }
}
