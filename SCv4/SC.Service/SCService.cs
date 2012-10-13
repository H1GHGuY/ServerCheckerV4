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

namespace SC.Service
{
    public class SCService : System.ServiceProcess.ServiceBase
    {
        private SC.Core.Program program = null;

        public SCService()
        {
            InitializeComponent();
        }

        [MTAThread()]
        public static void Main()
        {
            System.ServiceProcess.ServiceBase.Run(new SCService());
        }

        protected override void OnStart(string[] args)
        {
            if (program == null)
            {
                program = new SC.Core.Program();
                program.Start();
            }
            else
            {
                // TODO:
            }
            base.OnStart(args);
        }
        protected override void OnStop()
        {
            if (program != null)
            {
                program.Stop();
                program = null;
            }
            base.OnStop();
        }

        protected override void OnSessionChange(System.ServiceProcess.SessionChangeDescription changeDescription)
        {
            // TODO?
            base.OnSessionChange(changeDescription);
        }

        private void InitializeComponent()
        {
            // 
            // SCService
            // 
            this.ServiceName = "ServerCheckerV4";

        }
    }
}
