// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Bars.Buckling
{
    /// <summary>
    /// segmentposition_type
    /// </summary>
    [System.Serializable]
    public partial class Position
    {
        [XmlAttribute("start")]
        public double _start; // position_type
        [XmlIgnore]
        public double Start
        {
            get { return this._start; }
            set { this._start = RestrictedDouble.PositionType(value); }
        }
        [XmlAttribute("end")]
        public double _end; // position_type
        [XmlIgnore]
        public double End
        {
            get { return this._end; }
            set { this._end = RestrictedDouble.PositionType(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Position()
        {

        }

        private Position(double start, double end)
        {
            if (start > end)
            {
                throw new System.ArgumentException("start cannot be larger than end");
            }
            else
            {
                this.Start = start;
                this.End = end;
            }
        }
        
        /// <summary>
        /// Create a definition of buckling length along the full length of the bar. 
        /// </summary>
        /// <remarks>Private</remarks>
        /// <returns></returns>
        public static Position AlongBar()
        {
            return new Position(0, 1);
        }
        /// <summary>
        /// Create a definition of buckling length along the bar. Start and end by parameter.
        /// </summary>
        /// <remarks>Private</remarks>
        /// <param name="startParameter">Start of buckling length. Value equal to or between 0 and 1. 0 = start of bar, 1 = end of bar.</param>
        /// <param name="endParameter">End of buckling length. Value equal to or between 0 and 1. 0 = start of bar, 1 = end of bar</param>
        public static Position ByParameters(double startParameter, double endParameter)
        {
            return new Position(startParameter, endParameter);
        }
    }
}