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
    public class AsyncResult
    {
        private Message msg;

        internal AsyncResult(Message msg)
        {
            this.msg = msg;
        }
        /// <summary>
        /// Waits untill invocation has finished
        /// </summary>
        /// <returns>return value of the invoke</returns>
        /// <exception cref="Exception">This method takes the exception specification of the invoked call</exception>
        public object GetResult()
        {
            return msg.Wait();
        }
    }

    internal class Message
    {
        // call
        Delegate dg;
        // parameters
        object[] args;
        // user credentials
        System.Security.Principal.IPrincipal principal;

        // waiter
        System.Threading.Semaphore sem;

        // return value
        object ret = null;
        // exception
        System.Exception exception = null;

        internal Message(Delegate d, params object[] args)
        {
            dg = d;
            this.args = args;
            principal = System.Threading.Thread.CurrentPrincipal;
            sem = new System.Threading.Semaphore(0, 1);
        }
        internal void Execute()
        {
            System.Diagnostics.Debug.Assert(dg != null);

            // save old principal
            System.Security.Principal.IPrincipal oldPrincipal = System.Threading.Thread.CurrentPrincipal;
            try
            {
                // set new principal
                System.Threading.Thread.CurrentPrincipal = principal;
                ret = dg.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                dg = null;
                // restore old principal
                System.Threading.Thread.CurrentPrincipal = oldPrincipal;
                sem.Release();
            }
        }
        internal object Wait()
        {
            sem.WaitOne();

            System.Diagnostics.Debug.Assert(dg == null);

            if (exception != null)
                throw exception;

            return ret;
        }
    }
}
