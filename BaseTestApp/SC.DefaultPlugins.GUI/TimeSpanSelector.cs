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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SC.GUI.Utility
{
    public partial class TimeSpanSelector : UserControl
    {
        public TimeSpanSelector()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            foreach (SC.Interfaces.DefaultPlugins.TimeUnit unit in Enum.GetValues(typeof(SC.Interfaces.DefaultPlugins.TimeUnit)))
                cmbUnit.Items.Add(unit);
            cmbUnit.SelectedIndex = 0;
            base.OnLoad(e);
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(this, args);
        }

        private void numNr_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(e);
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnValueChanged(e);
        }

        public int Number
        {
            get
            {
                return Convert.ToInt32(numNr.Value);
            }
            set
            {
                numNr.Value = value;
            }
        }
        [System.ComponentModel.Browsable(false)]
        public SC.Interfaces.DefaultPlugins.TimeUnit Unit
        {
            get
            {
                if (DesignMode)
                    return SC.Interfaces.DefaultPlugins.TimeUnit.Minute;
                return (SC.Interfaces.DefaultPlugins.TimeUnit)cmbUnit.SelectedItem;
            }
            set
            {
                if (DesignMode)
                    return;
                cmbUnit.SelectedItem = value;
            }
        }
    }
}
