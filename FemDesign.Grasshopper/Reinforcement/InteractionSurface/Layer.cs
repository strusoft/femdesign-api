// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public partial class LayerComp : GH_Component
    {
        public LayerComp() : base("Layer", "Layer", "Define a series of rebar to be place in a curve.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve path for rebar placement", GH_ParamAccess.item);
            pManager.AddIntegerParameter("NumberOfRebar", "NumberOfRebar", "Number of rebar to insert on the curve. The point will be place on a same length distance.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Wire", "Wire", "", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Layer", "Layer", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Curve curve = null;
            if (!DA.GetData(0, ref curve)) return;

            int numberOfPoints = 2;
            if (!DA.GetData("NumberOfRebar", ref numberOfPoints)) return;

            FemDesign.Reinforcement.Wire wire = null;
            if (!DA.GetData("Wire", ref wire)) return;

            var obj = new FemDesign.Grasshopper.Layer(curve, numberOfPoints, wire.Diameter,  wire.ReinforcingMaterial, wire.Profile);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Layer;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{89A5FE32-7B0C-4F18-AA75-5B9B77D6AA27}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}