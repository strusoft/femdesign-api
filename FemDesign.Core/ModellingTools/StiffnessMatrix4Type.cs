using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class StiffnessMatrix4Type
    {
        [XmlAttribute("xx")]
        public double _xx;
        [XmlIgnore]
        public double XX
        {
            get
            {
                return this._xx;
            }
            set
            {
                this._xx = RestrictedDouble.NonNegMax_1e20(value);
            }
        }
        
        [XmlAttribute("xy")]
        public double _xy;
        [XmlIgnore]
        public double XY
        {
            get
            {
                return this._xy;
            }
            set
            {
                this._xy = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        [XmlAttribute("yy")]
        public double _yy;
        [XmlIgnore]
        public double YY
        {
            get
            {
                return this._yy;
            }
            set
            {
                this._yy = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        [XmlAttribute("gxy")]
        public double _gxy;
        [XmlIgnore]
        public double GXY
        {
            get
            {
                return this._gxy;
            }
            set
            {
                this._gxy = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StiffnessMatrix4Type()
        {

        }

        /// <summary>
        /// Construct a stiffness matrix 4 type.
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="xy"></param>
        /// <param name="yy"></param>
        /// <param name="gxy"></param>
        public StiffnessMatrix4Type(double xx, double xy, double yy, double gxy)
        {
            this.XX = xx;
            this.XY = xy;
            this.YY = yy;
            this.GXY = gxy;
        }

    }
}