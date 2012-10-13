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
    public class ServerEncryptionSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider nextProvider;

        public ServerEncryptionSinkProvider()
        {
        }
        #region IServerChannelSinkProvider Members

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink nextSink = null;
            if (nextProvider != null)
                nextSink = nextProvider.CreateSink(channel);
            return new ServerEncryptionSink(nextSink);
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

    public sealed class ServerEncryptionSink : BaseChannelSinkWithProperties, IServerChannelSink, ISecurableChannel
    {
        private IServerChannelSink nextSink;
        private Dictionary<Guid, EncryptionHelper> contexts = new Dictionary<Guid, EncryptionHelper>();
        private object locker = new object();
        
        public ServerEncryptionSink(IServerChannelSink next)
        {
            nextSink = next;
        }

        #region IServerChannelSink Members

        void IServerChannelSink.AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, System.IO.Stream stream)
        {
            sinkStack.AsyncProcessResponse(msg, headers, stream);
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
            if (EncryptionHelper.IsInitiation(requestHeaders))
            {
                EncryptionHelper helper = new EncryptionHelper();

                responseHeaders = new TransportHeaders();
                responseStream = new System.IO.MemoryStream();
                responseMsg = requestMsg;

                helper.Respond(requestHeaders, responseHeaders);

                contexts.Add(helper.Ticket, helper);

                return ServerProcessing.Complete;
            }
            else if (EncryptionHelper.IsEncrypted(requestHeaders) && contexts.ContainsKey(EncryptionHelper.GetTicket(requestHeaders)) && contexts[EncryptionHelper.GetTicket(requestHeaders)].HasKey)
            {
                EncryptionHelper helper = contexts[EncryptionHelper.GetTicket(requestHeaders)];

                helper.Decrypt(requestHeaders, ref requestStream);

                /*System.IO.MemoryStream memStream = EncryptionHelper.ToMemoryStream(requestStream);
                string text = new string(System.Text.UTF8Encoding.UTF8.GetChars(memStream.ToArray()));*/

                ServerProcessing result = nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);

                /*System.IO.MemoryStream respStream = EncryptionHelper.ToMemoryStream(responseStream);
                string text = new string(System.Text.UTF8Encoding.UTF8.GetChars(respStream.ToArray()));
                responseStream = respStream;*/

                helper.Encrypt(responseHeaders, ref responseStream);
                
                return result;
            }
            else
                throw new System.Security.Cryptography.CryptographicException("Message is not encrypted or no key could be found for this host.");
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