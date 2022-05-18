
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
        /// Create a default (rigid) EdgeConnection.
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static EdgeConnection Default() => GetDefault();

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
        public static EdgeConnection Hinged()
        {
            return EdgeConnection.GetHinged();
        }

        /// <summary>
        /// Create a rigid EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static EdgeConnection Rigid()
        {
            return EdgeConnection.GetRigid();
        }
    }
}