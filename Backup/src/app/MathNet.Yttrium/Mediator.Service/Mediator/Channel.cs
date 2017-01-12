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

namespace MathNet.Symbolics.Mediator
{
    public class Channel<EntryType>
    {
        private Queue<EntryType> _queue;
        private bool _enabled;

        /// <summary>
        /// Thrown if a new entry is available and the queue has been empty.
        /// </summary>
        public event EventHandler EntryAvailable;

        public Channel()
        {
            _queue = new Queue<EntryType>();
            _enabled = false;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
            }
        }

        public bool AcceptsNewEntries
        {
            get { return _enabled; }
        }

        
        public bool HasEntries
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _queue.Count > 0; }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public void PushEntry(EntryType entry)
        {
            if(_enabled)
            {
                _queue.Enqueue(entry);
                // notify if a new _first_ entry is available.
                if(_queue.Count == 1 && EntryAvailable != null)
                    EntryAvailable(this, EventArgs.Empty);
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public EntryType PopEntry()
        {
            return _queue.Dequeue();
        }

        [System.Diagnostics.DebuggerStepThrough]
        public EntryType PeekEntry()
        {
            return _queue.Peek();
        }
    }
}
