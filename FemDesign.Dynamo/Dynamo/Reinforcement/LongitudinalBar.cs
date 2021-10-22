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
        /// <param name="bar">Bar to add longitudinal rebars to</param>
        /// <param name="wire">Longitudinal rebar material and type.</param>
        /// <param name="yPos">YPos</param>
        /// <param name="zPos">ZPos</param>
        /// <param name="startAnchorage">Measure representing start anchorage of longitudinal rebar in meter.</param>
        /// <param name="endAnchorage">Measure representing end anchorage of longitudinal rebar in meter.</param>
        /// <param name="startParam">Parameter representing start position of longitudinal rebar. 0 is start of bar and 1 is end of bar</param>
        /// <param name="endParam">Parameter representing start position of longitudinal rebar. 0 is start of bar and 1 is end of bar</param>
        /// <param name="auxBar">Is bar auxiliary?</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bars.Bar LongitudinalBarByParam(Bars.Bar bar, Wire wire, double yPos, double zPos, double startAnchorage, double endAnchorage, double startParam, double endParam, bool auxBar)
        {
            // create longitudinal
            var pos = new FemDesign.Geometry.FdPoint2d(yPos, zPos);
            var longBar = new FemDesign.Reinforcement.LongitudinalBar(bar, pos, startAnchorage, endAnchorage, startParam, endParam, auxBar);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(bar, wire, longBar);

            // add to bar
            var clone = bar.DeepClone();
            clone.Reinforcement.Add(barReinf);

            return clone;
        }
    }
}