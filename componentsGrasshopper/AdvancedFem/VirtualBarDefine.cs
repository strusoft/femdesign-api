// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class VirtualBarDefine: GH_Component
    {
       public VirtualBarDefine(): base("FictitiousBar.Define", "Define", "Create a fictitious bar element.", "FemDesign", "Modeling tool")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity.", GH_ParamAccess.list);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "LocalY.", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("AE", "AE", "AE", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("ItG", "ItG", "ItG", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("I1E", "I1E", "I1E", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("I2E", "I2E", "I2E", GH_ParamAccess.item, 1E7);
           pManager[pManager.ParamCount - 1].Optional = true;
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
            
            Bars.Connectivity startConnectivity = Bars.Connectivity.Default();
            Bars.Connectivity endConnectivity = Bars.Connectivity.Default();
            List<Bars.Connectivity> connectivity = new List<Bars.Connectivity>();
            if (!DA.GetDataList(1, connectivity))
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
            if (!DA.GetData(2, ref v))
            {
                // pass
            }

            double stiffness = 1E7;
            double ae = stiffness;
            if (!DA.GetData(3, ref ae))
            {
                // pass
            }

            double itg = stiffness;
            if (!DA.GetData(4, ref itg))
            {
                // pass
            }

            double i1e = stiffness;
            if (!DA.GetData(5, ref i1e))
            {
                // pass
            }

            double i2e = stiffness;
            if (!DA.GetData(6, ref i2e))
            {
                // pass
            }

            string name = "BF";
            if (!DA.GetData(7, ref name))
            {
                // pass
            }

            if (curve == null || startConnectivity == null || endConnectivity == null || v == null || name == null)
            {
                return;
            }

            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromRhinoLineOrArc2(curve);

            // create virtual bar
            VirtualBar bar = new VirtualBar(edge, edge.coordinateSystem.localY, startConnectivity, endConnectivity, name, ae, itg, i1e, i2e);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.LocalY = FemDesign.Geometry.FdVector3d.FromRhino(v);
            }

            // output
            DA.SetData(0, bar);

       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return null;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("29e566d5-aba6-4719-a23f-d77e322e5a5d"); }
       }
    }
}