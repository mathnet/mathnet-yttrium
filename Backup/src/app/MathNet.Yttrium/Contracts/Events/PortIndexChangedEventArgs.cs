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

namespace MathNet.Symbolics.Events
{
    public sealed class PortIndexChangedEventArgs : EventArgs
    {
        public PortIndexChangedEventArgs(Port port, int indexBefore, int indexAfter)
        {
            this.port = port;
            this.indexBefore = indexBefore;
            this.indexAfter = indexAfter;
        }

        private readonly Port port;
        public Port Port { get { return port; } }

        private readonly int indexBefore;
        public int IndexBefore { get { return indexBefore; } }

        private readonly int indexAfter;
        public int IndexAfter { get { return indexAfter; } }
    }
}
