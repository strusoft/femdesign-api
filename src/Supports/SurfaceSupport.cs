// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Supports
{
    [System.Serializable]
    public class SurfaceSupport: EntityBase
    {
        [XmlAttribute("name")]
        public string _name;
        [XmlIgnore]
        public string Identifier
        {
            get
            {
                return this._name;
            }
            set
            {
                PointSupport._instance++;
                this._name = value + "." + PointSupport._instance.ToString();
            }
        }
        [XmlElement("region", Order=1)]
        public Geometry.Region Region { get; set; }
        [XmlElement("rigidity", Order=2)]
        public Releases.RigidityDataType1 Rigidity { get; set; }

        [XmlElement("local_system", Order=3)]
        public Geometry.FdCoordinateSystem CoordinateSystem { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private SurfaceSupport()
        {

        }

        /// <summary>
        /// Internal constructor
        /// </summary>
        public SurfaceSupport(Geometry.Region region, Releases.RigidityDataType1 rigidity, string identifier)
        {
            this.EntityCreated();
            this.Identifier = identifier;
            this.Region = region;
            this.Rigidity = rigidity;
            this.CoordinateSystem = region.CoordinateSystem;
        }

        /// <summary>
        /// Internal constructor with only translation rigidity defined
        /// </summary>
        public SurfaceSupport(Geometry.Region region, Releases.Motions motions, string identifier)
        {
            this.EntityCreated();
            this.Identifier = identifier;
            this.Region = region;
            this.Rigidity = new Releases.RigidityDataType1(motions);
            this.CoordinateSystem = region.CoordinateSystem;
        }

    }
}