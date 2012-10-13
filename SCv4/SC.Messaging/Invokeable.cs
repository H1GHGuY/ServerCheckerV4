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
    public delegate void NDelegate();
    public delegate void Delegate<T>(T arg1);
    public delegate void Delegate<T, T2>(T arg1, T2 arg2);
    public delegate void Delegate<T, T2, T3>(T arg1, T2 arg2, T3 arg3);
    public delegate void Delegate<T, T2, T3, T4>(T arg1, T2 arg2, T3 arg3, T4 arg4);
    public delegate void Delegate<T, T2, T3, T4, T5>(T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate R RDelegate<R>();
    public delegate R RDelegate<R, T>(T arg1);
    public delegate R RDelegate<R, T, T2>(T arg1, T2 arg2);
    public delegate R RDelegate<R, T, T2, T3>(T arg1, T2 arg2, T3 arg3);

    [System.Security.Permissions.ReflectionPermission(System.Security.Permissions.SecurityAction.Assert)]
    public class Invokeable : SC.Interfaces.EternalMarshalByRefObject
    {
        private System.Collections.Generic.Queue<Message> queue = new Queue<Message>(32);
        private System.Threading.Semaphore sem = new System.Threading.Semaphore(0, int.MaxValue);
        private volatile System.Threading.Thread thread = null;
        private InvokeableTask forwarder = null;

        public Invokeable()
        { }
        public Invokeable(InvokeableTask task)
        {
            AttachTask(task);
        }
        private void PostMessage(Message msg)
        {
            if (forwarder != null)
                forwarder.PostMessage(msg);
            else
            {
                if (!AcceptInvoke())
                    throw new InvalidOperationException("This object is unavailable for requests.");

                lock (queue)
                {
                    sem.Release();
                    queue.Enqueue(msg);
                }
            }
        }
        private Message GetNextMessage()
        {
            return GetNextMessage(System.Threading.Timeout.Infinite);
        }
        private Message GetNextMessage(int millis)
        {
            if (forwarder != null)
                throw new InvalidOperationException("Invokeable has a task attached");

            if (thread == null)
                AttachCurrentThread();

            if (sem.WaitOne(millis, false))
            {
                lock (queue)
                {
                    return queue.Dequeue();
                }
            }
            else
                return null;     
        }
        internal void AttachCurrentThread()
        {
            System.Diagnostics.Debug.Assert(thread == null);
            if (thread != null)
                throw new InvalidOperationException("Invokeable already associated with a thread");

            thread = System.Threading.Thread.CurrentThread;
        }
        internal void DetachCurrentThread()
        {
            if (thread != null && thread == System.Threading.Thread.CurrentThread)
                thread = null;
            else
                throw new InvalidOperationException("Current thread does not own this object");
        }
        internal virtual void AttachTask(InvokeableTask task)
        {
            lock (queue)
            {
                Message msg = GetNextMessage(0);
                while (msg != null)
                {
                    forwarder.PostMessage(msg);
                    msg = GetNextMessage(0);
                }
            }
            
            forwarder = task;
            thread = task.thread;
        }
        internal virtual void DetachTask()
        {
            forwarder = null;
            thread = null;
        }
        protected bool ExecuteMsg(int millis)
        {
            System.Diagnostics.Debug.Assert(!InvokeRequired);
            System.Diagnostics.Debug.Assert(forwarder == null);

            Message msg = null;
            try
            {
                msg = GetNextMessage(millis);
            }
            catch (InvalidOperationException)
            {
                // Semaphore released but no message was posted
            }
            if (msg != null)
                msg.Execute();

            return msg != null;
        }
        protected bool ExecuteMsg()
        {
            return ExecuteMsg(System.Threading.Timeout.Infinite);
        }
        protected bool InvokeRequired
        {
            get
            {
                return thread != System.Threading.Thread.CurrentThread;
            }
        }
        protected virtual object Invoke(Delegate d, params object[] args)
        {
            Message msg = new Message(d, args);
            PostMessage(msg);
            return msg.Wait();
        }
        protected virtual AsyncResult AsyncInvoke(Delegate d, params object[] args)
        {
            Message msg = new Message(d, args);
            PostMessage(msg);
            return new AsyncResult(msg);
        }
        protected virtual T InvokeGet<T>(string property)
        {
            return (T)Invoke(Delegate.CreateDelegate(typeof(SC.Messaging.RDelegate<T>), this, GetType().GetProperty(property, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetGetMethod()), null);
        }
        protected virtual void InvokeSet<T>(string property, T val)
        {
            Invoke(Delegate.CreateDelegate(typeof(SC.Messaging.Delegate<T>), this, GetType().GetProperty(property, System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetSetMethod()), val);
        }
        protected virtual void InvokeAdd(string evnt, Delegate handler)
        {
            Invoke(Delegate.CreateDelegate(GetType(), GetType().GetEvent(evnt, System.Reflection.BindingFlags.Default).GetAddMethod()), handler);
        }
        protected virtual void InvokeRemove(string evnt, Delegate handler)
        {
            Invoke(Delegate.CreateDelegate(GetType(), GetType().GetEvent(evnt, System.Reflection.BindingFlags.Default).GetRemoveMethod()), handler);
        }
        protected virtual bool AcceptInvoke()
        {
            return true;
        }
    }
}
