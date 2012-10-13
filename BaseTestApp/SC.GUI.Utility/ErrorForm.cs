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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SC.GUI.Utility
{
    public partial class ErrorForm : Form
    {
        public static void ShowErrorForm(Exception e)
        {
            using (ErrorForm ef = new ErrorForm())
            {
                ef.Exception = e;
                ef.ShowDialog();
            }
        }
        private Exception exc;

        public ErrorForm()
        {
            InitializeComponent();
        }

        public Exception Exception
        {
            get
            {
                return exc;
            }
            set
            {
                exc = value;
                if (lblText.Text == string.Empty)
                {
                    Exception e = exc;
                    while (e != null)
                    {
                        lblText.Text += exc.Message + "\r\n";
                        e = e.InnerException;
                    }
                }
                txtDetail.Text = exc.ToString();
            }
        }
        public string ExceptionText
        {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }
    }
}
