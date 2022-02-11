using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LongitudinalBar
    {
        /// <summary>
        /// Add longitudinal reinforcement to a bar. Curved bars are not supported.
        /// </summary>
        /// <param name="wire">Longitudinal rebar material and type.</param>
        /// <param name="yPos">Y-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="zPos">Z-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="startAnchorage">Measure representing start anchorage of longitudinal rebar in meter.</param>
        /// <param name="endAnchorage">Measure representing end anchorage of longitudinal rebar in meter.</param>
        /// <param name="start">Start x-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="end">End x-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="auxBar">Is bar auxiliary?</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Reinforcement.BarReinforcement LongitudinalBarByParam(Wire wire, double yPos, double zPos, double startAnchorage, double endAnchorage, double start, double end, bool auxBar)
        {
            // create longitudinal
            var pos = new FemDesign.Geometry.FdPoint2d(yPos, zPos);
            var longBar = new FemDesign.Reinforcement.LongitudinalBar(pos, startAnchorage, endAnchorage, start, end, auxBar);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, longBar);

            return barReinf;
        }
    }
}