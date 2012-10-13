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

namespace SC.PluginLoader
{
    public class MetaPlugin
    {
        private Type type;
        
        public static bool IsPlugin(Type t)
        {
            IList<System.Reflection.CustomAttributeData> attrs = System.Reflection.CustomAttributeData.GetCustomAttributes(t);
            foreach (System.Reflection.CustomAttributeData attr in attrs)
            {
                if (attr.Constructor.DeclaringType.FullName == typeof(SC.Interfaces.ScPluginAttribute).FullName)
                    return IsServerPlugin(t) || IsStandalonePlugin(t);
            }
            return false;
            //t.GetCustomAttributes(typeof(SC.Interfaces.ScPluginAttribute), true).Length == 1
        }
        private static bool IsStandalonePlugin(Type t)
        {
            return t.GetInterface(typeof(SC.Interfaces.IScStandalonePluginBase).Name) != null;
            //return typeof(SC.Interfaces.IScStandalonePluginBase).IsAssignableFrom(t);
        }
        private static bool IsServerPlugin(Type t)
        {
            return t.GetInterface(typeof(SC.Interfaces.IScServerPluginBase).Name) != null;
            //return typeof(SC.Interfaces.IScServerPluginBase).IsAssignableFrom(t);
        }
        private static string GetLicenseName(Type t)
        {
            IList<System.Reflection.CustomAttributeData> attrs = System.Reflection.CustomAttributeData.GetCustomAttributes(t);
            foreach (System.Reflection.CustomAttributeData attr in attrs)
            {
                if (attr.Constructor.DeclaringType.FullName == typeof(SC.Interfaces.LicenseAttribute).FullName)
                    return attr.ConstructorArguments[0].Value.ToString();
            }
            return string.Empty;
        }
        internal MetaPlugin(Type t)
        {
            if (!IsPlugin(t))
                throw new ArgumentException("Type is not a plugin");
            type = t;
        }
        public string Name
        {
            get
            {
                object[] attrs = type.GetCustomAttributes(typeof(SC.Interfaces.ScPluginAttribute), true);
                return (attrs[0] as SC.Interfaces.ScPluginAttribute).Name;
            }
        }
        public bool Standalone
        {
            get
            {
                return IsStandalonePlugin(type);
            }
        }
        public bool Server
        {
            get
            {
                return IsServerPlugin(type);
            }
        }
        public string License
        {
            get
            {
                return GetLicenseName(type);
            }
        }
        internal SC.Interfaces.IScStandalonePluginBase CreateStandaloneInstance()
        {
            if (!Standalone)
                throw new NotSupportedException("Plugin " + Name + " does not support standalone operation.");
            return CreateInstance() as SC.Interfaces.IScStandalonePluginBase;
        }
        internal SC.Interfaces.IScServerPluginBase CreateServerInstance()
        {
            if (!Server)
                throw new NotSupportedException("Plugin " + Name + " does not support server-supporting operation.");
            return CreateInstance() as SC.Interfaces.IScServerPluginBase;
        }
        internal object CreateInstance()
        {
            PluginLoader.Instance.Logger.Info("Loading plugin " + Name);
            try
            {
                object ret = Activator.CreateInstance(type);
                if (ret is SC.Interfaces.ILicensee)
                    SC.Security.LicenseManager.Instance.License(ret as SC.Interfaces.ILicensee);
                return ret;
            }
            catch (Exception e)
            {
                PluginLoader.Instance.Logger.Error("Could not load plugin " + Name, e);
                return null;
            }
        }
        public override string ToString()
        {
            System.Text.StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            builder.Append(" (Standalone: ");
            builder.Append(Standalone ? "yes" : "no");
            builder.Append(", Server: ");
            builder.Append(Server ? "yes" : "no");
            builder.Append(", License: ");
            builder.Append(License);
            builder.Append(")");
            return builder.ToString();
        }
    }

    internal class PluginChecker : MarshalByRefObject
    {
        public PluginChecker()
        {
        }
        public bool HasPlugins(string path)
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
            System.Reflection.Assembly a = System.Reflection.Assembly.ReflectionOnlyLoadFrom(path);
            foreach (Type t in a.GetExportedTypes())
            {
                if (MetaPlugin.IsPlugin(t))
                    return true;
            }
            return false;
        }

        System.Reflection.Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly ret = System.Reflection.Assembly.ReflectionOnlyLoad(args.Name);
            return ret;
        }
    }

    [System.Security.Permissions.FileIOPermission(System.Security.Permissions.SecurityAction.Assert)]
    public class PluginLoader : SC.Interfaces.EternalMarshalByRefObject, SC.Interfaces.IPluginLoader
    {
        private IDictionary<string, MetaPlugin> plugins = new Dictionary<string, MetaPlugin>();
        internal readonly log4net.ILog Logger = log4net.LogManager.GetLogger("PluginLoader");
        private SC.Interfaces.ISecurityProvider secProvider = null;

        private static PluginLoader pluginLoader = null;
        public static void Initialize()
        {
            if (pluginLoader != null)
                throw new InvalidOperationException("The plugin loader has already been instantiated.");

            pluginLoader = new PluginLoader(new SC.Security.SecurityProvider("Plugin Loader"));
            pluginLoader.SearchPlugins();
        }
        public static PluginLoader Instance
        {
            get
            {
                System.Diagnostics.Trace.Assert(pluginLoader != null);
                return pluginLoader;
            }
        }
        public static void Cleanup()
        {
            if (pluginLoader == null)
                throw new InvalidOperationException("The plugin loader has not been instantiated.");

            pluginLoader.CleanupInstance();
            pluginLoader = null;
        }

        private PluginLoader(SC.Interfaces.ISecurityProvider secProvider)
        {
            this.secProvider = secProvider;
        }
        private void CleanupInstance()
        {
            secProvider.Cleanup();
        }
        public void UploadPlugin(string fileName, System.IO.MemoryStream stream)
        {
            secProvider.DemandPermission();

            if (!fileName.EndsWith("dll", true, System.Globalization.CultureInfo.CurrentCulture))
                throw new SC.Interfaces.SCException("Filename is not a DLL.");

            fileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), System.IO.Path.GetFileName(fileName));
            if (System.IO.File.Exists(fileName))
                throw new SC.Interfaces.SCException("A plugin with the given filename already exists.");

            Logger.Info("User " + System.Threading.Thread.CurrentPrincipal.Identity.Name + " has uploaded plugin " + fileName);

            System.IO.FileStream outStream = new System.IO.FileStream(fileName, System.IO.FileMode.CreateNew);

            byte[] buffer = new byte[128 * 1024];
            int n = stream.Read(buffer, 0, 128*1024);

            while (n > 0)
            {
                outStream.Write(buffer, 0, n);
                n = stream.Read(buffer, 0, 128*1024);
            }

            outStream.Close();
            
            try
            {
                ProbeDLL(fileName);
            }
            catch (Exception e)
            {
                Logger.Error("Exception caught while trying to load the uploaded file. Deleting file.", e);
                System.IO.File.Delete(fileName);
            }
        }
        public void SearchPlugins()
        {
            secProvider.DemandPermission();
            ScanDirectory(System.IO.Directory.GetCurrentDirectory());
        }
        private void ScanDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                throw new ArgumentException(path + " is not a valid directory");
            }

            Logger.Info("Scanning directory for plugins: " + path);
            string[] files = System.IO.Directory.GetFiles(path, "*.dll");
            foreach (string file in files)
            {
                ProbeDLL(file);
            }
        }
        private void ProbeDLL(string path)
        {
            if (HasPlugins(path))
            {
                Logger.Debug("Plugins found in " + System.IO.Path.GetFileName(path) + ". Loading...");
                System.Reflection.Assembly a = System.Reflection.Assembly.LoadFile(path);
                ProbeAssembly(a);
            }
            else
                Logger.Debug("No plugins found in " + System.IO.Path.GetFileName(path) + ".");
        }
        private bool HasPlugins(string path)
        {
            AppDomain dom = AppDomain.CreateDomain("Plugin Checker");
            try
            {
                PluginChecker pc = dom.CreateInstanceAndUnwrap(System.Reflection.Assembly.GetAssembly(typeof(PluginChecker)).FullName, typeof(PluginChecker).FullName) as PluginChecker;
                return pc.HasPlugins(path);
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                AppDomain.Unload(dom);
            }
        }
        private void ProbeAssembly(System.Reflection.Assembly a)
        {
            foreach (Type t in a.GetExportedTypes())
            {
                if (MetaPlugin.IsPlugin(t))
                {
                    MetaPlugin plugin = new MetaPlugin(t);
                    if (!plugins.ContainsKey(plugin.Name))
                    {
                        plugins.Add(plugin.Name, plugin);
                        Logger.Info("Found plugin: " + plugin);
                    }
                    else
                        Logger.Info("Already have plugin: " + plugin);
                }
            }
        }
        private ICollection<MetaPlugin> Plugins
        {
            get
            {
                return plugins.Values;
            }
        }
        private object LoadPlugin(string Name)
        {
            if (plugins.ContainsKey(Name))
                return plugins[Name].CreateInstance();
            else
                return null;
        }
        public SC.Interfaces.IScStandalonePluginBase LoadStandalonePlugin(string Name)
        {
            if (plugins.ContainsKey(Name))
                return plugins[Name].CreateStandaloneInstance();
            else
                return null;
        }
        public SC.Interfaces.IScServerPluginBase LoadServerPlugin(string Name)
        {
            if (plugins.ContainsKey(Name))
                return plugins[Name].CreateServerInstance();
            else
                return null;
        }
        public bool HavePermission()
        {
            return secProvider.HavePermission();
        }
        public bool HavePermission(string operation)
        {
            return secProvider.HavePermission(operation);
        }

        #region IPluginLoader Members

        public string[] StandalonePlugins
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (MetaPlugin plugin in plugins.Values)
                {
                    if (plugin.Standalone)
                        ret.Add(plugin.Name);
                }
                return ret.ToArray();
            }
        }

        public string[] ServerPlugins
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (MetaPlugin plugin in plugins.Values)
                {
                    if (plugin.Server)
                        ret.Add(plugin.Name);
                }
                return ret.ToArray();
            }
        }

        #endregion
    }
}
