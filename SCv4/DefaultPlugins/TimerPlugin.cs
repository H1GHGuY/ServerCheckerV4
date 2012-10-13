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

namespace SC.DefaultPlugins
{
    [Serializable]
    public class SCTimeSpanSettings
    {
        public int Number = 0;
        public SC.Interfaces.DefaultPlugins.TimeUnit Unit = SC.Interfaces.DefaultPlugins.TimeUnit.Minute;

        public SCTimeSpanSettings()
        {
        }
        public SCTimeSpanSettings(int number, SC.Interfaces.DefaultPlugins.TimeUnit unit)
        {
            this.Number = number;
            this.Unit = unit;
        }
    }

    public class SCTimeSpan : SC.Interfaces.EternalMarshalByRefObject, SC.Interfaces.DefaultPlugins.ISCTimeSpan
    {
        public static readonly SCTimeSpan Zero = new SCTimeSpan();

        private int nr;
        private SC.Interfaces.DefaultPlugins.TimeUnit unit;

        public SCTimeSpan(SCTimeSpanSettings settings) : this(settings.Number, settings.Unit)
        {
        }
        public SCTimeSpan(SC.Interfaces.DefaultPlugins.ISCTimeSpan ts)
            : this(ts.Number, ts.Unit)
        {
        }
        public SCTimeSpan()
            : this(0, SC.Interfaces.DefaultPlugins.TimeUnit.Minute)
        {
        }
        public SCTimeSpan(int nr, SC.Interfaces.DefaultPlugins.TimeUnit unit)
        {
            if (nr < 0)
                throw new ArgumentOutOfRangeException("nr must be positive or zero");
            this.nr = nr;
            this.unit = unit;
        }
        public int Number { get { return nr; } }
        public SC.Interfaces.DefaultPlugins.TimeUnit Unit { get { return unit; } }
        public DateTime GetNextTime(DateTime date)
        {
            if (this == Zero)
                return date;

            switch (unit)
            {
                case SC.Interfaces.DefaultPlugins.TimeUnit.Year:
                    return date.AddYears(nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Month:
                    return date.AddMonths(nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Week:
                    return date.AddDays(7 * nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Day:
                    return date.AddDays(nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Hour:
                    return date.AddHours(nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Minute:
                    return date.AddMinutes(nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Second:
                    return date.AddSeconds(nr);
                default:
                    throw new InvalidOperationException();
            }
        }
        public DateTime GetPreviousTime(DateTime date)
        {
            if (this == Zero)
                return date;
            switch (unit)
            {
                case SC.Interfaces.DefaultPlugins.TimeUnit.Year:
                    return date.AddYears(-nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Month:
                    return date.AddMonths(-nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Week:
                    return date.AddDays(-7 * nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Day:
                    return date.AddDays(-nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Hour:
                    return date.AddHours(-nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Minute:
                    return date.AddMinutes(-nr);
                case SC.Interfaces.DefaultPlugins.TimeUnit.Second:
                    return date.AddSeconds(-nr);
                default:
                    throw new InvalidOperationException();
            }
        }
        public TimeSpan GetNextTimeSpan(DateTime date)
        {
            return GetNextTime(date) - date;
        }
        public TimeSpan GetPreviousTimeSpan(DateTime date)
        {
            return GetPreviousTime(date) - date;
        }
        public static bool operator<(SCTimeSpan left, SCTimeSpan right)
        {
            return (int)left.Unit < (int)right.Unit || (left.Unit == right.Unit && left.Number < right.Number);
        }
        public static bool operator <=(SCTimeSpan left, SCTimeSpan right)
        {
            return (int)left.Unit < (int)right.Unit || (left.Unit == right.Unit && left.Number <= right.Number);
        }
        public static bool operator>(SCTimeSpan left, SCTimeSpan right)
        {
            return (int)left.Unit > (int)right.Unit || (left.Unit == right.Unit && left.Number > right.Number);
        }
        public static bool operator >=(SCTimeSpan left, SCTimeSpan right)
        {
            return (int)left.Unit > (int)right.Unit || (left.Unit == right.Unit && left.Number >= right.Number);
        }
        public static bool operator==(SCTimeSpan left, SCTimeSpan right)
        {
            return left.Number == right.Number && left.Unit == right.Unit;
        }
        public static bool operator !=(SCTimeSpan left, SCTimeSpan right)
        {
            return left.Number != right.Number || left.Unit != right.Unit;
        }
        public SCTimeSpanSettings Settings
        {
            get
            {
                return new SCTimeSpanSettings(Number, Unit);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is SC.Interfaces.DefaultPlugins.ISCTimeSpan)
            {
                SC.Interfaces.DefaultPlugins.ISCTimeSpan other = obj as SC.Interfaces.DefaultPlugins.ISCTimeSpan;
                return this.Number == other.Number && this.Unit == other.Unit;
            }
            return false;
        }
        public override string ToString()
        {
            return nr.ToString() + " " + unit.ToString();
        }
    }

    [Serializable]
    public class ActivationSettings
    {
        public DateTime StartTime = DateTime.Now;
        public SC.Interfaces.DefaultPlugins.DurationType DurationType = SC.Interfaces.DefaultPlugins.DurationType.Duration;
        public SCTimeSpanSettings Duration = new SCTimeSpanSettings();
        public SCTimeSpanSettings RepetitionTime = new SCTimeSpanSettings();

        public ActivationSettings()
        {
        }
        public ActivationSettings(DateTime startTime, SC.Interfaces.DefaultPlugins.DurationType durationType, SCTimeSpanSettings duration, SCTimeSpanSettings repetitionTime)
        {
            this.StartTime = startTime;
            this.DurationType = durationType;
            this.Duration = duration;
            this.RepetitionTime = repetitionTime;
        }
    }

    public class Activation : SC.Interfaces.EternalMarshalByRefObject, SC.Interfaces.DefaultPlugins.IActivation
    {
        private System.DateTime startTime;
        private SC.Interfaces.DefaultPlugins.DurationType durationType;
        private SC.Interfaces.DefaultPlugins.ISCTimeSpan duration;
        private SC.Interfaces.DefaultPlugins.ISCTimeSpan repetitionTime;

        public Activation(ActivationSettings sett)
        {
            duration = new SCTimeSpan(sett.Duration);
            startTime = sett.StartTime;
            repetitionTime = new SCTimeSpan(sett.RepetitionTime);
        }
        public Activation(DateTime startTime, SC.Interfaces.DefaultPlugins.ISCTimeSpan duration)
            : this(startTime, SC.Interfaces.DefaultPlugins.DurationType.Duration, duration, new SCTimeSpan())
        {
        }
        public Activation(DateTime startTime, SC.Interfaces.DefaultPlugins.DurationType durationType, SC.Interfaces.DefaultPlugins.ISCTimeSpan duration, SC.Interfaces.DefaultPlugins.ISCTimeSpan repetitionTime)
        {
            if (new SCTimeSpan(repetitionTime) > SCTimeSpan.Zero && new SCTimeSpan(repetitionTime) <= new SCTimeSpan(duration))
                throw new ArgumentException("Repitition time must be larger than duration");

            this.startTime = startTime;
            this.duration = duration;
            this.durationType = durationType;
            this.repetitionTime = repetitionTime;
        }
        public SC.Interfaces.DefaultPlugins.ActiveState State
        {
            get
            {
                DateTime now = DateTime.Now;

                if (now < StartTime)
                    return SC.Interfaces.DefaultPlugins.ActiveState.NotStarted;
                else if (now > StopTime)
                    return SC.Interfaces.DefaultPlugins.ActiveState.Ended;
                else
                    return SC.Interfaces.DefaultPlugins.ActiveState.Active;
            }
        }
        public Activation GetPendingActivation()
        {
            if (!IsRepetitive)
            {
                return Expired ? null : this;
            }

            DateTime now = DateTime.Now;
            DateTime nextStart = startTime;

            /*while (nextStart > now)
                nextStart -= repetitionTime;*/

            while (duration.GetNextTime(nextStart) < now)
                nextStart = repetitionTime.GetNextTime(nextStart);

            return new Activation(nextStart, durationType, duration, repetitionTime);
        }
        public bool IsRepetitive
        {
            get
            {
                return new SCTimeSpan(repetitionTime) != SCTimeSpan.Zero;
            }
        }
        public SC.Interfaces.DefaultPlugins.ISCTimeSpan DurationTimeSpan
        {
            get
            {
                return duration;
            }
        }
        public SC.Interfaces.DefaultPlugins.DurationType DurationType
        {
            get
            {
                return durationType;
            }
        }
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
        }
        public DateTime StopTime
        {
            get
            {
                if (DurationType == SC.Interfaces.DefaultPlugins.DurationType.Duration)
                    return DurationTimeSpan.GetNextTime(StartTime);
                else if (DurationType == SC.Interfaces.DefaultPlugins.DurationType.StopEarly)
                    return DurationTimeSpan.GetPreviousTime(RepetitionTimeSpan.GetNextTime(startTime));
                else
                    throw new SC.Interfaces.SCException("Invalid duration type");
            }
        }
        public SC.Interfaces.DefaultPlugins.ISCTimeSpan RepetitionTimeSpan
        {
            get
            {
                return repetitionTime;
            }
        }
        public bool Expired
        {
            get
            {
                return !IsRepetitive && State == SC.Interfaces.DefaultPlugins.ActiveState.Ended;
            }
        }
        public ActivationSettings Settings
        {
            get
            {
                return new ActivationSettings(startTime, durationType, new SCTimeSpan(duration).Settings, new SCTimeSpan(repetitionTime).Settings);
            }
        }
        public override bool Equals(object obj)
        {
            if (!(obj is SC.Interfaces.DefaultPlugins.IActivation))
                return false;

            SC.Interfaces.DefaultPlugins.IActivation other = obj as SC.Interfaces.DefaultPlugins.IActivation;
            if (DurationTimeSpan != other.DurationTimeSpan || RepetitionTimeSpan != other.RepetitionTimeSpan)
                return false;

            DateTime otherStartTime = other.StartTime;
            while (otherStartTime < StartTime)
                otherStartTime = RepetitionTimeSpan.GetNextTime(otherStartTime);
            while (otherStartTime > StartTime)
                otherStartTime = RepetitionTimeSpan.GetPreviousTime(otherStartTime);

            return otherStartTime == StartTime;
        }
        public override string ToString()
        {
            System.Text.StringBuilder builder = new StringBuilder();
            builder.Append("Starts at '");
            builder.Append(startTime.ToString());
            if (durationType == SC.Interfaces.DefaultPlugins.DurationType.Duration)
            {
                builder.Append("', runs for '");
                builder.Append(duration.ToString());
                builder.Append("' and repeats every '");
                builder.Append(repetitionTime.ToString());
                builder.Append("'");
            }
            else if (durationType == SC.Interfaces.DefaultPlugins.DurationType.StopEarly)
            {
                builder.Append("', repeats every '");
                builder.Append(repetitionTime.ToString());
                builder.Append("' and stops '");
                builder.Append(duration.ToString());
                builder.Append("' before repeating.");
            }
            return builder.ToString();
        }
    }
    
    [Serializable]
    public class TimerPluginSettings
    {
        public bool DefaultActive = true;
        public List<ActivationSettings> Activations = new List<ActivationSettings>();

        public TimerPluginSettings()
        {
        }
        public TimerPluginSettings(bool defaultActive, List<ActivationSettings> activations)
        {
            DefaultActive = defaultActive;
            Activations = activations;
        }
    }

    [SC.Interfaces.License("DefaultPlugins")]
    [SC.Interfaces.ScPlugin("Timer Plugin")]
    public class TimerPlugin : SC.PluginBase.AbstractServerPlugin, SC.Interfaces.DefaultPlugins.ITimerPlugin
    {        
        private bool defaultActive = true;
        private bool currentRunningStatus = true;
        private SortedList<DateTime, Activation> times = new SortedList<DateTime, Activation>();

        public TimerPlugin()
        {
        }

        #region IScServerPluginBase Members

        protected override void SaveSettings()
        {
            settProvider.SaveSettings(Settings);
        }

        protected override void RestoreSettings()
        {
            TimerPluginSettings settings = settProvider.RestoreSettings(typeof(TimerPluginSettings)) as TimerPluginSettings;
            if (settings != null)
                Settings = settings;
        }

        public bool ShouldRun()
        {
            /*                        before          while           after
             * Repeating timer      
             *      IsActive()         no              yes             no
             *      Expired            no              no              no
             *      
             * Non-Repeating timer
             *      IsActive()         no              yes             no
             *      Expired            no              no              yes
             */

            DateTime now = DateTime.Now;

            if (times.Count == 0 || times.Keys[0] > now)
                return (currentRunningStatus = defaultActive);
            
            while (times.Count > 0)
            {
                Activation act = times.Values[0];
                switch (act.State)
                {
                    case SC.Interfaces.DefaultPlugins.ActiveState.Active:
                        return (currentRunningStatus = !defaultActive);
                    case SC.Interfaces.DefaultPlugins.ActiveState.Ended:
                        times.Remove(act.StartTime);
                        Activation pending = act.GetPendingActivation();
                        if (pending != null)
                            times.Add(pending.StartTime, pending);
                        break; // next iteration;
                    case SC.Interfaces.DefaultPlugins.ActiveState.NotStarted:
                        return (currentRunningStatus = defaultActive);
                }
            }
            
            return (currentRunningStatus = defaultActive);
        }

        public bool IsRunning()
        {
            return true;
        }

        public bool IsRunningStatus
        {
            get
            {
                return true;
            }
        }
        public bool ShouldRunStatus
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("ShouldRunStatus");
                else
                    return currentRunningStatus;
            }
        }

        #endregion

        private TimerPluginSettings Settings
        {
            get
            {
                List<ActivationSettings> actsett = new List<ActivationSettings>();
                foreach (Activation act in times.Values)
                {
                    actsett.Add(act.Settings);
                }
                return new TimerPluginSettings(defaultActive, actsett);
            }
            set
            {
                defaultActive = value.DefaultActive;
                times = new SortedList<DateTime, Activation>();
                foreach (ActivationSettings actsett in value.Activations)
                {
                    Activation act = new Activation(actsett).GetPendingActivation();
                    times.Add(act.StartTime, act);
                }
            }
        }

        public bool DefaultActive
        {
            get
            {
                if (InvokeRequired)
                    return InvokeGet<bool>("DefaultActive");
                else
                    return defaultActive;
            }
            set
            {
                if (InvokeRequired)
                    InvokeSet<bool>("DefaultActive", value);
                else
                    defaultActive = value;
            }
        }
        public void AddActivation(DateTime startTime, int duration, SC.Interfaces.DefaultPlugins.TimeUnit durationTimeUnit)
        {
            AddActivation(startTime, SC.Interfaces.DefaultPlugins.DurationType.Duration, duration, durationTimeUnit, 0, SC.Interfaces.DefaultPlugins.TimeUnit.Minute);
        }
        public void AddActivation(DateTime startTime, SC.Interfaces.DefaultPlugins.DurationType durationType, int duration, SC.Interfaces.DefaultPlugins.TimeUnit durationTimeUnit, int repetition, SC.Interfaces.DefaultPlugins.TimeUnit repetitionTimeUnit)
        {
            AddActivation(startTime, durationType, new SCTimeSpan(duration, durationTimeUnit), new SCTimeSpan(repetition, repetitionTimeUnit));
        }
        private void AddActivation(DateTime startTime, SC.Interfaces.DefaultPlugins.DurationType durationType, SCTimeSpan duration, SCTimeSpan repetition)
        {
            if (InvokeRequired)
                Invoke(new SC.Messaging.Delegate<DateTime, SC.Interfaces.DefaultPlugins.DurationType, SCTimeSpan, SCTimeSpan>(AddActivation), startTime, durationType, duration, repetition);
            else
            {
                Activation act = new Activation(startTime, durationType, duration, repetition);
                act = act.GetPendingActivation();
                times.Add(act.StartTime, act);
            }
        }
        public System.Collections.ArrayList GetActivations()
        {
            if (InvokeRequired)
                return Invoke(new SC.Messaging.RDelegate<System.Collections.ArrayList>(GetActivations)) as System.Collections.ArrayList;
            else
            {
                System.Collections.ArrayList ret = new System.Collections.ArrayList();
                foreach (Activation act in times.Values)
                    ret.Add(act);
                return ret;
            }
        }
        public void RemoveActivation(SC.Interfaces.DefaultPlugins.IActivation activation)
        {
            if (InvokeRequired)
                Invoke(new SC.Messaging.Delegate<SC.Interfaces.DefaultPlugins.IActivation>(RemoveActivation), activation);
            else
            {
                if (!(times.ContainsKey(activation.StartTime) && times[activation.StartTime].Equals(activation)))
                    throw new ArgumentException("There is not such activation.");
                times.Remove(activation.StartTime);
            }
        }
    }
}
