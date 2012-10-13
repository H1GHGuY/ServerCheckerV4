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
    partial class SecurityManagerPermissionControl
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
            this.pnlPermissions = new System.Windows.Forms.TableLayoutPanel();
            this.lstObjects = new System.Windows.Forms.ListBox();
            this.lstOperations = new System.Windows.Forms.ListBox();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.lstPermissions = new System.Windows.Forms.ListBox();
            this.lblObjects = new System.Windows.Forms.Label();
            this.lblOperations = new System.Windows.Forms.Label();
            this.lblAllowedUsers = new System.Windows.Forms.Label();
            this.lblDisallowedUsers = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpPermissions = new System.Windows.Forms.GroupBox();
            this.pnlPermissions.SuspendLayout();
            this.grpPermissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPermissions
            // 
            this.pnlPermissions.ColumnCount = 5;
            this.pnlPermissions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlPermissions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlPermissions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlPermissions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.pnlPermissions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlPermissions.Controls.Add(this.lstObjects, 0, 1);
            this.pnlPermissions.Controls.Add(this.lstOperations, 1, 1);
            this.pnlPermissions.Controls.Add(this.lstUsers, 4, 1);
            this.pnlPermissions.Controls.Add(this.lstPermissions, 2, 1);
            this.pnlPermissions.Controls.Add(this.lblObjects, 0, 0);
            this.pnlPermissions.Controls.Add(this.lblOperations, 1, 0);
            this.pnlPermissions.Controls.Add(this.lblAllowedUsers, 2, 0);
            this.pnlPermissions.Controls.Add(this.lblDisallowedUsers, 4, 0);
            this.pnlPermissions.Controls.Add(this.btnAdd, 3, 2);
            this.pnlPermissions.Controls.Add(this.btnDelete, 3, 1);
            this.pnlPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPermissions.Location = new System.Drawing.Point(3, 16);
            this.pnlPermissions.Name = "pnlPermissions";
            this.pnlPermissions.RowCount = 4;
            this.pnlPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.pnlPermissions.Size = new System.Drawing.Size(639, 206);
            this.pnlPermissions.TabIndex = 0;
            // 
            // lstObjects
            // 
            this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.Location = new System.Drawing.Point(3, 23);
            this.lstObjects.Name = "lstObjects";
            this.pnlPermissions.SetRowSpan(this.lstObjects, 3);
            this.lstObjects.Size = new System.Drawing.Size(141, 212);
            this.lstObjects.TabIndex = 1;
            this.lstObjects.SelectedValueChanged += new System.EventHandler(this.lstObjects_SelectedValueChanged);
            // 
            // lstOperations
            // 
            this.lstOperations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOperations.FormattingEnabled = true;
            this.lstOperations.Location = new System.Drawing.Point(150, 23);
            this.lstOperations.Name = "lstOperations";
            this.pnlPermissions.SetRowSpan(this.lstOperations, 3);
            this.lstOperations.Size = new System.Drawing.Size(141, 212);
            this.lstOperations.TabIndex = 2;
            this.lstOperations.SelectedValueChanged += new System.EventHandler(this.lstOperations_SelectedValueChanged);
            // 
            // lstUsers
            // 
            this.lstUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(494, 23);
            this.lstUsers.Name = "lstUsers";
            this.pnlPermissions.SetRowSpan(this.lstUsers, 3);
            this.lstUsers.Size = new System.Drawing.Size(142, 212);
            this.lstUsers.TabIndex = 3;
            this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
            // 
            // lstPermissions
            // 
            this.lstPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPermissions.FormattingEnabled = true;
            this.lstPermissions.Location = new System.Drawing.Point(297, 23);
            this.lstPermissions.Name = "lstPermissions";
            this.pnlPermissions.SetRowSpan(this.lstPermissions, 3);
            this.lstPermissions.Size = new System.Drawing.Size(141, 212);
            this.lstPermissions.TabIndex = 4;
            this.lstPermissions.SelectedIndexChanged += new System.EventHandler(this.lstPermissions_SelectedIndexChanged);
            // 
            // lblObjects
            // 
            this.lblObjects.AutoSize = true;
            this.lblObjects.Location = new System.Drawing.Point(3, 0);
            this.lblObjects.Name = "lblObjects";
            this.lblObjects.Size = new System.Drawing.Size(46, 13);
            this.lblObjects.TabIndex = 7;
            this.lblObjects.Text = "Objects:";
            // 
            // lblOperations
            // 
            this.lblOperations.AutoSize = true;
            this.lblOperations.Location = new System.Drawing.Point(150, 0);
            this.lblOperations.Name = "lblOperations";
            this.lblOperations.Size = new System.Drawing.Size(61, 13);
            this.lblOperations.TabIndex = 8;
            this.lblOperations.Text = "Operations:";
            // 
            // lblAllowedUsers
            // 
            this.lblAllowedUsers.AutoSize = true;
            this.lblAllowedUsers.Location = new System.Drawing.Point(297, 0);
            this.lblAllowedUsers.Name = "lblAllowedUsers";
            this.lblAllowedUsers.Size = new System.Drawing.Size(77, 13);
            this.lblAllowedUsers.TabIndex = 9;
            this.lblAllowedUsers.Text = "Allowed Users:";
            // 
            // lblDisallowedUsers
            // 
            this.lblDisallowedUsers.AutoSize = true;
            this.lblDisallowedUsers.Location = new System.Drawing.Point(494, 0);
            this.lblDisallowedUsers.Name = "lblDisallowedUsers";
            this.lblDisallowedUsers.Size = new System.Drawing.Size(91, 13);
            this.lblDisallowedUsers.TabIndex = 10;
            this.lblDisallowedUsers.Text = "Disallowed Users:";
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(444, 58);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 29);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "<<";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(444, 23);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(44, 29);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = ">>";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grpPermissions
            // 
            this.grpPermissions.Controls.Add(this.pnlPermissions);
            this.grpPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPermissions.Location = new System.Drawing.Point(0, 0);
            this.grpPermissions.Name = "grpPermissions";
            this.grpPermissions.Size = new System.Drawing.Size(645, 225);
            this.grpPermissions.TabIndex = 1;
            this.grpPermissions.TabStop = false;
            this.grpPermissions.Text = "Permissions";
            // 
            // SecurityManagerPermissionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpPermissions);
            this.MinimumSize = new System.Drawing.Size(645, 225);
            this.Name = "SecurityManagerPermissionControl";
            this.Size = new System.Drawing.Size(645, 225);
            this.pnlPermissions.ResumeLayout(false);
            this.pnlPermissions.PerformLayout();
            this.grpPermissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlPermissions;
        private System.Windows.Forms.ListBox lstObjects;
        private System.Windows.Forms.ListBox lstOperations;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.ListBox lstPermissions;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblObjects;
        private System.Windows.Forms.Label lblOperations;
        private System.Windows.Forms.Label lblAllowedUsers;
        private System.Windows.Forms.Label lblDisallowedUsers;
        private System.Windows.Forms.GroupBox grpPermissions;
    }
}
