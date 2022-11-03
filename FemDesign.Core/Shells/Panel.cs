
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Shells
{
    /// <summary>
    /// panel_type
    /// </summary>
    [System.Serializable]
    public partial class Panel: NamedEntityBase, IStructureElement, IStageElement
    {
        /// <summary>
        /// Panel instance counter
        /// </summary>
        private static int _panelInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_panelInstances;

        /// <summary>
        /// Coordinate system
        /// </summary>
        [XmlIgnore]
        private Geometry.CoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.CoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem == null)
                {
                    this._coordinateSystem = new Geometry.CoordinateSystem(this.LocalOrigin, this.LocalX, this.LocalZ.Cross(this.LocalX));
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
        public Geometry.Point3d _localOrigin;
        [XmlIgnore]
        public Geometry.Point3d LocalOrigin
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
        public Geometry.Vector3d _localX;
        [XmlIgnore]
        public Geometry.Vector3d LocalX
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
        public Geometry.Vector3d LocalY
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
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Region.LocalZ;
            }
            set
            {
                this.Region.LocalZ = value;
                
                foreach (InternalPanel intPanel in this.InternalPanels.IntPanels)
                {
                    intPanel.Region.LocalZ = value;
                }
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
        public Geometry.Point3d AnchorPoint { get; set; }

        [XmlElement("internal_panels", Order = 5)]
        public InternalPanels InternalPanels { get; set; }

        /// <summary>
        /// This property describes the material of the panel (if the panel is a timber panel).
        /// The PanelLibraryData referenced in this property can be either Orthotropic, CLT or GLC.
        /// </summary>
        [XmlElement("timber_application_data", Order = 6)]
        public Materials.TimberPanelType TimberPanelData { get; set; }

        /// <summary>
        /// This is a placeholder for the PanelLibraryData which is referenced in TimberPanelData
        /// </summary>
        [XmlIgnore]
        public Materials.OrthotropicPanelLibraryType OrthotropicPanelLibraryData { get; set; }

        /// <summary>
        /// This is a placeholder for the PanelLibraryData which is referenced in TimberPanelData
        /// </summary>
        [XmlIgnore]
        public Materials.CltPanelLibraryType CltPanelLibraryData { get; set; }

        /// <summary>
        /// This is a placeholder for the PanelLibraryData which is referenced in TimberPanelData
        /// </summary>
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
        public PanelType Type { get; set; }

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
        public VerticalAlignment Alignment { get; set; }
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
                this._panelWidth = RestrictedDouble.NonNegMax_100(value).ToString(CultureInfo.InvariantCulture);
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

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;


        [XmlAttribute("ignored_distance")]
        public double _ignoredDistance = 0.02;

        [XmlIgnore]
        public double IgnoredDistance
        {
            get
            {
                return this._ignoredDistance;
            }
            set
            {
                this._ignoredDistance = RestrictedDouble.NonNegMax_1000(value);
            }
        }

        [XmlAttribute("ignored_in_stability")]
        public bool IgnoredInStability { get; set; } = false;

        /// <summary>
        /// Set external edge connections (i.e. set edge connections around region). 
        /// When this is performed for panels the external rigidity is changed accordingly.
        /// </summary>
        /// <param name="ec">EdgeConnection</param>
        public void SetExternalEdgeConnections(EdgeConnection ec)
        {
            // set the edge connections of the external edges of the internal panels
            if (this.InternalPanels.IntPanels.Count == 1)
            {
                this.InternalPanels.IntPanels[0].Region.SetEdgeConnections(ec);
            }
            else
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }


            // set external rigidity property to reflect the majority of edge connections of the region
            if (ec.Rigidity != null)
                this.ExternalRigidity = ec.Rigidity;
            else if (ec.PredefRigidity != null)
                this.ExternalPredefinedRigidity = ec.PredefRigidity;
        }

        /// <summary>
        /// Set external edge connection at index (i.e. set edge connections at specific edge of region).
        /// </summary>
        /// <param name="ec">EdgeConnection</param>
        /// <param name="index">Index of edge to set at region of first (and only) internal panel.</param>
        public void SetExternalEdgeConnectionAtIndexForContinousAnalyticalModel(EdgeConnection ec, int index)
        {
            if (this.InternalPanels.IntPanels.Count != 1)
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }

            if (index >= 0 & index < this.InternalPanels.IntPanels[0].Region.GetEdgeConnections().Count)
            {
                this.InternalPanels.IntPanels[0].Region.SetEdgeConnection(ec, index);
            }
            else
            {
                throw new System.ArgumentException("Index is out of bounds.");
            }
        }

        /// <summary>
        /// Set external edge connection at indices (i.e. set edge connections at specific edges of region).
        /// </summary>
        /// <param name="shellEdgeConnection">EdgeConnection.</param>
        /// <param name="indices">Index. List of items</param>
        public void SetExternalEdgeConnectionsForContinuousAnalyticalModel(EdgeConnection shellEdgeConnection, List<int> indices)
        {
            if (this.InternalPanels.IntPanels.Count != 1)
            {
                throw new System.ArgumentException("Can't set external edge connections for panels with more than 1 internal panel (i.e. can only set external edge conncetions for panels with a continuous analytical model)");
            }

            foreach (int index in indices)
            {
                if (index >= 0 & index < this.InternalPanels.IntPanels[0].Region.GetEdgeConnections().Count)
                {
                    // pass
                }
                else
                {
                    throw new System.ArgumentException("Index is out of bounds.");
                }
                
                //
                this.SetExternalEdgeConnectionAtIndexForContinousAnalyticalModel(shellEdgeConnection, index);  
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
        /// <param name="anchorPoint"></param>
        /// <param name="type">Type of panel.</param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="panelName">Name of panel.</param>
        /// <param name="gap">Gap between panels.</param>
        /// <param name="orthotropy">Orthotropy.</param>
        /// <param name="ecc">ShellEccentricity.</param>
        /// <param name="externalMovingLocal">EdgeConnection LCS changes along edge?</param>
        /// <param name="internalPanels"></param>
        /// <param name="externalEdgeConnection"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        internal Panel(Geometry.Region region, Geometry.Point3d anchorPoint, InternalPanels internalPanels, EdgeConnection externalEdgeConnection, PanelType type, Materials.Material material, Sections.Section section, string identifier, string panelName, double gap, double orthotropy, ShellEccentricity ecc, bool externalMovingLocal)
        {
            this.EntityCreated();

            // elements
            this.Region = region;
            this.CoordinateSystem = region.CoordinateSystem;
            this.AnchorPoint = anchorPoint;
            this.InternalPanels = internalPanels;
            this.ExternalRigidity = externalEdgeConnection.Rigidity;

            // set edge connections
            this.SetExternalEdgeConnections(externalEdgeConnection);

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
        /// <param name="anchorPoint"></param>
        /// <param name="internalPanels"></param>
        /// <param name="externalEdgeConnection">Default value for shell border EdgeConnections. Can be overwritten by EdgeConnection for each specific edge in Region.</param>
        /// <param name="timberApplicationData"></param>
        /// <param name="type">Type of panel.</param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="panelName">Name of panel.</param>
        /// <param name="gap">Gap between panels.</param>
        /// <param name="orthotropy">Orthotropy.</param>
        /// <param name="ecc">ShellEccentricity.</param>
        /// <param name="externalMovingLocal">EdgeConnection LCS changes along edge?</param>
        /// <param name="panelWidth"></param>
        internal Panel(Geometry.Region region, Geometry.Point3d anchorPoint, InternalPanels internalPanels, Materials.TimberPanelType timberApplicationData, EdgeConnection externalEdgeConnection, PanelType type, string identifier, string panelName, double gap, double orthotropy, ShellEccentricity ecc, bool externalMovingLocal, double panelWidth)
        {
            this.EntityCreated();

            // elements
            this.Region = region;
            this.CoordinateSystem = region.CoordinateSystem;
            this.AnchorPoint = anchorPoint;
            this.InternalPanels = internalPanels;
            this.TimberPanelData = timberApplicationData;

            // set external rigidity
            this.SetExternalEdgeConnections(externalEdgeConnection);

            // set internal rigidity - not relevant for a panel with continuous analytical model

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
        /// <param name="region">Panel region.</param>
        /// <param name="externalEdgeConnection"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="orthotropy"></param>
        /// <param name="ecc"></param>
        /// <returns></returns>
        public static Panel DefaultContreteContinuous(Geometry.Region region, EdgeConnection externalEdgeConnection, Materials.Material material, Sections.Section section, string identifier, double orthotropy, ShellEccentricity ecc)
        {
            Geometry.Point3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            PanelType type = PanelType.Concrete;
            string panelName = "A";
            double gap = 0.003;
            bool externalMovingLocal = externalEdgeConnection.MovingLocal;
            
            return new Panel(region, anchorPoint, internalPanels, externalEdgeConnection, type, material, section, identifier, panelName, gap, orthotropy, ecc, externalMovingLocal);
        }

        /// <summary>
        /// Create a default timber shell with panels using a continuous analytical model.
        /// </summary>
        /// <param name="region">Panel region.</param>
        /// <param name="timberPlateMaterial">Timber material. See <see cref="FemDesign.Materials.TimberPanelType"/>.</param>
        /// <param name="direction">Timber panel span direction.</param>
        /// <param name="externalEdgeConnection"></param>
        /// <param name="identifier">Name of shell.</param>
        /// <param name="eccentricity"></param>
        /// <param name="panelWidth"></param>
        /// <returns></returns>
        public static Panel DefaultTimberContinuous(Geometry.Region region, Materials.TimberPanelType timberPlateMaterial, Geometry.Vector3d direction, EdgeConnection externalEdgeConnection = null, string identifier = "TP", ShellEccentricity eccentricity = null, double panelWidth = 1.5)
        {
            if (externalEdgeConnection == null)
                externalEdgeConnection = EdgeConnection.Default;

            if (eccentricity == null)
                eccentricity = ShellEccentricity.Default;
            
            Geometry.Point3d anchorPoint = region.Contours[0].Edges[0].Points[0];
            InternalPanel internalPanel = new InternalPanel(region);
            InternalPanels internalPanels = new InternalPanels(internalPanel);
            PanelType type = PanelType.Timber;
            string panelName = "A";
            double gap = 0.01;
            double orthotropy = 1;
            bool externalMovingLocal = externalEdgeConnection.MovingLocal;

            var panel = new Panel(region, anchorPoint, internalPanels, timberPlateMaterial, externalEdgeConnection, type, identifier, panelName, gap, orthotropy, eccentricity, externalMovingLocal, panelWidth);

            panel.LocalX = direction; // Set timber panel span direction
            
            return panel;
        }
    }
}