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
using MathNet.Symbolics.Backend.Containers;
using MathNet.Symbolics.Packages.Standard.Structures;
using MathNet.Symbolics.Packages.ObjectModel;
using MathNet.Symbolics.Manipulation;
using MathNet.Symbolics.Library;
using MathNet.Symbolics.Conversion;

namespace MathNet.Symbolics.Packages.Standard.Arithmetics
{
    public class MultiplicationArchitectures : GenericArchitectureFactory
    {
        private static readonly MathIdentifier _entityId = new MathIdentifier("Multiply", "Std");
        public static MathIdentifier EntityIdentifier
        {
            get { return _entityId; }
        }

        public MultiplicationArchitectures()
            : base(_entityId)
        {
            AddArchitecture(EntityId.DerivePrefix("Integer"),
                IntegerValueCategory.IsIntegerValueMember,
                delegate(Port port) { return new ProcessBase[] { new IntegerValue.MultiplyProcess(0, port.InputSignalCount - 1, 0) }; });

            AddArchitecture(EntityId.DerivePrefix("Rational"),
                RationalValueCategory.IsRationalValueMember,
                delegate(Port port) { return new ProcessBase[] { new RationalValue.MultiplyProcess(0, port.InputSignalCount - 1, 0) }; });

            AddArchitecture(EntityId.DerivePrefix("Real"),
                RealValueCategory.IsRealValueMember,
                delegate(Port port) { return new ProcessBase[] { new RealValue.MultiplyProcess(0, port.InputSignalCount - 1, 0) }; });

            AddArchitecture(EntityId.DerivePrefix("Complex"),
                ComplexValueCategory.IsComplexValueMember,
                delegate(Port port) { return new ProcessBase[] { new GenericStdFunctionProcess<ComplexValue>(delegate() { return ComplexValue.One; }, ComplexValue.ConvertFrom, ComplexValue.Multiply, port.InputSignalCount) }; });
        }

        public static bool CollectFactors(ISignalSet signals)
        {
            bool changed = false;
            for(int i = 0; i < signals.Count; i++)
            {
                Signal s = signals[i];
                if(!s.BehavesAsBeingDriven(false))
                    continue;

                Port p = s.DrivenByPort;
                if(p.Entity.EntityId.Equals(MultiplicationArchitectures.EntityIdentifier))
                {
                    signals.RemoveAt(i);
                    ISignalSet inputs = p.InputSignals;
                    for(int j = 0; j < inputs.Count; j++)
                        signals.Insert(i + j, inputs[j]);
                    i--;
                    changed = true;
                    continue;
                }

                if(p.Entity.EntityId.Equals(DivisionArchitectures.EntityIdentifier))
                {
                    ISignalSet inputs = p.InputSignals;
                    signals[i] = inputs[0];
                    i--;
                    for(int j = 1; j < inputs.Count; j++)
                        signals.Insert(i + j, Std.Invert(inputs[j]));
                    changed = true;
                    continue;
                }
            }
            return changed;
        }

        public static bool SimplifyFactors(ISignalSet signals)
        {
            bool changed = CollectFactors(signals);
            if(signals.Count < 2)
                return changed;
            IAccumulator acc = null;
            for(int i = signals.Count - 1; i >= 0; i--)
            {
                Signal s = signals[i];
                if(Std.IsConstantComplex(s))
                {
                    if(Std.IsAdditiveIdentity(s))
                    {
                        signals.Clear();
                        signals.Add(s);
                        return true;
                    }
                    if(acc == null)
                        acc = Accumulator<IntegerValue, RationalValue>.Create(IntegerValue.MultiplicativeIdentity);
                    signals.RemoveAt(i);
                    changed = true;
                    acc = acc.Multiply(s.Value);
                }
            }
            if(acc != null && !acc.Value.Equals(IntegerValue.MultiplicativeIdentity))
            {
                signals.Insert(0, Std.DefineConstant(acc.Value));
            }
            return changed;
        }

        public static void RegisterTheorems(ILibrary library)
        {
            Analysis.DerivativeTransformation.Provider.Add(
                new Analysis.DerivativeTransformation(_entityId,
                delegate(Port port, SignalSet manipulatedInputs, Signal variable, bool hasManipulatedInputs)
                {
                    int cnt = manipulatedInputs.Count;
                    Signal[] addSignals = new Signal[cnt];
                    Signal[] multiplySignals = new Signal[cnt];
                    for(int i = 0; i < cnt; i++)
                    {
                        for(int j = 0; j < cnt; j++)
                            multiplySignals[j] = i == j ? manipulatedInputs[j] : port.InputSignals[j];
                        addSignals[i] = Std.Multiply(multiplySignals);
                    }
                    return new SignalSet(Std.Add(addSignals));
                }));

            Algebra.AutoSimplifyTransformation.Provider.Add(
                new Algebra.AutoSimplifyTransformation(_entityId,
                delegate(Port port)
                {
                    // TODO
                    return ManipulationPlan.DoAlter;
                },
                delegate(Port port, SignalSet manipulatedInputs, bool hasManipulatedInputs)
                {
                    if(SimplifyFactors(manipulatedInputs) || hasManipulatedInputs)
                    {
                        if(manipulatedInputs.Count == 0)
                            return new SignalSet(IntegerValue.ConstantMultiplicativeIdentity);
                        if(manipulatedInputs.Count == 1)
                            return manipulatedInputs;
                        return new SignalSet(StdBuilder.Multiply(manipulatedInputs));
                    }
                    else
                        return port.OutputSignals;
                }));
        }
    }
}
