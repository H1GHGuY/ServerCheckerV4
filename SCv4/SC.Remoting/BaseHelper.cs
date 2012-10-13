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

namespace SC.Remoting
{
    class BaseHelper
    {
        private const int BUFFER_SIZE = 4096;
        
        public BaseHelper()
        {
        }

        public static System.IO.MemoryStream ToMemoryStream(System.IO.Stream stream)
        {
            if (stream is System.IO.MemoryStream)
                return stream as System.IO.MemoryStream;

            System.IO.MemoryStream ret = new System.IO.MemoryStream();
            
            byte[] buffer = new byte[BUFFER_SIZE]; // a full mem page
            int n = stream.Read(buffer, 0, BUFFER_SIZE);
            while (n > 0)
            {
                ret.Write(buffer, 0, n);
                n = stream.Read(buffer, 0, BUFFER_SIZE);
            }
            ret.Seek(0, System.IO.SeekOrigin.Begin);
            return ret;
        }
    }
}
