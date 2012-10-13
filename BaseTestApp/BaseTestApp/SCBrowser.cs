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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SC.GUI
{
    public partial class SCBrowser : UserControl
    {
        private List<ServerProvider> servers = new List<ServerProvider>();
        public SCBrowser()
        {
            InitializeComponent();
        }

        public void RefreshView()
        {
            prgRefresh.Maximum = treeView.Nodes.Count + 1;
            prgRefresh.Minimum = 0;
            prgRefresh.Value = 1;

            prgRefresh.Visible = true;
            prgRefresh.Update();
            treeView.BeginUpdate();
            foreach (RootTreeNode node in treeView.Nodes)
            {
                node.RefreshView();
                prgRefresh.Value++;
                prgRefresh.Update();
            }
            treeView.EndUpdate();
            prgRefresh.Visible = false;
        }

        public event ServerClickedEvent ServerClicked;
        public event StandalonePluginClickedEvent StandalonePluginClicked;
        public event ServerPluginClickedEvent ServerPluginClicked;
        public event SecurityManagerClickedEvent SecurityManagerClicked;
        public event RootClickedEvent RootClicked;

        protected virtual void OnServerClicked(ServerClickedEventArgs args)
        {
            if (ServerClicked != null)
                ServerClicked(this, args);
        }
        protected virtual void OnStandalonePluginClicked(StandalonePluginClickedEventArgs args)
        {
            if (StandalonePluginClicked != null)
                StandalonePluginClicked(this, args);
        }
        protected virtual void OnServerPluginClicked(ServerPluginClickedEventArgs args)
        {
            if (ServerPluginClicked != null)
                ServerPluginClicked(this, args);
        }
        protected virtual void OnSecurityManagerClicked(SecurityManagerClickedEventArgs args)
        {
            if (SecurityManagerClicked != null)
                SecurityManagerClicked(this, args);
        }
        protected virtual void OnRootClicked(RootClickedEventArgs args)
        {
            if (RootClicked != null)
                RootClicked(this, args);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void SCBrowser_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                ServerProvider provider = new ServerProvider(new System.Net.IPEndPoint(System.Net.IPAddress.Parse("84.193.247.68"), 8081), "Administrator", "ServerChecker4");

                treeView.Nodes.Add(new RootTreeNode(provider));
            }
            RefreshView();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnRemove.Enabled = (e.Node is RootTreeNode);
            if (e.Node is ServerTreeNode)
            {
                ServerTreeNode node = e.Node as ServerTreeNode;
                OnServerClicked(new ServerClickedEventArgs(node.Root, node.Text));
            }
            else if (e.Node is StandalonePluginTreeNode)
            {
                StandalonePluginTreeNode node = e.Node as StandalonePluginTreeNode;
                OnStandalonePluginClicked(new StandalonePluginClickedEventArgs(node.Root, node.Text));
            }
            else if (e.Node is ServerPluginTreeNode)
            {
                ServerPluginTreeNode node = e.Node as ServerPluginTreeNode;
                OnServerPluginClicked(new ServerPluginClickedEventArgs(node.Root, node.ServerName, node.Text));
            }
            else if (e.Node is SecurityManagerTreeNode)
            {
                SecurityManagerTreeNode node = e.Node as SecurityManagerTreeNode;
                OnSecurityManagerClicked(new SecurityManagerClickedEventArgs(node.Root));
            }
            else if (e.Node is RootTreeNode)
            {
                RootTreeNode node = e.Node as RootTreeNode;
                OnRootClicked(new RootClickedEventArgs(node.Root));
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode is RootTreeNode)
            {
                RootTreeNode node = (treeView.SelectedNode as RootTreeNode);
                treeView.Nodes.Remove(node);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (AddServerForm frm = new AddServerForm())
            {
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                System.Net.IPAddress address = frm.Address;
                string username = frm.Username;
                string password = frm.Password;

                if (address == null || username == string.Empty || password == string.Empty)
                    return;

                foreach (RootTreeNode rootNode in treeView.Nodes)
                {
                    if (rootNode.ServerAddress.Equals(address))
                    {
                        MessageBox.Show("Already added server with IP " + address.ToString());
                        return;
                    }
                }

                RootTreeNode node = new RootTreeNode(new ServerProvider(new System.Net.IPEndPoint(address, 8081), username, password));
                treeView.Nodes.Add(node);
            }
            RefreshView();
        }
    }

    public abstract class SCTreeNode : System.Windows.Forms.TreeNode
    {
        private SC.Interfaces.IRoot root;

        public SCTreeNode(SC.Interfaces.IRoot root)
        {
            this.root = root;
        }
        public SC.Interfaces.IRoot Root
        {
            get
            {
                return root;
            }
            set
            {
                if (value != null)
                    root = value;
            }
        }
        public abstract void RefreshView();
    }
    public class ServerTreeNode : SCTreeNode
    {
        public ServerTreeNode(SC.Interfaces.IRoot root, string serverName)
            : base(root)
        {
            this.Text = serverName;
        }
        public SC.Interfaces.IServer Server
        {
            get
            {
                return Root.GetServer(Text);
            }
        }
        public override void RefreshView()
        {
            List<string> plugins = new List<string>(Server.Plugins);

            foreach (ServerPluginTreeNode node in Nodes)
            {
                if (!plugins.Contains(node.Text))
                    Nodes.Remove(node);
            }
            foreach (string plugin in plugins)
            {
                ServerPluginTreeNode pluginNode = null;
                foreach (ServerPluginTreeNode node in Nodes)
                {
                    if (node.Text == plugin)
                        pluginNode = node;
                }

                if (pluginNode == null)
                {
                    pluginNode = new ServerPluginTreeNode(Root, Text, plugin);
                    Nodes.Add(pluginNode);
                }
                pluginNode.RefreshView();
            }

            switch (Server.ServerStatus)
            {
                case SC.Interfaces.ServerConstants.ServerStatus.Started:
                    BackColor = Color.DarkGreen; break;
                case SC.Interfaces.ServerConstants.ServerStatus.Starting:
                    BackColor = Color.Orange; break;
                case SC.Interfaces.ServerConstants.ServerStatus.Stopped:
                    BackColor = Color.Red; break;
                default:
                    BackColor = Color.White; break;
            }
        }
    }
    public class ServerPluginTreeNode : SCTreeNode
    {
        private string serverName;

        public ServerPluginTreeNode(SC.Interfaces.IRoot root, string serverName, string pluginName)
            : base(root)
        {
            this.serverName = serverName;
            this.Text = pluginName;
        }
        public SC.Interfaces.IScServerPluginClient Plugin
        {
            get
            {
                return Root.GetServer(serverName).GetPlugin(Text);
            }
        }
        public string ServerName
        {
            get
            {
                return serverName;
            }
        }
        public override void RefreshView()
        {
            this.BackColor = ((Plugin.IsRunningStatus && Plugin.ShouldRunStatus) ? Color.DarkGreen : Color.Red);
        }
    }
    public class StandalonePluginTreeNode : SCTreeNode
    {
        public StandalonePluginTreeNode(SC.Interfaces.IRoot root, string pluginName)
            : base(root)
        {
            this.Text = pluginName;
        }
        public SC.Interfaces.IScStandalonePluginClient Plugin
        {
            get
            {
                return Root.GetPlugin(Text);
            }
        }
        public override void RefreshView()
        {
        }
    }
    public class RootTreeNode : SCTreeNode
    {
        private ServerProvider provider;
        private System.Net.IPAddress serverAddress;

        private TreeNode ServersNode;
        private TreeNode PluginsNode;
        private SecurityManagerTreeNode SecurityManagerNode;

        public RootTreeNode(ServerProvider provider)
            : base(provider.Root)
        {
            this.provider = provider;
            this.Text = ServerAddress.ToString();

            Initialize();
        }
        private void Initialize()
        {
            Nodes.Clear();
            
            ServersNode = new TreeNode("Servers");
            PluginsNode = new TreeNode("Plugins");
            SecurityManagerNode = new SecurityManagerTreeNode(provider.Root);
            
            Nodes.Add(ServersNode);
            Nodes.Add(PluginsNode);
            Nodes.Add(SecurityManagerNode);
        }
        private void Reinitalize()
        {
            provider.RefreshConnection();
            Root = provider.Root;
            Initialize();
        }
        public System.Net.IPAddress ServerAddress
        {
            get
            {
                return provider.EndPoint.Address;
            }
        }
        public override void RefreshView()
        {
            List<string> servers = new List<string>(Root.Servers);
            List<string> plugins = new List<string>(Root.Plugins);

            foreach (ServerTreeNode node in ServersNode.Nodes)
            {
                if (!servers.Contains(node.Text))
                    ServersNode.Nodes.Remove(node);
            }
            foreach (StandalonePluginTreeNode node in PluginsNode.Nodes)
            {
                if (!plugins.Contains(node.Text))
                    PluginsNode.Nodes.Remove(node);
            }
            foreach (string server in servers)
            {
                ServerTreeNode serverNode = null;

                foreach (ServerTreeNode node in ServersNode.Nodes)
                {
                    if (node.Text == server)
                        serverNode = node;
                }

                if (serverNode == null)
                {
                    serverNode = new ServerTreeNode(Root, server);
                    ServersNode.Nodes.Add(serverNode);
                }
                serverNode.RefreshView();
            }
            foreach (string plugin in plugins)
            {
                StandalonePluginTreeNode pluginNode = null;

                foreach (StandalonePluginTreeNode node in PluginsNode.Nodes)
                {
                    if (node.Text == plugin)
                        pluginNode = node;
                }

                if (pluginNode == null)
                {
                    pluginNode = new StandalonePluginTreeNode(Root, plugin);
                    PluginsNode.Nodes.Add(pluginNode);
                }
                pluginNode.RefreshView();
            }
        }
    }
    public class SecurityManagerTreeNode : SCTreeNode
    {
        public SecurityManagerTreeNode(SC.Interfaces.IRoot root)
            : base(root)
        {
            this.Text = "Security Manager";
        }
        public SC.Interfaces.ISecurityManager SecurityManager
        {
            get
            {
                return Root.GetSecurityManager();
            }
        }
        public override void RefreshView()
        {
        }
    }

    public abstract class SCBrowserEventArgs : EventArgs
    {
        private SC.Interfaces.IRoot root;
        protected SCBrowserEventArgs(SC.Interfaces.IRoot root)
        {
            this.root = root;
        }
        public SC.Interfaces.IRoot Root
        {
            get
            {
                return root;
            }
        }
    }

    public class ServerClickedEventArgs : SCBrowserEventArgs
    {
        private string serverName;

        public ServerClickedEventArgs(SC.Interfaces.IRoot root, string serverName) : base(root)
        {
            this.serverName = serverName;
        }
        public string Name
        {
            get
            {
                return serverName;
            }
        }
        public SC.Interfaces.IServer Server
        {
            get
            {
                return Root.GetServer(serverName);
            }
        }
    }
    public delegate void ServerClickedEvent(object sender, ServerClickedEventArgs args);

    public class ServerPluginClickedEventArgs : SCBrowserEventArgs
    {
        private string serverName;
        private string pluginName;

        public ServerPluginClickedEventArgs(SC.Interfaces.IRoot root, string serverName, string pluginName) : base(root)
        {
            this.serverName = serverName;
            this.pluginName = pluginName;
        }
        public string ServerName
        {
            get
            {
                return serverName;
            }
        }
        public string PluginName
        {
            get
            {
                return pluginName;
            }
        }
        public SC.Interfaces.IScServerPluginClient Plugin
        {
            get
            {
                return Root.GetServer(serverName).GetPlugin(pluginName);
            }
        }
    }
    public delegate void ServerPluginClickedEvent(object sender, ServerPluginClickedEventArgs args);

    public class StandalonePluginClickedEventArgs : SCBrowserEventArgs
    {
        private string pluginName;

        public StandalonePluginClickedEventArgs(SC.Interfaces.IRoot root, string pluginName) : base(root)
        {
            this.pluginName = pluginName;
        }
        public string PluginName
        {
            get
            {
                return pluginName;
            }
        }
        public SC.Interfaces.IScStandalonePluginClient Plugin
        {
            get
            {
                return Root.GetPlugin(pluginName);
            }
        }
    }
    public delegate void StandalonePluginClickedEvent(object sender, StandalonePluginClickedEventArgs args);

    public class SecurityManagerClickedEventArgs : SCBrowserEventArgs
    {
        public SecurityManagerClickedEventArgs(SC.Interfaces.IRoot root) : base(root)
        {
        }
        public SC.Interfaces.ISecurityManager SecurityManager
        {
            get
            {
                return Root.GetSecurityManager();
            }
        }
    }
    public delegate void SecurityManagerClickedEvent(object sender, SecurityManagerClickedEventArgs args);

    public class RootClickedEventArgs : SCBrowserEventArgs
    {
        public RootClickedEventArgs(SC.Interfaces.IRoot root) : base(root)
        {
        }
    }
    public delegate void RootClickedEvent(object sender, RootClickedEventArgs args);
}
