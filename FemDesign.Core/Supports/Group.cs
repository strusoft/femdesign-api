// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Supports
{
    /// <summary>
    /// support_rigidity_data_type --> group
    /// </summary>
    [System.Serializable]
    public partial class Group
    {
        [XmlIgnore]
        private Geometry.FdCoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.FdCoordinateSystem CoordinateSystem
        {
            get
            {
                // if deserialized from file the cooridnate system element does not exist and has to be created from local x and local y fields.
                if (this._coordinateSystem == null)
                {
                    this._coordinateSystem = new Geometry.FdCoordinateSystem(Geometry.FdPoint3d.Origin(), this._localX, this._localY);
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
                this._localX = value.LocalX;
                this._localY = value.LocalY;
            }
        }

        [XmlElement("local_x", Order = 1)]
        public Geometry.FdVector3d _localX;

        [XmlIgnore]
        public Geometry.FdVector3d LocalX
        {
            get
            {
                return this._localX;
            }
        }

        [XmlElement("local_y", Order = 2)]
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
                this.CoordinateSystem.SetYAroundX(value);
                this._localY = this.CoordinateSystem.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.FdVector3d LocalZ
        {
            get
            {
                return this.CoordinateSystem.LocalZ;
            }
        }

        [XmlElement("rigidity", Order = 3)]
        public Releases.RigidityDataType2 Rigidity { get; set; }

        [XmlElement("predefined_rigidity", Order = 4)]
        public GuidListType _predefRigidityRef; // reference_type

        [XmlIgnore]
        public Releases.RigidityDataLibType2 _predefRigidity;

        [XmlIgnore]
        public Releases.RigidityDataLibType2 PredefRigidity
        {
            get
            {
                return this._predefRigidity;
            }
            set
            {
                this._predefRigidity = value;
                this._predefRigidityRef = new GuidListType(value.Guid);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Group()
        {
            
        }

        /// <summary>
        /// Constructor by edge and rigidity. Used to create group for line support
        /// </summary>
        internal Group(Geometry.FdCoordinateSystem coordSystem, Releases.Motions motions, Releases.Rotations rotations)
        {
            this.CoordinateSystem = coordSystem;
            this.Rigidity = Releases.RigidityDataType3.Define(motions, rotations);
        }

        /// <summary>
        /// Constructor by vectors and rigidity. Used to create group for point support
        /// </summary>
        public Group(Geometry.FdVector3d localX, Geometry.FdVector3d localY, Releases.Motions motions, Releases.Rotations rotations)
        {
            this._localX = localX;
            this._localY = localY;
            this.Rigidity = Releases.RigidityDataType3.Define(motions, rotations);
        }

        /// <summary>
        /// Orient this object's coordinate system to GCS
        /// <summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.CoordinateSystem;
            cs.OrientEdgeTypeLcsToGcs();
            this.CoordinateSystem = cs;
        }

    }
}