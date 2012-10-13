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

namespace SC.GUI
{
    [Serializable]
    public class ServerSettings
    {
        public ServerSettings()
        {
        }
        public ServerSettings(System.Net.IPEndPoint endpoint, string username, string password)
        {
            this.Endpoint = endpoint;
            this.Username = username;
            this.Password = password;
        }
        public System.Net.IPEndPoint Endpoint = new System.Net.IPEndPoint(0,0);
        public string Username = string.Empty;
        public string Password = string.Empty;
    }

    public class ServerProvider
    {
        private ServerSettings settings;
        private SC.Interfaces.IRoot root;

        public ServerProvider(System.Net.IPEndPoint ep, string username, string password)
        {
            settings = new ServerSettings(ep, username, password);
            RefreshConnection();
        }
        public ServerProvider(ServerSettings settings)
        {
            this.settings = settings;
            RefreshConnection();
        }
        public void RefreshConnection()
        {
            root = SC.Client.SCClientProvider.GetRoot(settings.Endpoint.ToString(), settings.Username, settings.Password);
        }
        public SC.Interfaces.IRoot Root
        {
            get
            {
                return root;
            }
        }
        public System.Net.IPEndPoint EndPoint
        {
            get
            {
                return settings.Endpoint;
            }
        }
        internal ServerSettings Settings
        {
            get
            {
                return settings;
            }
        }
    }
}
