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
using System.Linq;
using System.Text;

namespace SC.GUI.Utility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginPanelAttribute : Attribute
    {
        private Type type;
        public PluginPanelAttribute(Type InterfaceType)
        {
            if (!typeof(SC.Interfaces.IScPluginClient).IsAssignableFrom(InterfaceType))
                throw new ArgumentException("InterfaceType does not derive from SC.Interfaces.IScPluginBase");
            else if (!InterfaceType.IsInterface)
                throw new ArgumentException("InterfaceType is not an interface.");

            this.type = InterfaceType;
        }
        public Type InterfaceType
        {
            get
            {
                return type;
            }
        }
    }
}
