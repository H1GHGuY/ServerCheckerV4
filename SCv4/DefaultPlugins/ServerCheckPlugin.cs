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
using SC.Interfaces.DefaultPlugins;

namespace SC.DefaultPlugins
{
    public class ServerCheckPluginSettings
    {
        public eGameType GameType = eGameType.Custom;
        public byte[] CustomCommand = new byte[] { };
        public int Timeout = 100;
    }

    [SC.Interfaces.License("DefaultPlugins")]
    [SC.Interfaces.ScPlugin("ServerChecker Plugin")]
    [System.Net.SocketPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted=true)]
    public class ServerCheckPlugin : SC.PluginBase.AbstractServerPlugin, SC.Interfaces.DefaultPlugins.IServerCheckPlugin
    {
        /*static const byte[] pings = new byte[] {"\xFF\xFF\xFF\xFF\x54\x53\x6F\x75\x72\x63\x65\x20\x45\x6E\x67\x69\x6E\x65\x20\x51\x75\x65\x72\x79\x00", // HL/HL2
								"\x5C\x62\x61\x73\x69\x63\x5C", // UT/GS
								"\xFE\xFD\x00\x43\x4F\x52\x59\xFF\xFF\x00",	// GS2
								"\xFE\xFD\x00\x0C\xAE\x3D\x00\xFF\xFF\xFF\x01", // GS3 //"\xFE\xFD\x00\x04\xF7\x11\x00\x09\x04\x05\x06\x07\x0B\x01\x08\x0a\x13\x00\x00",
								"\xFF\xFF\xFF\xFF\x67\x65\x74\x73\x74\x61\x74\x75\x73", // Q3
								"\xFF\xFF\xFF\xFF\x73\x74\x61\x74\x75\x73", // Q2
								"\xFF\xFF\x67\x65\x74\x49\x6E\x66\x6F", //Doom3
								"\x73", // All Seeing Eye
								"\x52\x45\x50\x4F\x52\x54", // Raven Shield
								"" // Normal Program
								};*/

        private System.Net.Sockets.Socket socket = null;
        private ServerCheckPluginSettings settings = new ServerCheckPluginSettings();
        private DateTime lastSeen = DateTime.Now;
        private bool currentRunningStatus = true;
        private log4net.ILog Logger = null;

        public ServerCheckPlugin()
        {
            Logger = log4net.LogManager.GetLogger(GetName());
        }

        #region IScServerPluginBase Members

        public override void AttachServer(SC.Interfaces.IPluginServer server)
        {
            base.AttachServer(server);
            
            System.Net.Sockets.SocketType sockType;
            System.Net.Sockets.ProtocolType protoType;
            if (server.Protocol == System.Net.TransportType.Tcp || server.Protocol == System.Net.TransportType.ConnectionOriented)
            {
                sockType = System.Net.Sockets.SocketType.Stream;
                protoType = System.Net.Sockets.ProtocolType.Tcp;
            }
            else if (server.Protocol == System.Net.TransportType.Udp || server.Protocol == System.Net.TransportType.Connectionless)
            {
                sockType = System.Net.Sockets.SocketType.Dgram;
                protoType = System.Net.Sockets.ProtocolType.Udp;
            }
            else
                throw new ArgumentException("Invalid protocol specified");

            socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, sockType, protoType);
            
            socket.Blocking = false;
            socket.DontFragment = false;
            socket.EnableBroadcast = false;
            if (protoType == System.Net.Sockets.ProtocolType.Tcp)
            {
                socket.LingerState.Enabled = false;
                socket.LingerState.LingerTime = 0;
                socket.NoDelay = true;
            }
            socket.UseOnlyOverlappedIO = false;
            
            socket.Connect(server.EndPoint);

            Logger.Info("Attached to server " + server.GetName());
        }

        protected override void SaveSettings()
        {
            settProvider.SaveSettings(settings);
        }

        protected override void RestoreSettings()
        {
            Logger.Info("Loading settings for " + GetName());
            ServerCheckPluginSettings sett = settProvider.RestoreSettings(typeof(ServerCheckPluginSettings)) as ServerCheckPluginSettings;
            if (sett != null)
                settings = sett;
        }

        public bool ShouldRun()
        {
            return true;
        }
        public bool IsRunning()
        {
            byte[] bytes = Command;
            int n = socket.Send(Command, Command.Length, System.Net.Sockets.SocketFlags.None);
            if (n == 0)
                return false;
            
            int tries = 10;
            while (!socket.Poll(10, System.Net.Sockets.SelectMode.SelectRead) && tries > 0)
                tries--;

            bool active = (socket.Available > 0);
            while (socket.Available > 0)
            {
                byte[] buffer = new byte[1500];
                socket.Receive(buffer);
            }

            if (active)
                lastSeen = DateTime.Now;

            currentRunningStatus = lastSeen.AddMilliseconds(settings.Timeout) > DateTime.Now;
            if (!currentRunningStatus)
                Logger.Warn("Server " + server.GetName() + " unreachable since " + lastSeen);

            return currentRunningStatus;
        }

        #endregion

        #region IScPluginBase Members

        public bool IsRunningStatus
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("IsRunningStatus");
                else
                    return currentRunningStatus;
            }
        }
        public bool ShouldRunStatus
        {
            get
            {
                return true;
            }
        }

        #endregion

        private byte[] Command
        {
            get
            {
                if (settings.GameType == eGameType.Custom)
                    return settings.CustomCommand;
                else
                {
                    return Convert.FromBase64String(Commands.ResourceManager.GetString(Enum.GetName(typeof(eGameType), GameType)));
                }
            }
        }
        public eGameType GameType
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<eGameType>("GameType");
                else
                    return settings.GameType;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSet("GameType", value);
                else
                {
                    settings.GameType = value;
                }
            }
        }
        public byte[] CustomCommand
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<byte[]>("CustomCommand");
                else
                    return settings.CustomCommand;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSet("CustomCommand", value);
                else
                    settings.CustomCommand = value;
            }
        }
        public int Timeout
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<int>("Timeout");
                else
                    return settings.Timeout;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Timeout must be greater than or equal to 0");

                if (InvokeRequired)
                    InvokeSet("Timeout", value);
                else
                {
                    settings.Timeout = value;
                }
            }
        }
        private string GetName()
        {
            SC.Interfaces.ScPluginAttribute[] attrs = GetType().GetCustomAttributes(typeof(SC.Interfaces.ScPluginAttribute), false) as SC.Interfaces.ScPluginAttribute[];
            System.Diagnostics.Debug.Assert(attrs.Length == 1);
            if (attrs.Length == 1)
                return attrs[0].Name;
            else
                throw new NotImplementedException("ServerCheckPlugin object does not have a ScPluginAttribute");
        }
    }
}
