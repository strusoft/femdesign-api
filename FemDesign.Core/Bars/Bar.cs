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
        /// Construct beam or column with uniform section and uniform start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end</param>
        /// <param name="eccentricity">Analytical eccentricity, same at start/end</param>
        /// <param name="connectivity">Connectivity, same at start/end</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity eccentricity, Connectivity connectivity, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, section, eccentricity, connectivity, this.Identifier);
        }

        /// <summary>
        /// Construct beam or column with uniform section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end</param>
        /// <param name="startEccentricity">Analytical start eccentricity</param>
        /// <param name="endEccentricity">Analytical end eccentricity</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, this.Identifier);
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="startSection">Start section</param>
        /// <param name="endSection">End section</param>
        /// <param name="startEccentricity">Analytical start eccentricity</param>
        /// <param name="endEccentricity">Analytical end eccentricity</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection,  Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, this.Identifier);
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="sections">List of sections, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="eccentricities">List of analytical eccentricities, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="connectivities">List of connectivities, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier)
        {
           this.EntityCreated();
           this.Type = type;
           this.Identifier = identifier;
           this.BarPart = new BarPart(edge, this.Type, material, sections, eccentricities, connectivities, this.Identifier);
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="sections">List of sections, 2 or more items.</param>
        /// <param name="positions">List of parametric (0-1) section positions, 2 or more items.</param>
        /// <param name="eccentricities">List of analytical eccentricities, 2 or more items.</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
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
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section (uniform)</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, string identifier)
        {
            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, section, this.Identifier);
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