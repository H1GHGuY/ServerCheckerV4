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

namespace SC.Messaging
{
    public abstract class SecureInvokeableTask : InvokeableTask
    {
        private string name;
        private SC.Interfaces.ISecurityProvider securityProvider;

        public SecureInvokeableTask(string name, SC.Interfaces.ISecurityProvider secProvider) : base(name)
        {
            this.name = name;
            securityProvider = secProvider;
        }
        public SecureInvokeableTask(string Name) : this(Name, new SC.Security.SecurityProvider(Name))
        {
        }
        public SecureInvokeableTask(string Name, string[] additionalOperations) : this(Name, new SC.Security.SecurityProvider(Name, additionalOperations))
        {
        }
        protected SC.Interfaces.ISecurityProvider SecurityProvider
        {
            get
            {
                return securityProvider;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public bool HavePermission()
        {
            return SecurityProvider.HavePermission();
        }
        public bool HavePermission(string operation)
        {
            return SecurityProvider.HavePermission(operation);
        }
        protected void DemandPermissions()
        {
            SecurityProvider.DemandPermission();
        }
        protected void DemandPermissions(string operation)
        {
            SecurityProvider.DemandPermission(operation);
        }
        protected virtual void DestroySecurity()
        {
            SecurityProvider.Destroy();
        }
        protected override void Cleanup()
        {
            SecurityProvider.Cleanup();
        }
        protected override object Invoke(Delegate d, params object[] args)
        {
            DemandPermissions();
            return base.Invoke(d, args);
        }
        protected object InvokeOperation(Delegate d, string operation, params object[] args)
        {
            DemandPermissions(operation);
            return base.Invoke(d, args);
        }
        protected override AsyncResult AsyncInvoke(Delegate d, params object[] args)
        {
            DemandPermissions();
            return base.AsyncInvoke(d, args);
        }
        protected AsyncResult AsyncInvokeOperation(Delegate d, string operation, params object[] args)
        {
            DemandPermissions(operation);
            return base.AsyncInvoke(d, args);
        }
        protected override T InvokeGet<T>(string property)
        {
            DemandPermissions();
            return base.InvokeGet<T>(property);
        }
        protected T InvokeGetOperation<T>(string property, string operation)
        {
            DemandPermissions(operation);
            return base.InvokeGet<T>(property);
        }
        protected override void InvokeSet<T>(string property, T val)
        {
            DemandPermissions();
            base.InvokeSet<T>(property, val);
        }
        protected void InvokeSetOperation<T>(string property, T val, string operation)
        {
            DemandPermissions(operation);
            base.InvokeSet<T>(property, val);
        }
        protected override void InvokeAdd(string evnt, Delegate handler)
        {
            DemandPermissions();
            base.InvokeAdd(evnt, handler);
        }
        protected void InvokeAddOperation(string evnt, Delegate handler, string operation)
        {
            DemandPermissions(operation);
            base.InvokeAdd(evnt, handler);
        }
        protected override void InvokeRemove(string evnt, Delegate handler)
        {
            DemandPermissions();
            base.InvokeRemove(evnt, handler);
        }
        protected void InvokeRemoveOperation(string evnt, Delegate handler, string operation)
        {
            DemandPermissions(operation);
            base.InvokeRemove(evnt, handler);
        }
    }
}
