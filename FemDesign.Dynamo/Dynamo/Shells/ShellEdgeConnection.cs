
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class ShellEdgeConnection: EdgeConnectionBase
    {
        /// <summary>
        /// Create a default (rigid) ShellEdgeConnection.
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static ShellEdgeConnection Default() => GetDefault();

        /// <summary>
        /// Define a new ShellEdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions">Motions.</param>
        /// <param name="rotations">Rotations.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static ShellEdgeConnection Define(Releases.Motions motions, Releases.Rotations rotations)
        {
            return new ShellEdgeConnection(motions, rotations);
        }

        /// <summary>
        /// Create a hinged ShellEdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static ShellEdgeConnection Hinged()
        {
            return ShellEdgeConnection.GetHinged();
        }

        /// <summary>
        /// Create a rigid ShellEdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static ShellEdgeConnection Rigid()
        {
            return ShellEdgeConnection.GetRigid();
        }
    }
}