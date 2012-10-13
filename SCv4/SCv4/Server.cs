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
    public class HostChangedEventArgs
    {
        private System.Net.IPEndPoint host;
        private System.Net.TransportType protocol;

        public HostChangedEventArgs(System.Net.IPEndPoint newHost, System.Net.TransportType newProtocol)
        {
            host = newHost;
            protocol = newProtocol;
        }
        public System.Net.IPEndPoint NewHost { get { return host; } }
        public System.Net.TransportType NewProtocol { get { return protocol; } }
    }
    public delegate void HostChangedEvent(object sender, HostChangedEventArgs args);

    [Serializable()]
    public sealed class ServerSettings
    {
        // This needs to change!
        internal static byte[] key =    { 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x60 };
        internal static byte[] vector = { 0x45, 0x46, 0x47, 0x48, 0x49, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x60 };

        public ServerSettings()
        {
            this.Name = string.Empty;
            this.EndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            this.Protocol = System.Net.TransportType.All;
            this.Plugins = new List<string>();
            this.Arguments = string.Empty;
            this.Executable = string.Empty;
            this.Password = new SC.Utility.EncryptedString(key);
            this.Username = string.Empty;
            this.WorkingDirectory = string.Empty;
            this.StartupTimeout = 5000;
            this.StopOnExit = false;
            this.AcquireOnStart = true;
        }
        public static ServerSettings Empty
        {
            get
            {
                return new ServerSettings();
            }
        }
        public string Name;
        public string WorkingDirectory;
        public string Executable;
        public string Arguments;
        public string Username;

        /*[System.Xml.Serialization.XmlIgnore()]
        public System.Security.SecureString Password;

        
        
        [System.Security.SecurityCritical(System.Security.SecurityCriticalScope.Explicit)]
        [System.Xml.Serialization.XmlElement()]
        public byte[] EncryptedPassword
        {
            get
            {
                IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(Password);
                byte[] bytes = new byte[Password.Length * 2];
                
                for (int i = 0; i < Password.Length * 2; ++i) // unicode vs ANSI
                {
                    bytes[i] = System.Runtime.InteropServices.Marshal.ReadByte(ptr, i);
                }
                
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
                
                byte[] ret = System.Security.Cryptography.RijndaelManaged.Create().CreateEncryptor(key, vector).TransformFinalBlock(bytes, 0, bytes.Length);
                
                for (int i = 0; i < bytes.Length; ++i)
                    bytes[i] = 0;

                return ret;
            }
            set
            {
                byte[] bytes = System.Security.Cryptography.RijndaelManaged.Create().CreateDecryptor(key, vector).TransformFinalBlock(value, 0, value.Length);
                Password = new System.Security.SecureString();
                for (int i = 0; i < bytes.Length; i += 2)
                {
                    char c = (char)(bytes[i] * 256 + bytes[i + 1]);
                    Password.AppendChar(c);
                    bytes[i] = bytes[i + 1] = 0;
                }
                Password.MakeReadOnly();
            }
        }*/

        private SC.Utility.EncryptedString password;
        public SC.Utility.EncryptedString Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                password.SetKey(key);
            }
        }
        
        [System.Xml.Serialization.XmlIgnore()]
        public System.Net.IPEndPoint EndPoint;
        public string Host
        {
            get
            {
                return EndPoint.Address.ToString();
            }
            set
            {
                EndPoint.Address = System.Net.IPAddress.Parse(value);
            }
        }
        public int Port
        {
            get
            {
                return EndPoint.Port;
            }
            set
            {
                EndPoint.Port = value;
            }
        }
        public System.Net.TransportType Protocol;
        public List<string> Plugins;
        public int StartupTimeout;
        public bool StopOnExit;
        public bool AcquireOnStart;
    }

    sealed class Server : SC.Messaging.SecureInvokeableTask, SC.Interfaces.IServer
    {
        private IDictionary<string, SC.Interfaces.IScServerPluginBase> plugins = new Dictionary<string, SC.Interfaces.IScServerPluginBase>();
        private SC.Settings.SettingsProvider settProvider;
        private ServerSettings settings;
        private Process process;
        private System.DateTime lastIteration;
        private log4net.ILog Logger;
        private bool destroyed = false;

        private event HostChangedEvent _HostChanged;
        private event SC.Interfaces.ProcessChangedEvent _ProcessChangedEvent;

        public Server(string Name)
            : base(Name, SC.Interfaces.ServerConstants.ALL_PERMISSIONS )
        {
            Logger = log4net.LogManager.GetLogger("Server " + Name);
            
            settProvider = new SC.Settings.SettingsProvider(Name);
            
            settings = ServerSettings.Empty;
            settings.Name = Name;
        }
#region Plugins
        public SC.Interfaces.IScServerPluginClient GetPlugin(string Name)
        {
            if (InvokeRequired)
                return Invoke(new SC.Messaging.RDelegate<SC.Interfaces.IScServerPluginClient, string>(this.GetPlugin), Name) as SC.Interfaces.IScServerPluginClient;
            else
            {
                return plugins[Name];
            }
        }
        public void AddPlugin(string Name)
        {
            if (InvokeRequired)
                InvokeOperation(new SC.Messaging.Delegate<string>(AddPlugin), SC.Interfaces.ServerConstants.PLUGINS_PERMISSION, Name);
            else
            {
                if (plugins.ContainsKey(Name))
                {
                    Logger.Error("A plugin with name " + Name + " already exists.");
                    throw new ArgumentException("A plugin with name " + Name + " already exists.");
                }
                
                LoadPlugin(Name);
                settings.Plugins.Add(Name);
                OnSettingsChanged(new EventArgs());
            }
        }
        private void LoadPlugin(string Name)
        {
            SC.Interfaces.IScServerPluginBase plugin = null;
            try
            {
                plugin = SC.PluginLoader.PluginLoader.Instance.LoadServerPlugin(Name);
            }
            catch (NotSupportedException e)
            {
                throw new SC.Interfaces.SCException(e);
            }
            catch (Exception e)
            {
                throw new SC.Interfaces.SCException("An unexpected error occurred during construction of plugin with name " + Name, e);
            }

            if (plugin == null)
            {
                Logger.Error("Plugin " + Name + " does not exist.");
                throw new ArgumentException("Unknown plugin");
            }

            try
            {
                plugin.AssertLicense();
            }
            catch (SC.Interfaces.LicenseException e)
            {
                Logger.Error("Plugin is not licensed.", e);
                throw;
            }

            try
            {
                plugin.SetProvider(new PluginProvider(GetName() + "." + Name));
                plugin.AttachServer(this);
                SC.Messaging.Invokeable i = plugin as SC.Messaging.Invokeable;
                if (i != null)
                    this.Register(i);
                plugins.Add(Name, plugin);
            }
            catch (Exception e1)
            {
                Logger.Error("An exception occurred during creation of plugin " + Name + ". Will try to destruct plugin.", e1);
                try
                {
                    plugin.Destroy();
                    plugin.DetachServer();
                }
                catch (Exception e2)
                {
                    Logger.Error("A second exception occurred during destruction of plugin " + Name, e2);
                }
                throw new SC.Interfaces.SCException("An exception occurred during creation of plugin.", e1); ;
            }
        }
        public void RemovePlugin(string Name)
        {
            if (InvokeRequired)
                InvokeOperation(new SC.Messaging.Delegate<string>(RemovePlugin), SC.Interfaces.ServerConstants.PLUGINS_PERMISSION, Name);
            else
            {
                if (!plugins.ContainsKey(Name))
                {
                    Logger.Error("Plugin with name " + Name + " cannot be removed because it does not exist.");
                    throw new ArgumentException("Plugin with name " + Name + " does not exist.");
                }

                try
                {
                    UnloadPlugin(Name, true);
                }
                catch (Exception e)
                {
                    Logger.Error("An unexpected exception occurred during unloading of plugin " + Name, e);
                    throw new SC.Interfaces.SCException("An exception occurred during unloading of plugin.", e);
                }
                finally
                {
                    settings.Plugins.Remove(Name);
                    plugins.Remove(Name);
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        private void UnloadPlugin(string Name, bool destroy)
        {
            Logger.Info("Unloading plugin " + Name + "...");
            SC.Interfaces.IScServerPluginBase plugin = plugins[Name];

            SC.Messaging.Invokeable i = plugin as SC.Messaging.Invokeable;
            if (i != null)
                this.Unregister(i);
            if (destroy)
                plugin.Destroy();
            plugin.DetachServer();
        }
        public bool HasPlugin(string Name)
        {
            if (InvokeRequired)
                return (bool)Invoke(new SC.Messaging.RDelegate<bool, string>(HasPlugin), Name);
            else
            {
                return plugins.ContainsKey(Name) && plugins[Name].HavePermission();
            }
        }
        public string[] Plugins
        {
            get
            {
                if (InvokeRequired)
                {
                    if (!SecurityProvider.HavePermission())
                        return new string[] { };
                    else
                        return InvokeGet<string[]>("Plugins");
                }
                else
                {
                    List<string> ret = new List<string>();
                    foreach (KeyValuePair<string, SC.Interfaces.IScServerPluginBase> plugin in plugins)
                    {
                        if (plugin.Value.HavePermission())
                            ret.Add(plugin.Key);
                    }
                    return ret.ToArray();
                }
            }
        }
#endregion
#region Host
        public System.Net.IPEndPoint EndPoint
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<System.Net.IPEndPoint>("EndPoint");
                else
                    return settings.EndPoint;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation<System.Net.IPEndPoint>("EndPoint", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.EndPoint = value;
                    OnSettingsChanged(new EventArgs());
                    OnHostChanged(new HostChangedEventArgs(settings.EndPoint, settings.Protocol));
                }
            }
        }
        private void OnHostChanged(HostChangedEventArgs newHost)
        {
            if (_HostChanged != null)
                _HostChanged(this, newHost);
        }
        public event HostChangedEvent HostChanged
        {
            add
            {
                if (InvokeRequired)
                    InvokeAdd("HostChanged", value);
                else
                    _HostChanged += value;
            }
            remove
            {
                if (InvokeRequired)
                    InvokeRemove("HostChanged", value);
                else
                    _HostChanged -= value;
            }
        }
        public System.Net.TransportType Protocol
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<System.Net.TransportType>("Protocol");
                else
                    return settings.Protocol;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation<System.Net.TransportType>("Protocol", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.Protocol = value;
                    OnSettingsChanged(new EventArgs());
                    OnHostChanged(new HostChangedEventArgs(settings.EndPoint, settings.Protocol));
                }
            }
        }
#endregion
#region Process
        public SC.Interfaces.IProcess GetProcess()
        {
            if (InvokeRequired)
            {
                return Invoke(new SC.Messaging.RDelegate<SC.Interfaces.IProcess>(GetProcess), SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION, null) as SC.Interfaces.IProcess;
            }
            else
                return process;
        }
        private void OnProcessChanged(SC.Interfaces.ProcessChangedEventArgs newProcess)
        {
            if (_ProcessChangedEvent != null)
                _ProcessChangedEvent(this, newProcess);
        }
        public event SC.Interfaces.ProcessChangedEvent ProcessChanged
        {
            add
            {
                if (InvokeRequired)
                    InvokeAddOperation("ProcessChanged", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                    _ProcessChangedEvent += value;
            }
            remove
            {
                if (InvokeRequired)
                    InvokeRemoveOperation("ProcessChanged", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                    _ProcessChangedEvent -= value;
            }
        }
        private Process Process
        {
            get
            {
                return process;
            }
            set
            {
                process = value;
                OnProcessChanged(new SC.Interfaces.ProcessChangedEventArgs(value));
            }
        }
#endregion
#region Settings
        private void OnSettingsChanged(EventArgs e)
        {
            settProvider.SaveSettings(settings);
            // event?
        }
        public string Executable
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<string>("Executable") as string;
                else
                    return settings.Executable;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation("Executable", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.Executable = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public string WorkingDirectory
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<string>("WorkingDirectory") as string;
                else
                    return settings.WorkingDirectory;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation("WorkingDirectory", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.WorkingDirectory = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public string Arguments
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<string>("Arguments") as string;
                else
                    return settings.Arguments;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation("Arguments", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.Arguments = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public string Username
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGetOperation<string>("Username", SC.Interfaces.ServerConstants.CREDENTIALS_PERMISSION) as string;
                else
                    return settings.Username;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation("Username", value, SC.Interfaces.ServerConstants.CREDENTIALS_PERMISSION);
                else
                {
                    settings.Username = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public string Password
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGetOperation<string>("Password", SC.Interfaces.ServerConstants.CREDENTIALS_PERMISSION);
                else
                {
                    if (settings.Password.Length > 0)
                        return "PASSWORD";
                    else
                        return string.Empty;
                }
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation<string>("Password", value, SC.Interfaces.ServerConstants.CREDENTIALS_PERMISSION);
                else
                {
                    System.Security.SecureString newPass = new System.Security.SecureString();
                    foreach (char c in value)
                    {
                        newPass.AppendChar(c);
                    }
                    newPass.MakeReadOnly();
                    settings.Password = new SC.Utility.EncryptedString(newPass, ServerSettings.key);
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public int StartupTimeout
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<int>("StartupTimeout");
                else
                    return settings.StartupTimeout;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSet("StartupTimeout", value);
                else
                {
                    settings.StartupTimeout = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public bool StopOnExit
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("StopOnExit");
                else
                    return settings.StopOnExit;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation("StopOnExit", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.StopOnExit = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
        public bool AcquireOnStart
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("AcquireOnStart");
                else
                    return settings.AcquireOnStart;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSetOperation("AcquireOnStart", value, SC.Interfaces.ServerConstants.EXECUTABLE_PERMISSION);
                else
                {
                    settings.AcquireOnStart = value;
                    OnSettingsChanged(new EventArgs());
                }
            }
        }
#endregion
        public SC.Interfaces.ServerConstants.ServerStatus ServerStatus
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<SC.Interfaces.ServerConstants.ServerStatus>("ServerStatus");
                else
                {
                    if (process == null)
                        return SC.Interfaces.ServerConstants.ServerStatus.Stopped;
                    else if (process.StartTime.AddMilliseconds(StartupTimeout) > DateTime.Now)
                        return SC.Interfaces.ServerConstants.ServerStatus.Starting;
                    else
                        return SC.Interfaces.ServerConstants.ServerStatus.Started;
                }
            }
        }
        public string GetName()
        {
            if (InvokeRequired)
                return (string)Invoke(new SC.Messaging.RDelegate<string>(GetName));
            else
                return settings.Name;
        }
        public void Destroy()
        {
            if (InvokeRequired)
                Invoke(new SC.Messaging.NDelegate(Destroy), null);
            else
            {
                if (!destroyed)
                {
                    foreach (SC.Interfaces.IScServerPluginBase plugin in plugins.Values)
                    {
                        plugin.Destroy();
                        plugin.DetachServer();
                    }
                    settProvider.DeleteSettings();
                    base.DestroySecurity();
                    destroyed = true;
                }
            }
        }
        private void StopProcess()
        {
            if (Process != null)
            {
                if (!Process.HasExited)
                {
                    Logger.Info("Stopping server " + GetName() + ".");
                    Process.Kill();
                    Process.WaitForExit(1000);
                }
                Process.Close();
                Process.Dispose();
                Process = null;
            }
        }
        private bool StartProcess()
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            
            info.CreateNoWindow = true;
            info.ErrorDialog = false;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            info.WorkingDirectory = settings.WorkingDirectory;
            info.UseShellExecute = false; // Documents can also be opened
            info.Arguments = settings.Arguments;
            info.FileName = settings.Executable;
            
            info.LoadUserProfile = true;
            info.UserName = settings.Username;
            info.Password = settings.Password.SecureString;
            
            info.RedirectStandardError = info.RedirectStandardInput = info.RedirectStandardOutput = true;

            StopProcess();

            Logger.Info("Starting server " + GetName());
            Process newProcess = new Process();
            try
            {
                newProcess.StartInfo = info;
                if (newProcess.Start())
                {
                    this.Process = newProcess;
                    return true;
                }
            }
            catch (ObjectDisposedException ode)
            {
                Logger.Error("Object unexpectedly disposed.", ode);
                throw new SC.Interfaces.SCException(ode);
            }
            catch (ArgumentException ae)
            {
                Logger.Error("Invalid argument used.", ae);
                throw new SC.Interfaces.SCException(ae);
            }
            catch (InvalidOperationException ioe)
            {
                Logger.Error("Could not start process: " + settings.Executable + " " + settings.Arguments, ioe);
            }
            catch (System.ComponentModel.Win32Exception w32e)
            {
                Logger.Error("Unexpected error occurred.", w32e);
            }
            return false;
        }

        public override string ToString()
        {
            return "Server: " + GetName();
        }

        protected override void Initialize()
        {
            object set = settProvider.RestoreSettings(settings.GetType());
            if (set != null)
                settings = (ServerSettings)set;

            Logger.Info("Loading plugins...");
            foreach (string plugin in settings.Plugins)
                LoadPlugin(plugin);

            if (settings.AcquireOnStart)
            {
                Process = ProcessHelper.GetProcess(settings.EndPoint);
                if (Process != null)
                    Logger.Info("Process acquired...");
                else
                    Logger.Info("Process not found...");
            }

            lastIteration = DateTime.Now;
        }
        protected override int SingleIteration()
        {
            if (destroyed)
                return 100;

            DateTime now = DateTime.Now;
            DateTime next = lastIteration.AddMilliseconds(1000);

            if (now < next)
            {
                return Convert.ToInt32((next - now).TotalMilliseconds);
            }

            if (Process != null && Process.StartTime.AddMilliseconds(settings.StartupTimeout) > DateTime.Now)
            {
                return 1000;
            }

            bool IsRunning = !(Process == null || Process.HasExited); // plugins;
            bool ShouldRun = true;

            new System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityPermissionFlag.AllFlags).Deny();
            try
            {
                foreach (SC.Interfaces.IScServerPluginBase plugin in plugins.Values)
                {
                    try
                    {
                        ShouldRun &= plugin.ShouldRun();
                    }
                    catch (System.Security.SecurityException)
                    {
                        // TODO: log
                    }
                    catch (Exception)
                    {
                        // TODO: log
                    }
                }

                if (ShouldRun)
                {
                    foreach (SC.Interfaces.IScServerPluginBase plugin in plugins.Values)
                    {
                        try
                        {
                            IsRunning &= plugin.IsRunning();
                        }
                        catch (System.Security.SecurityException)
                        {
                            // TODO: log
                        }
                        catch (Exception)
                        {
                            // TODO: log
                        }
                    }
                }
            }
            finally
            {
                System.Security.Permissions.SecurityPermission.RevertDeny();
                SC.Security.SecurityManager.Instance.UnAuthenticate();
            }

            if (ShouldRun)
            {
                if (IsRunning)
                {
                    return 1000;
                }
                else if (StartProcess())
                {
                    return 1000;
                }
                else
                {
                    Logger.Error("Server " + GetName() + " is not running as expected and could not be started. Waiting 15s");
                    return 15000;
                }
            }
            else
            {
                StopProcess();
                return 5000;
            }
        }
        protected override void Cleanup()
        {
            if (!destroyed)
            {
                Logger.Info("Cleaning up server " + GetName() + "...");
                foreach (string plugin in plugins.Keys)
                {
                    try
                    {
                        UnloadPlugin(plugin, false);
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Unexpected error during unload", e);
                    }
                }
                settProvider.SaveSettings(settings);
            }
            if (settings.StopOnExit || destroyed)
            {
                Logger.Info("Stopping process...");
                StopProcess();
            }            
            base.Cleanup();
        }
    }
}
