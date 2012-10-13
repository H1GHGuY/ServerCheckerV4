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

namespace SC.GUI.Utility
{
    public partial class IPTextBox : UserControl
    {
        public IPTextBox()
        {
            InitializeComponent();
            txtIP.Text = string.Empty;
        }

        private void txtIP_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = (txtIP.Text != string.Empty && (txtIP.Text.Split('.').Length != 4 || Address == null));
            if (e.Cancel)
                txtIP.SelectAll();
        }
        public System.Net.IPAddress Address
        {
            get
            {
                System.Net.IPAddress address = null;
                System.Net.IPAddress.TryParse(txtIP.Text, out address);
                return address;
            }
        }
        public void Clear()
        {
            txtIP.Text = string.Empty;
        }

        public event EventHandler IPChanged;

        private void txtIP_TextChanged(object sender, EventArgs e)
        {
            if (IPChanged != null)
                IPChanged(this, e);
        }
    }
}
