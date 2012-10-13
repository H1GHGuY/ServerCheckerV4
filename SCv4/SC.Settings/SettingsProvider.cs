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

namespace SC.Settings
{
    public class SettingsProvider : SC.Interfaces.ISettingsProvider
    {
        private readonly string name;
        private bool destroyed = false;

        public SettingsProvider(string Name)
        {
            name = Name;
        }

        #region ISettingsProvider Members

        public object RestoreSettings(Type t)
        {
            if(!destroyed)
                return SC.Settings.SettingsManager.Instance.RestoreSettings(name, t);
            return null;
        }

        public void SaveSettings(object o)
        {
            if (!destroyed)
                SC.Settings.SettingsManager.Instance.SaveSettings(name, o);
        }

        public void DeleteSettings()
        {
            SC.Settings.SettingsManager.Instance.DeleteSettings(name);
        }

        /*public bool Destroyed
        {
            get
            {
                return destroyed;
            }
        }*/

        public void Destroy()
        {
            destroyed = true;
            DeleteSettings();
        }

        #endregion
    }
}
