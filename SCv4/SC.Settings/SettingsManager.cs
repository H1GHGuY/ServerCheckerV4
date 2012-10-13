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
    [System.Security.Permissions.FileIOPermission(System.Security.Permissions.SecurityAction.Assert)]
    public class SettingsManager
    {
        public static readonly SettingsManager Instance = new SettingsManager();
        private log4net.ILog logger = log4net.LogManager.GetLogger("Settings");

        private SettingsManager()
        {
        }
        /// <summary>
        /// </summary>
        /// <param name="Name">Name of the item</param>
        /// <exception cref="ArgumentNullException">Name is null</exception>
        /// <exception cref="ArgumentException">Error during construction of path</exception>
        /// <exception cref="System.IO.PathTooLongException">Path is too long</exception>
        /// <returns>full path name of the settings file</returns>
        private string FileNameOf(string Name)
        {
            if (Name == null)
            {
                logger.Error("Name is 'null'; Cannot construct path");
                throw new ArgumentNullException("Name");
            }

            try
            {
                return System.IO.Path.Combine(
                            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                            Name + ".SC4.xml");
            }
            catch (Exception e)
            {
                logger.Error("Could not construct path information for " + Name + ". Settings directory = \"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\"", e);
                throw;
            }
        }
        /// <summary>
        /// Restores settings previously saved with SaveXmlSettings
        /// </summary>
        /// <param name="Name">Name of the settings</param>
        /// <param name="t">Type of the settings object</param>
        /// <exception cref="ArgumentNullException">Name or type are null</exception>
        /// <exception cref="ArgumentException">Error during construction of path</exception>
        /// <exception cref="???">Error during deserialization</exception>
        /// <returns>saved settings or null if settings not found</returns>
        public object RestoreSettings(string Name, Type type)
        {
            if (type == null)
            {
                logger.Error("Trying to restore settings without specifying an object type for name: " + Name);
                throw new ArgumentNullException("type");
            }
            string path = FileNameOf(Name);
            if (!System.IO.File.Exists(path))
            {
                logger.Debug("No settings known for: " + Name);
                return null;
            }

            try
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(type);
                    object ret = s.Deserialize(stream);
                    logger.Debug("Read settings for " + Name);
                    return ret;
                }
            }
            catch (Exception e)
            {
                logger.Error("Could not read settings for " + Name + " of type " + type.ToString(), e);
                throw;
            }
        }
        public void SaveSettings(string Name, object Settings)
        {
            if (Settings == null)
            {
                logger.Error("Can not save 'null' as settings");
                throw new ArgumentNullException("Settings");
            }

            try
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(FileNameOf(Name), System.IO.FileMode.Create))
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(Settings.GetType());
                    s.Serialize(stream, Settings);
                    logger.Debug("Saved settings for " + Name);
                }
            }
            catch (Exception e)
            {
                logger.Error("Could not save settings for " + Name + " of type " + Settings.GetType().ToString(), e);
                throw;
            }
        }
        /// <summary>
        /// Removes settings from disk
        /// </summary>
        /// <param name="Name">Name of the settings</param>
        public void DeleteSettings(string Name)
        {
            try
            {
                System.IO.File.Delete(FileNameOf(Name));
                logger.Debug("Removed settings for " + Name);
            }
            catch (Exception e)
            {
                logger.Error("Could not remove settings for " + Name, e);
                throw;
            }
        }
    }
}
