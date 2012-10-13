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
    class AuthenticationHelper : BaseHelper
    {
        public const string AUTHENTICATION_LEVEL = "AuthLevel";
        public const string CHALLENGE_REQUEST = "ChallengeRequest";
        public const string CHALLENGE_RESPONSE = "ChallengeResponse";
        public const string AUTHENTICATED = "Authenticated";

        protected const string USERNAME = "Username";
        protected const string HMAC = "HMAC";
        protected const string CHALLENGE = "Challenge";
        protected const string TICKET = "AuthTicket";
        protected const string NOTICKET = "No Ticket";

        protected static byte[] GenerateKey(string password, byte[] nonce)
        {
            return new System.Security.Cryptography.Rfc2898DeriveBytes(password, nonce, 997).GetBytes(64);
        }
        protected static string GetBase64HMAC(byte[] key, System.IO.MemoryStream stream)
        {
            System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512(key);
            string ret = Convert.ToBase64String(hmac.ComputeHash(stream.ToArray()));
            
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            
            return ret;
        }
        public static bool HasValidTicket(ITransportHeaders headers)
        {
            try
            {
                if (headers[TICKET] != null)
                {
                    Guid g = new Guid(headers[TICKET].ToString());
                    return true;
                }   
            }
            catch (ArgumentException)
            {
            }
            return false;
        }
        public static bool HasNoTicket(ITransportHeaders headers)
        {
            return headers[TICKET] != null && headers[TICKET].ToString() == NOTICKET;
        }
        public static Guid GetTicket(ITransportHeaders headers)
        {
            if (!HasValidTicket(headers))
                throw new System.Security.SecurityException("No valid ticket");

            return new Guid(headers[TICKET].ToString());
        }
    }

    internal class ClientAuthenticationHelper : AuthenticationHelper
    {
        private string username;
        private string password;
        private byte[] key = null;
        private Guid ticket = Guid.Empty;
        
        public ClientAuthenticationHelper(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public bool HaveNonce
        {
            get
            {
                return key != null;
            }
        }
        public void SetRequest(ITransportHeaders headers)
        {
            headers[AUTHENTICATION_LEVEL] = CHALLENGE_REQUEST;
            headers[TICKET] = NOTICKET;
        }
        public void SetNonce(ITransportHeaders headers)
        {
            if (headers[AUTHENTICATION_LEVEL] == null || headers[AUTHENTICATION_LEVEL].ToString() != CHALLENGE_RESPONSE || headers[CHALLENGE] == null)
                throw new System.Security.Authentication.AuthenticationException("Reply does not contain a challenge");

            key = GenerateKey(password, Convert.FromBase64String( headers[CHALLENGE].ToString() ));
            ticket = GetTicket(headers);
        }
        public void Authenticate(ITransportHeaders headers, ref System.IO.Stream stream)
        {
            System.IO.MemoryStream memStream = ToMemoryStream(stream);

            headers[USERNAME] = username;
            headers[HMAC] = GetBase64HMAC(key, memStream);
            headers[AUTHENTICATION_LEVEL] = AUTHENTICATED;
            headers[TICKET] = ticket.ToString("N");

            stream = memStream;
        }
    }
    class ServerAuthenticationHelper : AuthenticationHelper
    {
        private byte[] nonce;
        private Guid ticket = Guid.Empty;
        public ServerAuthenticationHelper()
        {
            nonce = new byte[64];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(nonce);
        }
        public static bool IsChallengeRequest(ITransportHeaders headers)
        {
            return headers[AUTHENTICATION_LEVEL] != null && headers[AUTHENTICATION_LEVEL].ToString() == CHALLENGE_REQUEST && HasNoTicket(headers);
        }
        public void Challenge(ITransportHeaders headers)
        {
            headers[AUTHENTICATION_LEVEL] = CHALLENGE_RESPONSE;
            headers[CHALLENGE] = Convert.ToBase64String(nonce);
            ticket = Guid.NewGuid();
            headers[TICKET] = ticket.ToString("N");
        }
        public void Authenticate(ITransportHeaders headers, ref System.IO.Stream stream)
        {
            if (headers[AUTHENTICATION_LEVEL].ToString() != AUTHENTICATED)
                throw new System.Security.Authentication.AuthenticationException("Trying to authenticate an unauthenticated message");

            if (!HasValidTicket(headers) || !GetTicket(headers).Equals(ticket))
                throw new System.Security.SecurityException("No valid ticket");

            System.IO.MemoryStream memStream = ToMemoryStream(stream);
            stream = memStream;
            SC.Security.SecurityManager.Instance.Authenticate(headers[USERNAME].ToString(), headers[HMAC].ToString(), nonce, memStream);
        }
        public static void UnAuthenticate()
        {
            SC.Security.SecurityManager.Instance.UnAuthenticate();
        }
        public Guid Ticket
        {
            get
            {
                return ticket;
            }
        }
    }
}
