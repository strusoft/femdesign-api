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
    public class TrussBehaviour : FEM_Design_API_Component
    {
        public TrussBehaviour() : base("TrussBehaviour", "TrussBehaviour", "Define a truss behaviour object. For FictitiousBars, use the 'FictitiousBarTrussBehaviour' component.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Compression", "Compression", "Optional. Connect 'ValueList' to get the options.\nElastic\nBrittle\nPlastic\n\nDefault value is 'Elastic'.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("LimitForce", "LimitForce", "Optional. Default value is 1.000E+15 kN.\nUse 1 value if you want to set the same for all calculations. Otherwise, you can specify 9 values which will be map to the following calculation types:\nFirstOrderU\nFirstOrderSq\nFirstOrderSf\nFirstOrderSc\nSecondOrderU\nSecondOrderSq\nSecondOrderSf\nSecondOrderSc\nStability", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Tension", "Tension", "Optional. Connect 'ValueList' to get the options.\nElastic\nBrittle\nPlastic\n\nDefault value is 'Elastic'.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("LimitForce", "LimitForce", "Optional. Default value is 1.000E+15 kN.\nUse 1 value if you want to set the same for all calculations. Otherwise, you can specify 9 values which will be map to the following calculation types:\nFirstOrderU\nFirstOrderSq\nFirstOrderSf\nFirstOrderSc\nSecondOrderU\nSecondOrderSq\nSecondOrderSf\nSecondOrderSc\nStability", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TrussBehaviour", "TrussBehaviour", "TrussBehaviour", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string compressionBehaviour = ItemChoiceType.Elastic.ToString();
            DA.GetData(0, ref compressionBehaviour);

            var limitForceComp = new List<double>();
            if (!DA.GetDataList(1, limitForceComp))
                limitForceComp = new List<double>() { 1e+15 };

            string tensionBehaviour = ItemChoiceType.Elastic.ToString();
            DA.GetData(2, ref tensionBehaviour);

            var limitForceTens = new List<double>();
            if (!DA.GetDataList(3, limitForceTens))
                limitForceTens = new List<double>() { 1e+15 };

            // check inputs
            if (limitForceComp == null || limitForceTens == null) { return; }

            Truss_behaviour_type compression = Truss_behaviour_type.Elastic();

            if (compressionBehaviour == ItemChoiceType.Brittle.ToString())
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
                return FemDesign.Properties.Resources.TrussBehaviour;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{30D9BF01-EE29-4C16-8537-79D7EF16F5AD}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;



        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 0, Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.ItemChoiceType)).ToList(), null);

            ValueListUtils.UpdateValueLists(this, 2, Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.ItemChoiceType)).ToList(), null);
        }
    }
}