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
    [Serializable]
    public class Operation
    {
        internal const string DEFAULT_OPERATION = "Default";

        private string operation = DEFAULT_OPERATION;
        private AccessControlList list = new AccessControlList();

        public Operation()
        {
        }
        public Operation(Operation other)
        {
            operation = DEFAULT_OPERATION;
            list = other.list.Clone() as AccessControlList;
        }
        public Operation(string name)
        {
            if (name == DEFAULT_OPERATION)
                throw new ArgumentException("Operation name 'Default' is reserved.");
            operation = name;
        }
        public Operation(string name, Operation other)
        {
            if (name == DEFAULT_OPERATION)
                throw new ArgumentException("Operation name 'Default' is reserved.");

            operation = name;
            list = other.list.Clone() as AccessControlList;
        }
        public Operation(OperationSettings settings)
        {
            operation = settings.Name;
            list = new AccessControlList(settings.ACL);
        }
        public static Operation CreateDefaultOperation()
        {
            Operation op = new Operation();
            foreach (string user in SC.Security.SecurityManager.Instance.GetDefaultAccess())
                op.AddPermission(user);
            return op;
        }
        public string Name
        {
            get
            {
                return operation;
            }
        }
        public void DemandPermission()
        {
            list.DemandPermission();
        }
        public bool HavePermission()
        {
            return list.HavePermission(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }
        public void AddPermission(string username)
        {
            list.AddPermission(username);
        }
        public void RemovePermission(string username)
        {
            list.RemovePermission(username);
        }
        public string[] GetPermissions()
        {
            return list.GetPermissions();
        }

        internal OperationSettings Settings
        {
            get
            {
                return new OperationSettings(operation, list.Settings);
            }
        }
    }

    [Serializable]
    public class OperationSettings
    {
        public string Name;
        public AccessControlListSettings ACL;
        public OperationSettings() { }
        public OperationSettings(string name, AccessControlListSettings acl)
        {
            Name = name;
            ACL = acl;
        }
    }
}
