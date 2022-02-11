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
        /// <param name="wire">Stirrup rebar material and type.</param>
        /// <param name="profile">Surface representing the profile of the stirrup.</param>
        /// <param name="start">Start x-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="end">End x-position, of longitudinal rebar, in host bar local coordinate system.</param>
        /// <param name="spacing">Parameter representing spacing of stirrups.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Reinforcement.BarReinforcement StirrupsByParam(Wire wire, Autodesk.DesignScript.Geometry.Surface profile, double start, double end, double spacing)
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