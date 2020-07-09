// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class AxisDeconstruct: GH_Component
    {
       public AxisDeconstruct(): base("Axis.Deconstruct", "Deconstruct", "Deconstruct an axis element.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Axis", "Axis", "Axis.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Line", "Line", "Line.", GH_ParamAccess.item);
           pManager.AddTextParameter("Prefix", "Prefix", "Prefix.", GH_ParamAccess.item);
           pManager.AddIntegerParameter("Id", "Id", "Id.", GH_ParamAccess.item);
           pManager.AddBooleanParameter("IdIsLetter", "IdIsLetter", "IdIsLetter.", GH_ParamAccess.item);       
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.StructureGrid.Axis obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.guid);
            DA.SetData(1, new Rhino.Geometry.LineCurve(obj.startPoint.ToRhino(), obj.endPoint.ToRhino()));
            DA.SetData(2, obj.prefix);
            DA.SetData(3, obj.id);
            DA.SetData(4, obj.idIsLetter);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.AxisDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("c07a9120-8013-4227-87bb-da374511fbfe"); }
       }
    }
}