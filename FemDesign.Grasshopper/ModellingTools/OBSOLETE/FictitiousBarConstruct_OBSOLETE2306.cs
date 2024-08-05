// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using FemDesign.Loads;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using StruSoft.Interop.StruXml.Data;
using System.Reflection;


namespace FemDesign.Grasshopper
{
    public class FictitiousBarConstruct_OBSOLETE2306 : FEM_Design_API_Component
    {
        public FictitiousBarConstruct_OBSOLETE2306() : base("FictitiousBar.Construct", "Construct", "Construct a fictitious bar element.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
            pManager.AddNumberParameter("AE", "AE", "AE", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ItG", "ItG", "This parameter is ignored when using TrussBehaviour.", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("I1E", "I1E", "This parameter is ignored when using TrussBehaviour.", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("I2E", "I2E", "This parameter is ignored when using TrussBehaviour.", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Mass", "Mass", "Unit mass", GH_ParamAccess.item, 0.1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. Connectivity. If 1 item this item defines both start and end connectivity. If two items the first item defines the start connectivity and the last item defines the end connectivity.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
            pManager.AddGenericParameter("TrussBehaviour", "TrussBehaviour", "Optional. If null or empty, this parameter is ignored. To set up the truss behaviour, connect the 'FictitiousBarTrussBehaviour' component.", GH_ParamAccess.item);
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

            double stiffness = 1E7;
            double ae = stiffness;
            DA.GetData(1, ref ae);

            double itg = stiffness;
            DA.GetData(2, ref itg);

            double i1e = stiffness;
            DA.GetData(3, ref i1e);

            double i2e = stiffness;
            DA.GetData(4, ref i2e);

            double mass = 0.1;
            DA.GetData(5, ref mass);

            Bars.Connectivity startConnectivity = Bars.Connectivity.Default;
            Bars.Connectivity endConnectivity = Bars.Connectivity.Default;
            List<Bars.Connectivity> connectivity = new List<Bars.Connectivity>();
            if (DA.GetDataList(6, connectivity))
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
            DA.GetData(7, ref v);

            bool orientLCS = true;
            DA.GetData(8, ref orientLCS);

            StruSoft.Interop.StruXml.Data.Simple_truss_chr_type trussBehaviour = null;
            DA.GetData(9, ref trussBehaviour);

            string name = "BF";
            DA.GetData(10, ref name);

            // check inputs
            if (curve == null || startConnectivity == null || endConnectivity == null || v == null || name == null)
            {
                return;
            }
            // check truss behaviour

            // convert geometry
            Geometry.Edge edge = curve.FromRhinoLineOrArc2();

            // create virtual bar
            ModellingTools.FictitiousBar bar;
            if (trussBehaviour == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "Bended bar behaviour.");
                bar = new ModellingTools.FictitiousBar(edge, edge.Plane.LocalY, startConnectivity, endConnectivity, name, ae, itg, i1e, i2e, mass);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "Truss behaviour. Connectivity, ItG, I1E and I2E parameters are ignored.");
                bar = new ModellingTools.FictitiousBar(edge, edge.Plane.LocalY, name, ae, mass, trussBehaviour);
            }

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
            get { return new Guid("{1D179B29-59F4-4918-8A74-981E69ED1A13}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}