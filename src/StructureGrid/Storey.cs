using System.Xml.Serialization;


namespace FemDesign.StructureGrid
{
    public class Storey: EntityBase
    {
        [XmlElement("origo", Order=1)]
        public Geometry.FdPoint3d origo { get; set; }
        [XmlElement("direction", Order=2)]
        public Geometry.FdVector2d _direction;
        [XmlIgnore]
        public Geometry.FdVector3d direction
        {
            get
            {
                return this._direction.To3d();
            }
            set
            {
                this._direction = RestrictedObject.NonZeroFdVector2d(value.Normalize().To2d());
            }
        }

        [XmlAttribute("dimension_x")]
        public double _dimensionX; // positive double
        [XmlIgnore]
        public double dimensionX
        {
            get { return this._dimensionX; }
            set { this._dimensionX = RestrictedDouble.Positive(value); }
        }
        [XmlAttribute("dimension_y")]
        public double _dimensionY; // positive double
        [XmlIgnore]
        public double dimensionY
        {
            get { return this._dimensionY; }
            set { this._dimensionY = RestrictedDouble.Positive(value); }
        }
        [XmlAttribute("name")]
        public string name { get; set; }
        
        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private Storey()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="origo">Origo of storey.</param>
        /// <param name="direction">Direction of storey x'-axis. Optional, default value isGCS x-axis.</param>
        /// <param name="dimensionX">Dimension in x'-direction.</param>
        /// <param name="dimensionY">Dimension in y'-direction.</param>
        /// <param name="name">Name of storey.</param>
        internal Storey(Geometry.FdPoint3d origo, Geometry.FdVector3d direction, double dimensionX, double dimensionY, string name)
        {
            this.EntityCreated();
            this.origo = origo;
            this.direction = direction;
            this.dimensionX = dimensionX;
            this.dimensionY = dimensionY;
            this.name = name;
        }

    }
}