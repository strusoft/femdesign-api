using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class ConnectedPoints: EntityBase
    {
        [XmlIgnore]
        private static int _instance = 0;

        [XmlElement("point", Order = 1)]
        public Geometry.FdPoint3d[] _points;

        [XmlIgnore]
        public Geometry.FdPoint3d[] Points
        {
            get
            {
                return this._points;
            }
            set
            {
                if (value.Length == 2)
                {
                    this._points = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of points: {value.Length}, should be 2.");
                }
            }
        }

        [XmlElement("local_x", Order = 2)]
        public Geometry.FdVector3d LocalX { get; set; }

        [XmlElement("local_y", Order = 3)]
        public Geometry.FdVector3d LocalY { get; set; }
        
        // rigidity data choice

        [XmlElement("rigidity", Order = 4)]
        public Releases.RigidityDataType2 Rigidity { get; set; } 

        [XmlElement("predefined_rigidity", Order = 5)]
        public GuidListType _predefRigidityRef;

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

        // rigidity group choice

                
        [XmlElement("ref", Order = 6)]
        public GuidListType[] References { get; set; }

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
                ConnectedPoints._instance++;
                this._identifier = RestrictedString.Length(value, 50) + ConnectedPoints._instance.ToString();
            }
        }

        [XmlAttribute("interface")]
        public double _interface;

        [XmlIgnore]
        public double Interface
        {
            get
            {
                return this._interface;
            }
            set
            {
                this._interface = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private ConnectedPoints()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectedPoints(Geometry.FdPoint3d firstPoint, Geometry.FdPoint3d secondPoint, Releases.RigidityDataType2 rigidity, GuidListType[] references, string identifier)
        {
            this.EntityCreated();
            this.Points = new Geometry.FdPoint3d[2]
            {
                firstPoint,
                secondPoint
            };
            this.LocalX = Geometry.FdVector3d.UnitX();
            this.LocalY = Geometry.FdVector3d.UnitY();
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
        }

        #region dynamo

        [IsVisibleInDynamoLibrary(true)]
        public static ConnectedPoints Define(Autodesk.DesignScript.Geometry.Point firstPoint, Autodesk.DesignScript.Geometry.Point secondPoint, Releases.Motions motions, Releases.Rotations rotations, System.Guid[] references, string identifier)
        {
            // convert geometry
            Geometry.FdPoint3d p1 = Geometry.FdPoint3d.FromDynamo(firstPoint);
            Geometry.FdPoint3d p2 = Geometry.FdPoint3d.FromDynamo(secondPoint);

            // rigidity
            Releases.RigidityDataType2 rigidity = new Releases.RigidityDataType2(motions, rotations);

            // references
            GuidListType[] refs = new GuidListType[references.Length];
            for (int idx = 0; idx < refs.Length; idx++)
            {
                refs[idx] = new GuidListType(references[idx]);
            }

            return new ConnectedPoints(p1, p2, rigidity, refs, identifier);
        }
        #endregion 
    }
}
