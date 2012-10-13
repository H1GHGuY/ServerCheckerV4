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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SC.GUI
{
    public partial class MainForm : Form
    {
        private SC.GUI.PluginPanelLoader pluginPanelLoader = new SC.GUI.PluginPanelLoader();

        public MainForm()
        {
            InitializeComponent();
        }

        private void scBrowser_ServerClicked(object sender, ServerClickedEventArgs args)
        {
            try
            {
                pnlContent.Controls.Clear();
                ServerControl control = new ServerControl();
                control.Server = args.Server;
                control.PluginLoader = args.Root.GetPluginLoader();
                control.Location = new Point(0, 0);
                control.Name = "control";
                pnlContent.Controls.Add(control);
                control.Show();
            }
            catch (Exception e)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(e);
            }
        }

        private void scBrowser_SecurityManagerClicked(object sender, SecurityManagerClickedEventArgs args)
        {
            try
            {
                pnlContent.Controls.Clear();

                SecurityManagerUserControl userControl = new SecurityManagerUserControl();
                userControl.SecurityManager = args.SecurityManager;
                userControl.Location = new Point(0, 0);
                userControl.Name = "userControl";
                pnlContent.Controls.Add(userControl);
                userControl.Show();

                SecurityManagerPermissionControl permControl = new SecurityManagerPermissionControl();
                permControl.SecurityManager = args.SecurityManager;
                permControl.Location = new Point(0, userControl.Size.Height + 10);
                permControl.Name = "permControl";
                pnlContent.Controls.Add(permControl);
                permControl.Show();

                SecurityManagerIPControl ipControl = new SecurityManagerIPControl();
                ipControl.SecurityManager = args.SecurityManager;
                ipControl.Location = new Point(0, userControl.Size.Height + permControl.Size.Height + 20);
                ipControl.Name = "ipControl";
                pnlContent.Controls.Add(ipControl);
                ipControl.Show();
            }
            catch (Exception e)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(e);
            }
        }

        private void scBrowser_RootClicked(object sender, RootClickedEventArgs args)
        {
            try
            {
                pnlContent.Controls.Clear();

                RootControl control = new RootControl();
                control.Root = args.Root;
                control.Location = new Point(0, 0);
                control.Name = "control";
                pnlContent.Controls.Add(control);
                control.Show();
            }
            catch (Exception e)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(e);
            }
        }

        private void scBrowser_ServerPluginClicked(object sender, ServerPluginClickedEventArgs args)
        {
            PluginClicked(args.Plugin);
        }

        private void scBrowser_StandalonePluginClicked(object sender, StandalonePluginClickedEventArgs args)
        {
            PluginClicked(args.Plugin);
        }

        private void PluginClicked(SC.Interfaces.IScPluginClient plugin)
        {
            int y = 0;
            pnlContent.Controls.Clear();
            SC.GUI.Utility.PluginPanelBase[] panels = null;
            try
            {
                panels = pluginPanelLoader.GetPanels(plugin);
            }
            catch (Exception e)
            {
                SC.GUI.Utility.ErrorForm.ShowErrorForm(e);
                return;
            }
            foreach (SC.GUI.Utility.PluginPanelBase panel in panels)
            {
                try
                {
                    panel.Name = plugin.ToString() + y;
                    panel.Location = new Point(0, y);
                    y += panel.Size.Height + 10;
                    pnlContent.Controls.Add(panel);
                    panel.Show();
                }
                catch (Exception e)
                {
                    SC.GUI.Utility.ErrorForm.ShowErrorForm(e);
                }
            }
        }
    }
}
