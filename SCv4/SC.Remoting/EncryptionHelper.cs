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
using System.Security.Cryptography;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace SC.Remoting
{
    class EncryptionHelper : BaseHelper
    {
        protected const int BUFFER_SIZE = 4096;

        protected const string ENCRYPTION_LEVEL = "EncrLevel";
        protected const string INITIATE = "Initiate";
        protected const string RESPOND = "Respond";
        protected const string ENCRYPTED = "Encrypted";
        protected const string TICKET = "EncryptionTicket";
        protected const string NOTICKET = "No Ticket";

        protected const string PUBLICKEY = "PublicKey";


        private ECDiffieHellmanCng dh = new ECDiffieHellmanCng(521);
        private AesManaged aes = null;
        private enum ApplicationType
        {
            Client,
            Server,
            Unknown
        }
        private ApplicationType AppType = ApplicationType.Unknown;
        private string state = null;
        private Guid ticket = Guid.Empty;

        public static bool IsInitiation(ITransportHeaders headers)
        {
            return headers[ENCRYPTION_LEVEL] != null
                && headers[ENCRYPTION_LEVEL].ToString() == INITIATE
                && headers[PUBLICKEY] != null
                && HasNoTicket(headers);
        }
        public static bool IsResponse(ITransportHeaders headers)
        {
            return headers[ENCRYPTION_LEVEL] != null
                && headers[ENCRYPTION_LEVEL].ToString() == RESPOND
                && headers[PUBLICKEY] != null
                && HasValidTicket(headers);
        }
        public static bool IsEncrypted(ITransportHeaders headers)
        {
            return headers[ENCRYPTION_LEVEL] != null
                && headers[ENCRYPTION_LEVEL].ToString() == ENCRYPTED
                && HasValidTicket(headers);
        }
        public static bool HasValidTicket(ITransportHeaders headers)
        {
            try
            {
                if (headers[TICKET] != null)
                {
                    new Guid(headers[TICKET].ToString());
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
                throw new System.Security.SecurityException("No ticket found.");
            return new Guid(headers[TICKET].ToString());
        }
        
        public EncryptionHelper()
        {
        }
        protected string GetPublicKey()
        {
            return Convert.ToBase64String(dh.PublicKey.ToByteArray());
        }
        protected void DeriveSecretKey(string otherPartyPublicKey)
        {
            byte[] bytes = dh.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromByteArray(Convert.FromBase64String(otherPartyPublicKey), CngKeyBlobFormat.EccPublicBlob));
            
            aes = new AesManaged();
            
            aes.KeySize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            byte[] IV = new byte[16];
            byte[] Key = new byte[16];
            
            Array.Copy(bytes, IV, 16);
            Array.Copy(bytes, 16, Key, 0, 16);

            aes.IV = IV;
            aes.Key = Key;
        }
        public bool HasKey
        {
            get
            {
                return aes != null;
            }
        }
        public Guid Ticket
        {
            get
            {
                return ticket;
            }
        }
        /// <summary>
        /// Client side - Initiate encryption (1/2)
        /// </summary>
        /// <param name="headers"></param>
        public void Initiate(ITransportHeaders headers)
        {
            if (AppType != ApplicationType.Unknown && state != INITIATE)
                throw new InvalidOperationException("This object cannot be reused.");

            headers[ENCRYPTION_LEVEL] = INITIATE;
            headers[PUBLICKEY] = GetPublicKey();
            headers[TICKET] = NOTICKET;

            AppType = ApplicationType.Client;
            state = INITIATE;
        }
        /// <summary>
        /// Server side - Respond to an initiation (1/1)
        /// </summary>
        /// <param name="initiateHeaders"></param>
        /// <param name="respondHeaders"></param>
        public void Respond(ITransportHeaders initiateHeaders, ITransportHeaders respondHeaders)
        {
            if (AppType != ApplicationType.Unknown)
                throw new InvalidOperationException("Cannot reuse this object.");

            if (initiateHeaders[ENCRYPTION_LEVEL] == null || initiateHeaders[ENCRYPTION_LEVEL].ToString() != INITIATE || initiateHeaders[PUBLICKEY] == null)
                throw new System.Security.Cryptography.CryptographicException("The message has no initiator request");

            if (!HasNoTicket(initiateHeaders))
                throw new System.Security.SecurityException("Invalid Ticket");

            DeriveSecretKey(initiateHeaders[PUBLICKEY].ToString());
            respondHeaders[ENCRYPTION_LEVEL] = RESPOND;
            respondHeaders[PUBLICKEY] = GetPublicKey();

            ticket = Guid.NewGuid();
            respondHeaders[TICKET] = ticket.ToString("N");

            AppType = ApplicationType.Server;
            state = ENCRYPTED;
        }
        /// <summary>
        /// Client side - Finalize encryption (2/2)
        /// </summary>
        /// <param name="headers"></param>
        public void Finalize(ITransportHeaders headers)
        {
            if (AppType != ApplicationType.Client || state != INITIATE)
                throw new InvalidOperationException("Cannot finalize in the current state.");
            if (headers[ENCRYPTION_LEVEL] == null || headers[ENCRYPTION_LEVEL].ToString() != RESPOND || headers[PUBLICKEY] == null)
                throw new System.Security.Cryptography.CryptographicException("The message is not an encryption response");

            DeriveSecretKey(headers[PUBLICKEY].ToString());
            ticket = new Guid(headers[TICKET].ToString());

            state = ENCRYPTED;
        }
        public void Encrypt(ITransportHeaders headers, ref System.IO.Stream stream)
        {
            if (state != ENCRYPTED)
                throw new InvalidOperationException("Cannot encrypt message. Object is in invalid state.");

            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(outStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] buffer = new byte[BUFFER_SIZE];
            int n = stream.Read(buffer, 0, BUFFER_SIZE);
            while (n > 0)
            {
                cryptoStream.Write(buffer, 0, n);
                n = stream.Read(buffer, 0, BUFFER_SIZE);
            }
            cryptoStream.FlushFinalBlock();
            outStream.Seek(0, System.IO.SeekOrigin.Begin);

            headers[ENCRYPTION_LEVEL] = ENCRYPTED;
            headers[TICKET] = ticket.ToString("N");
#if DEBUG
            headers["AESKEY"] = Convert.ToBase64String(aes.Key);
            headers["AESIV"] = Convert.ToBase64String(aes.IV);
#endif
            stream = outStream;
        }
        public void Decrypt(ITransportHeaders headers, ref System.IO.Stream stream)
        {
            if (state != ENCRYPTED)
                throw new InvalidOperationException("Cannot decrypt message. Object is in invalid state.");

            if (headers[ENCRYPTION_LEVEL] == null || headers[ENCRYPTION_LEVEL].ToString() != ENCRYPTED)
                throw new System.Security.Cryptography.CryptographicException("The message was not encrypted at the sender side");

            if (!HasValidTicket(headers) || !(new Guid(headers[TICKET].ToString()).Equals(ticket)))
                throw new System.Security.SecurityException("Invalid ticket.");

#if DEBUG
            byte[] aeskey = Convert.FromBase64String(headers["AESKEY"].ToString());
            byte[] aesiv = Convert.FromBase64String(headers["AESIV"].ToString());

            System.Diagnostics.Trace.Assert(aeskey[0] == aes.Key[0]);
            System.Diagnostics.Trace.Assert(aesiv[0] == aes.IV[0]);
#endif

            System.IO.MemoryStream inStream = new System.IO.MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(inStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] buffer = new byte[BUFFER_SIZE];
            int n = stream.Read(buffer, 0, BUFFER_SIZE);
            while (n > 0)
            {
                cryptoStream.Write(buffer, 0, n);
                n = stream.Read(buffer, 0, BUFFER_SIZE);
            }
            cryptoStream.FlushFinalBlock();
            inStream.Seek(0, System.IO.SeekOrigin.Begin);

            stream = inStream;
        }
    }
}
