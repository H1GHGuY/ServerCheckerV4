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
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace SC.Remoting
{
    public sealed class ClientAuthenticationSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider nextProvider;
        private ClientAuthenticationHelper authHelper;

        public ClientAuthenticationSinkProvider(string username, string password)
        {
            authHelper = new ClientAuthenticationHelper(username, password);
        }

        #region IClientChannelSinkProvider Members

        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            IClientChannelSink nextSink = null;
            if (nextProvider != null)
                nextSink = nextProvider.CreateSink(channel, url, remoteChannelData);
            return new ClientAuthenticationSink(nextSink, authHelper);
        }

        public IClientChannelSinkProvider Next
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

    public class ClientAuthenticationSink : BaseChannelSinkWithProperties, IClientChannelSink, ISecurableChannel
    {
        private IClientChannelSink nextSink;
        private ClientAuthenticationHelper authHelper;

        internal ClientAuthenticationSink(IClientChannelSink next, ClientAuthenticationHelper authHelper)
        {
            nextSink = next;
            this.authHelper = authHelper;
        }

        #region IClientChannelSink Members

        void IClientChannelSink.AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, System.IO.Stream stream)
        {
            nextSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }

        void IClientChannelSink.AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, System.IO.Stream stream)
        {
            nextSink.AsyncProcessResponse(sinkStack, state, headers, stream);
        }

        System.IO.Stream IClientChannelSink.GetRequestStream(IMessage msg, ITransportHeaders headers)
        {
            return nextSink.GetRequestStream(msg, headers);
        }

        IClientChannelSink IClientChannelSink.NextChannelSink
        {
            get { return nextSink; }
        }

        void IClientChannelSink.ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, System.IO.Stream requestStream, out ITransportHeaders responseHeaders, out System.IO.Stream responseStream)
        {
            lock (authHelper)
            {
                if (!authHelper.HaveNonce)
                {
                    System.Runtime.Remoting.Channels.TransportHeaders authHeaders = new TransportHeaders();
                    System.IO.MemoryStream authStream = new System.IO.MemoryStream();
                    authHelper.SetRequest(authHeaders);

                    ITransportHeaders authedHeaders;
                    System.IO.Stream authedStream;
                    nextSink.ProcessMessage(msg, authHeaders, authStream, out authedHeaders, out authedStream);

                    //System.IO.MemoryStream memStream = BaseHelper.ToMemoryStream(authedStream);
                    //char[] chars = new System.Text.UTF8Encoding().GetChars(memStream.ToArray(), 0, (int)memStream.Length);


                    authHelper.SetNonce(authedHeaders);
                }
            }
            authHelper.Authenticate(requestHeaders, ref requestStream);

            nextSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
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
