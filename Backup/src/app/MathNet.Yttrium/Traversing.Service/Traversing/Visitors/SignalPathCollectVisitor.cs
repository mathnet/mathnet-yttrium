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
    public class SignalPathCollectVisitor : AbstractScanVisitor, ISignalPathCollectVisitor
    {
        private List<List<Signal>> _paths;
        private Stack<Signal> _currentPath;
        private Signal _source;

        public SignalPathCollectVisitor(Signal source)
        {
            _paths = new List<List<Signal>>();
            _currentPath = new Stack<Signal>();
            _source = source;
        }

        public override IScanStrategy DefaultStrategy
        {
            get { return Strategies.AllPathsStrategy.Instance; }
        }

        public void Reset(Signal source)
        {
            _paths.Clear();
            _currentPath.Clear();
            _source = source;
        }

        public List<List<Signal>> Paths
        {
            get { return _paths; }
        }

        public override bool EnterSignal(Signal signal, Port parent, bool again, bool root)
        {
            _currentPath.Push(signal);
            if(_source.Equals(signal))
            {
                List<Signal> p = new List<Signal>(_currentPath);
                _paths.Add(p);
                return false;
            }
            return base.EnterSignal(signal, parent, again, root);
        }

        public override bool LeaveSignal(Signal signal, Port parent, bool again, bool root)
        {
            _currentPath.Pop();
            return base.LeaveSignal(signal, parent, again, root);
        }
    }
}
