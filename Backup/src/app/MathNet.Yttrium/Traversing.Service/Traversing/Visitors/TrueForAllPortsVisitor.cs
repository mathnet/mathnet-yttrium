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

namespace MathNet.Symbolics.Traversing.Visitors
{
    public class TrueForAllPortsVisitor : AbstractScanVisitor, ITrueForAllPortsVisitor
    {
        private Predicate<Port> _match;
        private Port _failedPort;
        private Signal _failedPortTarget;

        public TrueForAllPortsVisitor(Predicate<Port> match)
        {
            _match = match;
        }

        public override IScanStrategy DefaultStrategy
        {
            get { return Strategies.AllPortsStrategy.Instance; }
        }

        public void Reset()
        {
            _failedPort = null;
            _failedPortTarget = null;
        }

        public bool TrueForAll
        {
            get { return _failedPort == null; }
        }

        public Port FailedPort
        {
            get { return _failedPort; }
        }

        public Signal FailedPortTarget
        {
            get { return _failedPortTarget; }
        }

        public override bool EnterPort(Port port, Signal parent, bool again, bool root)
        {
            if(again)
                return false;
            if(!_match(port))
            {
                _failedPort = port;
                _failedPortTarget = parent;
                return false;
            }
            return true;
        }

        public override bool LeavePort(Port port, Signal parent, bool again, bool root)
        {
            return _failedPort == null;
        }
    }
}
