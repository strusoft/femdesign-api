// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsColumnSimple: GH_Component
    {
       public BarsColumnSimple(): base("Bars.ColumnSimple", "ColumnSimple", "Create a bar element of type column with same start/end properties.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddCurveParameter("Line", "Line", "Local x of line must equal positive global Z.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. Optional, default value if undefined.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity. Optional, default value if undefined.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "C");
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

            FemDesign.Bars.Connectivity connectivity = FemDesign.Bars.Connectivity.Rigid;
            if (!DA.GetData(3, ref connectivity))
            {
                // pass
            }

            FemDesign.Bars.Eccentricity eccentricity = FemDesign.Bars.Eccentricity.Default;
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

            string identifier = "C";
            if (!DA.GetData(7, ref identifier))
            {
                // pass
            }

            if (curve == null || material == null || section == null || connectivity == null || eccentricity == null || identifier == null) { return; }

            // convert geometry
            if (curve.GetType() != typeof(LineCurve))
            {
                throw new System.ArgumentException("Curve must be a LineCurve");
            }
            FemDesign.Geometry.Edge edge = Convert.FromRhinoLineCurve((LineCurve)curve);

            // create bar
            var type = FemDesign.Bars.BarType.Column;
            FemDesign.Bars.Bar bar = new FemDesign.Bars.Bar(edge, material, section, type, eccentricity, connectivity, identifier);
            
            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.BarPart.LocalY = v.FromRhino();
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
                return FemDesign.Properties.Resources.ColumnDefine;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("4f7cdee1-2e06-403b-b503-acdb8cc5cd4f"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}