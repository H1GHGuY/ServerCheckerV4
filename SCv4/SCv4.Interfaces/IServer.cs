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

namespace SC.Interfaces
{
    public class ServerConstants
    {
        public const string PLUGINS_PERMISSION = "Plugins";
        public const string EXECUTABLE_PERMISSION = "Executable";
        public const string CREDENTIALS_PERMISSION = "Credentials";

        public static readonly string[] ALL_PERMISSIONS = new string[] { PLUGINS_PERMISSION, EXECUTABLE_PERMISSION, CREDENTIALS_PERMISSION };

        public enum ServerStatus
        {
            Stopped,
            Starting,
            Started,
        }

        private ServerConstants() { }
    }
    public interface IServer : IPluginServer
    {
        #region plugins
        SC.Interfaces.IScServerPluginClient GetPlugin(string Name);
        void AddPlugin(string Name);
        void RemovePlugin(string Name);
        bool HasPlugin(string Name);
        string[] Plugins { get; }
        #endregion

        #region parameters
        new System.Net.IPEndPoint EndPoint { get; set; }
        new System.Net.TransportType Protocol { get; set; }
        string Executable { get; set; }
        new string WorkingDirectory { get; set; }
        string Arguments { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        int StartupTimeout { get; set; }
        bool StopOnExit { get; set; }
        bool AcquireOnStart { get; set; }
        #endregion

        #region Status
        ServerConstants.ServerStatus ServerStatus { get; }
        bool HavePermission(string operation);
        #endregion
    }
    
    public class ProcessChangedEventArgs
    {
        private SC.Interfaces.IProcess process;

        public ProcessChangedEventArgs(SC.Interfaces.IProcess newProcess)
        {
            process = newProcess;
        }
        public SC.Interfaces.IProcess NewProcess { get { return process; } }
    }
    public delegate void ProcessChangedEvent(object sender, ProcessChangedEventArgs args);

    public interface IPluginServer
    {
        IProcess GetProcess();
        System.Net.IPEndPoint EndPoint { get; }
        System.Net.TransportType Protocol { get; }
        string WorkingDirectory { get; }
        string GetName();
        event ProcessChangedEvent ProcessChanged;
    }
}
