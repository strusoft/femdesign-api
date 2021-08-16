// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// straight (child of surface_rf_type)
    /// </summary>
    [System.Serializable]
    public partial class Straight
    {
        [XmlAttribute("direction")]
        public ReinforcementDirection Direction { get; set; }
        [XmlAttribute("space")]
        public double _space; // positive_double. Spacing in meters. Required
        [XmlIgnore]
        public double Space
        {
            get {return this._space;}
            set {this._space = RestrictedDouble.Positive(value);}
        }
        [XmlAttribute("face")]
        public GenericClasses.Face Face { get; set; }
        [XmlAttribute("cover")]
        public double _cover; // positive_double. Default = 0.02
        [XmlIgnore]
        public double Cover
        {
            get {return this._cover;}
            set {this._cover = RestrictedDouble.Positive(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Straight()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        public Straight(ReinforcementDirection direction, double space, GenericClasses.Face face, double cover)
        {
            this.Direction = direction;
            this.Space = space;
            this.Face = face;
            this.Cover = cover;
        }
    }
}