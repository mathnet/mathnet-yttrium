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

namespace MathNet.Symbolics.Packages.ObjectModel
{
    public delegate Tout ProcessItem<Tin,Tout>(Tin item);

    public class GenericStdParallelProcess<Tin, Tout> : GenericParallelProcess
        where Tin : IValueStructure
        where Tout : IValueStructure
    {
        private ProcessItem<Tin, Tout> processItem;
        private ConvertFrom<Tin> convertFrom;

        public GenericStdParallelProcess(ProcessItem<Tin, Tout> processItem, ConvertFrom<Tin> convertFrom, int firstInput, int firstOutput, int count, bool inputIsInternal, bool outputIsInternal)
            : base(firstInput, firstOutput, count, inputIsInternal, outputIsInternal)
        {
            this.processItem = processItem;
            this.convertFrom = convertFrom;
        }
        public GenericStdParallelProcess(ProcessItem<Tin, Tout> processItem, int firstInput, int firstOutput, int count, bool inputIsInternal, bool outputIsInternal)
            : base(firstInput, firstOutput, count, inputIsInternal, outputIsInternal)
        {
            this.processItem = processItem;
            this.convertFrom = delegate(IValueStructure value) { return (Tin)value; };
        }
        public GenericStdParallelProcess(ProcessItem<Tin, Tout> processItem, ConvertFrom<Tin> convertFrom, int count)
            : base(0, 0, count, false, false)
        {
            this.processItem = processItem;
            this.convertFrom = convertFrom;
        }
        public GenericStdParallelProcess(ProcessItem<Tin, Tout> processItem, int count)
            : base(0, 0, count, false, false)
        {
            this.processItem = processItem;
            this.convertFrom = delegate(IValueStructure value) { return (Tin)value; };
        }
        

        protected override void Process(Signal input, Signal output)
        {
            output.PostNewValue(processItem(convertFrom(input.Value)));
        }
    }
}
