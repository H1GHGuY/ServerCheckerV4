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
    partial class SCBrowser
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
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.prgRefresh = new System.Windows.Forms.ProgressBar();
            this.pnlBrowser = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pnlBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.pnlBrowser.SetColumnSpan(this.treeView, 2);
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HotTracking = true;
            this.treeView.Location = new System.Drawing.Point(3, 63);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(207, 272);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(79, 24);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Interval = 15000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // prgRefresh
            // 
            this.prgRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgRefresh.Location = new System.Drawing.Point(88, 3);
            this.prgRefresh.Name = "prgRefresh";
            this.prgRefresh.Size = new System.Drawing.Size(122, 24);
            this.prgRefresh.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgRefresh.TabIndex = 2;
            this.prgRefresh.Visible = false;
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.ColumnCount = 2;
            this.pnlBrowser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.pnlBrowser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBrowser.Controls.Add(this.treeView, 0, 2);
            this.pnlBrowser.Controls.Add(this.btnRefresh, 0, 0);
            this.pnlBrowser.Controls.Add(this.prgRefresh, 1, 0);
            this.pnlBrowser.Controls.Add(this.btnAdd, 0, 1);
            this.pnlBrowser.Controls.Add(this.btnRemove, 1, 1);
            this.pnlBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBrowser.Location = new System.Drawing.Point(0, 0);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.RowCount = 3;
            this.pnlBrowser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlBrowser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlBrowser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlBrowser.Size = new System.Drawing.Size(213, 338);
            this.pnlBrowser.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Location = new System.Drawing.Point(3, 33);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(79, 24);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add...";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Location = new System.Drawing.Point(88, 33);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(79, 24);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // SCBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBrowser);
            this.Name = "SCBrowser";
            this.Size = new System.Drawing.Size(213, 338);
            this.Load += new System.EventHandler(this.SCBrowser_Load);
            this.pnlBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.TableLayoutPanel pnlBrowser;
        private System.Windows.Forms.ProgressBar prgRefresh;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}
