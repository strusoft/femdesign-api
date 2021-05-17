using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars.Buckling
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class BucklingLength
    {
        /// <summary>
        /// Define BucklingLength in Flexural Stiff direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static BucklingLength FlexuralStiffDefine(double beta = 1, bool sway = false)
        {
            return FlexuralStiff(beta, sway);
        }

        /// <summary>
        /// Define BucklingLength in Flexural Weak direction.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="sway">Sway. True/false.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static BucklingLength FlexuralWeakDefine(double beta = 1, bool sway = false)
        {
            return FlexuralWeak(beta, sway);
        }

        /// <summary>
        /// Define BucklingLength for Pressured Top Flange.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continuouslyRestrained">Continuously restrained. True/false.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static BucklingLength PressuredTopFlangeDefine(double beta = 1, string loadPosition = "top", bool continuouslyRestrained = false)
        {
            return PressuredTopFlange(beta, loadPosition, continuouslyRestrained);
        }

        /// <summary>
        /// Define BucklingLength for Pressured Bottom Flange.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="beta">Beta factor.</param>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continuouslyRestrained">Continuously restrained. True/false.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static BucklingLength PressuredBottomFlangeDefine(double beta = 1, string loadPosition = "top", bool continuouslyRestrained = false)
        {
            return PressuredBottomFlange(beta, loadPosition, continuouslyRestrained);
        }

        /// <summary>
        /// Define BucklingLength for Lateral Torsional buckling.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="loadPosition">"top"/"center"/"bottom"</param>
        /// <param name="continouslyRestrained">Continously restrained. True/false.</param>
        /// <param name="cantilever">Cantilever. True/false.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static BucklingLength LateralTorsionalDefine(string loadPosition = "top", bool continouslyRestrained = false, bool cantilever = false)
        {
            return LateralTorsional(loadPosition, continouslyRestrained, cantilever);
        }
    }
}