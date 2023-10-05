// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public partial class PatchComp : FEM_Design_API_Component
    {
        public PatchComp() : base("Patch", "Patch", "Define the boundary of the structural section", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Surface", "Srf", "Surface or section that define the boundary.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Section material: Only concrete material can be specified", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Patch", "Patch", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            dynamic input = null;
            if (!DA.GetData(0, ref input)) return;

            FemDesign.Materials.Material material = null;
            if (!DA.GetData("Material", ref material)) return;

            if (input.Value is Rhino.Geometry.Brep surface)
            {
                foreach (var _srf in surface.Surfaces)
                {
                    if (_srf.IsPlanar() == false)
                        throw new Exception("Surface must be planar!");
                }

                var _obj = new FemDesign.Grasshopper.Patch(surface, material);
                DA.SetData(0, _obj);
            }
            else if (input.Value is FemDesign.Sections.Section section)
            {
                surface = section.RegionGroup.ToRhino()[0];
                var _obj = new FemDesign.Grasshopper.Patch(surface, material);
                DA.SetData(0, _obj);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input must be a Surface or Section");
                return;
            }



        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Patch;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{2264144C-145D-4F2D-8677-F9CFF323C81C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}