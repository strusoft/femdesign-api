// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// buckling_record
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class BucklingLength
    {
        [XmlAttribute("type")]
        public string type { get; set; } // bar_buckling_type
        [XmlAttribute("beta")]
        public double _beta; // non_neg_max_100
        [XmlIgnore]
        public double beta
        {
            get {return this._beta;}
            set {this._beta = RestrictedDouble.NonNegMax_100(value);}
        } 
        [XmlAttribute("sway")]
        public bool sway { get; set; } // boolean
        [XmlAttribute("load_position")]
        public string _loadPosition;  // ver_align
        [XmlIgnore]
        public string loadPosition
        {
            get{return this._loadPosition;}
            set{this._loadPosition = RestrictedString.VerticalAlign(value);}
        }
        [XmlAttribute("continously_restrained")]
        public bool continouslyRestrained { get; set; } // bool
        [XmlAttribute("cantilever")]
        public bool cantilever { get; set; } // bool
        [XmlElement("position")]
        public Position position { get; set; } // segmentposition_type

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
            this.position = position;
            this.type = type;
            this.beta = beta;
            if (sway)
            {
                this.sway = sway;
            }   
        }

        /// <summary>
        /// Constructor for pressured flange buckling length.
        /// </summary>
        internal BucklingLength(Position position, string type, double beta, string loadPosition, bool continouslyRestrained)
        {
            this.position = position;
            this.type = type;
            this.beta = beta;
            this.loadPosition = loadPosition;
            if (continouslyRestrained)
            {
                this.continouslyRestrained = continouslyRestrained;
            }
        }

        /// <summary>
        /// Constructor for lateral torsional buckling length.
        /// </summary>
        internal BucklingLength(Position position, string type, string loadPosition, bool continouslyRestrained, bool cantilever)
        {
            this.position = position;
            this.type = type;
            this.loadPosition = loadPosition;
            if (continouslyRestrained)
            {
                this.continouslyRestrained = continouslyRestrained;
                }
            if (cantilever)
            {
                this.cantilever = cantilever;
            }
        }
        /// <summary>
        /// Define BucklingLength in Flexural Stiff direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
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
        [IsVisibleInDynamoLibrary(true)]
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
        [IsVisibleInDynamoLibrary(true)]
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
        [IsVisibleInDynamoLibrary(true)]
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
        [IsVisibleInDynamoLibrary(true)]
        public static BucklingLength LateralTorsionalDefine(string loadPosition = "top", bool continouslyRestrained = false, bool cantilever = false)
        {
            string _type = "lateral_torsional";
            return new BucklingLength(Position.AlongBar(), _type, loadPosition, continouslyRestrained, cantilever);
        }
    }
}