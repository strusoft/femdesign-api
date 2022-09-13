// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class EdgeConnectionConstruct: GH_Component
    {
        public EdgeConnectionConstruct(): base("EdgeConnection.Construct", "Construct", "Construct a new EdgeConnection", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimM", "Plastic limits forces for motion springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Plastic Limits Moments Rotations", "PlaLimR", "Plastic limits moments for rotation springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("LibraryName", "LibraryName", "When libraryName is not null or empty, the edge connection will be treated as a \"predefined/library\" item. Default is to treat is a a unique \"custom\" edge connection.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Releases.Motions motions = null;
            Releases.Rotations rotations = null;
            Releases.MotionsPlasticLimits motionsPlasticLimit = null;
            Releases.RotationsPlasticLimits rotationsPlasticLimit = null;
            string libraryName = null;
            if (!DA.GetData("Motions", ref motions)) return;
            if (!DA.GetData("Rotations", ref rotations)) return;
            if (motions == null || rotations == null) return;
            DA.GetData("Plastic Limits Forces Motions", ref motionsPlasticLimit);
            DA.GetData("Plastic Limits Moments Rotations", ref rotationsPlasticLimit);
            DA.GetData("LibraryName", ref libraryName);

            Shells.EdgeConnection edgeConnection = new Shells.EdgeConnection(motions, motionsPlasticLimit, rotations, rotationsPlasticLimit, libraryName);

            DA.SetData("EdgeConnection", edgeConnection);
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.EdgeConnectionDefine;
        public override Guid ComponentGuid => new Guid("d4748927-2190-4444-82a0-82df4593eca6");

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}