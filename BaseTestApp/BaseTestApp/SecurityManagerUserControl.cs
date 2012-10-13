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
    public partial class SecurityManagerUserControl : UserControl
    {
        private SC.Interfaces.ISecurityManager secMan;
        public SecurityManagerUserControl()
        {
            InitializeComponent();
        }
        public SC.Interfaces.ISecurityManager SecurityManager
        {
            set
            {
                if (value != null)
                    secMan = value;
                ReloadView();
            }
        }
        protected void ReloadView()
        {
            lstUsers.Items.Clear();
            lstUsers.Items.AddRange(secMan.GetUsers());
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (SC.GUI.Utility.RequestString req = new SC.GUI.Utility.RequestString())
            {
                req.QuestionString = "Please enter the username";
                if (req.ShowDialog() != DialogResult.OK)
                    return;

                string username = req.UserInput;
                req.UserInput = "";

                req.QuestionString = "Please enter " + username + "'s password.";
                req.IsPassword = true;
                if (req.ShowDialog() != DialogResult.OK)
                    return;

                string password = req.UserInput;
                secMan.AddUser(username, password);
            }
            ReloadView();
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                try
                {
                    secMan.RemoveUser(lstUsers.SelectedItem.ToString());
                }
                catch (Exception ex)
                {
                    SC.GUI.Utility.ErrorForm.ShowErrorForm(ex);
                }
            }
            ReloadView();
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            string username = lstUsers.SelectedItem.ToString();
            using (SC.GUI.Utility.RequestString req = new SC.GUI.Utility.RequestString())
            {
                req.QuestionString = "Please enter password for " + username;
                req.IsPassword = true;
                if (req.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    secMan.SetPassword(username, req.UserInput);
                }
                catch (SC.Interfaces.SCException sce)
                {
                    SC.GUI.Utility.ErrorForm.ShowErrorForm(sce);
                }
                catch (System.Security.SecurityException se)
                {
                    SC.GUI.Utility.ErrorForm.ShowErrorForm(se);
                }
            }
        }

    }
}
