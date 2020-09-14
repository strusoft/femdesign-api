// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// buckling_record
    /// </summary>
    [System.Serializable]
    public class BucklingLength
    {
        [XmlAttribute("type")]
        public string Type { get; set; } // bar_buckling_type
        [XmlAttribute("beta")]
        public double _beta; // non_neg_max_100
        [XmlIgnore]
        public double Beta
        {
            get {return this._beta;}
            set {this._beta = RestrictedDouble.NonNegMax_100(value);}
        } 
        [XmlAttribute("sway")]
        public bool Sway { get; set; } // boolean
        [XmlAttribute("load_position")]
        public string _loadPosition;  // ver_align
        [XmlIgnore]
        public string LoadPosition
        {
            get{return this._loadPosition;}
            set{this._loadPosition = RestrictedString.VerticalAlign(value);}
        }
        [XmlAttribute("continously_restrained")]
        public bool ContinouslyRestrained { get; set; } // bool
        [XmlAttribute("cantilever")]
        public bool Cantilever { get; set; } // bool
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
        internal BucklingLength(Position position, string type, double beta = 1, bool sway = false)
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
        internal BucklingLength(Position position, string type, double beta, string loadPosition, bool continouslyRestrained)
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
        internal BucklingLength(Position position, string type, string loadPosition, bool continouslyRestrained, bool cantilever)
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
        public static BucklingLength FlexuralStiffDefine(double beta = 1, bool sway = false)
        {
            string _type = "flexural_stiff";
            return new BucklingLength(Position.AlongBar(), _type, beta, sway);
        }
        /// <summary>
        /// Define BucklingLength in Flexural Weak direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        public static BucklingLength FlexuralWeakDefine(double beta = 1, bool sway = false)
        {
            string _type = "flexural_weak";
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
        public static BucklingLength PressuredTopFlangeDefine(double beta = 1, string loadPosition = "top", bool continuouslyRestrained = false)
        {
            string _type = "pressured_flange";
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
        public static BucklingLength PressuredBottomFlangeDefine(double beta = 1, string loadPosition = "top", bool continuouslyRestrained = false)
        {
            string _type = "pressured_bottom_flange";
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
        public static BucklingLength LateralTorsionalDefine(string loadPosition = "top", bool continouslyRestrained = false, bool cantilever = false)
        {
            string _type = "lateral_torsional";
            return new BucklingLength(Position.AlongBar(), _type, loadPosition, continouslyRestrained, cantilever);
        }
    }
}