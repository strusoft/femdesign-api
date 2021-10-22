using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Stirrups
    {
        /// <summary>
        /// Add stirrup reinforcement to a bar. Curved bars are not supported.
        /// </summary>
        /// <param name="bar">Bar to add stirrups to</param>
        /// <param name="wire">Stirrup rebar material and type.</param>
        /// <param name="profile">Surface representing the profile of the stirrup.</param>
        /// <param name="startParam">Parameter representing start position of stirrups. 0 is start of bar and 1 is end of bar</param>
        /// <param name="endParam">Parameter representing start position of stirrups. 0 is start of bar and 1 is end of bar</param>
        /// <param name="spacing">arameter representing spacing of stirrups.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bars.Bar StirrupsByParam(Bars.Bar bar, Wire wire, Autodesk.DesignScript.Geometry.Surface profile, double startParam, double endParam, double spacing)
        {
            // transform profile to region
            var region = Geometry.Region.FromDynamo(profile);

            // create stirrups
            var stirrups = new FemDesign.Reinforcement.Stirrups(bar, region, startParam, endParam, spacing);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(bar, wire, stirrups);

            // add to bar
            var clone = bar.DeepClone();
            clone.Reinforcement.Add(barReinf);

            //
            return clone;
        }
    }
}