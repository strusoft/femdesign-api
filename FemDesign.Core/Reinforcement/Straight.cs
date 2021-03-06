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
        public string _direction; // direction_type. Required
        [XmlIgnore]
        public string Direction
        {
            get {return this._direction;}
            set {this._direction = RestrictedString.DirectionType(value);}
        }
        [XmlAttribute("space")]
        public double _space; // positive_double. Spacing in meters. Required
        [XmlIgnore]
        public double Space
        {
            get {return this._space;}
            set {this._space = RestrictedDouble.Positive(value);}
        }
        [XmlAttribute("face")]
        public string _face; // sf_rc_face. Optional
        [XmlIgnore]
        public string Face
        {
            get {return this._face;}
            set {this._face = RestrictedString.SfRcFace(value);}
        }
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
        public Straight(string direction, double space, string face, double cover)
        {
            this.Direction = direction;
            this.Space = space;
            this.Face = face;
            this.Cover = cover;
        }
    }
}