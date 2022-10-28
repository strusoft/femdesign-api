using System;
using System.Collections.Generic;
using System.Linq;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class BarReinforcement: EntityBase
    {
        ///<summary>
        ///
        ///</summary>
        ///<param name="bar">Bar</param>
        ///<param name="barReinforcement">Bar reinforcment to add to bar. Item or list.</param>
        ///<param name="overwrite">Overwrite if reinforcement bar already exists on bar - by guid.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bars.Bar AddToBar(Bars.Bar bar, List<Reinforcement.BarReinforcement> barReinforcement, [DefaultArgument("true")] bool overwrite)
        {
            // clone bar
            var clone = bar.DeepClone();

            // clone reinforcement
            var reinfClone = barReinforcement.Select(x => x.DeepClone()).ToList();

            // add reinforcement
            Bars.Bar obj = Reinforcement.BarReinforcement.AddReinforcementToBar(clone, reinfClone, overwrite);

            return obj;
        }
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
        public static Reinforcement.BarReinforcement LongitudinalBarByStartEnd(Wire wire, double yPos, double zPos, double startAnchorage, double endAnchorage, double start, double end, bool auxBar)
        {
            // create longitudinal
            var pos = new FemDesign.Geometry.Point2d(yPos, zPos);
            var longBar = new FemDesign.Reinforcement.LongitudinalBar(pos, startAnchorage, endAnchorage, start, end, auxBar);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, longBar);

            return barReinf;
        }
        /// <summary>
        /// Add stirrup reinforcement to a bar. Curved bars are not supported.
        /// </summary>
        /// <param name="wire">Stirrup rebar material and type.</param>
        /// <param name="profile">Surface representing the profile of the stirrup.</param>
        /// <param name="start">Start x-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="end">End x-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="spacing">Parameter representing spacing of stirrups.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Reinforcement.BarReinforcement StirrupsByStartEnd(Wire wire, Autodesk.DesignScript.Geometry.Surface profile, double start, double end, double spacing)
        {
            // transform profile to region
            var region = Geometry.Region.FromDynamo(profile);

            // create stirrups
            var stirrups = new FemDesign.Reinforcement.Stirrups(region, start, end, spacing);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, stirrups);

            //
            return barReinf;
        }
    }
}
