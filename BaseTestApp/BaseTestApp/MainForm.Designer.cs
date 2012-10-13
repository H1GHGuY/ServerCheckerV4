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
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scBrowser = new SC.GUI.SCBrowser();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // scBrowser
            // 
            this.scBrowser.Dock = System.Windows.Forms.DockStyle.Left;
            this.scBrowser.Location = new System.Drawing.Point(0, 0);
            this.scBrowser.Name = "scBrowser";
            this.scBrowser.Size = new System.Drawing.Size(170, 508);
            this.scBrowser.TabIndex = 0;
            this.scBrowser.ServerPluginClicked += new SC.GUI.ServerPluginClickedEvent(this.scBrowser_ServerPluginClicked);
            this.scBrowser.SecurityManagerClicked += new SC.GUI.SecurityManagerClickedEvent(this.scBrowser_SecurityManagerClicked);
            this.scBrowser.RootClicked += new SC.GUI.RootClickedEvent(this.scBrowser_RootClicked);
            this.scBrowser.ServerClicked += new SC.GUI.ServerClickedEvent(this.scBrowser_ServerClicked);
            this.scBrowser.StandalonePluginClicked += new SC.GUI.StandalonePluginClickedEvent(this.scBrowser_StandalonePluginClicked);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(170, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(686, 508);
            this.pnlContent.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 508);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.scBrowser);
            this.Name = "MainForm";
            this.Text = "ServerChecker Client";
            this.ResumeLayout(false);

        }

        #endregion

        private SCBrowser scBrowser;
        private System.Windows.Forms.Panel pnlContent;
    }
}