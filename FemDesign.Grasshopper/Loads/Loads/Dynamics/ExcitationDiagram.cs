// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign;
using FemDesign.Calculate;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Loads;
using FemDesign.Utils;

namespace FemDesign.Grasshopper
{
    public class ExcitationDiagram : FEM_Design_API_Component
    {
        public ExcitationDiagram() : base("ExcitationDiagram.Define", "Diagram", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of diagram.", GH_ParamAccess.item, "Diagram");
            pManager.AddNumberParameter("Time", "Time", "Time", GH_ParamAccess.list);
            pManager.AddNumberParameter("Values", "Values", "Values", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ExForceDiagram", "ExForceDiagram", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            string name = "";
            DA.GetData(0, ref name);

            List<double> time = new List<double>();
            DA.GetDataList(1, time);

            List<double> values = new List<double>();
            DA.GetDataList(2, values);

            // create diagram
            FemDesign.Loads.Diagram diagram = new FemDesign.Loads.Diagram(name, time, values);

            // output
            DA.SetData(0, diagram);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ExcitationForceDiagram;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{9E770B90-B78C-4564-8601-526E64F98B37}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }
}