using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class FictitiousBar
    {
        #region dynamo
        [IsVisibleInDynamoLibrary(true)]
        /// <summary>
        /// Create a fictitious bar element.
        /// </summary>
        /// <param name="curve">Line or Arc.</param>
        /// <param name="AE">AE</param>
        /// <param name="ItG">ItG</param>
        /// <param name="I1E">I1E</param>
        /// <param name="I2E">I2E</param>
        /// <param name="connectivity">Connectivity. If 1 item this item defines both start and end connectivity. If two items the first item defines the start connectivity and the last item defines the end connectivity.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        public static FictitiousBar Define(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("10000000")] double AE, [DefaultArgument("10000000")] double ItG, [DefaultArgument("10000000")] double I1E, [DefaultArgument("10000000")] double I2E, [DefaultArgument("FemDesign.Bars.Connectivity.Default()")] List<Bars.Connectivity> connectivity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, string identifier = "BF")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc2(curve);
            Geometry.Vector3d y = Geometry.Vector3d.FromDynamo(localY);

            // get connectivity
            Bars.Connectivity startConnectivity;
            Bars.Connectivity endConnectivity;
            if (connectivity.Count == 1)
            {
                startConnectivity = connectivity[0];
                endConnectivity = connectivity[0];
            }
            else if (connectivity.Count == 2)
            {
                startConnectivity = connectivity[0];
                endConnectivity = connectivity[1];
            }
            else
            {
                throw new System.ArgumentException($"Connectivity must contain 1 or 2 items. Number of items is {connectivity.Count}");
            }

            // create virtual bar
            FictitiousBar bar = new FictitiousBar(edge, edge.CoordinateSystem.LocalY, startConnectivity, endConnectivity, identifier, AE, ItG, I1E, I2E);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.LocalY = FemDesign.Geometry.Vector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {
                    bar.OrientCoordinateSystemToGCS();
                }
            }

            // return
            return bar;
        }
        #endregion
    }
}