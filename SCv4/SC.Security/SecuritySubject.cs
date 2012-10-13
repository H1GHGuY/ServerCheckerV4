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
    public class SecuritySubject
    {
        private string name;
        private SortedList<string, Operation> operations = new SortedList<string, Operation>();

        public SecuritySubject() { }
        public SecuritySubject(SecuritySubjectSettings settings)
        {
            name = settings.SubjectName;
            foreach (OperationSettings opsett in settings.Operations)
            {
                Operation op = new Operation(opsett);
                operations.Add(op.Name, op);
            }
        }
        internal SecuritySubject(string name)
        {
            this.name = name;
            operations.Add(Operation.DEFAULT_OPERATION, Operation.CreateDefaultOperation());
        }
        internal SecuritySubject(string name, string username)
        {
            this.name = name;
            operations.Add(Operation.DEFAULT_OPERATION, new Operation());
            AddPermission(username);
        }
        public string Name { get { return name; } }
        internal void AddOperation(string operation)
        {
            operations.Add(operation, new Operation(operation, operations[Operation.DEFAULT_OPERATION]));
        }
        internal void RemoveOperation(string operation)
        {
            operations.Remove(operation);
        }
        internal bool HavePermission()
        {
            return operations[Operation.DEFAULT_OPERATION].HavePermission();
        }
        internal bool HavePermission(string operation)
        {
            return operations[operation].HavePermission();
        }
        internal void DemandPermission()
        {
            DemandPermission(Operation.DEFAULT_OPERATION);
        }
        internal void DemandPermission(string operation)
        {
            operations[operation].DemandPermission();
        }
        internal string[] GetOperations()
        {
            return new List<string>(operations.Keys).ToArray();
        }
        internal bool HaveOperation(string operation)
        {
            return operations.ContainsKey(operation);
        }
        internal void AddPermission(string username, string operation)
        {
            operations[operation].AddPermission(username);
        }
        internal void AddPermission(string username)
        {
            AddPermission(username, Operation.DEFAULT_OPERATION);
        }
        internal void RemovePermission(string username)
        {
            foreach (Operation op in operations.Values)
            {
                op.RemovePermission(username);
            }
        }
        internal void RemovePermission(string username, string operation)
        {
            if (operation == Operation.DEFAULT_OPERATION)
                RemovePermission(username);
            else
                operations[operation].RemovePermission(username);
        }
        internal string[] GetPermissions()
        {
            return GetPermissions(Operation.DEFAULT_OPERATION);
        }
        internal string[] GetPermissions(string operation)
        {
            return operations[operation].GetPermissions();
        }
        internal SecuritySubjectSettings Settings
        {
            get
            {
                List<OperationSettings> ops = new List<OperationSettings>();
                foreach (Operation op in operations.Values)
                    ops.Add(op.Settings);
                return new SecuritySubjectSettings(name, ops);
            }
        }
    }

    [Serializable]
    public class SecuritySubjectSettings
    {
        public string SubjectName = string.Empty;
        public List<OperationSettings> Operations = new List<OperationSettings>();

        public SecuritySubjectSettings() { }
        public SecuritySubjectSettings(string name, List<OperationSettings> ops)
        {
            SubjectName = name;
            Operations = ops;
        }
    }
}
