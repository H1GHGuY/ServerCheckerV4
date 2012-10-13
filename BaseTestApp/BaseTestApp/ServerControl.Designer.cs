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
    partial class ServerControl
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lstPlugins = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.grpPlugins = new System.Windows.Forms.GroupBox();
            this.pnlPlugins = new System.Windows.Forms.TableLayoutPanel();
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.pnlCredentials = new System.Windows.Forms.TableLayoutPanel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass1 = new System.Windows.Forms.TextBox();
            this.txtPass2 = new System.Windows.Forms.TextBox();
            this.grpProcess = new System.Windows.Forms.GroupBox();
            this.pnlProcess = new System.Windows.Forms.TableLayoutPanel();
            this.lblWorkingDirectory = new System.Windows.Forms.Label();
            this.lblExecutable = new System.Windows.Forms.Label();
            this.lblArguments = new System.Windows.Forms.Label();
            this.chkAcquire = new System.Windows.Forms.CheckBox();
            this.chkStop = new System.Windows.Forms.CheckBox();
            this.txtWorkingDir = new System.Windows.Forms.TextBox();
            this.txtExecutable = new System.Windows.Forms.TextBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.lblStartupTimeout = new System.Windows.Forms.Label();
            this.txtStartupTimeout = new System.Windows.Forms.TextBox();
            this.grpPlugins.SuspendLayout();
            this.pnlPlugins.SuspendLayout();
            this.grpCredentials.SuspendLayout();
            this.pnlCredentials.SuspendLayout();
            this.grpProcess.SuspendLayout();
            this.pnlProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(3, 25);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(61, 13);
            this.lblIP.TabIndex = 1;
            this.lblIP.Text = "IP-Address:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(3, 50);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(106, 3);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(165, 20);
            this.txtName.TabIndex = 5;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(106, 53);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(50, 20);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "65535";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(106, 28);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(97, 20);
            this.txtIP.TabIndex = 8;
            this.txtIP.Text = "255.255.255.255";
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Location = new System.Drawing.Point(106, 78);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(97, 21);
            this.cmbProtocol.TabIndex = 9;
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(3, 75);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(49, 13);
            this.lblProtocol.TabIndex = 10;
            this.lblProtocol.Text = "Protocol:";
            // 
            // lstPlugins
            // 
            this.lstPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPlugins.FormattingEnabled = true;
            this.lstPlugins.Location = new System.Drawing.Point(3, 3);
            this.lstPlugins.Name = "lstPlugins";
            this.pnlPlugins.SetRowSpan(this.lstPlugins, 2);
            this.lstPlugins.Size = new System.Drawing.Size(248, 134);
            this.lstPlugins.TabIndex = 11;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(257, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 28);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add Plugin...";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(257, 37);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 28);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Delete Plugin";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(289, 12);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(74, 28);
            this.btnReload.TabIndex = 9;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(289, 43);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(74, 28);
            this.btnApply.TabIndex = 10;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // grpPlugins
            // 
            this.grpPlugins.Controls.Add(this.pnlPlugins);
            this.grpPlugins.Location = new System.Drawing.Point(3, 390);
            this.grpPlugins.Name = "grpPlugins";
            this.grpPlugins.Size = new System.Drawing.Size(360, 160);
            this.grpPlugins.TabIndex = 14;
            this.grpPlugins.TabStop = false;
            this.grpPlugins.Text = "Plugins:";
            // 
            // pnlPlugins
            // 
            this.pnlPlugins.ColumnCount = 2;
            this.pnlPlugins.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlPlugins.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlPlugins.Controls.Add(this.lstPlugins, 0, 0);
            this.pnlPlugins.Controls.Add(this.btnAdd, 1, 0);
            this.pnlPlugins.Controls.Add(this.btnDelete, 1, 1);
            this.pnlPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlugins.Location = new System.Drawing.Point(3, 16);
            this.pnlPlugins.Name = "pnlPlugins";
            this.pnlPlugins.RowCount = 2;
            this.pnlPlugins.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.pnlPlugins.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlPlugins.Size = new System.Drawing.Size(354, 141);
            this.pnlPlugins.TabIndex = 0;
            // 
            // grpCredentials
            // 
            this.grpCredentials.Controls.Add(this.pnlCredentials);
            this.grpCredentials.Location = new System.Drawing.Point(3, 289);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(277, 95);
            this.grpCredentials.TabIndex = 15;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Credentials:";
            // 
            // pnlCredentials
            // 
            this.pnlCredentials.ColumnCount = 2;
            this.pnlCredentials.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.pnlCredentials.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlCredentials.Controls.Add(this.lblUser, 0, 0);
            this.pnlCredentials.Controls.Add(this.lblPassword, 0, 1);
            this.pnlCredentials.Controls.Add(this.txtUser, 1, 0);
            this.pnlCredentials.Controls.Add(this.txtPass1, 1, 1);
            this.pnlCredentials.Controls.Add(this.txtPass2, 1, 2);
            this.pnlCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCredentials.Location = new System.Drawing.Point(3, 16);
            this.pnlCredentials.Name = "pnlCredentials";
            this.pnlCredentials.RowCount = 3;
            this.pnlCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlCredentials.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlCredentials.Size = new System.Drawing.Size(271, 76);
            this.pnlCredentials.TabIndex = 0;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(3, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(58, 13);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "Username:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(3, 25);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password:";
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(78, 3);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(190, 20);
            this.txtUser.TabIndex = 2;
            // 
            // txtPass1
            // 
            this.txtPass1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPass1.Location = new System.Drawing.Point(78, 28);
            this.txtPass1.Name = "txtPass1";
            this.txtPass1.PasswordChar = '-';
            this.txtPass1.Size = new System.Drawing.Size(190, 20);
            this.txtPass1.TabIndex = 3;
            // 
            // txtPass2
            // 
            this.txtPass2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPass2.Location = new System.Drawing.Point(78, 53);
            this.txtPass2.Name = "txtPass2";
            this.txtPass2.PasswordChar = '-';
            this.txtPass2.Size = new System.Drawing.Size(190, 20);
            this.txtPass2.TabIndex = 4;
            // 
            // grpProcess
            // 
            this.grpProcess.Controls.Add(this.pnlProcess);
            this.grpProcess.Location = new System.Drawing.Point(3, 4);
            this.grpProcess.Name = "grpProcess";
            this.grpProcess.Size = new System.Drawing.Size(280, 279);
            this.grpProcess.TabIndex = 16;
            this.grpProcess.TabStop = false;
            this.grpProcess.Text = "Process:";
            // 
            // pnlProcess
            // 
            this.pnlProcess.ColumnCount = 2;
            this.pnlProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.9562F));
            this.pnlProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.0438F));
            this.pnlProcess.Controls.Add(this.cmbProtocol, 1, 3);
            this.pnlProcess.Controls.Add(this.txtPort, 1, 2);
            this.pnlProcess.Controls.Add(this.lblProtocol, 0, 3);
            this.pnlProcess.Controls.Add(this.lblPort, 0, 2);
            this.pnlProcess.Controls.Add(this.lblIP, 0, 1);
            this.pnlProcess.Controls.Add(this.lblName, 0, 0);
            this.pnlProcess.Controls.Add(this.txtIP, 1, 1);
            this.pnlProcess.Controls.Add(this.txtName, 1, 0);
            this.pnlProcess.Controls.Add(this.lblWorkingDirectory, 0, 4);
            this.pnlProcess.Controls.Add(this.lblExecutable, 0, 5);
            this.pnlProcess.Controls.Add(this.lblArguments, 0, 6);
            this.pnlProcess.Controls.Add(this.chkAcquire, 1, 8);
            this.pnlProcess.Controls.Add(this.chkStop, 1, 9);
            this.pnlProcess.Controls.Add(this.txtWorkingDir, 1, 4);
            this.pnlProcess.Controls.Add(this.txtExecutable, 1, 5);
            this.pnlProcess.Controls.Add(this.txtArguments, 1, 6);
            this.pnlProcess.Controls.Add(this.lblStartupTimeout, 0, 7);
            this.pnlProcess.Controls.Add(this.txtStartupTimeout, 1, 7);
            this.pnlProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProcess.Location = new System.Drawing.Point(3, 16);
            this.pnlProcess.Name = "pnlProcess";
            this.pnlProcess.RowCount = 10;
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.pnlProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlProcess.Size = new System.Drawing.Size(274, 260);
            this.pnlProcess.TabIndex = 0;
            // 
            // lblWorkingDirectory
            // 
            this.lblWorkingDirectory.AutoSize = true;
            this.lblWorkingDirectory.Location = new System.Drawing.Point(3, 100);
            this.lblWorkingDirectory.Name = "lblWorkingDirectory";
            this.lblWorkingDirectory.Size = new System.Drawing.Size(95, 13);
            this.lblWorkingDirectory.TabIndex = 11;
            this.lblWorkingDirectory.Text = "Working Directory:";
            // 
            // lblExecutable
            // 
            this.lblExecutable.AutoSize = true;
            this.lblExecutable.Location = new System.Drawing.Point(3, 125);
            this.lblExecutable.Name = "lblExecutable";
            this.lblExecutable.Size = new System.Drawing.Size(63, 13);
            this.lblExecutable.TabIndex = 12;
            this.lblExecutable.Text = "Executable:";
            // 
            // lblArguments
            // 
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(3, 150);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(60, 13);
            this.lblArguments.TabIndex = 13;
            this.lblArguments.Text = "Arguments:";
            // 
            // chkAcquire
            // 
            this.chkAcquire.AutoSize = true;
            this.chkAcquire.Location = new System.Drawing.Point(106, 203);
            this.chkAcquire.Name = "chkAcquire";
            this.chkAcquire.Size = new System.Drawing.Size(102, 17);
            this.chkAcquire.TabIndex = 16;
            this.chkAcquire.Text = "Acquire on Start";
            this.chkAcquire.UseVisualStyleBackColor = true;
            // 
            // chkStop
            // 
            this.chkStop.AutoSize = true;
            this.chkStop.Location = new System.Drawing.Point(106, 228);
            this.chkStop.Name = "chkStop";
            this.chkStop.Size = new System.Drawing.Size(83, 17);
            this.chkStop.TabIndex = 17;
            this.chkStop.Text = "Stop on Exit";
            this.chkStop.UseVisualStyleBackColor = true;
            // 
            // txtWorkingDir
            // 
            this.txtWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkingDir.Location = new System.Drawing.Point(106, 103);
            this.txtWorkingDir.Name = "txtWorkingDir";
            this.txtWorkingDir.Size = new System.Drawing.Size(165, 20);
            this.txtWorkingDir.TabIndex = 18;
            // 
            // txtExecutable
            // 
            this.txtExecutable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExecutable.Location = new System.Drawing.Point(106, 128);
            this.txtExecutable.Name = "txtExecutable";
            this.txtExecutable.Size = new System.Drawing.Size(165, 20);
            this.txtExecutable.TabIndex = 19;
            // 
            // txtArguments
            // 
            this.txtArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArguments.Location = new System.Drawing.Point(106, 153);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(165, 20);
            this.txtArguments.TabIndex = 20;
            // 
            // lblStartupTimeout
            // 
            this.lblStartupTimeout.AutoSize = true;
            this.lblStartupTimeout.Location = new System.Drawing.Point(3, 175);
            this.lblStartupTimeout.Name = "lblStartupTimeout";
            this.lblStartupTimeout.Size = new System.Drawing.Size(85, 13);
            this.lblStartupTimeout.TabIndex = 21;
            this.lblStartupTimeout.Text = "Startup Timeout:";
            // 
            // txtStartupTimeout
            // 
            this.txtStartupTimeout.Location = new System.Drawing.Point(106, 178);
            this.txtStartupTimeout.Name = "txtStartupTimeout";
            this.txtStartupTimeout.Size = new System.Drawing.Size(50, 20);
            this.txtStartupTimeout.TabIndex = 22;
            this.txtStartupTimeout.Text = "30000";
            this.txtStartupTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ServerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpProcess);
            this.Controls.Add(this.grpCredentials);
            this.Controls.Add(this.grpPlugins);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnReload);
            this.Name = "ServerControl";
            this.Size = new System.Drawing.Size(371, 553);
            this.grpPlugins.ResumeLayout(false);
            this.pnlPlugins.ResumeLayout(false);
            this.grpCredentials.ResumeLayout(false);
            this.pnlCredentials.ResumeLayout(false);
            this.pnlCredentials.PerformLayout();
            this.grpProcess.ResumeLayout(false);
            this.pnlProcess.ResumeLayout(false);
            this.pnlProcess.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.ListBox lstPlugins;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TableLayoutPanel pnlPlugins;
        private System.Windows.Forms.GroupBox grpPlugins;
        private System.Windows.Forms.GroupBox grpCredentials;
        private System.Windows.Forms.TableLayoutPanel pnlCredentials;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass1;
        private System.Windows.Forms.TextBox txtPass2;
        private System.Windows.Forms.GroupBox grpProcess;
        private System.Windows.Forms.TableLayoutPanel pnlProcess;
        private System.Windows.Forms.Label lblWorkingDirectory;
        private System.Windows.Forms.Label lblExecutable;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.CheckBox chkAcquire;
        private System.Windows.Forms.CheckBox chkStop;
        private System.Windows.Forms.TextBox txtWorkingDir;
        private System.Windows.Forms.TextBox txtExecutable;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Label lblStartupTimeout;
        private System.Windows.Forms.TextBox txtStartupTimeout;
    }
}
