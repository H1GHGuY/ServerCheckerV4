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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace SC.Remoting
{
    internal class ClientContext
    {
        private System.DateTime lastSeen = DateTime.Now;
        private ServerAuthenticationHelper authHelper = new ServerAuthenticationHelper();

        internal ClientContext()
        {
        }
        public ServerAuthenticationHelper AuthHelper
        {
            get
            {
                if (SessionExpired)
                    throw new SC.Interfaces.SessionExpiredException();
                return authHelper;
            }
        }
        public bool SessionExpired
        {
            get
            {
                return lastSeen + new TimeSpan(0, 30, 0) < DateTime.Now;
            }
        }
    }

    public class ServerAuthenticationSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider nextProvider;

        private SC.Security.SecurityManager secMgr;
        private Dictionary<Guid, ClientContext> contexts = new Dictionary<Guid, ClientContext>();

        public ServerAuthenticationSinkProvider(SC.Security.SecurityManager mgr)
        {
            this.secMgr = mgr;
        }
        #region IServerChannelSinkProvider Members

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink nextSink = null;
            if (nextProvider != null)
                nextSink = nextProvider.CreateSink(channel);
            return new ServerAuthenticationSink(nextSink, secMgr, contexts);
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
            // EMPTY
        }

        public IServerChannelSinkProvider Next
        {
            get
            {
                return nextProvider;
            }
            set
            {
                nextProvider = value;
            }
        }

        #endregion
    }

    internal sealed class ServerAuthenticationSink : BaseChannelSinkWithProperties, IServerChannelSink, ISecurableChannel
    {
        private IServerChannelSink nextSink;
        private SC.Security.SecurityManager secMgr;
        private Dictionary<Guid, ClientContext> contexts;
        private log4net.ILog Logger = log4net.LogManager.GetLogger("Authentication Layer");

        public ServerAuthenticationSink(IServerChannelSink next, SC.Security.SecurityManager mgr, Dictionary<Guid, ClientContext> contexts)
        {
            nextSink = next;
            this.secMgr = mgr;
            this.contexts = contexts;
        }
        
        #region IServerChannelSink Members

        void IServerChannelSink.AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, System.IO.Stream stream)
        {
#if DEBUG
            System.Diagnostics.Trace.Assert((state as System.Threading.Thread).Equals(System.Threading.Thread.CurrentThread));
#endif
            sinkStack.AsyncProcessResponse(msg, headers, stream);
            ServerAuthenticationHelper.UnAuthenticate();
        }

        System.IO.Stream IServerChannelSink.GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
        {
            return sinkStack.GetResponseStream(msg, headers);
        }

        IServerChannelSink IServerChannelSink.NextChannelSink
        {
            get { return nextSink; }
        }

        ServerProcessing IServerChannelSink.ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, System.IO.Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out System.IO.Stream responseStream)
        {
            System.Net.IPAddress address = System.Net.IPAddress.Parse(requestHeaders[CommonTransportKeys.IPAddress].ToString());
            if (!IsTrustedIP(address))
            {
                Logger.Error("Client connection refused from  " + address.ToString());
                throw new SC.Interfaces.SCException("Connection refused from address " + address.ToString());
            }

            if (ServerAuthenticationHelper.IsChallengeRequest(requestHeaders))
            {
                Logger.Info("Client connection accepted from " + address.ToString());
                ClientContext context = new ClientContext();
                
                responseHeaders = new TransportHeaders();
                responseStream = new System.IO.MemoryStream();
                
                context.AuthHelper.Challenge(responseHeaders);
                responseMsg = requestMsg;

                lock (contexts)
                    contexts[context.AuthHelper.Ticket] = context;

                return ServerProcessing.Complete;
            }
            if (!AuthenticationHelper.HasValidTicket(requestHeaders))
                throw new SC.Interfaces.SCException("Protocol violation: No ticket");

            Guid ticket = AuthenticationHelper.GetTicket(requestHeaders);

            ClientContext ctxt = null;
            lock (contexts)
            {
                if (contexts.ContainsKey(ticket))
                    ctxt = contexts[ticket];
            }

            if (ctxt == null)
            {
                throw new SC.Interfaces.SCException("Protocol violation: issue a challenge request first.");
            }
            else if (ctxt.SessionExpired)
            {
                Logger.Info("Session for address " + address.ToString() + " expired.");
                throw new SC.Interfaces.SessionExpiredException();
            }

            ctxt.AuthHelper.Authenticate(requestHeaders, ref requestStream);
            ServerProcessing result = nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
            if (result == ServerProcessing.Async)
            {
                sinkStack.Push(this,
#if DEBUG
                    System.Threading.Thread.CurrentThread
#else
                    null
#endif
);
            }
            else
                ServerAuthenticationHelper.UnAuthenticate();
            return result;
        }

        private bool IsTrustedIP(System.Net.IPAddress address)
        {
            return secMgr.IsClientIPAllowed(address);
        }

        #endregion

        #region IChannelSinkBase Members

        System.Collections.IDictionary IChannelSinkBase.Properties
        {
            get { return nextSink.Properties; }
        }

        #endregion

        #region ISecurableChannel Members

        bool ISecurableChannel.IsSecured
        {
            get
            {
                return false;
            }
            set
            {
                // NOOP
            }
        }

        #endregion
    }
}