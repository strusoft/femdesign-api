using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class ConnectedPoints: EntityBase
    {

        #region dynamo

        [IsVisibleInDynamoLibrary(true)]
        public static ConnectedPoints Define(Autodesk.DesignScript.Geometry.Point firstPoint, Autodesk.DesignScript.Geometry.Point secondPoint, Releases.Motions motions, Releases.Rotations rotations, System.Guid[] references, string identifier)
        {
            // convert geometry
            Geometry.FdPoint3d p1 = Geometry.FdPoint3d.FromDynamo(firstPoint);
            Geometry.FdPoint3d p2 = Geometry.FdPoint3d.FromDynamo(secondPoint);

            // rigidity
            Releases.RigidityDataType2 rigidity = new Releases.RigidityDataType2(motions, rotations);

            // references
            GuidListType[] refs = new GuidListType[references.Length];
            for (int idx = 0; idx < refs.Length; idx++)
            {
                refs[idx] = new GuidListType(references[idx]);
            }

            return new ConnectedPoints(p1, p2, rigidity, refs, identifier);
        }

        #endregion 
    }
}
