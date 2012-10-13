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

namespace SC.GUI
{
    partial class SecurityManagerUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlUsers = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnDelUser = new System.Windows.Forms.Button();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.grpUsers = new System.Windows.Forms.GroupBox();
            this.pnlUsers.SuspendLayout();
            this.grpUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUsers
            // 
            this.pnlUsers.ColumnCount = 2;
            this.pnlUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlUsers.Controls.Add(this.btnAddUser, 1, 0);
            this.pnlUsers.Controls.Add(this.btnDelUser, 1, 1);
            this.pnlUsers.Controls.Add(this.btnSetPassword, 1, 2);
            this.pnlUsers.Controls.Add(this.lstUsers, 0, 0);
            this.pnlUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUsers.Location = new System.Drawing.Point(3, 16);
            this.pnlUsers.Name = "pnlUsers";
            this.pnlUsers.RowCount = 3;
            this.pnlUsers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlUsers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlUsers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.pnlUsers.Size = new System.Drawing.Size(294, 156);
            this.pnlUsers.TabIndex = 0;
            // 
            // btnAddUser
            // 
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.Location = new System.Drawing.Point(197, 3);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(94, 24);
            this.btnAddUser.TabIndex = 2;
            this.btnAddUser.Text = "Add User...";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnDelUser
            // 
            this.btnDelUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelUser.Location = new System.Drawing.Point(197, 33);
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.Size = new System.Drawing.Size(94, 24);
            this.btnDelUser.TabIndex = 3;
            this.btnDelUser.Text = "Delete User";
            this.btnDelUser.UseVisualStyleBackColor = true;
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // btnSetPassword
            // 
            this.btnSetPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetPassword.Location = new System.Drawing.Point(197, 63);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(94, 24);
            this.btnSetPassword.TabIndex = 4;
            this.btnSetPassword.Text = "Set Password...";
            this.btnSetPassword.UseVisualStyleBackColor = true;
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            // 
            // lstUsers
            // 
            this.lstUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(3, 3);
            this.lstUsers.Name = "lstUsers";
            this.pnlUsers.SetRowSpan(this.lstUsers, 3);
            this.lstUsers.Size = new System.Drawing.Size(188, 147);
            this.lstUsers.TabIndex = 5;
            // 
            // grpUsers
            // 
            this.grpUsers.Controls.Add(this.pnlUsers);
            this.grpUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpUsers.Location = new System.Drawing.Point(0, 0);
            this.grpUsers.Name = "grpUsers";
            this.grpUsers.Size = new System.Drawing.Size(300, 175);
            this.grpUsers.TabIndex = 1;
            this.grpUsers.TabStop = false;
            this.grpUsers.Text = "Users:";
            // 
            // SecurityManagerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpUsers);
            this.MinimumSize = new System.Drawing.Size(300, 175);
            this.Name = "SecurityManagerUserControl";
            this.Size = new System.Drawing.Size(300, 175);
            this.pnlUsers.ResumeLayout(false);
            this.grpUsers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlUsers;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnDelUser;
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.GroupBox grpUsers;
    }
}
