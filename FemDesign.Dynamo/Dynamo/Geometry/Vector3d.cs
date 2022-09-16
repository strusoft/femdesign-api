
using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Vector3d
    {
        #region dynamo
        /// <summary>
        /// Create FdVector3d from Dynamo vector.
        /// </summary>
        public static Vector3d FromDynamo(Autodesk.DesignScript.Geometry.Vector vector)
        {
            Vector3d newVector = new Vector3d(vector.X, vector.Y, vector.Z);
            return newVector;
        }

        /// <summary>
        /// Create Dynamo vector from FdVector3d.
        /// </summary>
        public Autodesk.DesignScript.Geometry.Vector ToDynamo()
        {
            return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(this.X, this.Y, this.Z);
        }
        
        #endregion
    }
}