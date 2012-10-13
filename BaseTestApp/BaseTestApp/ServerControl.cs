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
    public partial class ServerControl : UserControl
    {
        private SC.Interfaces.IServer server;
        private SC.Interfaces.IPluginLoader pluginLoader;

        public ServerControl()
        {
            InitializeComponent();
            cmbProtocol.Items.Add(System.Net.TransportType.Tcp);
            cmbProtocol.Items.Add(System.Net.TransportType.Udp);
        }

        public SC.Interfaces.IServer Server
        {
            set
            {
                if (value != null)
                    server = value;
                ReloadServer();
            }
        }
        public SC.Interfaces.IPluginLoader PluginLoader
        {
            set
            {
                if (value != null)
                    pluginLoader = value;
            }
        }

        private void ReloadServer()
        {
            txtName.Text = server.GetName();
            
            // Settings
            txtStartupTimeout.Text = server.StartupTimeout.ToString();

            txtWorkingDir.Text = server.WorkingDirectory;
            txtExecutable.Text = server.Executable;
            txtArguments.Text = server.Arguments;
            chkAcquire.Checked = server.AcquireOnStart;
            chkStop.Checked = server.StopOnExit;
            txtIP.Text = server.EndPoint.Address.ToString();
            txtPort.Text= server.EndPoint.Port.ToString();
            cmbProtocol.SelectedItem = server.Protocol;

            txtWorkingDir.Enabled =
                txtExecutable.Enabled =
                txtArguments.Enabled =
                chkAcquire.Enabled =
                chkStop.Enabled =
                txtIP.Enabled =
                txtPort.Enabled =
                cmbProtocol.Enabled =
                server.HavePermission(SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);

            switch (server.ServerStatus)
            {
                case SC.Interfaces.ServerConstants.ServerStatus.Stopped:
                    txtName.BackColor = System.Drawing.Color.Red;
                    break;
                case SC.Interfaces.ServerConstants.ServerStatus.Starting:
                    txtName.BackColor = System.Drawing.Color.Orange;
                    break;
                case SC.Interfaces.ServerConstants.ServerStatus.Started:
                    txtName.BackColor = System.Drawing.Color.Green;
                    break;
            }
            
            // Credentials
            if (server.HavePermission(SC.Interfaces.ServerConstants.CREDENTIALS_PERMISSION))
            {
                grpCredentials.Enabled = true;
                txtUser.Text = server.Username;
                txtPass1.Text = txtPass2.Text = server.Password;
            }
            else
                grpCredentials.Enabled = false;

            // Plugins
            lstPlugins.Items.Clear();
            lstPlugins.Items.AddRange(server.Plugins);
            btnAdd.Enabled = btnDelete.Enabled = server.HavePermission(SC.Interfaces.ServerConstants.PLUGINS_PERMISSION);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ReloadServer();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (txtPass1.Text != txtPass2.Text)
            {
                MessageBox.Show("Passwords don't match.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass1.Clear();
                txtPass2.Clear();
                txtPass1.Select();
                return;
            }
            System.Net.TransportType tt = (System.Net.TransportType)cmbProtocol.SelectedItem;
            if (cmbProtocol.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a protocol.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbProtocol.SelectAll();
                return;
            }
            uint timeout;
            if (!uint.TryParse(txtStartupTimeout.Text, out timeout))
            {
                MessageBox.Show("Please enter a valid timeout.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStartupTimeout.SelectAll();
                return;
            }
            System.Net.IPAddress address;
            if (!System.Net.IPAddress.TryParse(txtIP.Text, out address))
            {
                MessageBox.Show("Please enter a valid address.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIP.SelectAll();
                return;
            }
            ushort port;
            if (!ushort.TryParse(txtPort.Text, out port))
            {
                MessageBox.Show("Please enter a valid port.", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.SelectAll();
                return;
            }
            string workingDir = txtWorkingDir.Text;
            string executable = txtExecutable.Text;
            string arguments = txtArguments.Text;
            bool acquire = chkAcquire.Checked;
            bool stop = chkStop.Checked;
            string username = txtUser.Text;
            string password = txtPass1.Text;

            // end of client-validation
            if (server.StartupTimeout != timeout)
                server.StartupTimeout = (int)timeout;

            try
            {
                System.Net.IPEndPoint ep = new System.Net.IPEndPoint(address, port);
                if (ep != server.EndPoint)
                    server.EndPoint = ep;
                if (tt != server.Protocol)
                    server.Protocol = tt;
                if (server.AcquireOnStart != acquire)
                    server.AcquireOnStart = acquire;
                if (server.StopOnExit != stop)
                    server.StopOnExit = stop;
                if (server.WorkingDirectory != workingDir)
                    server.WorkingDirectory = workingDir;
                if (server.Executable != executable)
                    server.Executable = executable;
                if (server.Arguments != arguments)
                    server.Arguments = arguments;
            }
            catch (System.Security.SecurityException se)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(se);
            }

            try
            {
                if (server.Username != username)
                    server.Username = username;
                if (server.Password != password)
                    server.Password = password;
            }
            catch (System.Security.SecurityException se)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(se);
            }
            ReloadServer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (PluginSelector selector = new PluginSelector())
            {
                selector.Plugins = pluginLoader.ServerPlugins;
                if (selector.ShowDialog() == DialogResult.OK && selector.SelectedPlugin != null)
                    server.AddPlugin(selector.SelectedPlugin);
            }
            ReloadServer();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstPlugins.SelectedItem != null)
                server.RemovePlugin(lstPlugins.SelectedItem.ToString());
            ReloadServer();
        }
    }
}
