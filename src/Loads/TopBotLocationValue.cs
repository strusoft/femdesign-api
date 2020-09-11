// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Loads
{
    /// <summary>
    /// topbottom_value
    /// </summary>
    [System.Serializable]
    public class TopBotLocationValue: Geometry.FdPoint3d
    {
        /// <summary>
        /// Top value
        /// </summary>
        [XmlAttribute("top_val")]
        public double _topVal;
        [XmlIgnore]
        public double TopVal
        {
            get { return this._topVal; }
            set { this._topVal = RestrictedDouble.AbsMax_1e20(value); }
        }

        /// <summary>
        /// Bottom value
        /// </summary>
        [XmlAttribute("bottom_val")]
        public double _bottomVal;
        [XmlIgnore]
        public double BottomVal
        {
            get { return this._bottomVal; }
            set { this._bottomVal = RestrictedDouble.AbsMax_1e20(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal TopBotLocationValue()
        {

        }

        /// <summary>
        /// Construct top bottom location value by point, top value and bottom value.
        /// </summary>
        public TopBotLocationValue(Geometry.FdPoint3d point, double topVal, double bottomVal)
        {
            this.x = point.x;
            this.y = point.y;
            this.z = point.z;
            this.TopVal = topVal;
            this.BottomVal = bottomVal;
        }

        /// <summary>
        /// Convert coordinates to FdPoint.
        /// </summary>
        internal Geometry.FdPoint3d GetFdPoint()
        {
            return new Geometry.FdPoint3d(this.x, this.y, this.z);
        }

    }
}