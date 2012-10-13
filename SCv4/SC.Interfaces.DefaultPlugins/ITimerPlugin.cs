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

namespace SC.Interfaces.DefaultPlugins
{
    public interface ITimerPlugin : SC.Interfaces.IScServerPluginBase
    {
        void AddActivation(DateTime startTime, int duration, SC.Interfaces.DefaultPlugins.TimeUnit durationTimeUnit);
        void AddActivation(DateTime startTime, DurationType durationType, int duration, TimeUnit durationTimeUnit, int repitition, TimeUnit repititionUnit);
        System.Collections.ArrayList GetActivations();
        void RemoveActivation(IActivation activation);
        bool DefaultActive { get; set; }
    }
}
