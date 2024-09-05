using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    [System.Serializable]
    public partial class PeriodicExcitation : FemDesign.GenericClasses.ILoadElement
    {
        [XmlElement("record")]
        public List<PeriodicLoad> Records { get; set; } = new List<PeriodicLoad>();

        [XmlAttribute("last_change")]
        public DateTime LastChange { get; set; } = DateTime.Now;

        [XmlAttribute("action")]
        public string Action { get; set; } = "added";

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

        internal PeriodicExcitation() { }

        public PeriodicExcitation(List<PeriodicLoad> records)
        {
            this.Records = records;
        }
    }

    [System.Serializable]
    public partial class PeriodicLoad : FemDesign.GenericClasses.ILoadElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("frequency")]
        public double _frequency;

        [XmlIgnore]
        public double Frequency
        {
            get { return this._frequency; }
            set { this._frequency = RestrictedDouble.PositiveMax_1000(value); }
        }

        [XmlElement("case")]
        public List<PeriodicCase> Case { get; set; }

        public PeriodicLoad() { }

        public PeriodicLoad(string name, double frequency, List<PeriodicCase> cases)
        {
            this.Name = name;
            this.Frequency = frequency;
            this.Case = cases;
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

    public partial class PeriodicCase
    {
        public enum Shape
        {
            [Parseable("cos", "Cos", "c")]
            [XmlEnum("cos")]
            Cos,
            [Parseable("sin", "Sin", "s")]
            [XmlEnum("sin")]
            Sin,
        }

        [XmlAttribute("factor")]
        public double _factor;

        [XmlIgnore]
        public double Factor
        {
            get { return this._factor; }
            set { this._factor = RestrictedDouble.NonNegMax_1000(value); }
        }

        [XmlAttribute("phase")]
        public Shape phase { get; set; }

        [XmlAttribute("load_case")]
        public System.Guid LoadCaseGuid { get; set; }

        [XmlIgnore]
        public LoadCase _loadCase { get; set; }

        [XmlIgnore]
        public LoadCase LoadCase
        {
            get { return _loadCase; }
            set
            {
                _loadCase = value;
                LoadCaseGuid = value.Guid;
            }
        }


        public PeriodicCase() { }

        public PeriodicCase(double factor, Shape phase, LoadCase LoadCase)
        {
            this.Factor = factor;
            this.phase = phase;
            this.LoadCase = LoadCase;
        }

        public PeriodicCase(double factor, Shape phase, Guid LoadCaseGuid)
        {
            this.Factor = factor;
            this.phase = phase;
            this.LoadCaseGuid = LoadCaseGuid;
        }

    }

}
