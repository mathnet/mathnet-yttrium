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
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MathNet.Symbolics.Mediator.ObjectModel
{
    [Serializable]
    public abstract class PortSignalIndexCommand : CommandBase, ICommand
    {
        private CommandReference _signalRef, _portRef;
        private int _index;

        protected PortSignalIndexCommand() { }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public CommandReference SignalReference
        {
            get { return _signalRef; }
            set { _signalRef = value; }
        }
        public CommandReference PortReference
        {
            get { return _portRef; }
            set { _portRef = value; }
        }

        protected abstract void Action(Port port, Signal signal, int index);

        [System.Diagnostics.DebuggerStepThrough]
        public void Execute()
        {
            if(!BeginExecute())
                return;
            Signal signal = GetVerifySignal(_signalRef);
            Port port = GetVerifyPort(_portRef);
            Action(port, signal, _index);
            EndExecute();
        }

        #region Serialization
        protected PortSignalIndexCommand(SerializationInfo info, StreamingContext context)
        {
            _index = info.GetInt32("idx");
            _portRef = CommandReference.Deserialize("port", info);
            _signalRef = CommandReference.Deserialize("signal", info);
        }
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("idx", _index);
            _portRef.Serialize("port", info);
            _signalRef.Serialize("signal", info);
        }
        public virtual void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("PortSignalIndexCommand", Config.YttriumNamespace);
            _index = int.Parse(reader.ReadElementString("Index"));
            _portRef = CommandReference.Deserialize("Port", reader);
            _signalRef = CommandReference.Deserialize("Signal", reader);
            reader.ReadEndElement();
        }
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("PortSignalIndexCommand", Config.YttriumNamespace);
            writer.WriteElementString("Index", _index.ToString());
            _portRef.Serialize("Port", writer);
            _signalRef.Serialize("Signal", writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}
