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
using System.Xml;

using MathNet.Symbolics.Backend;
using MathNet.Symbolics.Backend.Containers;

namespace MathNet.Symbolics.SystemBuilder.Toolkit
{
    /// <summary>
    /// System Builder Director for reading a complete Xml representation.
    /// </summary>
    public class XmlSystemReader
    {
        private ISystemBuilder _builder;
        private XmlReader _reader;
        private StateMachine _fsm;

        private class StateMachine : BuilderStateMachine
        {
            private XmlReader _reader;

            public StateMachine()
            {
            }
            //public StateMachine(XmlReader reader)
            //{
            //    _reader = reader;
            //}

            public XmlReader Reader
            {
                //get { return _reader; }
                set { _reader = value; }
            }

            public BuilderState ReadNextState()
            {
                while(!_reader.IsStartElement())
                {
                    if(_reader.NodeType == XmlNodeType.EndElement && _reader.LocalName.Equals("System"))
                    {
                        AdvanceTo(BuilderState.Idle);
                        return CurrentState;
                    }
                    else
                        _reader.Read();
                }
                AdvanceTo((BuilderState)Enum.Parse(typeof(BuilderState), _reader.LocalName));
                return CurrentState;
            }
        }

        public XmlSystemReader(ISystemBuilder builder)
        {
            _builder = builder;
            _fsm = new StateMachine();
        }

        /// <returns>true if a start-element was found, false if an end-element was found.</returns>
        private bool ReadToElement()
        {
            while(!_reader.IsStartElement())
            {
                if(_reader.NodeType == XmlNodeType.EndElement)
                    return false;
                else
                    _reader.Read();
            }
            return true;
        }

        public void ReadSystems(XmlReader reader, bool multiple)
        {
            _reader = reader;
            _fsm.Reader = reader;
            _fsm.Reset();

            Dictionary<Guid,Guid> signalMappings = new Dictionary<Guid,Guid>();
            Dictionary<Guid,Guid> busMappings = new Dictionary<Guid,Guid>(); 

            bool active = false;

            BuilderState state;
            while(BuilderState.Idle != (state = _fsm.ReadNextState()) || active)
            {
                switch(state)
                {
                    case BuilderState.System:
                        int inputCnt = int.Parse(_reader.GetAttribute("inputCount"), Config.InternalNumberFormat);
                        int outputCnt = int.Parse(_reader.GetAttribute("outputCount"), Config.InternalNumberFormat);
                        int busCnt = int.Parse(_reader.GetAttribute("busCount"), Config.InternalNumberFormat);
                        _reader.Read();
                        _builder.BeginBuildSystem(inputCnt, outputCnt, busCnt);
                        active = true;
                        break;
                    case BuilderState.Signals:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "Signal")
                        {
                            Guid myGuid = new Guid(_reader.GetAttribute("iid"));
                            string label = _reader.GetAttribute("label");
                            bool hold = bool.Parse(_reader.GetAttribute("hold"));
                            bool isSource = bool.Parse(_reader.GetAttribute("isSource"));
                            _reader.Read();
                            Guid tGuid = _builder.BuildSignal(label, hold, isSource);
                            signalMappings.Add(myGuid, tGuid);
                        }
                        break;
                    case BuilderState.Buses:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "Bus")
                        {
                            Guid myGuid = new Guid(_reader.GetAttribute("iid"));
                            string label = _reader.GetAttribute("label");
                            _reader.Read();
                            Guid tGuid = _builder.BuildBus(label);
                            busMappings.Add(myGuid, tGuid);
                        }
                        break;
                    case BuilderState.Ports:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "Port")
                        {
                            InstanceIdSet inputSignals = new InstanceIdSet();
                            InstanceIdSet outputSignals = new InstanceIdSet();
                            InstanceIdSet buses = new InstanceIdSet();
                            //Guid myGuid = new Guid(_reader.GetAttribute("iid"));
                            MathIdentifier entityId = MathIdentifier.Parse(_reader.GetAttribute("entityId"));
                            _reader.ReadToDescendant("InputSignals");
                            _reader.Read();
                            while(_reader.IsStartElement("SignalRef"))
                                inputSignals.Add(signalMappings[new Guid(_reader.ReadElementString())]);
                            _reader.ReadEndElement();
                            _reader.ReadToFollowing("OutputSignals");
                            _reader.Read();
                            while(_reader.IsStartElement("SignalRef"))
                                outputSignals.Add(signalMappings[new Guid(_reader.ReadElementString())]);
                            _reader.ReadEndElement();
                            _reader.ReadToFollowing("Buses");
                            _reader.Read();
                            while(_reader.IsStartElement("BusRef"))
                                buses.Add(busMappings[new Guid(_reader.ReadElementString())]);
                            _reader.ReadEndElement();
                            _builder.BuildPort(entityId, inputSignals, outputSignals, buses);
                        }
                        break;
                    case BuilderState.SignalDetails:
                        _reader.Read();
                        while(ReadToElement())
                        {
                            Guid tGuid = signalMappings[new Guid(_reader.GetAttribute("iid"))];
                            switch(_reader.LocalName)
                            {
                                case "SignalValue":
                                    {
                                        CustomDataPack<IValueStructure> pack = CustomDataPack<IValueStructure>.Repack(_reader.ReadInnerXml(), signalMappings, busMappings);
                                        _builder.AppendSignalValue(tGuid, pack);
                                    }
                                    break;
                                case "SignalProperty":
                                    {
                                        CustomDataPack<IProperty> pack = CustomDataPack<IProperty>.Repack(_reader.ReadInnerXml(), signalMappings, busMappings);
                                        _builder.AppendSignalProperty(tGuid, pack);
                                    }
                                    break;
                                case "SignalConstraint":
                                    {
                                        CustomDataPack<IProperty> pack = CustomDataPack<IProperty>.Repack(_reader.ReadInnerXml(), signalMappings, busMappings);
                                        _builder.AppendSignalConstraint(tGuid, pack);
                                    }
                                    break;
                            }
                        }
                        break;
                    case BuilderState.InputSignals:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "SignalRef")
                        {
                            Guid tGuid = signalMappings[new Guid(_reader.ReadElementString())];
                            _builder.AppendSystemInputSignal(tGuid);
                        }
                        break;
                    case BuilderState.OutputSignals:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "SignalRef")
                        {
                            Guid tGuid = signalMappings[new Guid(_reader.ReadElementString())];
                            _builder.AppendSystemOutputSignal(tGuid);
                        }
                        break;
                    case BuilderState.NamedSignals:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "SignalRef")
                        {
                            string name = _reader.GetAttribute("name");
                            Guid tGuid = signalMappings[new Guid(_reader.ReadElementString())];
                            _builder.AppendSystemNamedSignal(tGuid, name);
                        }
                        break;
                    case BuilderState.NamedBuses:
                        _reader.Read();
                        while(ReadToElement() && _reader.LocalName == "BusRef")
                        {
                            string name = _reader.GetAttribute("name");
                            Guid tGuid = busMappings[new Guid(_reader.ReadElementString())];
                            _builder.AppendSystemNamedBus(tGuid, name);
                        }
                        break;
                    case BuilderState.Idle:
                        _builder.EndBuildSystem();
                        active = false;
                        if(!multiple)
                            return;
                        break;
                }
            }
        }
    }
}
