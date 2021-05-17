using System.Xml.Serialization;


namespace FemDesign
{
    [System.Serializable()]
    public partial class MechProps
    {  
        [XmlIgnore]
        public string _material;
        
        [XmlAttribute("material")]
        public string Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = RestrictedString.Length(value, 256);
            }
        }

        [XmlIgnore]
        public double _thickness;

        [XmlAttribute("thickness")]
        public double Thicknesss
        {
            get
            {
                return this._thickness;
            }
            set
            {
                this._thickness = RestrictedDouble.NonNegMax_1(value);
            }
        }

        [XmlAttribute("theta")]
        public double Theta { get; set; }

        [XmlAttribute("Ex")]
        public double Ex { get; set; }

        [XmlAttribute("Ey")]
        public double Ey { get; set; }

        [XmlAttribute("nuxy")]
        public double Nuxy { get; set; }

        [XmlAttribute("Gxy")]
        public double Gxy { get; set; }

        [XmlAttribute("Gxz")]
        public double Gxz { get; set; }

        [XmlAttribute("Gyz")]
        public double Gyz { get; set; }

        [XmlAttribute("rho")]
        public double Rho { get; set; }

        public MechProps()
        {

        }
    }
    
    
}