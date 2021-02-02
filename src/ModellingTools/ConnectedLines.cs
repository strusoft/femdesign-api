using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ConnectedLines: EntityBase
    {
        [XmlIgnore]
        private static int _instance = 0;

        [XmlElement("edge" , Order = 1)]
        public Geometry.Edge[] Edges { get; set; }

        /// <summary>
        /// This property is optional.
        /// Represents first interface point, i.e Point@Parameter on Line between Edge[0].StartPoint and Edge[1].StartPoint.
        /// </summary>
        [XmlElement("point", Order = 2)]
        public Geometry.FdPoint3d[] Points { get; set; }

        [XmlElement("local_x", Order = 3)]
        public Geometry.FdVector3d LocalX { get; set; }

        [XmlElement("local_y", Order = 4)]
        public Geometry.FdVector3d LocalY { get; set; }

        // simple stiffness choice

        // rigidity data choice
        [XmlElement("rigidity", Order = 5)]
        public Releases.RigidityDataType3 Rigidity { get; set; } 

        [XmlElement("predefined_rigidity", Order = 6)]
        public GuidListType _predefRigidityRef;

        [XmlIgnore]
        public Releases.RigidityDataLibType3 _predefRigidity;

        [XmlIgnore]
        public Releases.RigidityDataLibType3 PredefRigidity
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

        
        [XmlElement("ref", Order = 7)]
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
                ConnectedLines._instance++;
                this._identifier = RestrictedString.Length(value, 50) + ConnectedLines._instance.ToString();
            }
        }

        [XmlAttribute("moving_local")]
        public bool MovingLocal { get; set; }

        [XmlIgnore]
        private double[] _interface = new double[2]{ 0.5, 0.5 };

        [XmlAttribute("interface_start")]
        public double InterfaceStart
        {
            get
            {
                return this._interface[0];
            }
            set
            {
                this._interface[0] = RestrictedDouble.NonNegMax_1(value);
            }
        }

        [XmlAttribute("interface_end")]
        public double InterfaceEnd
        {
            get
            {
                return this._interface[1];
            }
            set
            {
                this._interface[1] = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private ConnectedLines()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.FdVector3d localX, Geometry.FdVector3d localY, Releases.RigidityDataType3 rigidity, GuidListType[] references, string identifier, bool movingLocal, double interfaceStart, double interfaceEnd)
        {
            this.EntityCreated();
            this.Edges[0] = firstEdge;
            this.Edges[1] = secondEdge;
            this.LocalX = localX;
            this.LocalY = localY;
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
            this.MovingLocal = movingLocal;
            this.InterfaceStart = interfaceStart;
            this.InterfaceEnd = interfaceEnd;
        }

        #region dynamo

        [IsVisibleInDynamoLibrary(true)]
        public static ConnectedLines Define(Autodesk.DesignScript.Geometry.Curve firstCurve, Autodesk.DesignScript.Geometry.Curve secondCurve, Autodesk.DesignScript.Geometry.Vector localX, Autodesk.DesignScript.Geometry.Vector localY, Releases.Motions motions, Releases.Rotations rotations, System.Guid[] references, string identifier, bool movingLocal, double interfaceStart, double interfaceEnd)
        {
            // convert geometry
            Geometry.Edge edge0 = Geometry.Edge.FromDynamoLineOrArc2(firstCurve);
            Geometry.Edge edge1 = Geometry.Edge.FromDynamoLineOrArc2(secondCurve);
            Geometry.FdVector3d x = Geometry.FdVector3d.FromDynamo(localX);
            Geometry.FdVector3d y = Geometry.FdVector3d.FromDynamo(localY);

            // rigidity
            Releases.RigidityDataType3 rigidity = new Releases.RigidityDataType3(motions, rotations);

            // references
            GuidListType[] refs = new GuidListType[references.Length];
            for (int idx = 0; idx < refs.Length; idx++)
            {
                refs[idx] = new GuidListType(references[idx]);
            }

            return new ConnectedLines(edge0, edge1, x, y, rigidity, refs, identifier, movingLocal, interfaceStart, interfaceEnd);
        }
        #endregion 

    }
}
