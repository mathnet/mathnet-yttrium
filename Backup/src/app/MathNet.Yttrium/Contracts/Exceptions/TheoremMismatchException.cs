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

namespace MathNet.Symbolics.Exceptions
{
    [Serializable]
    public class TheoremMismatchException : YttriumException
    {
        public TheoremMismatchException()
            : base(MathNet.Symbolics.Properties.Resources.ex_Theorem_Mismatch)
        {
        }

        public TheoremMismatchException(string message)
            : base(message)
        {
        }

        public TheoremMismatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TheoremMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
