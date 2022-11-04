// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsModify: GH_Component
    {
       public BarsModify(): base("Bars.Modify", "Modify", "Modify properties of an exiting bar element of any type.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar element", GH_ParamAccess.item);
           pManager.AddBooleanParameter("NewGuid", "NewGuid", "Generate a new guid for this bar?", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Section", "Section", "Section. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, default value if undefined.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end. Optional, default value if undefined.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            Bars.Bar bar = null;
            if (DA.GetData(0, ref bar))
            {
                if (bar.BarPart.HasComplexCompositeRef || bar.BarPart.HasDeltaBeamComplexSectionRef)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The bar has a Composite Section. The object has not been implemented yet. Please, get in touch if needed.");
                    return;
                }

                bar = bar.DeepClone();
            }

            bool newGuid = false;
            if (DA.GetData(1, ref newGuid))
            {
                if (newGuid)
                {
                    bar.EntityCreated();
                    bar.BarPart.EntityCreated();
                }
            }

            Curve curve = null;
            if (DA.GetData(2, ref curve))
            {
                // convert geometry 
                FemDesign.Geometry.Edge edge = Convert.FromRhinoLineOrArc2(curve);

                // update edge
                bar.BarPart.Edge = edge;
            }
            
            FemDesign.Materials.Material material = null;
            if (DA.GetData(3, ref material))
            {
                bar.BarPart.ComplexMaterialObj = material;
            }

            List<FemDesign.Sections.Section> sections = new List<Sections.Section>();
            if (DA.GetDataList(4, sections))
            {
                bar.BarPart.ComplexSectionObj.Sections = sections.ToArray();
            }

            List<FemDesign.Bars.Connectivity> connectivities = new List<Bars.Connectivity>();
            if (DA.GetDataList(5, connectivities))
            {
                bar.BarPart.Connectivity = connectivities.ToArray();
            }

            List<FemDesign.Bars.Eccentricity> eccentricities = new List<Bars.Eccentricity>();
            if (DA.GetDataList(6, eccentricities))
            {
                if(bar.Type != Bars.BarType.Truss)
                {
                    bar.BarPart.ComplexSectionObj.Eccentricities = eccentricities.ToArray();
                }
            }
            
            Vector3d v = Vector3d.Zero;
            if (DA.GetData(7, ref v))
            {
                bar.BarPart.LocalY = v.FromRhino();
            }

            bool orientLCS = true;
            if (DA.GetData(8, ref orientLCS))
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            string identifier = null;
            if (DA.GetData(9, ref identifier))
            {
                bar.Identifier = identifier;
            }

            // output
            DA.SetData(0, bar);

       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.BarModify;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("24c7400b-e73b-4b65-b2f9-c7b2fe9f27c3"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}