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

using MathNet.Symbolics.Backend;
using MathNet.Symbolics.Packages.Standard.Structures;
using MathNet.Symbolics.Packages.ObjectModel;
using MathNet.Symbolics.Manipulation;
using MathNet.Symbolics.Library;

namespace MathNet.Symbolics.Packages.Standard.Trigonometry
{
    public class HyperbolicCotangentArchitectures : GenericArchitectureFactory
    {
        private static readonly MathIdentifier _entityId = new MathIdentifier("HyperbolicCotangent", "Std");
        public static MathIdentifier EntityIdentifier
        {
            get { return _entityId; }
        }

        public HyperbolicCotangentArchitectures()
            : base(_entityId)
        {
            AddArchitecture(EntityId.DerivePrefix("Real"),
                RealValueCategory.IsRealValueMember,
                delegate(Port port) { return new ProcessBase[] { new GenericStdParallelProcess<RealValue, RealValue>(delegate(RealValue value) { return value.HyperbolicCotangent(); }, RealValue.ConvertFrom, port.InputSignalCount) }; });

            AddArchitecture(EntityId.DerivePrefix("Complex"),
                ComplexValueCategory.IsComplexValueMember,
                delegate(Port port) { return new ProcessBase[] { new GenericStdParallelProcess<ComplexValue, ComplexValue>(delegate(ComplexValue value) { return value.HyperbolicCotangent(); }, ComplexValue.ConvertFrom, port.InputSignalCount) }; });
        }

        public static void RegisterTheorems(ILibrary library)
        {
        }
    }
}
