// https://strusoft.com/
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Bars
{
    /// <summary>
    /// bar_type
    /// 
    /// Bar-element
    /// </summary>
    [System.Serializable]
    public partial class Bar: EntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _barInstance = 0; // used for counter of name)
        [XmlIgnore]
        private static int _columnInstance = 0; // used for counter of name
        [XmlIgnore]
        private static int _trussInstance = 0; // used for counter of name

        /// <summary>
        /// Truss only.
        /// </summary>enum
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
        public string _identifier { get; set; } // identifier

        [XmlIgnore]
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                if (this.Type == BarType.Beam)
                {
                    Bar._barInstance++;
                    this._identifier = value + "." + Bar._barInstance.ToString();

                    // update barpart identifier
                    if (this.BarPart != null)
                    {
                        this.BarPart.Identifier = this._identifier;
                    }
                }
                else if (this.Type == BarType.Column)
                {
                    Bar._columnInstance++;
                    this._identifier = value + "." + Bar._columnInstance.ToString();
                    
                    // update barpart identifier
                    if (this.BarPart != null)
                    {
                        this.BarPart.Identifier = this._identifier;
                    }
                }
                else if (this.Type == BarType.Truss)
                {
                    Bar._trussInstance++;
                    this._identifier = value + "." + Bar._trussInstance.ToString();
                    
                    // update barpart identifier
                    if (this.BarPart != null)
                    {
                        this.BarPart.Identifier = this._identifier;
                    }
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
                }
            }
        }

        [XmlAttribute("type")]
        public BarType _type; // beamtype

        [XmlIgnore]
        public BarType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
            // get {return this._type;}
            // set {this._type = RestrictedString.BeamType(value);}
        }

        [XmlElement("bar_part", Order = 1)]
        public BarPart BarPart { get; set; } // bar_part_type

        [XmlElement("end", Order = 2)]
        public string End = "";

        [XmlIgnore]
        public List<Reinforcement.Ptc> Ptc = new List<Reinforcement.Ptc>();

        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> Reinforcement = new List<Reinforcement.BarReinforcement>();

        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> Stirrups
        {
            get
            {
                return this.Reinforcement.Where( x => x.Stirrups != null).ToList();
            }
        }
        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> LongitudinalBars
        {
            get
            {
                return this.Reinforcement.Where( x => x.LongitudinalBar != null).ToList();
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Bar()
        {
            
        }

        /// <summary>
        /// Construct beam or column with uniform section 
        /// </summary>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity eccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, section, eccentricity, startConnectivity, endConnectivity, this.Identifier);
        }

        /// <summary>
        /// Construct beam or column with start/end section 
        /// </summary>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection,  Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, this.Identifier);
        }

        /// <summary>
        /// Construct beam or column with non-uniform section 
        /// </summary>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, double[] positions, Eccentricity[] eccentricities, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, sections, positions, eccentricities, startConnectivity, endConnectivity, this.Identifier);
        }

        /// <summary>
        /// Construct truss
        /// <summary>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, string identifier)
        {
            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, section, this.Identifier);
        }

        /// Create a bar of type beam.
        public static Bar BeamDefine(Geometry.Edge edge, Materials.Material material, Sections.Section[] sections, Connectivity[] connectivities, Eccentricity[] eccentricities, string identifier)
        {
           return new Bar(edge, BarType.Beam, material, sections, connectivities, eccentricities, identifier);
        }

        /// Create a bar of type column.
        public static Bar ColumnDefine(Geometry.Edge edge, Materials.Material material, Sections.Section[] sections, Connectivity[] connectivities, Eccentricity[] eccentricities, string identifier)
        {
           return new Bar(edge, BarType.Column, material, sections, connectivities, eccentricities, identifier);            
        }

        /// Create a bar of type truss without compression or tension limits.
        public static Bar TrussDefine(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier)
        {
           return new Bar(edge, BarType.Truss, material, section, identifier);            
        }

        /// Create a bar of type truss.
        public static Bar TrussDefine(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier, double maxCompression,  double maxTension, bool compressionPlasticity, bool tensionPlasticity)
        {
            Bar bar = new Bar(edge, BarType.Truss, material, section, identifier);
            bar.MaxCompression = maxCompression;
            bar.CompressionPlasticity = compressionPlasticity;
            bar.MaxTension = maxTension;
            bar.TensionPlasticity = tensionPlasticity;
            return bar;
        }

        /// Update entities if this bar should be "reconstructed"
        public void UpdateEntities()
        {
            if (this.Type == BarType.Truss)
            {
                this.EntityCreated();
                this.BarPart.EntityCreated();
            }
            else
            {
                this.EntityCreated();
                this.BarPart.EntityCreated();
                this.BarPart.ComplexSectionObj.EntityCreated();
            }
        }


    }
}