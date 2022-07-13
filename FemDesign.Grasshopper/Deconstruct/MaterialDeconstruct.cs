// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialDeconstruct: GH_Component
    {
       public MaterialDeconstruct(): base("Material.Deconstruct", "Deconstruct", "Deconstruct basic Material information.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {   
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Standard", "Standard", "Standard.", GH_ParamAccess.item);
           pManager.AddTextParameter("Country", "Country", "Country.", GH_ParamAccess.item);
           pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            Materials.IMaterial obj = null;
            if(!DA.GetData(0, ref obj)) return;
            DA.SetData("Guid", obj.Guid);
            DA.SetData("Name", obj.Name);

            if (obj is Materials.Material material)
            {
                DA.SetData("Standard", material.Standard);
                DA.SetData("Country", material.Country);
            }
        }
        protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.MaterialDeconstrut;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("31dd70b8-f328-4c33-96f9-779e6d49e908"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}