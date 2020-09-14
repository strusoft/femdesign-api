using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.StructureGrid
{
    [IsVisibleInDynamoLibrary(false)]
    public class Storey: EntityBase
    {
        [XmlElement("origo", Order=1)]
        public Geometry.FdPoint3d Origo { get; set; }
        [XmlElement("direction", Order=2)]
        public Geometry.FdVector2d _direction;
        [XmlIgnore]
        public Geometry.FdVector3d Direction
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
        public double DimensionX
        {
            get { return this._dimensionX; }
            set { this._dimensionX = RestrictedDouble.Positive(value); }
        }
        [XmlAttribute("dimension_y")]
        public double _dimensionY; // positive double
        [XmlIgnore]
        public double DimensionY
        {
            get { return this._dimensionY; }
            set { this._dimensionY = RestrictedDouble.Positive(value); }
        }
        [XmlAttribute("name")]
        public string Name { get; set; }
        
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
            this.Origo = origo;
            this.Direction = direction;
            this.DimensionX = dimensionX;
            this.DimensionY = dimensionY;
            this.Name = name;
        }

        #region dynamo
        /// <summary>
        /// Create a storey.
        /// </summary>
        /// <param name="origo">Origo of storey. Storeys can only have unique Z-coordinates. If several storeys are placed in a model their origos should share XY-coordinates.</param>
        /// <param name="direction">Direction of storey x'-axis in the XY-plane. If several storeys are placed in a model their direction should be identical. Optional, default value is GCS x-axis.</param>
        /// <param name="dimensionX">Dimension in x'-direction.</param>
        /// <param name="dimensionY">Dimension in y'-direction.</param>
        /// <param name="name">Name of storey.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Storey Define(string name, Autodesk.DesignScript.Geometry.Point origo, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.XAxis()")] Autodesk.DesignScript.Geometry.Vector direction, double dimensionX = 50, double dimensionY = 30)
        {
            // convert geometry
            Geometry.FdPoint3d p = Geometry.FdPoint3d.FromDynamo(origo);
            Geometry.FdVector3d v = Geometry.FdVector3d.FromDynamo(direction);

            // return
            return new Storey(p, v, dimensionX, dimensionY, name);
        }
        #endregion
    }
}