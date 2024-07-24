using FemDesign.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Design parameters for steel bars
    /// </summary>
    [System.Serializable]
    public partial class SteelBarDesignParameters : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "CCDESPARAMBARST";

        [XmlAttribute("LimitUtilization")]
        public double _utilizationLimit;

        [XmlIgnore]
        public double UtilizationLimit
        {
            get { return _utilizationLimit; }
            set { _utilizationLimit = RestrictedDouble.ValueInOpenInterval(value, 0, 1); }
        }

        /// <summary>
        /// List of BarPart GUIDs to apply the parameters to
        /// </summary>
        [XmlElement("GUID")]
        public List<Guid> Guids { get; set; }

        // Dynamic attributes can be handled by creating a dictionary or list of key-value pairs
        [XmlIgnore]
        public Dictionary<string, Guid> _sections { get; set; } = new Dictionary<string, Guid>();

        // Serialization logic for dynamic attributes
        [XmlAnyAttribute]
        public XmlAttribute[] _additionalAttributes
        {
            get
            {
                var xmlAttributes = new List<XmlAttribute>();
                foreach (var kvp in _sections)
                {
                    var attribute = new XmlDocument().CreateAttribute(kvp.Key.ToString());
                    attribute.Value = kvp.Value.ToString();
                    xmlAttributes.Add(attribute);
                }
                return xmlAttributes.ToArray();
            }
            set
            {
                foreach (var attr in value)
                {
                    _sections[attr.Name] = new Guid(attr.Value);
                }
            }
        }

        [XmlAttribute("vSection_itemcnt")]
        public int _sectionCount
        {
            get { return _sections.Count; }
            set { }
        }

        public bool ShouldSerializeSectionCount()
        {
            return _sections.Count != 0;
        }

        private SteelBarDesignParameters()
        {

        }

        public SteelBarDesignParameters(double utilizationLimit, List<Sections.Section> sections)
        {
            UtilizationLimit = utilizationLimit;

            if ( sections.Any(x => x.MaterialFamily != "Steel"))
            {
                throw new System.ArgumentException("Section must be steel.");
            }

            // Add dynamic attributes
            for (int i = 0; i < sections.Count; i++)
            {
                this._sections[$"vSection_csec_{i}"] = sections[i].Guid;
            }
        }

        public SteelBarDesignParameters(double utilizationLimit, List<Guid> sectionGuids)
        {
            UtilizationLimit = utilizationLimit;

            // Add dynamic attributes
            for (int i = 0; i < sectionGuids.Count; i++)
            {
                this._sections[$"vSection_csec_{i}"] = sectionGuids[i];
            }
        }


        public SteelBarDesignParameters(double utilizationLimit, List<Sections.Section> sections, FemDesign.Bars.Bar bar) : this(utilizationLimit, sections)
        {
            SetParametersOnBars(bar);
        }

        public void SetParametersOnBars(List<Bar> bars)
        {
            if (bars.Any(x => x.IsSteel() == false))
            {
                throw new System.ArgumentException("Bar must be steel.");
            }
            this.Guids = bars.Select(x => x.BarPart.Guid).ToList();
        }

        public void SetParametersOnBars(Bar bars)
        {
            SetParametersOnBars(new List<Bar> { bars });
        }

    }
}
