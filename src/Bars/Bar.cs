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

        #region dynamo
        /// <summary>
        /// Create a bar-element of type beam.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve. Only line and arc are supported.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="connectivity">Connectivity. Both ends of the bar-element are given the same connectivity. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. Both ends of the bar-element are given the same eccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Beam(Autodesk.DesignScript.Geometry.Curve curve, Materials.Material material, Sections.Section section, [DefaultArgument("Connectivity.Default()")] Connectivity connectivity, [DefaultArgument("Eccentricity.Default()")] Eccentricity eccentricity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, string identifier = "B")
        {
            // convert class
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc2(curve);

            // create bar
            Bar bar = Bar.Beam(identifier, edge, connectivity, eccentricity, material, section);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            // return
            return bar;
        }
        /// <summary>
        /// Create a bar-element of type column.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="connectivity">Connectivity. Both ends of the bar-element are given the same connectivity. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. Both ends of the bar-element are given the same eccentricity. Optional, if undefined default value will be used.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Column(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, [DefaultArgument("Connectivity.Default()")] Connectivity connectivity, [DefaultArgument("Eccentricity.Default()")] Eccentricity eccentricity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, string identifier = "C")
        {
            // convert class
            Geometry.Edge _line = Geometry.Edge.FromDynamoLine(line);

            // create bar
            Bar bar = Bar.Column(identifier, _line, connectivity, eccentricity, material, section);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            // return
            return bar;
        }
        /// <summary>
        /// Create a bar-element of type truss.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Truss(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, string identifier = "T")
        {
            // convert class
            Geometry.Edge _line = Geometry.Edge.FromDynamoLine(line);

            // create bar
            Bar bar = Bar.Truss(identifier, _line, material, section);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            // return
            return bar;
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
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar TrussLimitedCapacity(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, double maxCompression, double maxTension, bool compressionPlasticity,  bool tensionPlasticity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, string identifier = "T")
        {
            // convert class
            Geometry.Edge _line = Geometry.Edge.FromDynamoLine(line);

            // create bar
            Bar bar = Bar.Truss(identifier, _line, material, section, maxCompression,  maxTension, compressionPlasticity, tensionPlasticity);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            // return
            return bar;
        }

        /// <summary>
        /// Create Dynamo curve from underlying Edge (Line or Arc) of Bar.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Curve GetDynamoCurve()
        {
            return this.BarPart.Edge.ToDynamo();
        }
        #endregion

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