using System.Xml.Serialization;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Calculate
{

    [IsVisibleInDynamoLibrary(false)]
    public partial class Design
    {
        /// <summary>Set parameters for design.</summary>
        /// <remarks>Create</remarks>
        /// <param name="autoDesign">Auto-design elements.</param>
        /// <param name="check">Check elements.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Design Define(bool autoDesign = false, bool check = true)
        {
            return new Design(autoDesign, check);
        }
    }
}