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

namespace SC.Client
{
    public class SCClientProvider
    {
        private static int priority = 1;

        private SCClientProvider()
        {
        }
        public static SC.Interfaces.IRoot GetRoot(string Host, string Username, string Password)
        {
            string channelname = Host + ":" + Username;
            if (System.Runtime.Remoting.Channels.ChannelServices.GetChannel(channelname) == null)
            {
                System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider soapFormatter = new System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider();
                SC.Remoting.ClientEncryptionSinkProvider encrProvider = new SC.Remoting.ClientEncryptionSinkProvider();
                SC.Remoting.ClientAuthenticationSinkProvider authProvider = new SC.Remoting.ClientAuthenticationSinkProvider(Username, Password);

                soapFormatter.Next = authProvider;
                                     authProvider.Next = encrProvider;

                Dictionary<string, string> props = new Dictionary<string, string>();
                props["name"] = channelname;
                props["priority"] = System.Threading.Interlocked.Increment(ref priority).ToString();

                System.Runtime.Remoting.Channels.Http.HttpClientChannel channel = new System.Runtime.Remoting.Channels.Http.HttpClientChannel(props, soapFormatter);
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, false);
            }
            SC.Interfaces.IRoot root = System.Runtime.Remoting.RemotingServices.Connect(typeof(SC.Interfaces.IRoot), "http://" + Username + "@" + Host + "/ServerChecker4Root") as SC.Interfaces.IRoot;
            
            //SC.Interfaces.IRoot root = Activator.GetObject(typeof(SC.Interfaces.IRoot), "http://" + Host + "/ServerChecker4Root") as SC.Interfaces.IRoot;
            return root;
        }
        public static void InvalidateRoot(string Host, string Username)
        {
            System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(
                System.Runtime.Remoting.Channels.ChannelServices.GetChannel(Host + ":" + Username));
        }
    }
}
