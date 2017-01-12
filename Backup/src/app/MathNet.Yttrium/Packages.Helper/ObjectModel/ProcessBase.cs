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

using MathNet.Symbolics.Events;

namespace MathNet.Symbolics.Packages.ObjectModel
{
    public abstract class ProcessBase
    {
        private List<Signal> sensitiveSignals;

        protected ProcessBase()
        {
            this.sensitiveSignals = new List<Signal>(8);
        }

        private bool paused; // = false;
        public bool Paused
        {
            get { return paused; }
            set
            {
                if(value)
                    Pause();
                else
                    Continue();
            }
        }

        public abstract void Register(IList<Signal> inputSignals, IList<Signal> outputSignals, IList<Signal> internalSignals, IList<Bus> buses);

        public virtual void Unregister()
        {
            foreach(Signal signal in sensitiveSignals)
                signal.ValueChanged -= signal_SignalValueChanged;
            sensitiveSignals.Clear();
        }

        public void Pause()
        {
            if(paused)
                return;
            foreach(Signal signal in sensitiveSignals)
                signal.ValueChanged -= signal_SignalValueChanged;
            paused = true;
        }

        public void Continue()
        {
            if(!paused)
                return;
            foreach(Signal signal in sensitiveSignals)
                signal.ValueChanged += signal_SignalValueChanged;
            paused = false;
        }

        public void ForceUpdate()
        {
            Action(true, null);
        }

        protected void SenseSignal(Signal signal)
        {
            signal.ValueChanged += signal_SignalValueChanged;
            sensitiveSignals.Add(signal);
        }

        protected void StopSenseSignal(Signal signal)
        {
            signal.ValueChanged -= signal_SignalValueChanged;
            sensitiveSignals.Remove(signal);

        }

        /// <param name="origin">note: may be null if not applicable</param>
        protected abstract void Action(bool isInit, Signal origin);

        private void signal_SignalValueChanged(object sender, ValueNodeEventArgs e)
        {
            Action(false, (Signal)e.ValueNode);
        }
    }
}
