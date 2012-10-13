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
    internal class AccessControlList : ICloneable
    {
        private System.Collections.Generic.SortedList<string, AccessControlEntry> entries = new SortedList<string, AccessControlEntry>();

        internal AccessControlList()
        {
        }
        internal AccessControlList(AccessControlListSettings sett)
        {
            foreach (AccessControlEntrySettings entry in sett.Entries)
            {
                entries.Add(entry.Username, new AccessControlEntry(entry));
            }
        }
        private System.Security.IPermission CreatePermission()
        {
            System.Security.IPermission ret = new System.Security.Permissions.PrincipalPermission(SC.Security.SecurityManager.SYSTEM_ACCOUNT, SC.Security.SecurityManager.SYSTEM_ROLE);
            foreach (AccessControlEntry entry in entries.Values)
            {
                ret = ret.Union(entry.CreatePermission());
            }
            return ret;
        }
        internal void DemandPermission()
        {
            System.Security.IPermission perm = CreatePermission();
            if (perm != null)
            {
                perm.Demand();
            }
            else
                throw new System.Security.SecurityException("You do not have the required priviliges for this operation.");
        }
        internal bool HavePermission(string username)
        {
            System.Security.IPermission perm = CreatePermission();
            if (perm != null)
                return new AccessControlEntry(username).CreatePermission().IsSubsetOf(perm);
            else
                return false;
        }
        internal void AddPermission(string username)
        {
            entries.Add(username, new AccessControlEntry(username));
        }
        internal void RemovePermission(string username)
        {
            entries.Remove(username);
        }
        internal string[] GetPermissions()
        {
            return new List<string>(entries.Keys).ToArray();
        }
        internal AccessControlListSettings Settings
        {
            get
            {
                List<AccessControlEntrySettings> ret = new List<AccessControlEntrySettings>();
                foreach (AccessControlEntry entry in entries.Values)
                    ret.Add(entry.Settings);
                return new AccessControlListSettings(ret);
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            AccessControlList list = new AccessControlList();
            foreach (string user in entries.Keys)
            {
                list.AddPermission(user);
            }
            return list;
        }

        #endregion
    }
    [Serializable]
    public class AccessControlListSettings
    {
        public List<AccessControlEntrySettings> Entries = new List<AccessControlEntrySettings>();
        public AccessControlListSettings() { }
        public AccessControlListSettings(List<AccessControlEntrySettings> entries)
        {
            this.Entries = entries;
        }
    }

    internal class AccessControlEntry
    {
        private string username;
        internal AccessControlEntry(string username)
        {
            this.username = username;
        }
        internal AccessControlEntry(AccessControlEntrySettings sett)
        {
            this.username = sett.Username;
        }
        internal System.Security.IPermission CreatePermission()
        {
            return new System.Security.Permissions.PrincipalPermission(username, SC.Security.SecurityManager.USER_ROLE);
        }
        internal AccessControlEntrySettings Settings
        {
            get
            {
                return new AccessControlEntrySettings(username);
            }
        }
    }

    [Serializable]
    public class AccessControlEntrySettings
    {
        public string Username;
        public AccessControlEntrySettings()
        {
        }
        public AccessControlEntrySettings(string username)
        {
            Username = username;
        }
    }
}
