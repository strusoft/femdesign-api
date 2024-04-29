// https://strusoft.com/
using System;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class EdgeConnectionConstruct : FEM_Design_API_Component
    {
        public EdgeConnectionConstruct() : base("EdgeConnection.Construct", "Construct", "Construct a new EdgeConnection", CategoryName.Name(), SubCategoryName.Cat2b())
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
            pManager.AddNumberParameter("Friction", "Friction", "Friction coefficient. Optional.", GH_ParamAccess.item, 0.3);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("DetachType", "DetachType", "Detach type. Optional.", GH_ParamAccess.item, "None");
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
            Releases.Motions _motions = null;
            Releases.Rotations rotations = null;
            Releases.MotionsPlasticLimits motionsPlasticLimit = null;
            Releases.RotationsPlasticLimits rotationsPlasticLimit = null;
            string libraryName = null;
            double friction = 0.3;
            string _detachType = "None";
            if (!DA.GetData("Motions", ref _motions)) return;
            if (!DA.GetData("Rotations", ref rotations)) return;
            if (_motions == null || rotations == null) return;
            DA.GetData("Plastic Limits Forces Motions", ref motionsPlasticLimit);
            DA.GetData("Plastic Limits Moments Rotations", ref rotationsPlasticLimit);
            DA.GetData("LibraryName", ref libraryName);
            DA.GetData("Friction", ref friction);
            DA.GetData("DetachType", ref _detachType);

            var detachType = (Releases.DetachType)Enum.Parse(typeof(Releases.DetachType), _detachType);

            var motions = _motions.DeepClone();

            Shells.EdgeConnection edgeConnection = new Shells.EdgeConnection(motions, motionsPlasticLimit, rotations, rotationsPlasticLimit, friction, detachType, libraryName);


            DA.SetData("EdgeConnection", edgeConnection);
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.EdgeConnectionDefine;
        public override Guid ComponentGuid => new Guid("{4546DDE0-DE6C-450B-88C6-C52E4C279830}");
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 5, Enum.GetNames(typeof(Releases.DetachType)).ToList() , null, GH_ValueListMode.DropDown);
        }
    }
}