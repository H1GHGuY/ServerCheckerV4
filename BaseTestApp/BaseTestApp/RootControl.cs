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

namespace SC.GUI
{
    public partial class RootControl : UserControl
    {
        private SC.Interfaces.IRoot root;

        public RootControl()
        {
            InitializeComponent();
        }

        public SC.Interfaces.IRoot Root
        {
            set
            {
                if (value != null)
                    root = value;
                RefreshView();
            }
        }
        private void RefreshView()
        {
            lstPlugins.Items.Clear();
            lstPlugins.Items.AddRange(root.Plugins);

            lstServers.Items.Clear();
            lstServers.Items.AddRange(root.Servers);
        }

        private void lstServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelServer.Enabled = (lstServers.SelectedIndex != -1);
        }

        private void lstPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelPlugin.Enabled = (lstPlugins.SelectedIndex != -1);
        }

        private void btnAddServer_Click(object sender, EventArgs e)
        {
            using (SC.GUI.Utility.RequestString req = new SC.GUI.Utility.RequestString())
            {
                req.QuestionString = "Please enter the name of the server:";
                if (req.ShowDialog() != DialogResult.OK)
                    return;

                string servername = req.UserInput;
                root.AddServer(servername);
                RefreshView();
            }
        }

        private void btnDelServer_Click(object sender, EventArgs e)
        {
            if (lstServers.SelectedItem != null)
            {
                root.RemoveServer(lstServers.SelectedItem.ToString());
                RefreshView();
            }
        }

        private void btnAddPlugin_Click(object sender, EventArgs e)
        {
            using (PluginSelector selector = new PluginSelector())
            {
                selector.Plugins = root.GetPluginLoader().StandalonePlugins;
                if (selector.ShowDialog() != DialogResult.OK)
                    return;

                root.AddPlugin(selector.SelectedPlugin);
                RefreshView();
            }
        }

        private void btnDelPlugin_Click(object sender, EventArgs e)
        {
            if (lstPlugins.SelectedItem != null)
            {
                root.RemovePlugin(lstPlugins.SelectedItem.ToString());
                RefreshView();
            }
        }
    }
}
