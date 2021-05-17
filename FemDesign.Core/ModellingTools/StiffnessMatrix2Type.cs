using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class StiffnessMatrix2Type
    {
        [XmlAttribute("xz")]
        public double _xz;
        [XmlIgnore]
        public double XZ
        {
            get
            {
                return this._xz;
            }
            set
            {
                this._xz = RestrictedDouble.NonNegMax_1e20(value);
            }
        }
        
        [XmlAttribute("yz")]
        public double _yz;
        [XmlIgnore]
        public double YZ
        {
            get
            {
                return this._yz;
            }
            set
            {
                this._yz = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StiffnessMatrix2Type()
        {

        }

        /// <summary>
        /// Construct a stiffness matrix 2 type
        /// </summary>
        /// <param name="xz"></param>
        /// <param name="yz"></param>
        public StiffnessMatrix2Type(double xz, double yz)
        {
            this.XZ = xz;
            this.YZ = yz;
        }

    }
}