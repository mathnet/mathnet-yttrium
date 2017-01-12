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
using System.Collections.Generic;
using System.Text;

namespace MathNet.Symbolics
{
    [Serializable]
    public struct EncapsulationFormat : IEquatable<EncapsulationFormat>
    {
        private readonly char prefix, postfix;
        private readonly bool distinguishable;

        public EncapsulationFormat(char prefix, char postfix)
        {
            this.prefix = prefix;
            this.postfix = postfix;
            this.distinguishable = prefix != postfix;
        }

        public char Prefix
        {
            get { return prefix; }
        }
        public char Postfix
        {
            get { return postfix; }
        }
        public bool Distinguishable
        {
            get { return distinguishable; }
        }

        #region IEquatable<EncapsulationFormat> Members
        public bool Equals(EncapsulationFormat other)
        {
            return prefix == other.prefix && postfix == other.postfix;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is EncapsulationFormat))
                throw new ArgumentException();
            return Equals((EncapsulationFormat)obj);
        }

        public override int GetHashCode()
        {
            return prefix.GetHashCode() ^ postfix.GetHashCode();
        }

        public static bool operator ==(EncapsulationFormat x, EncapsulationFormat y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(EncapsulationFormat x, EncapsulationFormat y)
        {
            return !x.Equals(y);
        }
        #endregion
    }
}
