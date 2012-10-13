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

namespace SC.Core
{
    class ProcessHelper
    {
        public static ushort NetworkToHostOrder(ushort s)
        {
            return (ushort)((s >> 8) | (s << 8));
        }
        public static uint NetworkToHostOrder(uint i)
        {
            return ((i & 0xFF) << 24) | ((i & 0xFF00) << 8) | ((i & 0xFF0000) >> 8) | ((i & 0xFF000000) >> 24);
        }

        public enum UDP_TABLE_CLASS
        {
            UDP_TABLE_BASIC,
            UDP_TABLE_OWNER_PID,
            UDP_TABLE_OWNER_MODULE
        }

        public enum Address_Family
        {
            AF_INET = 2,
            AF_INET6 = 23
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public class MIB_UDPROW_OWNER_PID
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public System.UInt32 _Address;
            [System.Runtime.InteropServices.FieldOffset(4)]
            public System.UInt16 _Port;
            [System.Runtime.InteropServices.FieldOffset(6)]
            public System.UInt16 _Padding;
            [System.Runtime.InteropServices.FieldOffset(8)]
            public System.UInt32 _PID;

            
            public System.Net.IPAddress IPAddress { get { return new System.Net.IPAddress(_Address); } }
            public System.UInt16 Port { get { return ProcessHelper.NetworkToHostOrder(_Port); } }
            public System.Int32 PID { get { return Convert.ToInt32(_PID); } }
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public class MIB_UDP6ROW_OWNER_PID
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst=16, ArraySubType=System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] _LocalAddress = new byte[16];
            [System.Runtime.InteropServices.FieldOffset(16)]
            public System.UInt32 _ScopeID;
            [System.Runtime.InteropServices.FieldOffset(20)]
            public System.UInt16 _Port;
            [System.Runtime.InteropServices.FieldOffset(22)]
            public System.UInt16 _Padding;
            [System.Runtime.InteropServices.FieldOffset(24)]
            public System.UInt32 _PID;

            public System.Net.IPAddress IPAddress { get { return new System.Net.IPAddress(_LocalAddress, _ScopeID); } }
            public System.UInt16 Port { get { return ProcessHelper.NetworkToHostOrder(_Port); } }
            public System.Int32 PID { get { return Convert.ToInt32(_PID); } }
        }

        /*DWORD GetExtendedUdpTable(
              __out    PVOID pUdpTable,
              __inout  PDWORD pdwSize,
              __in     BOOL bOrder,
              __in     ULONG ulAf,
              __in     UDP_TABLE_CLASS TableClass,
              __in     ULONG Reserved
           );*/


        [System.Runtime.InteropServices.DllImport("Iphlpapi.dll", SetLastError=true)]
        public static extern System.UInt32 GetExtendedUdpTable(
            System.IntPtr Table,
            ref System.UInt32 Size, 
            bool Order, 
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
            Address_Family AF,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I4)]
            UDP_TABLE_CLASS TableClass,
            UInt32 Reserved);

        public static SC.Core.Process GetProcess(System.Net.EndPoint ep)
        {
            MIB_UDPROW_OWNER_PID[] Table = GetUdpTable();
            foreach (MIB_UDPROW_OWNER_PID entry in Table)
            {
                if (new System.Net.IPEndPoint(entry.IPAddress, entry.Port) == ep)
                    return SC.Core.Process.GetProcessById(entry.PID);
            }
            MIB_UDP6ROW_OWNER_PID[] Table6 = GetUdp6Table();
            foreach (MIB_UDP6ROW_OWNER_PID entry in Table6)
            {
                if (new System.Net.IPEndPoint(entry.IPAddress, entry.Port) == ep)
                    return SC.Core.Process.GetProcessById(entry.PID);
            }
            return null;
        }

        public static MIB_UDPROW_OWNER_PID[] GetUdpTable()
        {
            MIB_UDPROW_OWNER_PID[] Entries = null;

            System.IntPtr Table = System.Runtime.InteropServices.Marshal.AllocHGlobal(5000);
            System.UInt32 Size = 5000;
            
            System.UInt32 ret = GetExtendedUdpTable(Table, ref Size, false, Address_Family.AF_INET, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);
            if (ret == 0)
            {
                System.UInt32 NumEntries = Convert.ToUInt32(System.Runtime.InteropServices.Marshal.ReadInt32(Table));
                System.IntPtr PtrEntries = AddToPointer(Table, 4);
                
                Entries = new MIB_UDPROW_OWNER_PID[NumEntries];
                for (int i = 0; i < NumEntries; ++i)
                {
                    Entries[i] = new MIB_UDPROW_OWNER_PID();

                    System.IntPtr PtrEntry = AddToPointer(PtrEntries, (i * System.Runtime.InteropServices.Marshal.SizeOf(Entries[i])));
                    System.Runtime.InteropServices.Marshal.PtrToStructure(PtrEntry, Entries[i]);
                }
            }
            System.Runtime.InteropServices.Marshal.FreeHGlobal(Table);
            return Entries;
        }
        public static MIB_UDP6ROW_OWNER_PID[] GetUdp6Table()
        {
            MIB_UDP6ROW_OWNER_PID[] Entries = null;

            System.IntPtr Table = System.Runtime.InteropServices.Marshal.AllocHGlobal(5000);
            System.UInt32 Size = 5000;

            System.UInt32 ret = GetExtendedUdpTable(Table, ref Size, false, Address_Family.AF_INET6, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);
            if (ret == 0)
            {
                System.UInt32 NumEntries = Convert.ToUInt32(System.Runtime.InteropServices.Marshal.ReadInt32(Table));
                System.IntPtr PtrEntries = AddToPointer(Table, 4);

                Entries = new MIB_UDP6ROW_OWNER_PID[NumEntries];
                for (int i = 0; i < NumEntries; ++i)
                {
                    Entries[i] = new MIB_UDP6ROW_OWNER_PID();

                    System.IntPtr PtrEntry = AddToPointer(PtrEntries, (i * System.Runtime.InteropServices.Marshal.SizeOf(Entries[i])));
                    System.Runtime.InteropServices.Marshal.PtrToStructure(PtrEntry, Entries[i]);
                }
            }
            System.Runtime.InteropServices.Marshal.FreeHGlobal(Table);
            return Entries;
        }
        private static unsafe IntPtr AddToPointer(IntPtr ptr, int bytes)
        {
            return new IntPtr((void*)(((char*)(ptr.ToPointer())) + (bytes * 4 /IntPtr.Size)));
        }
    }
}
