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
            pManager.AddNumberParameter("Diameter", "Diameter", "Diameter of reinforcement bar [m].", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material of reinforcement bar. Only reinforcement material can be use.", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile", "Profile", "Profile of reinforcement bar. Allowed values: smooth/ribbed", GH_ParamAccess.item, "ribbed");
            pManager[pManager.ParamCount - 1].Optional = true;
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

            double diameter = 0;
            if (!DA.GetData("Diameter", ref diameter)) return;

            FemDesign.Materials.Material material = null;
            if (!DA.GetData("Material", ref material)) return;

            string profile = "ribbed";
            DA.GetData("Profile", ref profile);

            WireProfileType _profile = EnumParser.Parse<WireProfileType>(profile);

            var obj = new FemDesign.Grasshopper.Layer(curve, numberOfPoints, diameter, material, _profile);

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