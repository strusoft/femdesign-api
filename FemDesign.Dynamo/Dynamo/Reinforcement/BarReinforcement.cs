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
        public static Bars.Bar AddToBar(Bars.Bar bar, List<Reinforcement.BarReinforcement> barReinforcement, bool overwrite)
        {
            // clone bar
            var clone = bar.DeepClone();

            // clone reinforcement
            var reinfClone = barReinforcement.Select(x => x.DeepClone()).ToList();

            // add reinforcement
            Bars.Bar obj = Reinforcement.BarReinforcement.AddReinforcementToBar(clone, reinfClone, overwrite);

            return obj;
        }
    }
}