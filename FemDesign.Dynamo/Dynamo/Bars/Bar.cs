using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Bar: EntityBase
    {
        #region dynamo
        /// <summary>
        /// Create a bar-element of type beam.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve. Only line and arc are supported.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end.</param>
        /// <param name="connectivity">Connectivity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, if undefined default value will be used.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Beam(Autodesk.DesignScript.Geometry.Curve curve, Materials.Material material, Sections.Section[] section, [DefaultArgument("Connectivity.Default()")] Connectivity[] connectivity, [DefaultArgument("Eccentricity.Default()")] Eccentricity[] eccentricity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, string identifier = "B")
        {
            // convert class
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc2(curve);

            // create bar
            Bar bar = Bar.BeamDefine(edge, material, section, connectivity, eccentricity, identifier);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    bar.BarPart.OrientCoordinateSystemToGCS();
                }
            }

            // return
            return bar;
        }
        /// <summary>
        /// Create a bar-element of type column.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Local x of line must equal positive global Z.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end.</param>
        /// <param name="connectivity">Connectivity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, if undefined default value will be used.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Column(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section[] section, [DefaultArgument("Connectivity.Default()")] Connectivity[] connectivity, [DefaultArgument("Eccentricity.Default()")] Eccentricity[] eccentricity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, string identifier = "C")
        {
            // convert class
            Geometry.Edge edge = Geometry.Edge.FromDynamoLine(line);

            // create bar
            Bar bar = Bar.ColumnDefine(edge, material, section, connectivity, eccentricity, identifier);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                { 
                    bar.BarPart.OrientCoordinateSystemToGCS();
                }
            }

            // return
            return bar;
        }
        /// <summary>
        /// Create a bar-element of type truss.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Truss(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, string identifier = "T")
        {
            // convert class
            Geometry.Edge edge = Geometry.Edge.FromDynamoLine(line);

            // create bar
            Bar bar = Bar.TrussDefine(edge, material, section, identifier);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    bar.BarPart.OrientCoordinateSystemToGCS();
                }
            }

            // return
            return bar;
        }
        /// <summary>
        /// Create a bar-element of type truss with limited capacity in compression and tension.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="line">Line.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section.</param>
        /// <param name="maxCompression">Compression force limit.</param>
        /// <param name="compressionPlasticity">True if plastic behaviour. False if brittle behaviour.</param>
        /// <param name="maxTension">Tension force limit.</param>
        /// <param name="tensionPlasticity">True if plastic behaviour. False if brittle behaviour.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar TrussLimitedCapacity(Autodesk.DesignScript.Geometry.Line line, Materials.Material material, Sections.Section section, double maxCompression, double maxTension, bool compressionPlasticity,  bool tensionPlasticity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, string identifier = "T")
        {
            // convert class
            Geometry.Edge edge = Geometry.Edge.FromDynamoLine(line);

            // create bar
            Bar bar = Bar.TrussDefine(edge, material, section, identifier, maxCompression,  maxTension, compressionPlasticity, tensionPlasticity);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {
                    bar.BarPart.OrientCoordinateSystemToGCS();
                }
            }

            // return
            return bar;
        }

        /// <summary>
        /// Modify properties of an exiting bar element of any type.
        /// </summary>
        /// <param name="newGuid">Generate a new guid for this bar?</param>
        /// <param name="curve">Curve. Only line and arc are supported.</param>
        /// <param name="material">Material.</param>
        /// <param name="section">Section. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end.</param>
        /// <param name="connectivity">Connectivity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, if undefined default value will be used.</param>
        /// <param name="eccentricity">Eccentricity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, if undefined default value will be used.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Bar Modify(Bar bar, [DefaultArgument("false")] bool newGuid, [DefaultArgument("null")] Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("null")] Materials.Material material, [DefaultArgument("null")] Sections.Section[] section, [DefaultArgument("null")] Connectivity[] connectivity, [DefaultArgument("null")] Eccentricity[] eccentricity, [DefaultArgument("null")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("false")] bool orientLCS, [DefaultArgument("null")] string identifier)
        {
            // deep clone input bar
            bar = bar.DeepClone();

            if (newGuid)
            {
                bar.EntityCreated();
                bar.BarPart.EntityCreated();
            }

            if (curve != null)
            {
                // convert geometry
                Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc2(curve);

                // update edge
                bar.BarPart.Edge = edge;
            }

            if (material != null)
            {
                bar.BarPart.Material = material;
            }

            if (section != null)
            {
                bar.BarPart.Sections = section;
            }

            if (connectivity != null)
            {
                bar.BarPart.Connectivities = connectivity;
            }

            if (eccentricity != null)
            {
                bar.BarPart.Eccentricities = eccentricity;
            }

            if (localY != null)
            {
                bar.BarPart.LocalY = Geometry.FdVector3d.FromDynamo(localY);
            }

            if (orientLCS)
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            if (identifier != null)
            {
                bar.Identifier = identifier;
                bar.BarPart.Identifier = bar.Identifier;
            }

            return bar;
        }

        /// <summary>
        /// Create Dynamo curve from underlying Edge (Line or Arc) of Bar.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Curve GetDynamoCurve()
        {
            return this.BarPart._edge.ToDynamo();
        }
        #endregion
    }
}
