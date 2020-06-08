// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    /// <summary>
    /// location_value_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class LocationValue: Geometry.FdPoint3d
    {
        /// <summary>
        /// Value.
        /// </summary>
        [XmlAttribute("val")]
        public double _val;
        [XmlIgnore]
        public double val
        {
            get { return this._val; }
            set { this._val = RestrictedDouble.AbsMax_1e20(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal LocationValue()
        {

        }

        /// <summary>
        /// Convert coordinates of LocationValue to FdPoint.
        /// </summary>
        internal Geometry.FdPoint3d GetFdPoint()
        {
            return new Geometry.FdPoint3d(this.x, this.y, this.z);
        }
    }
}