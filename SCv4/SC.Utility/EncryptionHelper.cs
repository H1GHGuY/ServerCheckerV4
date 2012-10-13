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

namespace SC.Utility
{
    public sealed class EncryptionHelper
    {
        private System.Security.Cryptography.Aes aes;

        public EncryptionHelper(byte[] key)
        {
            aes = System.Security.Cryptography.Aes.Create();
            aes.Key = key;
            aes.IV = System.Text.UTF8Encoding.UTF8.GetBytes(System.Reflection.Assembly.GetExecutingAssembly().FullName).Take(aes.BlockSize/8).ToArray();
            aes.Mode = System.Security.Cryptography.CipherMode.CBC;
            aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
        }
        public string EncryptToBase64FromString(string inString)
        {
            return Convert.ToBase64String(Encrypt(System.Text.UnicodeEncoding.Unicode.GetBytes(inString)));
        }
        public string DecryptToStringFromBase64(string inString)
        {
            return System.Text.UnicodeEncoding.Unicode.GetString(Decrypt(Convert.FromBase64String(inString)));
        }
        public System.Security.SecureString DecryptToSecureStringFromBase64(string inString)
        {
            byte[] bytes = Convert.FromBase64String(inString);
            bytes = Decrypt(bytes);
            System.Security.SecureString ret = new System.Security.SecureString();
            for (int i = 0; i < bytes.Length; i+=2)
            {
                ret.AppendChar( (char)(bytes[i]*256 + bytes[i+1]));
                bytes[i] = bytes[i+1] = 0;
            }
            ret.MakeReadOnly();
            return ret;
        }
        public string EncryptToBase64FromSecurestring(System.Security.SecureString inString)
        {
            byte[] bytes = new byte[inString.Length * 2];
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(inString);

            for (int i = 0; i < inString.Length; ++i)
            {
                short c = System.Runtime.InteropServices.Marshal.ReadInt16(ptr, 2*i);
                bytes[i*2] = (byte)(c / 256);
                bytes[i*2 + 1] = (byte)(c % 256);
                char x = (char)c;
            }
            System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            bytes = Encrypt(bytes);
            return Convert.ToBase64String(bytes);
        }
        public byte[] Encrypt(byte[] inBytes)
        {
            return Crypt(inBytes, aes.CreateEncryptor());
        }
        public byte[] Decrypt(byte[] inBytes)
        {
            return Crypt(inBytes, aes.CreateDecryptor());
        }
        private byte[] Crypt(byte[] inBytes, System.Security.Cryptography.ICryptoTransform xfrm)
        {
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream crypto = new System.Security.Cryptography.CryptoStream(outStream, xfrm, System.Security.Cryptography.CryptoStreamMode.Write);
            crypto.Write(inBytes, 0, inBytes.Length);
            crypto.FlushFinalBlock();
            for (int i = 0; i < inBytes.Length; ++i)
                inBytes[i] = 0;
            return outStream.ToArray();
        }
        ~EncryptionHelper()
        {
            aes.Clear();
        }
    }

    public sealed class EncryptedString : System.Runtime.Serialization.ISerializable, System.Xml.Serialization.IXmlSerializable
    {
        private System.Security.SecureString str = null;
        private EncryptionHelper encr = null;
        private string base64 = null;

        public EncryptedString(byte[] key)
        {
            str = new System.Security.SecureString();
            str.MakeReadOnly();
            encr = new EncryptionHelper(key);
        }
        public EncryptedString(System.Security.SecureString str, byte[] key)
        {
            if (!str.IsReadOnly())
                throw new ArgumentException("SecureString is not read only");

            this.encr = new EncryptionHelper(key);
            this.str = str;
        }
        public EncryptedString(string base64String, byte[] key)
        {
            this.encr = new EncryptionHelper(key);
            this.str = encr.DecryptToSecureStringFromBase64(base64String);
        }
        private EncryptedString(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (context.State != System.Runtime.Serialization.StreamingContextStates.Persistence && context.State != System.Runtime.Serialization.StreamingContextStates.File)
                throw new System.Runtime.Serialization.SerializationException("Cannot serialize to anything other than file or persistent storage");

            base64 = info.GetString("EncryptedString");
        }
        private EncryptedString()
        {
        }
#if !NDEBUG
        public override string ToString()
        {
            return encr.DecryptToStringFromBase64(encr.EncryptToBase64FromSecurestring(str));
        }
#endif
        public System.Security.SecureString SecureString
        {
            get
            {
                return str;
            }
        }
        public string Base64String
        {
            get
            {
                return encr.EncryptToBase64FromSecurestring(str);
            }
        }
        public int Length
        {
            get
            {
                return str.Length;
            }
        }
        public void SetKey(byte[] key)
        {
            EncryptionHelper encrhelper = new EncryptionHelper(key);
            if (base64 != null)
            {
                str = encrhelper.DecryptToSecureStringFromBase64(base64);
                base64 = null;
            }
            encr = encrhelper;
        }

        #region ISerializable Members

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (context.State != System.Runtime.Serialization.StreamingContextStates.Persistence && context.State != System.Runtime.Serialization.StreamingContextStates.File)
                throw new System.Runtime.Serialization.SerializationException("Cannot serialize to anything other than file or persistent storage");

            info.AddValue("EncryptedString", Base64String);
        }

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            base64 = reader.ReadElementContentAsString();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(Base64String);
        }

        #endregion
    }
}
