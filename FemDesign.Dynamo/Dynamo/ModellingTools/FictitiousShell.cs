using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class FictitiousShell
    {
        /// <summary>
        /// Set EdgeConnection by indices.
        /// </summary>
        /// <param name="fictShell">Fictitious Shell</param>
        /// <param name="edgeConnection">EdgeConnection</param>
        /// <param name="indices">Index. List of items. Deconstruct fictitious shell to extract index for each respective edge.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static FictitiousShell SetEdgeConnection(FictitiousShell fictShell, Shells.EdgeConnection edgeConnection, List<int> indices)
        {
            return UpdateEdgeConnection(fictShell, edgeConnection, indices);
        }

        #region dynamo
        /// <summary>
        /// Define a fictitious shell
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <param name="d">Membrane stiffness matrix</param>
        /// <param name="k">Flexural stiffness matrix</param>
        /// <param name="h">Shear stiffness matrix</param>
        /// <param name="density">Density in t/m2</param>
        /// <param name="t1">t1 in m</param>
        /// <param name="t2">t2 in m</param>
        /// <param name="alpha1">Alpha1 in 1/°C</param>
        /// <param name="alpha2">Alpha2 in 1/°C</param>
        /// <param name="ignoreInStImpCalc">Ignore in stability/imperfection calculation</param>
        /// <param name="edgeConnection">EdgeConnection. Optional, if rigid if undefined.</param>
        /// <param name="localX">Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.</param>
        /// <param name="localZ">Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.</param>
        /// <param name="avgSrfElemSize">Finite element size. Set average surface element size. If set to 0 FEM-Design will automatically caculate the average surface element size.</param>
        /// <param name="identifier">Identifier.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static FictitiousShell Define(Autodesk.DesignScript.Geometry.Surface surface, StiffnessMatrix4Type d, StiffnessMatrix4Type k, StiffnessMatrix2Type h, double density, double t1, double t2, double alpha1, double alpha2, bool ignoreInStImpCalc, [DefaultArgument("EdgeConnection.Default()")] Shells.EdgeConnection edgeConnection, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localX, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localZ, double avgSrfElemSize = 0, string identifier = "FS")
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.Vector3d x = Geometry.Vector3d.FromDynamo(localX);
            Geometry.Vector3d z = Geometry.Vector3d.FromDynamo(localZ);

            // add edge connections to region
            region.SetEdgeConnections(edgeConnection);

            //
            FictitiousShell obj = new FictitiousShell(region, d, k, h, density, t1, t2, alpha1, alpha2, ignoreInStImpCalc, avgSrfElemSize, identifier);

            // set local x-axis
            if (!localX.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.LocalX = FemDesign.Geometry.Vector3d.FromDynamo(localX);
            }

            // set local z-axis
            if (!localZ.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.LocalZ = FemDesign.Geometry.Vector3d.FromDynamo(localZ);
            }

            // return
            return obj;
        }
        #endregion
    }
}