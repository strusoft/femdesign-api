
using System.Globalization;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    /// <summary>
    /// panel_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Panel: EntityBase
    {
        /// <summary>
        /// Panel instance counter
        /// </summary>
        private static int Instance = 0;

        /// <summary>
        /// Coordinate system
        /// </summary>
        [XmlIgnore]
        private Geometry.FdCoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.FdCoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem == null)
                {
                    this._coordinateSystem = new Geometry.FdCoordinateSystem(this.LocalOrigin, this.LocalX, this.LocalZ.Cross(this.LocalX));
                    return this._coordinateSystem;
                }
                else
                {
                    return this._coordinateSystem;
                }
            }
            set
            {
                this._coordinateSystem = value;
                this._localOrigin = value.Origin;
                this._localX = value.LocalX;
            }
        }

         /// <summary>
        /// Deinfes the position of the local coordinate system
        /// </summary>
        [XmlElement("local_pos", Order = 4)]
        public Geometry.FdPoint3d _localOrigin;
        [XmlIgnore]
        public Geometry.FdPoint3d LocalOrigin
        {
            get
            {
                return this._localOrigin;
            }
            set
            {
                this.CoordinateSystem.Origin = value;
                this._localOrigin = value;
            }
        }
        
        /// <summary>
        /// LocalX and direction of panels
        /// </summary>
        /// <value></value>
        [XmlElement("direction", Order = 2)]
        public Geometry.FdVector3d _localX;
        [XmlIgnore]
        public Geometry.FdVector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                this.CoordinateSystem.SetXAroundZ(value);
                this._localX = this.CoordinateSystem.LocalX;
            }
        }

        [XmlIgnore]
        public Geometry.FdVector3d LocalY
        {
            get
            {
                return this.CoordinateSystem.LocalY;
            }
            set
            {
                this.CoordinateSystem.SetYAroundZ(value);
                this._localX = this.CoordinateSystem.LocalX;
            }
        }

        /// <summary>
        /// LocalZ
        /// </summary>
        [XmlIgnore]
        public Geometry.FdVector3d LocalZ
        {
            get
            {
                return this.Region.LocalZ;
            }
            set
            {
                this.Region.LocalZ = value;
            }
        }

        /// <summary>
        /// Region of panel
        /// </summary>
        [XmlElement("region", Order = 1)]
        public Geometry.Region _region;
        [XmlIgnore]
        public Geometry.Region Region
        {
            get
            {
                return this._region;
            }
            set
            {
                this._region = value.RemoveEdgeConnections();
            }
        }

        /// <summary>
        /// Defines position of base line when using panel type analytical model
        /// </summary>
        /// <value></value>
        [XmlElement("anchor_point", Order = 3)]
        public Geometry.FdPoint3d AnchorPoint { get; set; }

        [XmlElement("internal_panels", Order = 5)]
        public InternalPanels InternalPanels { get; set; }

        [XmlElement("timber_application_data", Order = 6)]
        public Materials.TimberApplicationData TimberApplicationData { get; set; }

        [XmlIgnore]
        public Materials.TimberPanelLibraryType TimberPanelLibraryData { get; set; }

        [XmlIgnore]
        public Materials.CltPanelLibraryType CltPanelLibraryData { get; set; }

        [XmlIgnore]
        public Materials.GlcPanelLibraryType GlcPanelLibraryData { get; set; }

        [XmlElement("camber_simulation", Order = 7)]
        public Camber Camber { get; set; }

        [XmlElement("internal_plastic_limit_forces", Order = 8)]
        public Releases.PlasticityType3d InternalPlasticLimitForces { get; set; }

        [XmlElement("internal_plastic_limit_moments", Order = 9)]
        public Releases.PlasticityType3d InternalPlasticLimitMoments { get; set; }

        [XmlElement("external_plastic_limit_forces", Order = 10)]
        public Releases.PlasticityType3d ExternalPlasticLimitForces { get; set; }

        [XmlElement("external_plastic_limit_moments", Order = 11)]
        public Releases.PlasticityType3d ExternalPlasticLimitMoments { get; set; }

        [XmlElement("internal_stiffness", Order = 12)]
        public Releases.StiffnessWithFriction InternalStiffness { get; set; }

        [XmlElement("external_stiffness", Order = 13)]
        public Releases.StiffnessWithFriction ExternalStiffness { get; set; }

        [XmlElement("internal_rigidity", Order = 14)]
        public Releases.RigidityDataType3 InternalRigidity { get; set; }

        [XmlElement("internal_predefined_rigidity", Order = 15)]
        public GuidListType InternalPredefinedRigidity { get; set; }

        [XmlElement("external_rigidity", Order = 16)]
        public Releases.RigidityDataType3 ExternalRigidity { get; set; } // // sets region border. region edgeconnection sets edgeconnection for specific edge. default should be hinged?

        [XmlElement("external_predefined_rigidity", Order = 17)]
        public GuidListType ExternalPredefinedRigidity { get; set; }

        [XmlAttribute("type")]
        public string _type; // paneltype
        
        [XmlIgnore]
        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = RestrictedString.PanelType(value);
            }
        }

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; }
        [XmlIgnore]
        public Materials.Material _material;
        [XmlIgnore]
        public Materials.Material Material
        {
            set
            {
                if (value.Concrete != null)
                {
                    // material must be concrete
                    this.ComplexMaterial = value.Guid;
                    this._material = value;
                }
                else
                {
                    throw new System.ArgumentException("Only support for concrete material has been added.");
                }
            }
            get
            {
                return this._material;
            }
        }
        [XmlAttribute("complex_section")]
        public System.Guid ComplexSection;
        [XmlIgnore]
        public Sections.Section _section;
        [XmlIgnore]
        public Sections.Section Section
        {
            set
            {
                this.ComplexSection = value.Guid;
                this._section = value;
            }
            get
            {
                return this._section;
            }
        }
        [XmlAttribute("name")]
        public string _identifier; // identifier
        [XmlIgnore]
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                Panel.Instance++;
                this._identifier = RestrictedString.Length(value, 50) + Panel.Instance.ToString();
            }
        }
        [XmlAttribute("panelname")]
        public string PanelName { get; set; }
        [XmlAttribute("in_situ_fabricated")]
        public bool InSituFabricated { get; set; } // deault="false"
        [XmlAttribute("gap")]
        public double Gap { get; set; }
        [XmlAttribute("orthotropy")]
        public double _orthotropy; // orthotropy_type
        [XmlIgnore]
        public double Orthotropy
        {
            get
            {
                return this._orthotropy;
            }
            set
            {
                this._orthotropy = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Only for struxml export to Revit and/or Tekla. Not used in calculation.
        /// </summary>
        [XmlAttribute("thickness")]
        public double Thickness { get; set; }
        [XmlAttribute("alignment")]
        public string _alignment; // ver_align
        [XmlIgnore]
        public string Alignment
        {
            get
            {
                return this._alignment;
            }
            set
            {
                this._alignment = RestrictedString.VerticalAlign(value);
            }
        }
        [XmlAttribute("align_offset")]
        public double AlignOffset { get; set; }
        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalculation { get; set; } // default="false"
        [XmlAttribute("ecc_crack")]
        public bool EccentricityByCracking { get; set; } // default="false"
        [XmlAttribute("internal_moving_local")]
        public bool InternalMovingLocal { get; set; } // default="false"
        [XmlAttribute("external_moving_local")]
        public bool ExternalMovingLocal { get; set; } // default="false" according to strusoft.xsd. default="True" according to FD gui.
        [XmlAttribute("forced_plate")]
        public bool ForcedPlate { get; set; } // default="false" if false a vertical panel will be represented as a wall in FD, if true a vertical panel will not be represented as a wall
        [XmlAttribute("panel_width")]
        public string _panelWidth; // positive_max_100, default="1.5"
        [XmlIgnore]
        public double PanelWidth
        {
            get
            {
                return System.Convert.ToDouble(_panelWidth, CultureInfo.InvariantCulture);
            }
            set
            {
                this._panelWidth = RestrictedDouble.NonNegMax_100(value).ToString();
            }
        }
        [XmlIgnore]
        public double UniformAvgMeshSize
        {
            set
            {    
                foreach (InternalPanel intPanel in this.InternalPanels.IntPanels)
                {
                    intPanel.MeshSize = value;
                }    
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Panel()
        {

        }

        /// <summary>
        /// Construct standard panel with "Continuous" analytical model.
        /// </summary>
        /// <param name="region">Region of shell containing panels.</param>
        /// <param name="localX">Direction of panels.</param>
        /// <param name="anchorPoint"></param>
        /// <param name="externalRigidity">Default value for shell border EdgeConnections. Can be overwritten by EdgeConnection for each specific edge in Region.</param>
        /// <param name="type">Type of panel.</param>
        /// <param name="complexMaterial">Guid reference to material.</param>
        /// <param name="complexSection">Guid reference to complex section.</param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="panelName">Name of panel.</param>
        /// <param name="gap">Gap between panels.</param>
        /// <param name="orthotropy">Orthotropy.</param>
        /// <param name="ecc">ShellEccentricity.</param>
        /// <param name="externalMovingLocal">EdgeConnection LCS changes along edge?</param>
        public Panel(Geometry.Region region, Geometry.FdPoint3d anchorPoint, InternalPanels internalPanels, Releases.RigidityDataType3 externalRigidity, string type, Materials.Material material, Sections.Section section, string identifier, string panelName, double gap, double orthotropy, ShellEccentricity ecc, bool externalMovingLocal)
        {
            this.EntityCreated();

            // elements
            this.Region = region;
            this.CoordinateSystem = region.CoordinateSystem;
            this.AnchorPoint = anchorPoint;
            this.InternalPanels = internalPanels;
            this.ExternalRigidity = externalRigidity;

            // attributes
            this.Type = type;
            this.Material = material; // note that material and section are not added directly to complexMaterial and complexSection fields.
            this.Section = section;
            this.Identifier = identifier;
            this.PanelName = panelName;
            this.Gap = gap;
            this.Orthotropy = orthotropy;
            this.Alignment = ecc.Alignment;
            this.AlignOffset = ecc.Eccentricity;
            this.EccentricityCalculation = ecc.EccentricityCalculation;
            this.EccentricityByCracking = ecc.EccentricityByCracking;
            this.ExternalMovingLocal = externalMovingLocal;
        }

        /// <summary>
        /// Construct timber panel with "Continuous" analytical model.
        /// </summary>
        /// <param name="region">Region of shell containing panels.</param>
        /// <param name="localX">Direction of panels.</param>
        /// <param name="anchorPoint"></param>
        /// <param name="externalRigidity">Default value for shell border EdgeConnections. Can be overwritten by EdgeConnection for each specific edge in Region.</param>
        /// <param name="type">Type of panel.</param>
        /// <param name="complexMaterial">Guid reference to material.</param>
        /// <param name="complexSection">Guid reference to complex section.</param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="panelName">Name of panel.</param>
        /// <param name="gap">Gap between panels.</param>
        /// <param name="orthotropy">Orthotropy.</param>
        /// <param name="ecc">ShellEccentricity.</param>
        /// <param name="externalMovingLocal">EdgeConnection LCS changes along edge?</param>
        public Panel(Geometry.Region region, Geometry.FdPoint3d anchorPoint, InternalPanels internalPanels, Materials.TimberApplicationData timberApplicationData, Releases.RigidityDataType3 externalRigidity, string type, string identifier, string panelName, double gap, double orthotropy, ShellEccentricity ecc, bool externalMovingLocal, double panelWidth)
        {
            this.EntityCreated();

            // elements
            this.Region = region;
            this.CoordinateSystem = region.CoordinateSystem;
            this.AnchorPoint = anchorPoint;
            this.InternalPanels = internalPanels;
            this.TimberApplicationData = timberApplicationData;
            this.ExternalRigidity = externalRigidity;

            // attributes
            this.Type = type;
            this.Identifier = identifier;
            this.PanelName = panelName;
            this.Gap = gap;
            this.Alignment = ecc.Alignment;
            this.AlignOffset = ecc.Eccentricity;
            this.EccentricityCalculation = ecc.EccentricityCalculation;
            this.EccentricityByCracking = ecc.EccentricityByCracking;
            this.ExternalMovingLocal = externalMovingLocal;
            this.PanelWidth = panelWidth;
        }

        /// <summary>
        /// Create a default concrete shell with panels using a continuous analytical model.
        /// </summary>
        public static Panel DefaultContreteContinuous(Geometry.Region region, Releases.RigidityDataType3 externalRigidity, Materials.Material material, Sections.Section section, string identifier, double orthotropy, ShellEccentricity ecc)
        {
            Geometry.FdPoint3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            string type = "concrete";
            string panelName = "A";
            double gap = 0.003;
            bool externalMovingLocal = true;
            
            return new Panel(region, anchorPoint, internalPanels, externalRigidity, type, material, section, identifier, panelName, gap, orthotropy, ecc, externalMovingLocal);
        }

        /// <summary>
        /// Create a default timber shell with panels using a continuous analytical model.
        /// </summary>
        public static Panel DefaultTimberContinuous(Geometry.Region region, Materials.TimberApplicationData timberApplicationData,  Releases.RigidityDataType3 externalRigidity, string identifier, ShellEccentricity ecc, double panelWidth)
        {
            Geometry.FdPoint3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            string type = "timber";
            string panelName = "A";
            double gap = 0.01;
            double orthotropy = 1;
            bool externalMovingLocal = true;

            return new Panel(region, anchorPoint, internalPanels, timberApplicationData, externalRigidity, type, identifier, panelName, gap, orthotropy, ecc, externalMovingLocal, panelWidth);
        }

        #region dynamo
        /// <summary>
        /// Create a profiled plate.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="eccentricity">ShellEccentricity. Optional.</param>
        /// <param name="orthoRatio">Transverse flexural stiffness factor.</param>
        /// <param name="edgeConnection">ShellEdgeConnection. Optional.</param>
        /// <param name="LocalX">"Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined."</param>
        /// <param name="LocalZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.</param>
        /// <param name="identifier">Average mesh size. If zero an automatic value will be used by FEM-Design. Optional.</param>
        /// <param name="identifier">Identifier.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Panel ProfiledPlate(Autodesk.DesignScript.Geometry.Surface surface, Materials.Material material, Sections.Section section, [DefaultArgument("ShellEccentricity.Default()")] ShellEccentricity eccentricity, [DefaultArgument("1")] double orthoRatio, [DefaultArgument("ShellEdgeConnection.Default()")] ShellEdgeConnection edgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, [DefaultArgument("0")] double avgMeshSize, string identifier = "PP")
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // create panel
            Panel obj = Panel.DefaultContreteContinuous(region, edgeConnection.Rigidity, material, section, identifier, orthoRatio, eccentricity);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.LocalX = FemDesign.Geometry.FdVector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.LocalZ = FemDesign.Geometry.FdVector3d.FromDynamo(localZ);
            }

            // set mesh
            obj.UniformAvgMeshSize = avgMeshSize;

            // return
            return obj;
        }

        ///<summary>
        /// Set camber simulation (by prestress) defining the prestress force and the related eccentricity
        ///</summary>
        ///<param name="panel">Panel.</param>
        ///<param name="force">Total prestress force in kN</param>
        ///<param name="eccentricity">Eccentricity of prestress force</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Panel SetCamberSimByPreStress(Panel panel, double force, double eccentricity)
        {
            // deep clone to create a new instance
            var obj = panel.DeepClone();

            // set camber of new instance
            obj.Camber = new Camber(force, eccentricity);

            // return new instance
            return obj;
        }
        #endregion
    }
}