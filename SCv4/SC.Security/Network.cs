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

namespace SC.Security
{
    [Serializable]
    public sealed class Network : MarshalByRefObject, SC.Interfaces.INetwork
    {
        private static readonly byte[] validmaskbytes = new byte[] { 0x00, 0x80, 0xC0, 0xE0, 0xF0, 0xF8, 0xFC, 0xFE, 0xFF };
        private System.Net.IPAddress address;
        private System.Net.IPAddress netmask;

        public Network(NetworkSettings sett)
        {
            if (sett.address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new ArgumentException("Address must be a IPv4 address");
            if (sett.netmask.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new ArgumentException("Netmask must be a IPv4 netmask");
            if (!IsValidNetmask(sett.netmask))
                throw new ArgumentException("Netmask is not a valid netmask");

            this.netmask = sett.netmask;
            this.address = ApplyNetmask(sett.address, sett.netmask);

            if (!IsValidIPv4Address(this.address))
                throw new ArgumentException("Address is not a valid IPv4 address");
        }
        public Network()
        {
            netmask = address = new System.Net.IPAddress(0);
        }
        public Network(System.Net.IPAddress address, int prefixlen)
        {
            if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new ArgumentException("Address must be a IPv4 address");
            if (prefixlen < 0 || prefixlen > 32)
                throw new ArgumentOutOfRangeException("Prefixlength must be an integer between 0 and 32 inclusive");

            byte[] bytes = new byte[4];
            for (int i = 0; i < prefixlen / 8; ++i)
                bytes[i] = 0xFF;
            for (int i = 0; i < prefixlen % 8; ++i)
                bytes[prefixlen / 8] |= Convert.ToByte(1 << (7 - i));

            netmask = new System.Net.IPAddress(bytes);

            this.address = ApplyNetmask(address, netmask);

            if (!IsValidIPv4Address(this.address))
                throw new ArgumentException("Address is not a valid IPv4 address");
        }
        public Network(System.Net.IPAddress address, System.Net.IPAddress netmask)
        {
            if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new ArgumentException("Address must be a IPv4 address");
            if (netmask.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new ArgumentException("Netmask must be a IPv4 netmask");
            if (!IsValidNetmask(netmask))
                throw new ArgumentException("Netmask is not a valid netmask");

            this.netmask = netmask;
            this.address = ApplyNetmask(address, netmask);

            if (!IsValidIPv4Address(this.address))
                throw new ArgumentException("Address is not a valid IPv4 address");
        }
        public override bool Equals(object obj)
        {
            SC.Interfaces.INetwork network = obj as SC.Interfaces.INetwork;

            return network != null && network.Address.Equals(Address) && network.Netmask.Equals(Netmask);
        }
        public bool IsHostInNet(System.Net.IPAddress ip)
        {
            return ApplyNetmask(ip, netmask).Equals(address);
        }
        public System.Net.IPAddress Address { get { return address; } }
        public System.Net.IPAddress Netmask { get { return netmask; } }
        
        public static bool IsValidNetmask(System.Net.IPAddress netmask)
        {
            bool bits = true;
            byte[] bytes = netmask.GetAddressBytes();
            for (int i = 0; i < bytes.Length; ++i)
            {
                if (bits && bytes[i] == 0xFF)
                    continue;
                else if (!bits && bytes[i] == 0x00)
                    continue;
                else if (bits && Array.IndexOf<byte>(validmaskbytes, bytes[i]) != -1)
                    bits = false;
                else
                    return false;
            }
            return true;
        }
        public static System.Net.IPAddress ApplyNetmask(System.Net.IPAddress address, System.Net.IPAddress netmask)
        {
            byte[] addr = address.GetAddressBytes();
            byte[] mask = netmask.GetAddressBytes();

            if (addr.Length != mask.Length)
                throw new ArgumentOutOfRangeException("Address and netmask do not have the same address length");

            for (int i = 0; i < addr.Length; ++i)
            {
                addr[i] &= mask[i];
            }
            return new System.Net.IPAddress(addr);
        }

        public override string ToString()
        {
            return address.ToString() + '/' + netmask.ToString();
        }
        public NetworkSettings Settings
        {
            get
            {
                return new NetworkSettings(Address, Netmask);
            }
        }
        private static bool IsValidIPv4Address(System.Net.IPAddress address)
        {
            return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                address.GetAddressBytes()[0] < (byte)224 &&
                address.GetAddressBytes()[0] > (byte)0 &&
                Array.IndexOf<byte>(address.GetAddressBytes(), 0xFF) == -1;
        }
    }

    [Serializable]
    public class NetworkSettings
    {
        [System.Xml.Serialization.XmlIgnore]
        public System.Net.IPAddress address = System.Net.IPAddress.None;
        [System.Xml.Serialization.XmlIgnore]
        public System.Net.IPAddress netmask = System.Net.IPAddress.None;

        public string Address
        {
            get
            {
                return address.ToString();
            }
            set
            {
                if (!System.Net.IPAddress.TryParse(value, out address))
                    address = System.Net.IPAddress.None;
            }
        }
        public string Netmask
        {
            get
            {
                return netmask.ToString();
            }
            set
            {
                if (!System.Net.IPAddress.TryParse(value, out netmask))
                    netmask = System.Net.IPAddress.None;
            }
        }

        public NetworkSettings() { }
        public NetworkSettings(System.Net.IPAddress address, System.Net.IPAddress netmask)
        {
            this.address = address;
            this.netmask = netmask;
        }
    }
}
