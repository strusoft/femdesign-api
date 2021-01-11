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

        [XmlElement("point", Order = 2)]
        public Geometry.FdPoint3d[] Points { get; set; }

        [XmlElement("local_x", Order = 3)]
        public Geometry.FdVector3d LocalX { get; set; }

        [XmlElement("local_y", Order = 4)]
        public Geometry.FdVector3d LocalY { get; set; }

        // simple stiffness choice

        // rigidity data choice
        [XmlElement("rigidity", Order = 5)]
        public Releases.RigidityDataType2 Rigidity { get; set; } 

        [XmlElement("predefined_rigidity", Order = 6)]
        public GuidListType _predefRigidityRef;

        [XmlIgnore]
        public LineConnectionTypes.PredefinedType _predefRigidity;

        [XmlIgnore]
        public LineConnectionTypes.PredefinedType PredefRigidity
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

    }
}
