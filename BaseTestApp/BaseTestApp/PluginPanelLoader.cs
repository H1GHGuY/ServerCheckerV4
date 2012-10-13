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

namespace SC.GUI
{
    class MetaPluginPanel
    {
        public static SC.GUI.Utility.PluginPanelAttribute GetPluginPanelAttribute(Type t)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            SC.GUI.Utility.PluginPanelAttribute[] attrs = t.GetCustomAttributes(typeof(SC.GUI.Utility.PluginPanelAttribute), false) as SC.GUI.Utility.PluginPanelAttribute[];
            if (attrs == null || attrs.Length != 1)
                return null;

            return attrs[0];
        }
        public static bool IsPluginPanel(Type t)
        {
            SC.GUI.Utility.PluginPanelAttribute attr = GetPluginPanelAttribute(t);
            return attr != null && 
                typeof(SC.Interfaces.IScPluginClient).IsAssignableFrom(attr.InterfaceType) &&
                typeof(SC.GUI.Utility.PluginPanelBase).IsAssignableFrom(t);
        }
        
        private Type pluginPanelType;

        public MetaPluginPanel(Type panelType)
        {
            if (!IsPluginPanel(panelType))
                throw new ArgumentException("Panel Type is not a plugin panel");

            this.pluginPanelType = panelType;
        }

        public bool IsPanelForPlugin(SC.Interfaces.IScPluginClient plugin)
        {
            SC.GUI.Utility.PluginPanelAttribute attr = GetPluginPanelAttribute(pluginPanelType);
            return attr.InterfaceType.IsInstanceOfType(plugin);
        }

        public SC.GUI.Utility.PluginPanelBase CreatePluginPanel(SC.Interfaces.IScPluginClient plugin)
        {
            if (!IsPanelForPlugin(plugin))
                throw new ArgumentException("This plugin panel is for a different type of plugins.");

            SC.GUI.Utility.PluginPanelBase pluginpanel = Activator.CreateInstance(pluginPanelType) as SC.GUI.Utility.PluginPanelBase;
            pluginpanel.SetPlugin(plugin);
            return pluginpanel;
        }
    }
    
    class PluginPanelLoader
    {
        private List<MetaPluginPanel> plugins = new List<MetaPluginPanel>();

        public PluginPanelLoader()
        {
            ProbeAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            ScanDirectory(System.IO.Directory.GetCurrentDirectory());
        }
        private void ScanDirectory(string path)
        {
            foreach (string file in System.IO.Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    ProbeDLL(file);
                }
                catch (Exception e)
                {
                    SC.GUI.Utility.ErrorForm.ShowErrorForm(e);
                }
            }
        }
        private void ProbeDLL(string path)
        {
            ProbeAssembly(System.Reflection.Assembly.LoadFrom(path));
        }
        private void ProbeAssembly(System.Reflection.Assembly ass)
        {
            foreach (Type t in ass.GetTypes())
            {
                if (MetaPluginPanel.IsPluginPanel(t))
                {
                    plugins.Add(new MetaPluginPanel(t));
                }
            }
        }
        public SC.GUI.Utility.PluginPanelBase[] GetPanels(SC.Interfaces.IScPluginClient plugin)
        {
            List<SC.GUI.Utility.PluginPanelBase> ret = new List<SC.GUI.Utility.PluginPanelBase>();
            foreach (MetaPluginPanel meta in plugins)
            {
                if (meta.IsPanelForPlugin(plugin))
                {
                    ret.Add(meta.CreatePluginPanel(plugin));
                }
            }
            return ret.ToArray();
        }
    }
}
