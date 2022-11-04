
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class EdgeConnection: EdgeConnectionBase
    {
        /// <summary>
        /// Define a new EdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions">Motions.</param>
        /// <param name="rotations">Rotations.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static EdgeConnection Define(Releases.Motions motions, Releases.Rotations rotations)
        {
            return new EdgeConnection(motions, rotations);
        }

        /// <summary>
        /// Create a hinged EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static EdgeConnection GetHinged()
        {
            return EdgeConnection.Hinged;
        }

        /// <summary>
        /// Create a rigid EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static EdgeConnection GetRigid()
        {
            return EdgeConnection.Rigid;
        }
    }
}