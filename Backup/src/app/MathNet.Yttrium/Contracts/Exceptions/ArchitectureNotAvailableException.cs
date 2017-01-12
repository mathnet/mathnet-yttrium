#region Math.NET Yttrium (GPL) by Christoph Ruegg
// Math.NET Yttrium, part of the Math.NET Project
// http://mathnet.opensourcedotnet.info
//
// Copyright (c) 2001-2007, Christoph R�egg,  http://christoph.ruegg.name
//						
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using MathNet.Symbolics.Properties;
using System.Globalization;

namespace MathNet.Symbolics.Exceptions
{
    [Serializable]
    public class ArchitectureNotAvailableException : YttriumException
    {
        Port _port;

        public ArchitectureNotAvailableException()
            : base()
        {
        }

        public ArchitectureNotAvailableException(string message)
            : base(message)
        {
        }

        public ArchitectureNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArchitectureNotAvailableException(Port port)
            : base(string.Format(CultureInfo.CurrentCulture, Resources.ex_NotAvailable_Architecture, (port == null ? "N/A" : port.Entity.ToString())))
        {
            _port = port;
        }

        protected ArchitectureNotAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _port = (Port)info.GetValue("port", typeof(Port));
        }

        public Port Port
        {
            get { return _port; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("port", _port, typeof(Port));
            base.GetObjectData(info, context);
        }
    }
}
