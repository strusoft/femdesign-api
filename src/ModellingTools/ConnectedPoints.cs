using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public class ConnectedPoints: EntityBase
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

        private ConnectedPoints()
        {

        }
    }
}
