// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class WireDefine: GH_Component
    {
        public WireDefine(): base("Wire.Define", "Define", "Define a reinforcement bar (wire) for a normal reinforcement layout.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Diameter", "Diameter", "Diameter of reinforcement bar.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material of reinforcement bar.", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile", "Profile", "Profile of reinforcement bar. Allowed values: smooth/ribbed", GH_ParamAccess.item, "ribbed");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Wire", "Wire", "Wire.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double diameter = 0;
            FemDesign.Materials.Material material = null;
            string profile = "ribbed";
            if (!DA.GetData("Diameter", ref diameter)) return;
            if (!DA.GetData("Material", ref material)) return;
            DA.GetData("Profile", ref profile);
            
            if (material == null || profile == null)
                return;

            WireProfileType _profile = EnumParser.Parse<WireProfileType>(profile);
            FemDesign.Reinforcement.Wire obj = new FemDesign.Reinforcement.Wire(diameter, material, _profile);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.WireDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("0e5099d2-67e8-49ac-8669-edac85735f36"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }   
}