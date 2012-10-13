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
    public partial class SecurityManagerIPControl : UserControl
    {
        private SC.Interfaces.ISecurityManager secMan;
        
        public SecurityManagerIPControl()
        {
            InitializeComponent();
        }

        public SC.Interfaces.ISecurityManager SecurityManager
        {
            set
            {
                if (value != null)
                    secMan = value;
                RefreshView();
            }
        }

        private void RefreshView()
        {
            lstNetworks.Items.Clear();
            lstNetworks.Items.AddRange(secMan.GetAllowedClientNetworks());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            System.Net.IPAddress address = ipAddress.Address;
            System.Net.IPAddress netmask = ipNetmask.Address;

            if (address == null || netmask == null)
            {
                MessageBox.Show("Invalid IP address or netmask.");
                return;
            }

            try
            {
                secMan.AddAllowedClientNetwork(address, netmask);
            }
            catch (Exception ex)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(ex);
            }
            ipAddress.Clear();
            ipNetmask.Clear();
            RefreshView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SC.Interfaces.INetwork network = lstNetworks.SelectedItem as SC.Interfaces.INetwork;
            try
            {
                secMan.RemoveAllowedClientNetwork(network);
            }
            catch (Exception ex)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(ex);
            }
            RefreshView();
        }
    }
}
