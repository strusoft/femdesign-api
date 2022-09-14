// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FictitiousBarConstruct: GH_Component
    {
       public FictitiousBarConstruct(): base("FictitiousBar.Construct", "Construct", "Construct a fictitious bar element.", "FEM-Design", "ModellingTools")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager.AddNumberParameter("AE", "AE", "AE", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("ItG", "ItG", "ItG", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("I1E", "I1E", "I1E", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("I2E", "I2E", "I2E", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. Connectivity. If 1 item this item defines both start and end connectivity. If two items the first item defines the start connectivity and the last item defines the end connectivity.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "BF");
           pManager[pManager.ParamCount - 1].Optional = true;
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("FictitiousBar", "FictitiousBar", "FictitiousBar.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) { return; }

            double stiffness = 1E7;
            double ae = stiffness;
            if (!DA.GetData(1, ref ae))
            {
                // pass
            }

            double itg = stiffness;
            if (!DA.GetData(2, ref itg))
            {
                // pass
            }

            double i1e = stiffness;
            if (!DA.GetData(3, ref i1e))
            {
                // pass
            }

            double i2e = stiffness;
            if (!DA.GetData(4, ref i2e))
            {
                // pass
            }

            Bars.Connectivity startConnectivity = Bars.Connectivity.Default;
            Bars.Connectivity endConnectivity = Bars.Connectivity.Default;
            List<Bars.Connectivity> connectivity = new List<Bars.Connectivity>();
            if (!DA.GetDataList(5, connectivity))
            {
                // pass
            }
            else
            {
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
            }

            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(6, ref v))
            {
                // pass
            }

            bool orientLCS = true;
            if (!DA.GetData(7, ref orientLCS))
            {
                // pass
            }

            string name = "BF";
            if (!DA.GetData(8, ref name))
            {
                // pass
            }

            if (curve == null || startConnectivity == null || endConnectivity == null || v == null || name == null)
            {
                return;
            }

            // convert geometry
            Geometry.Edge edge = curve.FromRhinoLineOrArc2();

            // create virtual bar
            ModellingTools.FictitiousBar bar = new ModellingTools.FictitiousBar(edge, edge.CoordinateSystem.LocalY, startConnectivity, endConnectivity, name, ae, itg, i1e, i2e);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.LocalY = v.FromRhino();
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    bar.OrientCoordinateSystemToGCS();
                }
            }

            // output
            DA.SetData(0, bar);

       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.FictBar;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("29e566d5-aba6-4719-a23f-d77e322e5a5d"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}