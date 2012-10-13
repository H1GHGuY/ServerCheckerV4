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
    /// <summary>
    /// Base interface of all plugins towards clients (and system)
    /// </summary>
    public interface IScPluginClient
    {
        bool HavePermission();
        bool HavePermission(string operation);
    }

    /// <summary>
    /// Base interface of all plugins towards system
    /// </summary>
    public interface IScPluginBase : IScPluginClient, ILicensee
    {
        void Destroy();
    }

    /// <summary>
    /// Base interface of all standalone plugins towards clients
    /// </summary>
    public interface IScStandalonePluginClient : IScPluginClient
    {
    }
    /// <summary>
    /// Base interface of all standalone plugins towards system
    /// </summary>
    public interface IScStandalonePluginBase : IScPluginBase, IScStandalonePluginClient
    {
        void SetProvider(SC.Interfaces.IStandaloneProvider provider);
        void Start();
        void Stop();
    }
    /// <summary>
    /// Base interface of all server plugins towards clients
    /// </summary>
    public interface IScServerPluginClient : IScPluginClient
    {
        bool IsRunningStatus { get; }
        bool ShouldRunStatus { get; }
    }
    /// <summary>
    /// Base interface of all server pluginstowards system
    /// </summary>
    public interface IScServerPluginBase : IScPluginBase, IScServerPluginClient
    {
        void SetProvider(SC.Interfaces.IProvider provider);
        void AttachServer(IPluginServer server);
        void DetachServer();

        bool ShouldRun();
        bool IsRunning();
    }
}
