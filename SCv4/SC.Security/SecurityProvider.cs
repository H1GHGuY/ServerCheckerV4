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

namespace SC.Security
{
    public class SecurityProvider : SC.Interfaces.ISecurityProvider
    {
        private string name;
        private Guid guid;
        private bool registered = true;
        private bool destroyed = false;

        public SecurityProvider(string name)
        {
            this.name = name;
            guid = SC.Security.SecurityManager.Instance.RegisterSubject(name);
        }
        public SecurityProvider(string name, string[] additionalOperations)
        {
            this.name = name;
            guid = SC.Security.SecurityManager.Instance.RegisterSubject(name, additionalOperations);
        }
        public string Name { get { return name; } }
        public void DemandPermission()
        {
            //if (registered)
                SC.Security.SecurityManager.Instance.DemandPermissions(guid);
        }
        public void DemandPermission(string operation)
        {
            //if (registered)
                SC.Security.SecurityManager.Instance.DemandPermissions(guid, operation);
        }
        public void Destroy()
        {
            if (registered)
            {
                SC.Security.SecurityManager.Instance.UnregisterSubject(guid, true);
                registered = false;
                destroyed = true;
            }
            else if (!destroyed)
            {
                SC.Security.SecurityManager.Instance.Logger.Error("Trying to destroy security attributes after cleaning up");
                throw new InvalidOperationException("Trying to destroy security attributes after cleaning up");
            }
        }
        public void Cleanup()
        {
            if (registered)
            {
                SC.Security.SecurityManager.Instance.UnregisterSubject(guid);
                registered = false;
            }
        }
        public bool HavePermission()
        {
            //if (registered)
                return SC.Security.SecurityManager.Instance.HavePermission(guid);
        }
        public bool HavePermission(string operation)
        {
            // if (registered)
                return SC.Security.SecurityManager.Instance.HavePermission(guid, operation);
        }
        ~SecurityProvider()
        {
            System.Diagnostics.Debug.Assert(!registered);
            if (registered)
            {
                SC.Security.SecurityManager.Instance.Logger.Error("Subject " + name + " was not cleaned up properly");
                SC.Security.SecurityManager.Instance.UnregisterSubject(guid);
            }
        }
    }
}
