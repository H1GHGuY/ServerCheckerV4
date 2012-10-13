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
    public class Program
    {
        static byte[][] pings = new byte[][] { 
                new byte[] { 0xFF,0xFF,0xFF,0xFF,0x54,0x53,0x6F,0x75,0x72,0x63,0x65,0x20,0x45,0x6E,0x67,0x69,0x6E,0x65,0x20,0x51,0x75,0x65,0x72,0x79,0x00}, // HL/HL2
				new byte[] { 0x5C,0x62,0x61,0x73,0x69,0x63,0x5C}, // UT/GS
				new byte[] { 0xFE,0xFD,0x00,0x43,0x4F,0x52,0x59,0xFF,0xFF,0x00},	// GS2
				new byte[] { 0xFE,0xFD,0x00,0x0C,0xAE,0x3D,0x00,0xFF,0xFF,0xFF,0x01}, // GS3 //",0xFE,0xFD,0x00,0x04,0xF7,0x11,0x00,0x09,0x04,0x05,0x06,0x07,0x0B,0x01,0x08,0x0a,0x13,0x00,0x00",
				new byte[] { 0xFF,0xFF,0xFF,0xFF,0x67,0x65,0x74,0x73,0x74,0x61,0x74,0x75,0x73}, // Q3
				new byte[] { 0xFF,0xFF,0xFF,0xFF,0x73,0x74,0x61,0x74,0x75,0x73}, // Q2
				new byte[] { 0xFF,0xFF,0x67,0x65,0x74,0x49,0x6E,0x66,0x6F}, //Doom3
				new byte[] { 0x73}, // All Seeing Eye
				new byte[] { 0x52,0x45,0x50,0x4F,0x52,0x54}, // Raven Shield
                new byte[] { 0xFE,0xFD,0x09,0x01,0x02,0x03,0x04}
        };
        private static log4net.ILog Logger = log4net.LogManager.GetLogger("Service");

        private System.Threading.Semaphore semaphore = null;
        private System.Threading.Thread thread = null;

        public void Start()
        {
            if (thread != null)
                throw new Exception("Cannot start service twice.");
            semaphore = new System.Threading.Semaphore(0, 1);
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(InternalMain));
            thread.Start();
        }

        public void Stop()
        {
            semaphore.Release();
            if (thread.Join(5000))
                thread = null;
            else
                throw new Exception("Service did not stop within timeframe.");
        }

        [MTAThread()]
        static void Main()
        {
            //new SC.Security.LicenseManager().CheckLicense();

            bool created;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "ServerChecker", out created);
            if (!created)
            {
                System.Console.WriteLine("Program is already running.");
                System.Console.ReadKey();
                return;
            }

            new Program().InternalMain();
        }

        private void InternalMain()
        {
            System.Threading.Thread.CurrentThread.Name = "Root";

            System.AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.UnauthenticatedPrincipal);
            System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            {
                List<string> addrs = new List<string>();
                foreach (System.Net.NetworkInformation.NetworkInterface itf in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (System.Net.NetworkInformation.UnicastIPAddressInformation info in itf.GetIPProperties().UnicastAddresses)
                    {
                        if ( !System.Net.IPAddress.IsLoopback(info.Address)
                            && (     info.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                                 || (info.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && !info.Address.IsIPv6LinkLocal)
                               )
                            )
                            addrs.Add(info.Address.ToString());
                    }
                }
                Logger.Info("Local addresses: " + string.Join(",", addrs.ToArray()));
            }

            int port = 8081;

            Root root = new Root();
            root.Initialize();

            SC.Remoting.ServerEncryptionSinkProvider encrProvider = new SC.Remoting.ServerEncryptionSinkProvider();
            SC.Remoting.ServerAuthenticationSinkProvider authProvider = new SC.Remoting.ServerAuthenticationSinkProvider(SC.Security.SecurityManager.Instance);
            System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider soapFormatter = new System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider();
            soapFormatter.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            encrProvider.Next = authProvider;
            authProvider.Next = soapFormatter;

            System.Runtime.Remoting.Channels.Http.HttpServerChannel channel = null;
            while (channel == null)
            {
                try
                {
                    channel = new System.Runtime.Remoting.Channels.Http.HttpServerChannel("Access", port, encrProvider);
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    Logger.Error("Waiting 5 seconds for socket on port " + port, se);
                    System.Threading.Thread.Sleep(5000);
                }
            }
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, false);

            System.Runtime.Remoting.ObjRef Ref = System.Runtime.Remoting.RemotingServices.Marshal(root, "ServerChecker4Root");

            //root.AddServer("Blah");
            /*Server s = root.GetServer("Blah");
            s.Arguments = "file.txt";
            s.EndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("81.19.219.88"), 27015);
            s.Executable = "notepad.exe";
            s.Protocol = System.Net.TransportType.Udp;
            s.StartupTimeout = 1000;
            //s.Username = "Admin";
            s.WorkingDirectory = "c:\\";
            s.StopOnExit = true;
            s.AcquireOnStart = true;
            System.Console.WriteLine(s.ToString());

            SC.DefaultPlugins.ServerCheckPlugin plugin = s.GetPlugin("ServerCheckPlugin") as SC.DefaultPlugins.ServerCheckPlugin;
            plugin.GameType = SC.DefaultPlugins.eGameType.HalfLife;
            plugin.Timeout = 1000;*/

            if (semaphore != null)
                semaphore.WaitOne();
            else
                System.Console.ReadKey();

            System.Runtime.Remoting.RemotingServices.Disconnect(root);
            System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(channel);

            root.Cleanup();


        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
                Logger.Fatal("An unhandled exception occurred.", e.ExceptionObject as Exception);
            else if (e.ExceptionObject != null)
                Logger.Fatal("An unhandled exception occurred: " + e.ExceptionObject.ToString());
            else
                Logger.Fatal("An unhandled exception occurred.");

            if (e.IsTerminating)
                Logger.Fatal("The application will terminate.");
            else
                Logger.Fatal("The application will try to resume.");
        }
    }
}
