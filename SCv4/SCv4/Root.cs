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
using System.Text;

namespace SC.Core
{
    public class RootSettings
    {
        public RootSettings(ICollection<string> Servers, ICollection<string> Plugins)
        {
            this.Servers = new List<string>(Servers);
            this.Plugins = new List<string>(Plugins);
        }
        public RootSettings()
        {
            Servers = new List<string>();
            Plugins = new List<string>();
        }
        public List<string> Servers;
        public List<string> Plugins;
    }

    class Root : SC.Interfaces.EternalMarshalByRefObject, SC.Interfaces.IRoot
    {
        private Dictionary<string, SC.Core.Server> servers = new Dictionary<string, SC.Core.Server>();
        private Dictionary<string, SC.Interfaces.IScStandalonePluginBase> plugins = new Dictionary<string, SC.Interfaces.IScStandalonePluginBase>();
        private object locker = new object();
        private RootSettings settings = new RootSettings();
        private SC.Security.SecurityProvider secProvider = null;

        private readonly log4net.ILog Logger = log4net.LogManager.GetLogger("Root");

        public Root()
        {
        }
        public void Initialize()
        {
            Logger.Info("Starting up...");
            Logger.Info("Loading Security settings...");
            SC.Security.SecurityManager.Instance.ToString();
            secProvider = new SC.Security.SecurityProvider("Root");
            Logger.Info("Loading Licenses...");
            SC.Security.LicenseManager.Initialize();

            Logger.Info("Scanning for plugins...");
            SC.PluginLoader.PluginLoader.Initialize();

            Logger.Info("Loading Root settings...");
            RootSettings newSettings = SC.Settings.SettingsManager.Instance.RestoreSettings("Root", typeof(RootSettings)) as RootSettings;
            if (newSettings != null)
                settings = newSettings;

            Logger.Info("Loading Servers...");
            foreach (string Server in settings.Servers)
                LoadServer(Server);
            Logger.Info("Loading Plugins...");
            foreach (string Plugin in settings.Plugins)
                LoadPlugin(Plugin);
        }
        public void Cleanup()
        {
            Logger.Info("Shutting Down...");
            Logger.Info("Saving Root Settings...");
            RootSettings settings = new RootSettings(servers.Keys, plugins.Keys);
            SC.Settings.SettingsManager.Instance.SaveSettings("Root", settings);

            Logger.Info("Stopping Servers...");
            foreach (SC.Core.Server s in servers.Values)
            {
                try
                {
                    s.Stop(true);
                }
                catch (Exception)
                {
                    // TODO
                }
            }
            Logger.Info("Stopping Plugins...");
            foreach (SC.Interfaces.IScStandalonePluginBase plugin in plugins.Values)
            {
                try
                {
                    plugin.Stop();
                }
                catch (Exception)
                {
                    // TODO
                }
            }
            secProvider.Cleanup();
            Logger.Info("Stopping Plugin Loader...");
            SC.PluginLoader.PluginLoader.Cleanup();
            Logger.Info("Shutdown complete...");
        }
        private void LoadServer(string Name)
        {
            if (servers.ContainsKey(Name))
            {
                Logger.Error("A server with name '" + Name + "' already exists.");
                throw new ArgumentException("A server with name '" + Name + "' already exists.", "Name");
            }

            try
            {
                SC.Core.Server s = new Server(Name);
                s.Start();
                servers[Name] = s;
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred when adding server " + Name, e);
                throw new SC.Interfaces.SCException("Could not add server", e);
            }
        }
        public void AddServer(string Name)
        {
            lock (locker)
            {
                secProvider.DemandPermission();

                LoadServer(Name);
            }
        }
        private void UnloadServer(string Name)
        {
            if (!servers.ContainsKey(Name))
            {
                Logger.Error("A server with name '" + Name + "' cannot be removed since it does not exist.");
                throw new ArgumentException("A server with name '" + Name + "' does not exists.", "Name");
            }

            SC.Core.Server s = servers[Name];
            servers.Remove(Name);
            try
            {
                s.Destroy();
            }
            catch (Exception e)
            {
                Logger.Error("Exception during destruction of server.", e);
                throw new SC.Interfaces.SCException(e);
            }
            finally
            {
                s.Stop(true);
            }
        }
        public void RemoveServer(string Name)
        {
            lock (locker)
            {
                secProvider.DemandPermission();
                UnloadServer(Name);
            }
        }
        public SC.Interfaces.IServer GetServer(string Name)
        {
            lock (locker)
            {
                if (servers.ContainsKey(Name) && servers[Name].HavePermission())
                    return servers[Name];
                else
                    return null;
            }
        }
        public string[] Servers
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (KeyValuePair<string, Server> server in servers)
                {
                    if (server.Value.HavePermission())
                        ret.Add(server.Key);
                }
                return ret.ToArray();
            }
        }

        public void AddPlugin(string Name)
        {
            if (Name == null)
                throw new ArgumentNullException("Name");

            lock (locker)
            {
                secProvider.DemandPermission();

                LoadPlugin(Name);
            }
        }
        private void LoadPlugin(string Name)
        {
            if (plugins.ContainsKey(Name))
                throw new SC.Interfaces.SCException("Plugin " + Name + " is already present.");
            
            // Load plugin
            SC.Interfaces.IScStandalonePluginBase plugin = null;
            try
            {
                plugin = SC.PluginLoader.PluginLoader.Instance.LoadStandalonePlugin(Name);
            }
            catch (NotSupportedException e)
            {
                Logger.Error("Plugin " + Name + " does not support standalone operation.", e);
                throw new SC.Interfaces.SCException(e);
            }
            catch (Exception e)
            {
                Logger.Error("Unexpected exception during creation of plugin " + Name, e);
                throw new SC.Interfaces.SCException(e);
            }

            if (plugin == null)
            {
                Logger.Error("No plugin found with name " + Name);
                throw new ArgumentException("Plugin with name " + Name + " does not exist.", "Name");
            }

            // Make sure it's licensed
            try
            {
                plugin.AssertLicense();
            }
            catch (SC.Interfaces.LicenseException e)
            {
                Logger.Error("Plugin is not licensed.", e);
                throw;
            }

            // Initialize and start it.
            try
            {
                plugin.SetProvider(new StandaloneProvider(Name, this));
                plugin.Start();
                plugins[Name] = plugin;
            }
            catch (SC.Interfaces.LicenseException lic)
            {
                Logger.Error("No license available for plugin.", lic);
                throw;
            }
            catch (Exception e)
            {
                Logger.Error("Could not start plugin.", e);
                throw new SC.Interfaces.SCException("Could not start plugin", e);
            }
        }
        public SC.Interfaces.IScStandalonePluginClient GetPlugin(string Name)
        {
            lock (locker)
            {
                SC.Interfaces.IScStandalonePluginBase plugin;
                if (plugins.TryGetValue(Name, out plugin) && plugin.HavePermission())
                    return plugin;
                else
                    return null;
            }
        }
        public string[] Plugins
        {
            get
            {
                lock (locker)
                {
                    List<string> ret = new List<string>();
                    foreach (KeyValuePair<string, SC.Interfaces.IScStandalonePluginBase> plugin in plugins)
                    {
                        if (plugin.Value.HavePermission())
                            ret.Add(plugin.Key);
                    }
                    return ret.ToArray();
                }
            }
        }
        private void UnloadPlugin(string Name)
        {
            if (!plugins.ContainsKey(Name))
            {
                Logger.Error("Cannot removed plugin with name " + Name);
                throw new ArgumentException("The plugin with name " + Name + " cannot be removed because it does not exist.", "Name");
            }

            SC.Interfaces.IScStandalonePluginBase plugin = plugins[Name];
            plugins.Remove(Name);

            try
            {
                plugin.Destroy();
            }
            catch (Exception e)
            {
                Logger.Error("An unknown error occurred during destruction of plugin " + Name, e);
                throw new SC.Interfaces.SCException("Unexpected error occurred during unload of plugin", e);
            }
            finally
            {
                try
                {
                    plugin.Stop();
                }
                catch (Exception e)
                {
                    Logger.Error("An unknown error occurred while stopping plugin " + Name, e);
                    throw new SC.Interfaces.SCException("Unexpected error occurred during unload of plugin", e);
                }
                SC.Security.SecurityManager.Instance.UnAuthenticate();
            }
        }
        public void RemovePlugin(string Name)
        {
            lock (locker)
            {
                secProvider.DemandPermission();
                UnloadPlugin(Name);
            }
        }

        public SC.Interfaces.ISecurityManager GetSecurityManager()
        {
            return SC.Security.SecurityManager.Instance;
        }
        public SC.Interfaces.IPluginLoader GetPluginLoader()
        {
            return SC.PluginLoader.PluginLoader.Instance;
        }

        public void IsAlive()
        {
        }
    }
}
