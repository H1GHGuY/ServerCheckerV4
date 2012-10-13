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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SC.GUI
{
    public partial class AddServerForm : Form
    {
        public AddServerForm()
        {
            InitializeComponent();
        }
        public System.Net.IPAddress Address
        {
            get
            {
                return ipAddress.Address;
            }
        }
        public string Username
        {
            get
            {
                return txtUser.Text;
            }
        }
        public string Password
        {
            get
            {
                return txtPass.Text;
            }
        }

        private void CheckOK()
        {
            btnOK.Enabled = (txtUser.Text != string.Empty && txtPass.Text != string.Empty && ipAddress.Address != null);
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            CheckOK();
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            CheckOK();
        }

        private void ipAddress_IPChanged(object sender, EventArgs e)
        {
            CheckOK();
        }
    }
}
