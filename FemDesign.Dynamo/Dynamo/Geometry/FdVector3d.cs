
using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class FdVector3d
    {
        #region dynamo
        /// <summary>
        /// Create FdVector3d from Dynamo vector.
        /// </summary>
        public static FdVector3d FromDynamo(Autodesk.DesignScript.Geometry.Vector vector)
        {
            FdVector3d newVector = new FdVector3d(vector.X, vector.Y, vector.Z);
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