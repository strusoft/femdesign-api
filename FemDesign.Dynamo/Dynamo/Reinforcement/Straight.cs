
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using Autodesk.DesignScript.Runtime;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Straight
    {
        #region dynamo
        
        /// <summary>
        /// Define straight reinforcement layout for surface reinforcement.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="direction">x/y</param>
        /// <param name="space">Spacing between bars.</param>
        /// <param name="face">top/mid/bottom</param>
        /// <param name="cover">Reinforcement concrete cover.</param>
        /// <returns></returns>
        public static Straight ReinforcementLayout(ReinforcementDirection direction, double space, Face face, double cover)
        {
            return new Straight(direction, space, face, cover);
        }

        /// <summary>
        /// Define straight reinforcement layout for surface reinforcement.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="direction">"x"/"y"</param>
        /// <param name="space">Spacing between bars.</param>
        /// <param name="face">"top"/"bottom"</param>
        /// <param name="cover">Reinforcement concrete cover.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Straight ReinforcementLayout(string direction, double space, string face, double cover)
        {
            return new Straight(direction, space, face, cover);
        }

        #endregion
    }
}