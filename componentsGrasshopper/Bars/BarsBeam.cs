// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class BarsBeam: GH_Component
    {
       public BarsBeam(): base("Bars.Beam", "Beam", "Create a bar element of type beam.", "FemDesign", "Bars")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. Both ends of the bar-element are given the same connectivity. Optional, default value if undefined.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity. Both ends of the bar-element are given the same eccentricity. Optional, default value if undefined.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "B");
           pManager[pManager.ParamCount - 1].Optional = true;
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) { return; }
            
            FemDesign.Materials.Material material = null;
            if (!DA.GetData(1, ref material)) { return; }

            FemDesign.Sections.Section section = null;
            if (!DA.GetData(2, ref section)) { return; }

            FemDesign.Bars.Connectivity connectivity = FemDesign.Bars.Connectivity.Rigid();
            if (!DA.GetData(3, ref connectivity))
            {
                // pass
            }

            FemDesign.Bars.Eccentricity eccentricity = FemDesign.Bars.Eccentricity.Default();
            if (!DA.GetData(4, ref eccentricity))
            {
                // pass
            }
            
            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(5, ref v))
            {
                // pass
            }

            bool orientLCS = true;
            if (!DA.GetData(6, ref orientLCS))
            {
                // pass
            }

            string identifier = "B";
            if (!DA.GetData(7, ref identifier))
            {
                // pass
            }

            if (curve == null || material == null || section == null || connectivity == null || eccentricity == null || identifier == null) { return; }

            // convert geometry
            FemDesign.Geometry.Edge edge = FemDesign.Geometry.Edge.FromRhinoLineOrArc2(curve);

            // create bar
            FemDesign.Bars.Bar bar = FemDesign.Bars.Bar.Beam(identifier, edge, connectivity, eccentricity, material, section);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromRhino(v);
            }
            
            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {
                    bar.BarPart.OrientCoordinateSystemToGCS();
                }
            }

            // output
            DA.SetData(0, bar);

       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.BeamDefine;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("135b6331-bf19-4d89-9e81-9e5e0d137f67"); }
       }
    }
}