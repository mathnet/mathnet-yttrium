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

using MathNet.Symbolics.Events;

namespace MathNet.Symbolics.Packages.ObjectModel
{
    //public class CompoundProcess : ProcessBase
    //{
    //    private IMathSystem system;
    //    private int[] inputSignalIndexes, outputSignalIndexes, busIndexes;
    //    private Signal[] inputSignals, outputSignals;
    //    private Bus[] buses;

    //    /// <param name="inputSignalIndexes">note: negative indexes are interpreted as internal signal indexes, incremented by one (thus -1 means the internal signal with index zero)!</param>
    //    /// <param name="outputSignalIndexes">note: negative indexes are interpreted as internal signal indexes, incremented by one (thus -1 means the internal signal with index zero)!</param>
    //    public CompoundProcess(IMathSystem system, int[] inputSignalIndexes, int[] outputSignalIndexes, int[] busIndexes)
    //    {
    //        this.system = system;
    //        this.inputSignalIndexes = inputSignalIndexes;
    //        this.outputSignalIndexes = outputSignalIndexes;
    //        this.busIndexes = busIndexes;

    //        if(inputSignalIndexes.Length != system.InputCount)
    //            throw new ArgumentException("The count of input signal mappings doesn't match the compound process structure.", "inputSignalIndexes");
    //        if(outputSignalIndexes.Length != system.OutputCount)
    //            throw new ArgumentException("The count of output signal mappings doesn't match the compound process structure.", "outputSignalIndexes");
    //        if(busIndexes.Length != system.BusCount)
    //            throw new ArgumentException("The count of bus mappings doesn't match the compound process structure.", "busIndexes");

    //        this.inputSignals = new Signal[inputSignalIndexes.Length];
    //        this.outputSignals = new Signal[outputSignalIndexes.Length];
    //        this.buses = new Bus[busIndexes.Length];

    //        this.system.OutputValueChanged += system_OutputValueChanged;
    //    }

    //    void system_OutputValueChanged(object sender, IndexedSignalEventArgs e)
    //    {
    //        outputSignals[e.Index].PostNewValue(e.Signal.Value);
    //    }

    //    public override void Register(IList<Signal> inputSignals, IList<Signal> outputSignals, IList<Signal> internalSignals, IList<Bus> buses)
    //    {
    //        for(int i = 0; i < inputSignalIndexes.Length; i++)
    //        {
    //            this.inputSignals[i] = inputSignalIndexes[i] >= 0 ? inputSignals[inputSignalIndexes[i]] : internalSignals[-inputSignalIndexes[i] - 1];
    //            SenseSignal(this.inputSignals[i]);
    //        }
    //        for(int i = 0; i < outputSignalIndexes.Length; i++)
    //            this.outputSignals[i] = outputSignalIndexes[i] >= 0 ? outputSignals[outputSignalIndexes[i]] : internalSignals[-outputSignalIndexes[i] - 1];
    //        for(int i = 0; i < busIndexes.Length; i++)
    //            this.buses[i] = buses[busIndexes[i]];
    //    }

    //    protected override void Action(bool isInit, Signal origin)
    //    {
    //        if(origin != null)
    //        {
    //            int originIdx = Array.IndexOf<Signal>(inputSignals, origin);
    //            system.PushInputValue(originIdx, inputSignals[originIdx].Value);
    //        }
    //        else
    //            system.PushInputValueRange(inputSignals);
    //    }
    //}
}
