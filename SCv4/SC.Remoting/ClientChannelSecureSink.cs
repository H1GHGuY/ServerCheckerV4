using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace SC.Remoting
{
    public sealed class ClientChannelSecureSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider nextProvider;

        #region IClientChannelSinkProvider Members

        IClientChannelSink IClientChannelSinkProvider.CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            IClientChannelSink nextSink = null;
            if (nextProvider != null)
                nextSink = nextProvider.CreateSink(channel, url, remoteChannelData);
            return new ClientChannelSecureSink(nextSink);
        }

        IClientChannelSinkProvider IClientChannelSinkProvider.Next
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

    public class ClientChannelSecureSink : BaseChannelSinkWithProperties, IClientChannelSink, ISecurableChannel
    {
        private IClientChannelSink nextSink;
        private CryptoHelper crypto = new CryptoHelper();

        public ClientChannelSecureSink(IClientChannelSink next)
        {
            nextSink = next;
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
            if (!crypto.IsCryptoReady)
                Authenticate(msg);

            requestHeaders[CommonHeaders.EncryptionType] = EncryptionType.Encrypted;

            nextSink.ProcessMessage(msg, requestHeaders, crypto.EncryptEx(requestStream), out responseHeaders, out responseStream);

            if (responseHeaders[CommonHeaders.EncryptionType] != null && responseHeaders[CommonHeaders.EncryptionType].ToString() == EncryptionType.Encrypted.ToString())
            {
                responseStream = crypto.DecryptEx(responseStream);
            }
        }

        

        private void Authenticate(IMessage msg)
        {
            System.IO.Stream requestStream = new System.IO.MemoryStream();
            TransportHeaders requestHeaders = new System.Runtime.Remoting.Channels.TransportHeaders();
            requestHeaders[CommonHeaders.EncryptionType] = EncryptionType.HandshakeRequest;
            requestHeaders[CommonHeaders.AuthenticationType] = AuthenticationType.ChallengeRequest;
            requestHeaders[CommonHeaders.PublicKey] = crypto.PublicKey;
            requestHeaders["Content-Type"] = "text/xml; charset=utf-8";
            //requestHeaders["Content-Type"] = "application/octet-stream"; 

            System.IO.Stream responseStream;
            ITransportHeaders responseHeaders;

            nextSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);

            string xmlKey = responseHeaders[CommonHeaders.PublicKey].ToString();
            crypto.DeriveKey(xmlKey);
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
