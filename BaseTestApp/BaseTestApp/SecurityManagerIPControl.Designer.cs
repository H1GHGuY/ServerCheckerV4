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
    partial class SecurityManagerIPControl
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
            this.pnlIP = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ipNetmask = new SC.GUI.Utility.IPTextBox();
            this.ipAddress = new SC.GUI.Utility.IPTextBox();
            this.lstNetworks = new System.Windows.Forms.ListBox();
            this.grpIP = new System.Windows.Forms.GroupBox();
            this.pnlIP.SuspendLayout();
            this.grpIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIP
            // 
            this.pnlIP.ColumnCount = 3;
            this.pnlIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.pnlIP.Controls.Add(this.btnAdd, 2, 0);
            this.pnlIP.Controls.Add(this.btnDelete, 2, 1);
            this.pnlIP.Controls.Add(this.ipNetmask, 1, 0);
            this.pnlIP.Controls.Add(this.ipAddress, 0, 0);
            this.pnlIP.Controls.Add(this.lstNetworks, 0, 1);
            this.pnlIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIP.Location = new System.Drawing.Point(3, 16);
            this.pnlIP.Name = "pnlIP";
            this.pnlIP.RowCount = 3;
            this.pnlIP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlIP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlIP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.pnlIP.Size = new System.Drawing.Size(282, 149);
            this.pnlIP.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Location = new System.Drawing.Point(197, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(82, 24);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(197, 33);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 24);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ipNetmask
            // 
            this.ipNetmask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipNetmask.Location = new System.Drawing.Point(100, 3);
            this.ipNetmask.Name = "ipNetmask";
            this.ipNetmask.Size = new System.Drawing.Size(91, 24);
            this.ipNetmask.TabIndex = 3;
            // 
            // ipAddress
            // 
            this.ipAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipAddress.Location = new System.Drawing.Point(3, 3);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(91, 24);
            this.ipAddress.TabIndex = 4;
            // 
            // lstNetworks
            // 
            this.pnlIP.SetColumnSpan(this.lstNetworks, 2);
            this.lstNetworks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNetworks.FormattingEnabled = true;
            this.lstNetworks.Location = new System.Drawing.Point(3, 33);
            this.lstNetworks.Name = "lstNetworks";
            this.pnlIP.SetRowSpan(this.lstNetworks, 2);
            this.lstNetworks.Size = new System.Drawing.Size(188, 108);
            this.lstNetworks.TabIndex = 6;
            // 
            // grpIP
            // 
            this.grpIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grpIP.Controls.Add(this.pnlIP);
            this.grpIP.Location = new System.Drawing.Point(3, 3);
            this.grpIP.Name = "grpIP";
            this.grpIP.Size = new System.Drawing.Size(288, 168);
            this.grpIP.TabIndex = 1;
            this.grpIP.TabStop = false;
            this.grpIP.Text = "Network";
            // 
            // SecurityManagerIPControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpIP);
            this.MinimumSize = new System.Drawing.Size(300, 175);
            this.Name = "SecurityManagerIPControl";
            this.Size = new System.Drawing.Size(300, 175);
            this.pnlIP.ResumeLayout(false);
            this.grpIP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlIP;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private SC.GUI.Utility.IPTextBox ipNetmask;
        private SC.GUI.Utility.IPTextBox ipAddress;
        private System.Windows.Forms.ListBox lstNetworks;
        private System.Windows.Forms.GroupBox grpIP;
    }
}
