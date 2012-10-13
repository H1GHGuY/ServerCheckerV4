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
    partial class RootControl
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
            this.lstServers = new System.Windows.Forms.ListBox();
            this.lstPlugins = new System.Windows.Forms.ListBox();
            this.btnAddServer = new System.Windows.Forms.Button();
            this.btnAddPlugin = new System.Windows.Forms.Button();
            this.btnDelServer = new System.Windows.Forms.Button();
            this.btnDelPlugin = new System.Windows.Forms.Button();
            this.grpServers = new System.Windows.Forms.GroupBox();
            this.ctrSplitter = new System.Windows.Forms.SplitContainer();
            this.grpPlugins = new System.Windows.Forms.GroupBox();
            this.pnlServers = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.grpServers.SuspendLayout();
            this.ctrSplitter.Panel1.SuspendLayout();
            this.ctrSplitter.Panel2.SuspendLayout();
            this.ctrSplitter.SuspendLayout();
            this.grpPlugins.SuspendLayout();
            this.pnlServers.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstServers
            // 
            this.lstServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstServers.FormattingEnabled = true;
            this.lstServers.Location = new System.Drawing.Point(3, 3);
            this.lstServers.Name = "lstServers";
            this.pnlServers.SetRowSpan(this.lstServers, 2);
            this.lstServers.Size = new System.Drawing.Size(178, 160);
            this.lstServers.TabIndex = 2;
            this.lstServers.SelectedIndexChanged += new System.EventHandler(this.lstServers_SelectedIndexChanged);
            // 
            // lstPlugins
            // 
            this.lstPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPlugins.FormattingEnabled = true;
            this.lstPlugins.Location = new System.Drawing.Point(3, 3);
            this.lstPlugins.Name = "lstPlugins";
            this.tableLayoutPanel2.SetRowSpan(this.lstPlugins, 2);
            this.lstPlugins.Size = new System.Drawing.Size(178, 147);
            this.lstPlugins.TabIndex = 3;
            this.lstPlugins.SelectedIndexChanged += new System.EventHandler(this.lstPlugins_SelectedIndexChanged);
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(187, 3);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(79, 24);
            this.btnAddServer.TabIndex = 4;
            this.btnAddServer.Text = "Add Server...";
            this.btnAddServer.UseVisualStyleBackColor = true;
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);
            // 
            // btnAddPlugin
            // 
            this.btnAddPlugin.Location = new System.Drawing.Point(187, 3);
            this.btnAddPlugin.Name = "btnAddPlugin";
            this.btnAddPlugin.Size = new System.Drawing.Size(79, 24);
            this.btnAddPlugin.TabIndex = 5;
            this.btnAddPlugin.Text = "Add Plugin...";
            this.btnAddPlugin.UseVisualStyleBackColor = true;
            this.btnAddPlugin.Click += new System.EventHandler(this.btnAddPlugin_Click);
            // 
            // btnDelServer
            // 
            this.btnDelServer.Enabled = false;
            this.btnDelServer.Location = new System.Drawing.Point(187, 33);
            this.btnDelServer.Name = "btnDelServer";
            this.btnDelServer.Size = new System.Drawing.Size(79, 24);
            this.btnDelServer.TabIndex = 6;
            this.btnDelServer.Text = "Delete Server";
            this.btnDelServer.UseVisualStyleBackColor = true;
            this.btnDelServer.Click += new System.EventHandler(this.btnDelServer_Click);
            // 
            // btnDelPlugin
            // 
            this.btnDelPlugin.Enabled = false;
            this.btnDelPlugin.Location = new System.Drawing.Point(187, 33);
            this.btnDelPlugin.Name = "btnDelPlugin";
            this.btnDelPlugin.Size = new System.Drawing.Size(79, 24);
            this.btnDelPlugin.TabIndex = 7;
            this.btnDelPlugin.Text = "Delete Plugin";
            this.btnDelPlugin.UseVisualStyleBackColor = true;
            this.btnDelPlugin.Click += new System.EventHandler(this.btnDelPlugin_Click);
            // 
            // grpServers
            // 
            this.grpServers.Controls.Add(this.pnlServers);
            this.grpServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpServers.Location = new System.Drawing.Point(0, 0);
            this.grpServers.Name = "grpServers";
            this.grpServers.Size = new System.Drawing.Size(275, 185);
            this.grpServers.TabIndex = 1;
            this.grpServers.TabStop = false;
            this.grpServers.Text = "Servers";
            // 
            // ctrSplitter
            // 
            this.ctrSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrSplitter.Location = new System.Drawing.Point(0, 0);
            this.ctrSplitter.Name = "ctrSplitter";
            this.ctrSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ctrSplitter.Panel1
            // 
            this.ctrSplitter.Panel1.Controls.Add(this.grpServers);
            this.ctrSplitter.Panel1MinSize = 150;
            // 
            // ctrSplitter.Panel2
            // 
            this.ctrSplitter.Panel2.Controls.Add(this.grpPlugins);
            this.ctrSplitter.Panel2MinSize = 150;
            this.ctrSplitter.Size = new System.Drawing.Size(275, 376);
            this.ctrSplitter.SplitterDistance = 185;
            this.ctrSplitter.SplitterWidth = 8;
            this.ctrSplitter.TabIndex = 2;
            // 
            // grpPlugins
            // 
            this.grpPlugins.Controls.Add(this.tableLayoutPanel2);
            this.grpPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPlugins.Location = new System.Drawing.Point(0, 0);
            this.grpPlugins.Name = "grpPlugins";
            this.grpPlugins.Size = new System.Drawing.Size(275, 183);
            this.grpPlugins.TabIndex = 0;
            this.grpPlugins.TabStop = false;
            this.grpPlugins.Text = "Plugins";
            // 
            // pnlServers
            // 
            this.pnlServers.ColumnCount = 2;
            this.pnlServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlServers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.pnlServers.Controls.Add(this.btnAddServer, 1, 0);
            this.pnlServers.Controls.Add(this.btnDelServer, 1, 1);
            this.pnlServers.Controls.Add(this.lstServers, 0, 0);
            this.pnlServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlServers.Location = new System.Drawing.Point(3, 16);
            this.pnlServers.Name = "pnlServers";
            this.pnlServers.RowCount = 2;
            this.pnlServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlServers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlServers.Size = new System.Drawing.Size(269, 166);
            this.pnlServers.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel2.Controls.Add(this.btnAddPlugin, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDelPlugin, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lstPlugins, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(269, 164);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // RootControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrSplitter);
            this.Name = "RootControl";
            this.Size = new System.Drawing.Size(275, 376);
            this.grpServers.ResumeLayout(false);
            this.ctrSplitter.Panel1.ResumeLayout(false);
            this.ctrSplitter.Panel2.ResumeLayout(false);
            this.ctrSplitter.ResumeLayout(false);
            this.grpPlugins.ResumeLayout(false);
            this.pnlServers.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstServers;
        private System.Windows.Forms.ListBox lstPlugins;
        private System.Windows.Forms.Button btnAddServer;
        private System.Windows.Forms.Button btnAddPlugin;
        private System.Windows.Forms.Button btnDelServer;
        private System.Windows.Forms.Button btnDelPlugin;
        private System.Windows.Forms.GroupBox grpServers;
        private System.Windows.Forms.SplitContainer ctrSplitter;
        private System.Windows.Forms.GroupBox grpPlugins;
        private System.Windows.Forms.TableLayoutPanel pnlServers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
