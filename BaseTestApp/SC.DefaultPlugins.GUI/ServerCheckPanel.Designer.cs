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

namespace SC.DefaultPlugins.GUI
{
    partial class ServerCheckPanel
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.cmbGameType = new System.Windows.Forms.ComboBox();
            this.txtCustomCommand = new System.Windows.Forms.TextBox();
            this.lblServerType = new System.Windows.Forms.Label();
            this.lblCustomCommand = new System.Windows.Forms.Label();
            this.lblTimeUnit = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.lblTimeout, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbGameType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtCustomCommand, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblServerType, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomCommand, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTimeUnit, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnApply, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRefresh, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtTimeout, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(380, 88);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(3, 0);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(48, 13);
            this.lblTimeout.TabIndex = 0;
            this.lblTimeout.Text = "Timeout:";
            // 
            // cmbGameType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cmbGameType, 2);
            this.cmbGameType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGameType.FormattingEnabled = true;
            this.cmbGameType.Location = new System.Drawing.Point(113, 33);
            this.cmbGameType.Name = "cmbGameType";
            this.cmbGameType.Size = new System.Drawing.Size(109, 21);
            this.cmbGameType.TabIndex = 2;
            // 
            // txtCustomCommand
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtCustomCommand, 3);
            this.txtCustomCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCustomCommand.Enabled = false;
            this.txtCustomCommand.Location = new System.Drawing.Point(113, 63);
            this.txtCustomCommand.Name = "txtCustomCommand";
            this.txtCustomCommand.Size = new System.Drawing.Size(189, 20);
            this.txtCustomCommand.TabIndex = 3;
            // 
            // lblServerType
            // 
            this.lblServerType.AutoSize = true;
            this.lblServerType.Location = new System.Drawing.Point(3, 30);
            this.lblServerType.Name = "lblServerType";
            this.lblServerType.Size = new System.Drawing.Size(68, 13);
            this.lblServerType.TabIndex = 4;
            this.lblServerType.Text = "Server Type:";
            // 
            // lblCustomCommand
            // 
            this.lblCustomCommand.AutoSize = true;
            this.lblCustomCommand.Location = new System.Drawing.Point(3, 60);
            this.lblCustomCommand.Name = "lblCustomCommand";
            this.lblCustomCommand.Size = new System.Drawing.Size(95, 13);
            this.lblCustomCommand.TabIndex = 5;
            this.lblCustomCommand.Text = "Custom Command:";
            // 
            // lblTimeUnit
            // 
            this.lblTimeUnit.AutoSize = true;
            this.lblTimeUnit.Location = new System.Drawing.Point(188, 0);
            this.lblTimeUnit.Name = "lblTimeUnit";
            this.lblTimeUnit.Size = new System.Drawing.Size(32, 13);
            this.lblTimeUnit.TabIndex = 6;
            this.lblTimeUnit.Text = "msec";
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.Location = new System.Drawing.Point(308, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(69, 24);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Location = new System.Drawing.Point(308, 33);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(69, 24);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(113, 3);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(69, 20);
            this.txtTimeout.TabIndex = 9;
            // 
            // ServerCheckPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ServerCheckPanel";
            this.Size = new System.Drawing.Size(388, 98);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.ComboBox cmbGameType;
        private System.Windows.Forms.TextBox txtCustomCommand;
        private System.Windows.Forms.Label lblServerType;
        private System.Windows.Forms.Label lblCustomCommand;
        private System.Windows.Forms.Label lblTimeUnit;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtTimeout;
    }
}
