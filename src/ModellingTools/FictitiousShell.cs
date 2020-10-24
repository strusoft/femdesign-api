using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class FictitiousShell: EntityBase
    {
        
        [XmlIgnore]
        private static int Instance = 0;

        [XmlIgnore]
        private Geometry.FdCoordinateSystem _coordinateSystem;

        [XmlIgnore]
        public Geometry.FdCoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem == null)
                {
                    this._coordinateSystem = new Geometry.FdCoordinateSystem(this.LocalPosition, this.LocalX, this.LocalY);
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
                this._localPosition = value.Origin;
                this._localX = value._localX;
                this._localY = value._localY;
            }

        }
        
        [XmlElement("region", Order=1)]
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
                this._region = value;
            }
        }
        
        [XmlElement("local_pos", Order=2)]
        public Geometry.FdPoint3d _localPosition;
        [XmlIgnore]
        public Geometry.FdPoint3d LocalPosition
        {
            get
            {
                return this._localPosition;
            }
            set
            {
                this.CoordinateSystem.Origin = value;
                this._localPosition = value;
            }
        }

        [XmlElement("local_x", Order=3)]
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

        [XmlElement("local_y", Order=4)]
        public Geometry.FdVector3d _localY;
        [XmlIgnore]
        public Geometry.FdVector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                this.CoordinateSystem.SetYAroundZ(value);
                this._localY = this.CoordinateSystem.LocalY;
            }
        }

        [XmlElement("membrane_stiffness", Order=5)]
        public StiffnessMatrix4Type MembraneStiffness { get; set; }

        [XmlElement("flexural_stiffness", Order=6)]
        public StiffnessMatrix4Type FlexuralStiffness { get; set; }

        [XmlElement("shear_stiffness", Order=7)]
        public StiffnessMatrix2Type ShearStiffness { get; set; }

        /// <summary>
        /// Identifier. Default FS.
        /// </summary>
        [XmlAttribute("name")]
        public string _identifier;

        [XmlIgnore]
        public string Identifier
        { 
            get
            {
                return this._identifier;
            }
            set
            {
                FictitiousShell.Instance++;
                this._identifier = RestrictedString.Length(value, 40) + "." + FictitiousShell.Instance.ToString();
            }
        }

        /// <summary>
        /// Density in t/m2
        /// </summary>
        [XmlAttribute("density")]
        public double _density;
        [XmlIgnore]
        public double Density
        {
            get
            {
                return this._density;
            }
            set
            {
                this._density = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// t1 in m
        /// </summary>
        [XmlAttribute("t1")]
        public double _t1;
        [XmlIgnore]
        public double T1
        {
            get
            {
                return this._t1;
            }
            set
            {
                this._t1 = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// t2 in m
        /// </summary>
        [XmlAttribute("t2")]
        public double _t2;
        [XmlIgnore]
        public double T2
        {
            get
            {
                return this._t2;
            }
            set
            {
                this._t2 = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// alfa_1 in 1/°C
        /// </summary>
        [XmlAttribute("alfa_1")]
        public double _alfa1;
        [XmlIgnore]
        public double Alpha1
        {
            get
            {
                return this._alfa1;
            }
            set
            {
                this._alfa1 = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// alfa_2 in 1/°C
        /// </summary>
        [XmlAttribute("alfa_2")]
        public double _alfa2;
        [XmlIgnore]
        public double Alpha2
        {
            get
            {
                return this._alfa2;
            }
            set
            {
                this._alfa2 = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Avg. mesh size in m
        /// </summary>
        [XmlAttribute("mesh_size")]
        public double _meshSize;
        [XmlIgnore]
        public double MeshSize
        {
            get
            {
                return this._meshSize;
            }
            set
            {
                this._meshSize = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Ignore stiffness in stability and imperfection calculation
        /// </summary>
        /// <value></value>
        [XmlAttribute("ignore_in_st_imp_calculation")]
        public bool IgnoreInStImpCalculation { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private FictitiousShell()
        {

        }

        /// <summary>
        /// Construct virtual shell
        /// </summary>
        /// <param name="region"></param>
        /// <param name="membraneStiffness"></param>
        /// <param name="flexuralStiffness"></param>
        /// <param name="shearStiffness"></param>
        /// <param name="density"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="alpha1"></param>
        /// <param name="alpha2"></param>
        /// <param name="ignoreInStImpCalc"></param>
        /// <param name="identifier"></param>
        public FictitiousShell(Geometry.Region region, StiffnessMatrix4Type membraneStiffness, StiffnessMatrix4Type flexuralStiffness, StiffnessMatrix2Type shearStiffness, double density, double t1, double t2, double alpha1, double alpha2, bool ignoreInStImpCalc, double meshSize, string identifier)
        {
            this.EntityCreated();
            this.Region = region;
            this.CoordinateSystem = region.CoordinateSystem;
            this.MembraneStiffness = membraneStiffness;
            this.FlexuralStiffness = flexuralStiffness;
            this.ShearStiffness = shearStiffness;
            this.Density = density;
            this.T1 = t1;
            this.T2 = t2;
            this.Alpha1 = alpha1;
            this.Alpha2 = alpha2;
            this.IgnoreInStImpCalculation = ignoreInStImpCalc;
            this.MeshSize = meshSize;
            this.Identifier = identifier;
        }

        // /// <summary>
        // /// Set ShellEdgeConnection by indices.
        // /// </summary>
        // /// <param name="fictShell">Fictitious Shell</param>
        // /// <param name="edgeConnection">ShellEdgeConnection</param>
        // /// <param name="indices">Index. List of items. Deconstruct fictitious shell to extract index for each respective edge.</param>
        // /// <returns></returns>
        // [IsVisibleInDynamoLibrary(true)]
        // public static FictitiousShell SetShellEdgeConnection(FictitiousShell fictShell, Shells.ShellEdgeConnection edgeConnection, List<int> indices)
        // {
        //     // deep clone. downstreams objs will contain changes made in this method, upstream objs will not.
        //     // downstream and uppstream objs will share guid.
        //     FictitiousShell clone = fictShell.DeepClone();

        //     foreach (int index in indices)
        //     {
        //         if (index >= 0 & index < fictShell.Region.GetEdgeConnections().Count)
        //         {
        //             // pass
        //         }
        //         else
        //         {
        //             throw new System.ArgumentException("Index is out of bounds.");
        //         }
                
        //         //
        //         clone.Region.SetEdgeConnection(edgeConnection, index);  

        //     }

        //     //
        //     return clone;
        // } 

        #region dynamo
        /// <summary>
        /// Define a fictitious shell
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <param name="d">Membrane stiffness matrix</param>
        /// <param name="k">Flexural stiffness matrix</param>
        /// <param name="h">Shear stiffness matrix</param>
        /// <param name="density">Density in t/m2</param>
        /// <param name="t1">t1 in m</param>
        /// <param name="t2">t2 in m</param>
        /// <param name="alpha1">Alpha1 in 1/°C</param>
        /// <param name="alpha2">Alpha2 in 1/°C</param>
        /// <param name="ignoreInStImpCalc">Ignore in stability/imperfection calculation</param>
        /// <param name="edgeConnection">ShellEdgeConnection. Optional, if rigid if undefined.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.</param>
        /// <param name="avgSrfElemSize">Finite element size. Set average surface element size. If set to 0 FEM-Design will automatically caculate the average surface element size.</param>
        /// <param name="identifier">Identifier.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static FictitiousShell Define(Autodesk.DesignScript.Geometry.Surface surface, StiffnessMatrix4Type d, StiffnessMatrix4Type k, StiffnessMatrix2Type h, double density, double t1, double t2, double alpha1, double alpha2, bool ignoreInStImpCalc, [DefaultArgument("ShellEdgeConnection.Default()")] Shells.ShellEdgeConnection edgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, double avgSrfElemSize = 0, string identifier = "FS")
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.FdVector3d x = Geometry.FdVector3d.FromDynamo(localX);
            Geometry.FdVector3d z = Geometry.FdVector3d.FromDynamo(localZ);

            // add edge connections to region
            region.SetEdgeConnections(edgeConnection);

            //
            FictitiousShell obj = new FictitiousShell(region, d, k, h, density, t1, t2, alpha1, alpha2, ignoreInStImpCalc, avgSrfElemSize, identifier);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                var cs = obj.CoordinateSystem;
                cs.SetXAroundZ(FemDesign.Geometry.FdVector3d.FromDynamo(localX));
                obj.CoordinateSystem = cs;
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                var cs = obj.CoordinateSystem;
                cs.SetZAroundX(FemDesign.Geometry.FdVector3d.FromDynamo(localZ));
                obj.CoordinateSystem = cs;
            }

            // return
            return obj;
        }
        #endregion
    }
}