// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using FemDesign;
using System.Collections.Generic;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Grasshopper
{
    public class TrussBehaviour : GH_Component
    {
        public TrussBehaviour() : base("TrussBehaviour", "TrussBehaviour", "Define a truss behaviour object.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Compression", "Compression", "Connect 'ValueList' to get the options.\nElastic\nBrittle\nPlastic", GH_ParamAccess.item);
            pManager.AddNumberParameter("LimitForce", "LimitForce", "Use 1 value if you want to set the same for all calculations. Otherwise, you can specify 9 values which will be map to the following calculation type.\nFirstOrderU\nFirstOrderSq\nFirstOrderSf\nFirstOrderSc\nSecondOrderU\nSecondOrderSq\nSecondOrderSf\nSecondOrderSc\nStability", GH_ParamAccess.list);
            pManager.AddTextParameter("Tension", "Tension", "Connect 'ValueList' to get the options.\nElastic\nBrittle\nPlastic", GH_ParamAccess.item);
            pManager.AddNumberParameter("LimitForce", "LimitForce", "Use 1 value if you want to set the same for all calculations. Otherwise, you can specify 9 values which will be map to the following calculation type.\nFirstOrderU\nFirstOrderSq\nFirstOrderSf\nFirstOrderSc\nSecondOrderU\nSecondOrderSq\nSecondOrderSf\nSecondOrderSc\nStability", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TrussBehaviour", "TrussBehaviour", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input

            string compressionBehaviour = null;
            if (!DA.GetData(0, ref compressionBehaviour)) { return; }

            var limitForceComp = new List<double>();
            if (!DA.GetDataList(1, limitForceComp)) { return; }

            string tensionBehaviour = null;
            if (!DA.GetData(2, ref tensionBehaviour)) { return; }

            var limitForceTens = new List<double>();
            if (!DA.GetDataList(3, limitForceTens)) { return; }

            Truss_behaviour_type compression = Truss_behaviour_type.Elastic();

            if(compressionBehaviour == ItemChoiceType.Brittle.ToString())
            {
                compression = Truss_behaviour_type.Brittle(limitForceComp);
            }
            else if (compressionBehaviour == ItemChoiceType.Plastic.ToString())
            {
                compression = Truss_behaviour_type.Plastic(limitForceComp);
            }


            Truss_behaviour_type tension = Truss_behaviour_type.Elastic();

            if (tensionBehaviour == ItemChoiceType.Brittle.ToString())
            {
                tension = Truss_behaviour_type.Brittle(limitForceTens);
            }
            else if (tensionBehaviour == ItemChoiceType.Plastic.ToString())
            {
                tension = Truss_behaviour_type.Plastic(limitForceTens);
            }


            var trussBehaviour = new Truss_chr_type(compression, tension);
            // return
            DA.SetData(0, trussBehaviour);
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
            get { return new Guid("{D00B5B5B-D427-4403-A831-16D052E6A1B0}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;



        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 0, Enum.GetNames(typeof( StruSoft.Interop.StruXml.Data.ItemChoiceType)).ToList(), null);

            ValueListUtils.updateValueLists(this, 2, Enum.GetNames(typeof( StruSoft.Interop.StruXml.Data.ItemChoiceType)).ToList(), null);
        }
    }
}