using StruSoft.Interop.StruXml.Data;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Diagnostics;


namespace FemDesign.Loads
{
    [System.Serializable]
    public partial class ExcitationForce : FemDesign.GenericClasses.ILoadElement
    {
        [XmlElement("diagram")]
        public List<Diagram> Diagram { get; set; }

        [XmlElement("combination")]
        public List<ExcitationForceCombination> Combination { get; set; }

        [XmlAttribute("last_change")]
        public DateTime LastChange { get; set; } = DateTime.Now;

        [XmlAttribute("action")]
        public string Action { get; set; } = "added";

        public ExcitationForce() { }
        public ExcitationForce(List<Diagram> diagrams, List<ExcitationForceCombination> combinations)
        {
            this.Diagram = diagrams;
            this.Combination = combinations;
        }

        [XmlIgnore]
        public Guid Guid { get; set; }

        public void EntityCreated()
        {
            return;
        }

        public void EntityModified()
        {
            return;
        }

    }
    [System.Serializable]
    public partial class ExcitationForceCombination
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("dt")]
        public double dT { get; set; }

        [XmlElement("record")]
        public List<ExcitationForceLoadCase> records { get; set; }

        public ExcitationForceCombination() { }
        public ExcitationForceCombination(string name, double dT, List<ExcitationForceLoadCase> records)
        {
            this.Name = name;
            this.dT = dT;
            this.records = records;
        }
    }

    [System.Serializable]
    public partial class ExcitationForceLoadCase
    {
        [XmlAttribute("load_case")]
        public Guid _loadCaseGuid { get; set; }

        [XmlIgnore]
        public LoadCase _loadCase { get; set; }

        [XmlIgnore]
        public LoadCase LoadCase
        {
            get { return _loadCase; }
            set
            {
                _loadCase = value;
                _loadCaseGuid = value.Guid;
            }
        }


        [XmlAttribute("diagram")]
        public string _diagramName { get; set; }

        [XmlIgnore]
        public Diagram _diagram { get; set; }

        [XmlIgnore]
        public Diagram Diagram
        {
            get { return _diagram; }
            set
            {
                _diagram = value;
                _diagramName = value.Name;
            }
        }

        [XmlAttribute("factor")]
        public double Force { get; set; }


        public ExcitationForceLoadCase() { }
        public ExcitationForceLoadCase(LoadCase loadCase, double force, Diagram diagram)
        {
            this.LoadCase = loadCase;
            this.Force = force;
            this.Diagram = diagram;
        }
    }

    [System.Serializable]
    public partial class Diagram
    {
        [XmlElement("record")]
        public List<TimeHistoryDiagram> _records;

        [XmlIgnore]
        public List<TimeHistoryDiagram> Records
        {
            get
            {
                // add element to the first index of _records using system.linq
                return _records.Prepend(new TimeHistoryDiagram(0, _startValue)).ToList();
            }
            set
            {
                if (value[0].Time != 0)
                {
                    throw new System.ArgumentException("First record must have time = 0");
                }

                _startValue = value[0].Value;
                _records = value.Skip(1).ToList();

            }
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("direction")]
        public Direction Direction { get; set; } = Direction.Horizontal;

        [Browsable(false)]
        [XmlAttribute("start_value")]
        public double _startValue;

        public Diagram() { }
        public Diagram(string name, List<TimeHistoryDiagram> records)
        {
            this.Name = name;
            this.Records = records;
        }

        public Diagram(string name, List<double> times, List<double> values)
        {
            Name = name;
            if (times.Count != values.Count)
                throw new System.ArgumentException("Time and values must have the same length.");
            Records = times.Zip(values, (t, v) => new TimeHistoryDiagram(t, v)).ToList();
        }

    }

    [System.Serializable]
    public partial class TimeHistoryDiagram
    {
        [XmlAttribute("T")]
        public double Time { get; set; }

        [XmlAttribute("value")]
        [Browsable(false)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double _value;

        [XmlIgnore]
        public double Value
        {
            get { return _value; }
            set
            {
                if (value < -1.00 || value > 1.00)
                {
                    throw new System.ArgumentException("Value must be in the range -1.00 to 1.00");
                }
                _value = value;
            }
        }

        public TimeHistoryDiagram() { }
        
        public TimeHistoryDiagram(double time, double value)
        {
            this.Time = time;
            this.Value = value;
        }
    }

    public enum Direction
    {
        [XmlEnum("horizontal")]
        Horizontal,
        [XmlEnum("vertical")]
        Vertical,
    }

}
