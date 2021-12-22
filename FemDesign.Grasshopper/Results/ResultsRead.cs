// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class ReadResults : GH_Component
    {
        public ReadResults() : base("Results.Read", "Results", "Read results.", "FemDesign", "Results")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "path", "File path to the results file to be read.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("results", "results", "Read results", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            DA.GetData("filePath", ref path);

            var results = Results.ResultsReader.Parse(path);

            DA.SetDataList("results", results);
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
            get { return new Guid("9d4151ad-d873-4739-95e3-0557dcf2bf84"); }
        }
    }
}