// https://strusoft.com/
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// buckling_record
    /// </summary>
    [System.Serializable]
    public partial class BucklingLength
    {
        [XmlAttribute("type")]
        public BucklingType Type { get; set; } // bar_buckling_type

        [XmlAttribute("beta")]
        public string _beta; // non_neg_max_100

        [XmlIgnore]
        public double Beta
        {
            get
            {
                return double.Parse(this._beta, CultureInfo.InvariantCulture);
            }
            set
            {
                this._beta = RestrictedDouble.NonNegMax_100(value).ToString(CultureInfo.InvariantCulture);
            }
        }

        [XmlAttribute("sway")]
        public string _sway;

        [XmlIgnore]
        public bool Sway
        {
            get
            {
                return bool.Parse(this._sway);
            }
            set
            {
                this._sway = value.ToString();
            }
        }

        [XmlAttribute("load_position")]
        public VerticalAlignment LoadPosition { get; set; }

        [XmlAttribute("continously_restrained")]
        public string _continouslyRestrained;
        
        [XmlIgnore]
        public bool ContinouslyRestrained
        {
            get
            {
                return bool.Parse(this._continouslyRestrained);
            }
            set
            {
                this._continouslyRestrained = value.ToString();
            }
        }

        [XmlAttribute("cantilever")]
        public string _cantilever;

        [XmlIgnore]
        public bool Cantilever
        {
            get
            {
                return bool.Parse(this._cantilever);
            }
            set
            {
                this._cantilever = value.ToString();
            }
        }

        [XmlElement("position")]
        public Position Position { get; set; } // segmentposition_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal BucklingLength()
        {
            
        }

        /// <summary>
        /// Constructor for flexural buckling length.
        /// </summary>
        internal BucklingLength(Position position, BucklingType type, double beta = 1, bool sway = false)
        {
            this.Position = position;
            this.Type = type;
            this.Beta = beta;
            if (sway)
            {
                this.Sway = sway;
            }   
        }

        /// <summary>
        /// Constructor for pressured flange buckling length.
        /// </summary>
        internal BucklingLength(Position position, BucklingType type, double beta, VerticalAlignment loadPosition, bool continouslyRestrained)
        {
            this.Position = position;
            this.Type = type;
            this.Beta = beta;
            this.LoadPosition = loadPosition;
            if (continouslyRestrained)
            {
                this.ContinouslyRestrained = continouslyRestrained;
            }
        }

        /// <summary>
        /// Constructor for lateral torsional buckling length.
        /// </summary>
        internal BucklingLength(Position position, BucklingType type, VerticalAlignment loadPosition, bool continouslyRestrained, bool cantilever)
        {
            this.Position = position;
            this.Type = type;
            this.LoadPosition = loadPosition;
            if (continouslyRestrained)
            {
                this.ContinouslyRestrained = continouslyRestrained;
                }
            if (cantilever)
            {
                this.Cantilever = cantilever;
            }
        }

        /// <summary>
        /// Define BucklingLength in Flexural Stiff direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        public static BucklingLength FlexuralStiff(double beta = 1, bool sway = false)
        {
            BucklingType _type = BucklingType.FlexuralStiff;
            return new BucklingLength(Position.AlongBar(), _type, beta, sway);
        }
        /// <summary>
        /// Define BucklingLength in Flexural Weak direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        public static BucklingLength FlexuralWeak(double beta = 1, bool sway = false)
        {
            BucklingType _type = BucklingType.FlexuralWeak;
            return new BucklingLength(Position.AlongBar(), _type, beta, sway);
        }
        /// <summary>
        /// Define BucklingLength for Pressured Top Flange.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continuouslyRestrained">Continuously restrained. True/false.</param>
        /// <returns></returns>
        public static BucklingLength PressuredTopFlange(VerticalAlignment loadPosition, double beta = 1, bool continuouslyRestrained = false)
        {
            BucklingType _type = BucklingType.PressuredTopFlange;
            return new BucklingLength(Position.AlongBar(), _type, beta, loadPosition, continuouslyRestrained);
        }
        /// <summary>
        /// Define BucklingLength for Pressured Bottom Flange.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continuouslyRestrained">Continuously restrained. True/false.</param>
        /// <returns></returns>
        public static BucklingLength PressuredBottomFlange(VerticalAlignment loadPosition, double beta = 1, bool continuouslyRestrained = false)
        {
            BucklingType _type = BucklingType.PressuredBottomFlange;
            return new BucklingLength(Position.AlongBar(), _type, beta, loadPosition, continuouslyRestrained);
        }
        /// <summary>
        /// Define BucklingLength for Lateral Torsional buckling.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continouslyRestrained">Continously restrained. True/false.</param>
        /// <param name="cantilever">Cantilever. True/false.</param>
        /// <returns></returns>
        public static BucklingLength LateralTorsional(VerticalAlignment loadPosition, bool continouslyRestrained = false, bool cantilever = false)
        {
            BucklingType _type = BucklingType.LateralTorsional;
            return new BucklingLength(Position.AlongBar(), _type, loadPosition, continouslyRestrained, cantilever);
        }
    }
}