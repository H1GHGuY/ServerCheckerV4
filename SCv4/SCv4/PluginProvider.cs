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

namespace SC.Core
{
    public class PluginProvider : SC.Interfaces.IProvider
    {
        private object locker = new object();

        private string fullName;
        private SC.Interfaces.ISecurityProvider secProvider = null;
        private SC.Interfaces.ISettingsProvider settProvider = null;

        public PluginProvider(string fullName)
        {
            this.fullName = fullName;
        }

        #region IProvider Members

        public SC.Interfaces.ISecurityProvider GetSecurityProvider()
        {
            lock (locker)
            {
                if (secProvider == null)
                    secProvider = new SC.Security.SecurityProvider(fullName);
                return secProvider;
            }
        }

        public SC.Interfaces.ISecurityProvider GetSecurityProvider(string[] additionalOperations)
        {
            lock (locker)
            {
                if (secProvider == null)
                    throw new InvalidOperationException("Additional operations cannot be specified twice.");
                
                secProvider = new SC.Security.SecurityProvider(fullName, additionalOperations);
                return secProvider;
            }
        }
        public SC.Interfaces.ISettingsProvider GetSettingsProvider()
        {
            lock (locker)
            {
                if (settProvider == null)
                    settProvider = new SC.Settings.SettingsProvider(fullName);
                return settProvider;
            }
        }

        #endregion
    }

    public class StandaloneProvider : PluginProvider, SC.Interfaces.IStandaloneProvider
    {
        private SC.Interfaces.IRoot root;

        public StandaloneProvider(string name, SC.Interfaces.IRoot root)
            : base(name)
        {
            this.root = root;
        }

        #region IStandaloneProvider Members

        public SC.Interfaces.IRoot GetRoot()
        {
            return root;
        }

        public SC.Interfaces.IAuthenticationManager GetAuthenticationManager()
        {
            return SC.Security.SecurityManager.Instance;
        }

        #endregion
    }

}
