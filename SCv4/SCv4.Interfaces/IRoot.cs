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
    public interface IRoot
    {
        void AddServer(string Name);
        void RemoveServer(string Name);
        SC.Interfaces.IServer GetServer(string Name);
        string[] Servers { get; }

        void AddPlugin(string Name);
        SC.Interfaces.IScStandalonePluginClient GetPlugin(string Name);
        string[] Plugins { get; }
        void RemovePlugin(string Name);

        SC.Interfaces.ISecurityManager GetSecurityManager();
        SC.Interfaces.IPluginLoader GetPluginLoader();

        void IsAlive();
    }
}
