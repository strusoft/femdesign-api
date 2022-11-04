using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class ConnectedLines
    {
        #region dynamo
        [IsVisibleInDynamoLibrary(true)]
        public static ConnectedLines Define(Autodesk.DesignScript.Geometry.Curve firstCurve, Autodesk.DesignScript.Geometry.Curve secondCurve, Autodesk.DesignScript.Geometry.Vector localX, Autodesk.DesignScript.Geometry.Vector localY, Releases.Motions motions, Releases.Rotations rotations, System.Guid[] references, string identifier, bool movingLocal, double interfaceStart, double interfaceEnd)
        {
            // convert geometry
            Geometry.Edge edge0 = Geometry.Edge.FromDynamoLineOrArc2(firstCurve);
            Geometry.Edge edge1 = Geometry.Edge.FromDynamoLineOrArc2(secondCurve);
            Geometry.Vector3d x = Geometry.Vector3d.FromDynamo(localX);
            Geometry.Vector3d y = Geometry.Vector3d.FromDynamo(localY);

            // rigidity
            Releases.RigidityDataType3 rigidity = new Releases.RigidityDataType3(motions, rotations);

            // references
            GuidListType[] refs = new GuidListType[references.Length];
            for (int idx = 0; idx < refs.Length; idx++)
            {
                refs[idx] = new GuidListType(references[idx]);
            }

            return new ConnectedLines(edge0, edge1, x, y, rigidity, refs, identifier, movingLocal, interfaceStart, interfaceEnd);
        }
        #endregion 
    }
}
