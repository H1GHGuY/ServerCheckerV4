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
    partial class ServerPluginPanel
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
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblShouldRun = new System.Windows.Forms.Label();
            this.lblIsRunning = new System.Windows.Forms.Label();
            this.txtShouldRun = new System.Windows.Forms.TextBox();
            this.txtIsRunning = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grpStatus.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpStatus
            // 
            this.grpStatus.Controls.Add(this.tableLayoutPanel1);
            this.grpStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStatus.Location = new System.Drawing.Point(0, 0);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(319, 73);
            this.grpStatus.TabIndex = 0;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "Status:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.lblShouldRun, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblIsRunning, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtShouldRun, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtIsRunning, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRefresh, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(313, 54);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblShouldRun
            // 
            this.lblShouldRun.AutoSize = true;
            this.lblShouldRun.Location = new System.Drawing.Point(3, 0);
            this.lblShouldRun.Name = "lblShouldRun";
            this.lblShouldRun.Size = new System.Drawing.Size(66, 13);
            this.lblShouldRun.TabIndex = 0;
            this.lblShouldRun.Text = "Should Run:";
            // 
            // lblIsRunning
            // 
            this.lblIsRunning.AutoSize = true;
            this.lblIsRunning.Location = new System.Drawing.Point(3, 27);
            this.lblIsRunning.Name = "lblIsRunning";
            this.lblIsRunning.Size = new System.Drawing.Size(61, 13);
            this.lblIsRunning.TabIndex = 1;
            this.lblIsRunning.Text = "Is Running:";
            // 
            // txtShouldRun
            // 
            this.txtShouldRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShouldRun.Location = new System.Drawing.Point(78, 3);
            this.txtShouldRun.Name = "txtShouldRun";
            this.txtShouldRun.ReadOnly = true;
            this.txtShouldRun.Size = new System.Drawing.Size(157, 20);
            this.txtShouldRun.TabIndex = 2;
            // 
            // txtIsRunning
            // 
            this.txtIsRunning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIsRunning.Location = new System.Drawing.Point(78, 30);
            this.txtIsRunning.Name = "txtIsRunning";
            this.txtIsRunning.ReadOnly = true;
            this.txtIsRunning.Size = new System.Drawing.Size(157, 20);
            this.txtIsRunning.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Location = new System.Drawing.Point(241, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(69, 21);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ServerPluginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpStatus);
            this.Name = "ServerPluginPanel";
            this.Size = new System.Drawing.Size(319, 73);
            this.grpStatus.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblShouldRun;
        private System.Windows.Forms.Label lblIsRunning;
        private System.Windows.Forms.TextBox txtShouldRun;
        private System.Windows.Forms.TextBox txtIsRunning;
        private System.Windows.Forms.Button btnRefresh;
    }
}
