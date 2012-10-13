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

namespace SC.Messaging
{
    public abstract class InvokeableTask : Invokeable
    {
        private System.Threading.Thread thread = null;
        private volatile bool keepRunning = true;
        private IList<Invokeable> invokeables = new List<Invokeable>();

        public InvokeableTask(string name)
        {
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.Run));
            thread.Name = name;
        }
        public virtual void Start()
        {
            thread.Start();
        }
        public virtual void Stop(bool join)
        {
            System.Diagnostics.Debug.Assert(thread != System.Threading.Thread.CurrentThread);

            keepRunning = false;
            if (join && thread != System.Threading.Thread.CurrentThread)
                thread.Join();
        }
        private void Run()
        {
            AttachCurrentThread();
            Initialize();
            while (keepRunning)
            {
                int timeout = SingleIteration();
                bool executed = false;
                while (timeout > 0 && !executed && keepRunning)
                {
                    int time = (timeout > 1000) ? 1000 : timeout;
                    timeout -= time;

                    executed = ExecuteMsg(time);
                }
            }
            Cleanup();
        }
        protected void Register(Invokeable i)
        {
            if (InvokeRequired)
                Invoke(new SC.Messaging.Delegate<Invokeable>(Register), i);
            else
            {
                System.Diagnostics.Debug.Assert(!invokeables.Contains(i));
                if (invokeables.Contains(i))
                {
                    throw new ArgumentException("Invokeable is already associated with InvokeableTask");
                }
                i.AttachTask(this);
                invokeables.Add(i);
            }
        }
        protected void Unregister(Invokeable i)
        {
            if (InvokeRequired)
                Invoke(new SC.Messaging.Delegate<Invokeable>(Register), i);
            else
            {
                System.Diagnostics.Debug.Assert(invokeables.Contains(i));
                if (invokeables.Contains(i))
                {
                    i.DetachTask();
                    invokeables.Remove(i);
                }
            }
        }
        internal override void AttachTask(InvokeableTask task)
        {
        }
        internal override void DetachTask()
        {
        }
        protected override bool AcceptInvoke()
        {
            return keepRunning;
        }
        /// <summary>
        /// Perform a single iteration of work
        /// </summary>
        /// <param name="millis">milliseconds this function is called too early with respect to the previous return value</param>
        /// <remarks>
        /// SingleIteration may be called before the timeout has elapsed in case a message has arrived and was processed.
        /// </remarks>
        /// <returns>
        /// Time of milliseconds to wait untill called again.
        /// Return 0 to be called immediately without processing a single message.
        /// Return System.Threading.Timeout.Infinite to wait untill a message arrives
        /// </returns>
        [System.Security.Permissions.ReflectionPermission(System.Security.Permissions.SecurityAction.Deny)]
        protected abstract int SingleIteration();
        [System.Security.Permissions.ReflectionPermission(System.Security.Permissions.SecurityAction.Deny)]
        protected abstract void Initialize();
        [System.Security.Permissions.ReflectionPermission(System.Security.Permissions.SecurityAction.Deny)]
        protected abstract void Cleanup();
    }
}
