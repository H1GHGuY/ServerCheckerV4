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
using System.Security.Cryptography;
using System.Text;

namespace SC.Remoting
{
    internal sealed class CryptoHelper
    {
        private ECDiffieHellmanCng dh = new ECDiffieHellmanCng(521);
        private RijndaelManaged aes = null;

        internal CryptoHelper()
        {
            dh.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hmac;

            byte[] bytes = new byte[521];
            RandomNumberGenerator.Create().GetBytes(bytes);

            dh.HmacKey = bytes;
        }
        internal string PublicKey
        {
            get
            {
                return dh.PublicKey.ToXmlString();
            }
        }
        internal void DeriveKey(string otherPartyPublicKey)
        {
            aes = new RijndaelManaged();
            aes.Padding = PaddingMode.PKCS7;

            try
            {
                byte[] aesBytes = dh.DeriveKeyMaterial(ECDiffieHellmanCngPublicKey.FromXmlString(otherPartyPublicKey));

                for (int i = 0; i < aes.Key.Length; ++i)
                    aes.Key[i] = aesBytes[i];
                for (int i = 0; i < aes.IV.Length; ++i)
                    aes.IV[i] = aesBytes[i];
            }
            catch (Exception)
            {
                aes = null;
                throw;
            }
        }
        internal bool IsCryptoReady
        {
            get
            {
                return aes != null;
            }
        }
        internal System.IO.Stream Encrypt(System.IO.Stream inStream)
        {
            if (!IsCryptoReady)
                throw new InvalidOperationException("Diffie Hellman Key Exchange not finished.");

            System.Security.Cryptography.ICryptoTransform xfrm = aes.CreateEncryptor();

            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream stream = new CryptoStream(outStream, xfrm, CryptoStreamMode.Write);

            byte[] bytes = new byte[1024];
            int len = 0;
            while (len < inStream.Length)
            {
                int n = inStream.Read(bytes, 0, 1024);
                stream.Write(bytes, 0, 1024);
                len += n;
            }
            stream.FlushFinalBlock();
            return outStream;
        }
        internal System.IO.Stream DecryptEx(System.IO.Stream inStream)
        {
            if (!IsCryptoReady)
                throw new InvalidOperationException("Diffie Hellman Key Exchange not finished.");

            System.Security.Cryptography.ICryptoTransform xfrm = aes.CreateDecryptor();

            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cryptoStream = new CryptoStream(inStream, xfrm, CryptoStreamMode.Read);

            byte[] inBytes = new byte[aes.BlockSize];
            int bytesread = cryptoStream.Read(inBytes, 0, aes.BlockSize);
            while (bytesread > 0)
            {
                memStream.Write(inBytes, 0, bytesread);
                bytesread = inStream.Read(inBytes, 0, aes.BlockSize);
            }
            memStream.Position = 0;
            return memStream;
        }
        internal System.IO.Stream EncryptEx(System.IO.Stream inStream)
        {
            if (!IsCryptoReady)
                throw new InvalidOperationException("Diffie Hellman Key Exchange not finished.");

            System.Security.Cryptography.ICryptoTransform xfrm = aes.CreateEncryptor();

            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cryptoStream = new CryptoStream(memStream, xfrm, CryptoStreamMode.Write);

            byte[] inBytes = new byte[aes.BlockSize];
            int bytesread = inStream.Read(inBytes, 0, aes.BlockSize);
            while (bytesread > 0)
            {
                cryptoStream.Write(inBytes, 0, bytesread);
                bytesread = inStream.Read(inBytes, 0, aes.BlockSize);
            }
            cryptoStream.FlushFinalBlock();
            memStream.Position = 0;
            return memStream;
        }
        internal System.IO.Stream Decrypt(System.IO.Stream inStream)
        {
            if (!IsCryptoReady)
                throw new InvalidOperationException("Diffie Hellman Key Exchange not finished.");

            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            byte[] bytes = new byte[1024];
            int bytesRead = 0;
            do
            {
                bytesRead = inStream.Read(bytes, 0, 1024);
                memStream.Write(bytes, 0, bytesRead);
            } while (bytesRead > 0);

            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            System.Security.Cryptography.ICryptoTransform xfrm = aes.CreateDecryptor();
            System.Security.Cryptography.CryptoStream stream = new CryptoStream(outStream, xfrm, CryptoStreamMode.Write);

            int len = 0;
            while (len < memStream.Length)
            {
                int n = memStream.Read(bytes, 0, 1024);
                stream.Write(bytes, 0, n);
                len += n;
            }
            stream.Flush();
            outStream.Seek(0, System.IO.SeekOrigin.Begin);
            
            return outStream;
        }
        private System.IO.Stream Crypt(System.IO.Stream inStream, ICryptoTransform transform)
        {
            inStream.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream stream = new CryptoStream(outStream, transform, CryptoStreamMode.Write);

            byte[] bytes = new byte[1024];
            int len = 0;
            while (len < inStream.Length)
            {
                int n = inStream.Read(bytes, 0, 1024);
                stream.Write(bytes, 0, n);
                len += n;
            }
            stream.Flush();
            outStream.Seek(0, System.IO.SeekOrigin.Begin);

            return outStream;
            /*
            byte[] inBytes = new byte[inStream.Length];
            int n = inStream.Read(inBytes, 0, (int)inStream.Length);
            System.Diagnostics.Debug.Assert(n == inStream.Length);

            byte[] outBytes = new byte[n];
            transform.TransformBlock(inBytes, 0, n, outBytes, 0);

            return new System.IO.MemoryStream(outBytes);
            */
        }
    }
}
