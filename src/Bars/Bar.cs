// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    /// <summary>
    /// bar_type
    /// 
    /// Bar-element
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Bar: EntityBase
    {
        [XmlIgnore]
        private static int barInstance = 0; // used for counter of name)
        [XmlIgnore]
        private static int columnInstance = 0; // used for counter of name
        [XmlIgnore]
        private static int trussInstance = 0; // used for counter of name
        [XmlIgnore]
        public Materials.Material material { get; set; } // for internal use, not to be serialized
        [XmlIgnore]
        public Sections.Section section { get; set; } // for internal use, not to be serialized
        [XmlIgnore]
        public Sections.ComplexSection complexSection { get; set; } // for internal use, not to be serialized
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("maxforce")]
        public double _maxCompression; // non_neg_max_1e30
        [XmlIgnore]
        public double maxCompression
        {
            get{return this._maxCompression;}
            set{this._maxCompression = RestrictedDouble.NonNegMax_1e30(value);}
        } 
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("compressions_plasticity")]
        public bool compressionPlasticity { get; set;} // bool
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tension")]
        public double _maxTension; // non_neg_max_1e30
        [XmlIgnore]
        public double maxTension
        {
            get{return this._maxTension;}
            set{this._maxTension = RestrictedDouble.NonNegMax_1e30(value);}
        }
        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tensions_plasticity")]
        public bool tensionPlasticity { get; set; } // bool
        [XmlAttribute("name")]
        public string name { get; set; } // identifier
        [XmlAttribute("type")]
        public string _type; // beamtype
        [XmlIgnore]
        public string type
        {
            get {return this._type;}
            set {this._type = RestrictedString.BeamType(value);}
        }
        [XmlElement("bar_part")]
        public BarPart barPart { get; set; } // bar_part_type
        [XmlElement("end")]
        public string end { get; set; } // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Bar()
        {
            
        }
        private Bar(string type, string name)
        {
            this.EntityCreated();
            this.type = type;
            this.name = name;
            this.end = "";
        }

        /// <summary>
        /// Create a bar of type beam.
        /// </summary>
        internal static Bar Beam(string identifier, Geometry.Edge edge, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.Section section)
        {
            barInstance++;
            string type = "beam";
            string name = identifier + "." + barInstance.ToString();

            // bar
            Bar bar = new Bar(type, name);
            Sections.ComplexSection complexSection = new Sections.ComplexSection(section, eccentricity);
            bar.material = material;
            bar.section = section;
            bar.complexSection = complexSection;


            // barPart
            bar.barPart = BarPart.Beam(name, edge, connectivity, eccentricity, material, complexSection);

            // return
            return bar;
        }

        /// <summary>
        /// Create a bar of type column.
        /// </summary>
        internal static Bar Column(string identifier, Geometry.Edge line, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.Section section)
        {
            columnInstance++;
            string type = "column";
            string name = identifier + "." + columnInstance.ToString();

            // bar
            Bar bar = new Bar(type, name);
            Sections.ComplexSection complexSection = new Sections.ComplexSection(section, eccentricity);
            bar.material = material;
            bar.section = section;
            bar.complexSection = complexSection;

            // barPart
            bar.barPart = BarPart.Column(name, line, connectivity, eccentricity, material, complexSection);

            // return
            return bar;
        }

        /// <summary>
        /// Create a bar of type truss without compression or tension limits.
        /// </summary>
        internal static Bar Truss(string identifier, Geometry.Edge line, Materials.Material material, Sections.Section section)
        {
            trussInstance++;
            string type = "truss";
            string name = identifier + "." + trussInstance.ToString();
            Bar bar = new Bar(type, name);
            bar.barPart = BarPart.Truss(name, line, material, section);
            bar.material = material;
            bar.section = section;
            bar.complexSection = null;
            return bar;
        }

        /// <summary>
        /// Create a bar of type truss.
        /// </summary>
        internal static Bar Truss(string identifier, Geometry.Edge line, Materials.Material material, Sections.Section section, double maxCompression,  double maxTension, bool compressionPlasticity, bool tensionPlasticity)
        {
            Bar bar = Bar.Truss(identifier, line, material, section);
            bar.maxCompression = maxCompression;
            bar.compressionPlasticity = compressionPlasticity;
            bar.maxTension = maxTension;
            bar.tensionPlasticity = tensionPlasticity;
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

        #region dynamo
        /// <summary>
        /// Create a bar-element of type beam.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve. Only line and arc are supported.</param>
        /// <param name="connectivity">Connectivity. Both ends of the bar-element are given the same connectivity. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. Both ends of the bar-element are given the same eccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Beam(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("Connectivity.Default()")] Connectivity connectivity, [DefaultArgument("Eccentricity.Default()")] Eccentricity eccentricity, Materials.Material material, Sections.Section section, string identifier = "B")
        {
            // convert class
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc2(curve);

            // return
            return Bar.Beam(identifier, edge, connectivity, eccentricity, material, section);
        }
        // /// <summary>
        // /// Create a bar-element of type beam with default properties.
        // /// </summary>
        // /// <remarks>Create</remarks>
        // /// <param name="curve">Curve. Only line and arc are supported.</param>
        // /// <param name="material">Material.</param>
        // /// <param name="section">Section.</param>
        // /// <param name="identifier">Identifier. Optional.</param>
        // /// <returns></returns>
        // [IsVisibleInDynamoLibrary(true)]
        // public static Bar BeamDefault(Autodesk.DesignScript.Geometry.Curve curve, Materials.Material material, Sections.Section section, string identifier = "B")
        // {
        //     return Bar.Beam(curve, Connectivity.Rigid(), Eccentricity.Default(), material, section, identifier);
        // }
        /// <summary>
        /// Create a bar-element of type column.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="connectivity">Connectivity. Both ends of the bar-element are given the same connectivity. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. Both ends of the bar-element are given the same eccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Column(Autodesk.DesignScript.Geometry.Line line, [DefaultArgument("Connectivity.Default()")] Connectivity connectivity, [DefaultArgument("Eccentricity.Default()")] Eccentricity eccentricity, Materials.Material material, Sections.Section section, string identifier = "C")
        {
            // convert class
            Geometry.Edge _line = Geometry.Edge.FromDynamoLine(line);

            // return
            return Bar.Column(identifier, _line, connectivity, eccentricity, material, section);
        }
        // /// <summary>
        // /// Create a bar-element of type column with default properties.
        // /// </summary>
        // /// <remarks>Create</remarks>
        // /// <param name="line">Line.</param>
        // /// <param name="material">Material.</param>
        // /// <param name="section">Section.</param>
        // /// <param name="identifier">Identifier. Optional.</param>
        // /// <returns></returns>
        // [IsVisibleInDynamoLibrary(true)]
        // public static Bar ColumnDefault(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, string identifier = "C")
        // {
        //     return Bar.Column(line, Connectivity.Rigid(), Eccentricity.Default(), material, section, identifier);
        // }
        /// <summary>
        /// Create a bar-element of type truss.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Truss(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, string identifier = "T")
        {
            // convert class
            Geometry.Edge _line = Geometry.Edge.FromDynamoLine(line);

            // return
            return Bar.Truss(identifier, _line, material, section);
        }
        /// <summary>
        /// Create a bar-element of type truss with limited capacity in compression and tension.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="maxCompression">Compression force limit.</param>
        /// <param name="compressionPlasticity">True if plastic behaviour. False if brittle behaviour.</param>
        /// <param name="maxTension">Tension force limit.</param>
        /// <param name="tensionPlasticity">True if plastic behaviour. False if brittle behaviour.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar TrussLimitedCapacity(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, double maxCompression, double maxTension, bool compressionPlasticity = false,  bool tensionPlasticity = false, string identifier = "T")
        {
            // convert class
            Geometry.Edge _line = Geometry.Edge.FromDynamoLine(line);

            // return
            return Bar.Truss(identifier, _line, material, section, maxCompression,  maxTension, compressionPlasticity, tensionPlasticity);
        }

        /// <summary>
        /// Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis.
        /// </summary>
        /// <param name="localY">Vector. Local y-axis.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public Bar SetLocalY(Autodesk.DesignScript.Geometry.Vector localY)
        {
            //
            Bar bar = this.DeepClone();

            //
            bar.barPart.localY = Geometry.FdVector3d.FromDynamo(localY);

            //
            return bar;
        }

        /// <summary>
        /// Create Dynamo curve from underlying Edge (Line or Arc) of Bar.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Curve GetDynamoCurve()
        {
            return this.barPart.edge.ToDynamo();
        }
        #endregion

        #region grasshopper
        /// <summary>
        /// Create Rhino curve from underlying Edge (Line or Arc) of Bar.
        /// </summary>
        internal Rhino.Geometry.Curve GetRhinoCurve()
        {
            return this.barPart.edge.ToRhino();
        }
        #endregion
    }
}