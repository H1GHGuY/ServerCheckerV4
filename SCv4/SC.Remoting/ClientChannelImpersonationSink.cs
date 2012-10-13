using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;

namespace SC.Remoting
{
    public class ClientChannelImpersonationSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider nextProvider = null;
        private string username;
        private string password;

        public ClientChannelImpersonationSinkProvider(string Username, string Password)
        {
            username = Username;
            password = Password;
        }

        #region IClientChannelSinkProvider Members

        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            IClientChannelSink nextSink = null;
            if (nextProvider != null)
                nextSink = nextProvider.CreateSink(channel, url, remoteChannelData);
            return new ClientChannelImpersonationSink(nextSink, username, password);
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

    class ClientChannelImpersonationSink : BaseChannelSinkWithProperties, IClientChannelSink
    {
        private IClientChannelSink nextSink = null;
        private string username;
        private string password;
        
        public ClientChannelImpersonationSink(IClientChannelSink nextSink, string Username, string Password)
        {
            this.nextSink = nextSink;
            this.username = Username;
            this.password = Password;
        }

        #region IClientChannelSink Members

        void IClientChannelSink.AsyncProcessRequest(IClientChannelSinkStack sinkStack, System.Runtime.Remoting.Messaging.IMessage msg, ITransportHeaders headers, System.IO.Stream stream)
        {
            nextSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }

        void IClientChannelSink.AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, System.IO.Stream stream)
        {
            nextSink.AsyncProcessResponse(sinkStack, state, headers, stream);
        }

        System.IO.Stream IClientChannelSink.GetRequestStream(System.Runtime.Remoting.Messaging.IMessage msg, ITransportHeaders headers)
        {
            return null;
        }

        public IClientChannelSink NextChannelSink
        {
            get { return nextSink; }
        }

        public void ProcessMessage(System.Runtime.Remoting.Messaging.IMessage msg, ITransportHeaders requestHeaders, System.IO.Stream requestStream, out ITransportHeaders responseHeaders, out System.IO.Stream responseStream)
        {
            requestHeaders[CommonHeaders.Username] = username;
            new System.Security.Cryptography.SHA512CryptoServiceProvider().ComputeHash()
            nextSink.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
        }

        #endregion
    }
}
