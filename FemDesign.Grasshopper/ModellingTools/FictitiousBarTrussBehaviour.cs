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
    public class FictitiousBarTrussBehaviour : FEM_Design_API_Component
    {
        public FictitiousBarTrussBehaviour() : base("FictitiousBarTrussBehaviour", "FictBarTrussBehaviour", "Define a truss behaviour object for FictitiousBars.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Compression", "Compression", "Optional. Connect 'ValueList' to get the options.\nElastic\nBrittle\nPlastic\n\nDefault value is 'Elastic'.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("LimitForce", "LimitForce", "Optional. Default value is 1.000E+15 kN.\nNote: For FictitiousBars, the limit force is the same for all calculations and only one value can be set.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Tension", "Tension", "Optional. Connect 'ValueList' to get the options.\nElastic\nBrittle\nPlastic\n\nDefault value is 'Elastic'.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("LimitForce", "LimitForce", "Optional. Default value is 1.000E+15 kN.\nNote: For FictitiousBars, the limit force is the same for all calculations and only one value can be set.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousBarTrussBehaviour", "FictBarTrussBehaviour", "FictitiousBar TrussBehaviour", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string compressionBehaviour = ItemChoiceType1.Elastic.ToString();
            DA.GetData(0, ref compressionBehaviour);

            double limitForceComp = 1e+15;
            DA.GetData(1, ref limitForceComp);

            string tensionBehaviour = ItemChoiceType1.Elastic.ToString();
            DA.GetData(2, ref tensionBehaviour);

            double limitForceTens = 1e+15;
            DA.GetData(3, ref limitForceTens);


            Simple_truss_behaviour_type compression = Simple_truss_behaviour_type.Elastic();

            if (compressionBehaviour == ItemChoiceType1.Brittle.ToString())
            {
                compression = Simple_truss_behaviour_type.Brittle(limitForceComp);
            }
            else if (compressionBehaviour == ItemChoiceType1.Plastic.ToString())
            {
                compression = Simple_truss_behaviour_type.Plastic(limitForceComp);
            }


            Simple_truss_behaviour_type tension = Simple_truss_behaviour_type.Elastic();

            if (tensionBehaviour == ItemChoiceType1.Brittle.ToString())
            {
                tension = Simple_truss_behaviour_type.Brittle(limitForceTens);
            }
            else if (tensionBehaviour == ItemChoiceType1.Plastic.ToString())
            {
                tension = Simple_truss_behaviour_type.Plastic(limitForceTens);
            }


            var trussBehaviour = new Simple_truss_chr_type(compression, tension);

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
            get { return new Guid("{E9AC4B0D-1BD3-48BA-903F-E4213CFCBD1D}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;



        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 0, Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.ItemChoiceType1)).ToList(), null);

            ValueListUtils.UpdateValueLists(this, 2, Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.ItemChoiceType1)).ToList(), null);
        }
    }
}