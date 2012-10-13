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
    public sealed class ClientEncryptionSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider nextProvider;
        private EncryptionHelper encrHelper = new EncryptionHelper();

        public ClientEncryptionSinkProvider()
        {
        }

        #region IClientChannelSinkProvider Members

        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            IClientChannelSink nextSink = null;
            if (nextProvider != null)
                nextSink = nextProvider.CreateSink(channel, url, remoteChannelData);
            return new ClientEncryptionSink(nextSink, encrHelper);
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

    public class ClientEncryptionSink : BaseChannelSinkWithProperties, IClientChannelSink, ISecurableChannel
    {
        private IClientChannelSink nextSink;
        private EncryptionHelper encrHelper;

        internal ClientEncryptionSink(IClientChannelSink next, EncryptionHelper encrHelper)
        {
            nextSink = next;
            this.encrHelper = encrHelper;
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
            if (!encrHelper.HasKey)
            {
                ITransportHeaders encrHeaders = new TransportHeaders();
                System.IO.MemoryStream encrStream = new System.IO.MemoryStream();
                encrHelper.Initiate(encrHeaders);

                ITransportHeaders encredHeaders;
                System.IO.Stream encredStream;
                try
                {
                    nextSink.ProcessMessage(msg, encrHeaders, encrStream, out encredHeaders, out encredStream);
                    encrHelper.Finalize(encredHeaders);
                }
                catch (Exception ex)
                {
                    encrHelper = new EncryptionHelper();
                }
            }
            
            /*System.IO.MemoryStream memStream = EncryptionHelper.ToMemoryStream(requestStream);
            string text = new string(System.Text.UTF8Encoding.UTF8.GetChars(memStream.ToArray()));
            requestStream = memStream;*/

            encrHelper.Encrypt(requestHeaders, ref requestStream);
            nextSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
            if (EncryptionHelper.IsEncrypted(responseHeaders))
            {
                encrHelper.Decrypt(responseHeaders, ref responseStream);
            }
            else
            {
                encrHelper = new EncryptionHelper();
            }
            /*
            System.IO.MemoryStream respStream = EncryptionHelper.ToMemoryStream(responseStream);
            text = new string(System.Text.UTF8Encoding.UTF8.GetChars(respStream.ToArray()));
            responseStream = respStream;*/
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
