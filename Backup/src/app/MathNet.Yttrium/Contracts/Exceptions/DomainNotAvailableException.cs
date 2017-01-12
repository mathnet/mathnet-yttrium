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
using System.Globalization;

namespace MathNet.Symbolics.Exceptions
{
    [Serializable]
    public class DomainNotAvailableException : YttriumException
    {
        string domain;

        public DomainNotAvailableException()
            : base()
        {
        }

        public DomainNotAvailableException(string domain)
            : base(string.Format(CultureInfo.CurrentCulture, MathNet.Symbolics.Properties.Resources.ex_NotAvailable_Domain, domain))
        {
            this.domain = domain;
        }

        public DomainNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DomainNotAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            domain = info.GetString("domain");
        }

        public string Domain
        {
            get { return domain; }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("domain", domain);
            base.GetObjectData(info, context);
        }
    }
}
