// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class EdgeConnectionDeconstruct_OBSOLETE: GH_Component
    {
       public EdgeConnectionDeconstruct_OBSOLETE(): base("EdgeConnection.Deconstruct", "Deconstruct", "Deconstruct a EdgeConnection element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid of edge connection.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
           pManager.AddTextParameter("PredefinedName", "PredefinedName", "Name of predefined type.", GH_ParamAccess.item);
           pManager.AddTextParameter("PredefinedGuid", "PredefinedGuid", "Guid of predefined type.", GH_ParamAccess.item);
           pManager.AddTextParameter("Friction", "Friction", "Friction.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Motions", "Motions", "Motions", GH_ParamAccess.item);
           pManager.AddGenericParameter("Rotations", "Rotations", "Rotations", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Shells.EdgeConnection obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.Guid);
            DA.SetData(1, obj.Name);

            // catch pre-defined rigidity
            if (obj.Rigidity != null)
            {
                DA.SetData(2, null);
                DA.SetData(3, null);
                if (obj.Rigidity._friction == null)
                {  
                    DA.SetData(4, null);
                }
                else
                {    
                    DA.SetData(4, obj.Rigidity.Friction);
                }
                DA.SetData(5, obj.Rigidity.Motions);
                DA.SetData(6, obj.Rigidity.Rotations);
            }
            else
            {
                DA.SetData(2, obj.PredefRigidity.Name);
                DA.SetData(3, obj.PredefRigidity.Guid);
                if (obj.PredefRigidity.Rigidity._friction == null)
                {  
                    DA.SetData(4, null);
                }
                else
                {    
                    DA.SetData(4, obj.PredefRigidity.Rigidity.Friction);
                }
                DA.SetData(5, obj.PredefRigidity.Rigidity.Motions);
                DA.SetData(6, obj.PredefRigidity.Rigidity.Rotations);
            }
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.EdgeConnectionDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145b6831-bf19-4d89-9e81-9e5e0d137f87"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}