// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// bar_type
    /// 
    /// Bar-element
    /// </summary>
    [System.Serializable]
    public class Bar: EntityBase
    {
        [XmlIgnore]
        private static int _barInstance = 0; // used for counter of name)
        [XmlIgnore]
        private static int _columnInstance = 0; // used for counter of name
        [XmlIgnore]
        private static int _trussInstance = 0; // used for counter of name
        [XmlIgnore]
        public Materials.Material Material { get; set; } // for internal use, not to be serialized
        [XmlIgnore]
        public Sections.Section Section { get; set; } // for internal use, not to be serialized
        [XmlIgnore]
        public Sections.ComplexSection ComplexSection { get; set; } // for internal use, not to be serialized
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("maxforce")]
        public double _maxCompression; // non_neg_max_1e30
        [XmlIgnore]
        public double MaxCompression
        {
            get{return this._maxCompression;}
            set{this._maxCompression = RestrictedDouble.NonNegMax_1e30(value);}
        } 
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("compressions_plasticity")]
        public bool CompressionPlasticity { get; set;} // bool
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tension")]
        public double _maxTension; // non_neg_max_1e30
        [XmlIgnore]
        public double MaxTension
        {
            get{return this._maxTension;}
            set{this._maxTension = RestrictedDouble.NonNegMax_1e30(value);}
        }
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tensions_plasticity")]
        public bool TensionPlasticity { get; set; } // bool
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier
        [XmlAttribute("type")]
        public string _type; // beamtype
        [XmlIgnore]
        public string Type
        {
            get {return this._type;}
            set {this._type = RestrictedString.BeamType(value);}
        }
        [XmlElement("bar_part")]
        public BarPart BarPart { get; set; } // bar_part_type
        [XmlElement("end")]
        public string End { get; set; } // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Bar()
        {
            
        }
        private Bar(string type, string name)
        {
            this.EntityCreated();
            this.Type = type;
            this.Name = name;
            this.End = "";
        }

        /// Create a bar of type beam.
        internal static Bar Beam(string identifier, Geometry.Edge edge, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.Section section)
        {
            Bar._barInstance++;
            string type = "beam";
            string name = identifier + "." + Bar._barInstance.ToString();

            // bar
            Bar bar = new Bar(type, name);
            Sections.ComplexSection complexSection = new Sections.ComplexSection(section, eccentricity);
            bar.Material = material;
            bar.Section = section;
            bar.ComplexSection = complexSection;


            // barPart
            bar.BarPart = BarPart.Beam(name, edge, connectivity, eccentricity, material, complexSection);

            // return
            return bar;
        }

        /// Create a bar of type column.
        internal static Bar Column(string identifier, Geometry.Edge line, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.Section section)
        {
            Bar._columnInstance++;
            string type = "column";
            string name = identifier + "." + Bar._columnInstance.ToString();

            // bar
            Bar bar = new Bar(type, name);
            Sections.ComplexSection complexSection = new Sections.ComplexSection(section, eccentricity);
            bar.Material = material;
            bar.Section = section;
            bar.ComplexSection = complexSection;

            // barPart
            bar.BarPart = BarPart.Column(name, line, connectivity, eccentricity, material, complexSection);

            // return
            return bar;
        }

        /// Create a bar of type truss without compression or tension limits.
        internal static Bar Truss(string identifier, Geometry.Edge line, Materials.Material material, Sections.Section section)
        {
            Bar._trussInstance++;
            string type = "truss";
            string name = identifier + "." + Bar._trussInstance.ToString();
            Bar bar = new Bar(type, name);
            bar.BarPart = BarPart.Truss(name, line, material, section);
            bar.Material = material;
            bar.Section = section;
            bar.ComplexSection = null;
            return bar;
        }

        /// Create a bar of type truss.
        internal static Bar Truss(string identifier, Geometry.Edge line, Materials.Material material, Sections.Section section, double maxCompression,  double maxTension, bool compressionPlasticity, bool tensionPlasticity)
        {
            Bar bar = Bar.Truss(identifier, line, material, section);
            bar.MaxCompression = maxCompression;
            bar.CompressionPlasticity = compressionPlasticity;
            bar.MaxTension = maxTension;
            bar.TensionPlasticity = tensionPlasticity;
            return bar;
        }

        // internal static Bar Truss(FdLine line, Materials.Material material, Sections.Section section, TrussLimitedCapacity lcCompression, TrussLimitedCapacity lcTension)
        // {
        //     tInstance++;
        //     string type = "truss";
        //     string name = "T." + tInstance.ToString();

        //     // create bar
        //     Bar bar = new Bar(type, name);
        //     bar.barPart = BarPart.Truss(name, line, material, section);
        //     bar.material = material;
        //     bar.section = section;
        //     bar.complexSection = null;

        //     // limited capacity
        //     if (lcCompression != null)
        //     {
        //         bar.maxCompression = lcCompression.maxCapacity;
        //         bar.compressionPlasticity = lcCompression.plasticBehaviour;
        //     }
        //     if (lcTension != null)
        //     {
        //         bar.maxTension = lcTension.maxCapacity;
        //         bar.tensionPlasticity = lcTension.plasticBehaviour;
        //     }
        //     return bar;
        // }


        #region grasshopper
        /// <summary>
        /// Create Rhino curve from underlying Edge (Line or Arc) of Bar.
        /// </summary>
        internal Rhino.Geometry.Curve GetRhinoCurve()
        {
            return this.BarPart.Edge.ToRhino();
        }
        #endregion
    }
}