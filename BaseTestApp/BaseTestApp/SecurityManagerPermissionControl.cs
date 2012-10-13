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
    public partial class SecurityManagerPermissionControl : UserControl
    {
        private SC.Interfaces.ISecurityManager secMan;

        public SecurityManagerPermissionControl()
        {
            InitializeComponent();
        }

        public SC.Interfaces.ISecurityManager SecurityManager
        {
            set
            {
                if (value != null)
                {
                    secMan = value;
                }
                ReloadView();
            }
        }
        private void ReloadView()
        {
            lstObjects.Items.Clear();
            lstObjects.Items.AddRange(secMan.GetSubjects());
        }
        private void ReloadUser()
        {
            lstPermissions.Items.Clear();
            lstUsers.Items.Clear();
            List<string> allowedUsers = new List<string>();
            List<string> disallowedUsers = new List<string>();
            allowedUsers.AddRange(secMan.GetPermissions(lstObjects.SelectedItem.ToString(), lstOperations.SelectedItem.ToString()));
            disallowedUsers.AddRange(secMan.GetUsers());
            foreach (string user in allowedUsers)
            {
                disallowedUsers.Remove(user);
            }
            lstPermissions.Items.AddRange(allowedUsers.ToArray());
            lstUsers.Items.AddRange(disallowedUsers.ToArray());
        }

        private void lstObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            lstOperations.Items.Clear();
            lstOperations.Items.AddRange(secMan.GetAdditionalOperations(lstObjects.SelectedItem.ToString()));
            lstPermissions.Items.Clear();
            lstUsers.Items.Clear();
        }

        private void lstOperations_SelectedValueChanged(object sender, EventArgs e)
        {
            ReloadUser();
        }

        private void lstPermissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = lstPermissions.SelectedIndex != -1;
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = lstUsers.SelectedIndex != -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            secMan.AddPermission(lstObjects.SelectedItem.ToString(), lstUsers.SelectedItem.ToString(), lstOperations.SelectedItem.ToString());
            ReloadUser();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            secMan.RemovePermission(lstObjects.SelectedItem.ToString(), lstPermissions.SelectedItem.ToString(), lstOperations.SelectedItem.ToString());
            ReloadUser();
        }
    }
}
