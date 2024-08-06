// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Grasshopper.Kernel;
using Rhino.Geometry;

using StruSoft.Interop.StruXml.Data;


namespace FemDesign.Grasshopper
{
    public class FictitiousBarConstruct : FEM_Design_API_Component
    {
        public FictitiousBarConstruct() : base("FictitiousBar.Construct", "Construct", "Construct a fictitious bar element.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
            pManager.AddNumberParameter("AE", "AE", "AE [kN]", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ItG", "ItG", "ItG [kNm2]. This parameter is ignored when using TrussBehaviour.", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("I1E", "I1E", "I1E [kNm2]. This parameter is ignored when using TrussBehaviour.", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("I2E", "I2E", "I2E [kNm2]. This parameter is ignored when using TrussBehaviour.", GH_ParamAccess.item, 1E7);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Mass", "Mass", "Unit mass [t/m].", GH_ParamAccess.item, 0.1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. Connectivity. If 1 item this item defines both start and end connectivity. If two items the first item defines the start connectivity and the last item defines the end connectivity.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
            pManager.AddGenericParameter("TrussBehaviour", "TrussBehaviour", "Optional. If null or empty, this parameter is ignored. To set up the truss behaviour, connect the 'TrussBehaviour' component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "BF");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousBar", "FictBar", "Fictitious bar.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) { return; }

            double stiffness = 1E7;
            double ae = stiffness;
            DA.GetData(1, ref ae);

            double itg = stiffness;
            DA.GetData(2, ref itg);

            double i1e = stiffness;
            DA.GetData(3, ref i1e);

            double i2e = stiffness;
            DA.GetData(4, ref i2e);

            double mass = 0.1;
            DA.GetData(5, ref mass);

            Bars.Connectivity startConnectivity = Bars.Connectivity.Default;
            Bars.Connectivity endConnectivity = Bars.Connectivity.Default;
            List<Bars.Connectivity> connectivity = new List<Bars.Connectivity>();
            if (DA.GetDataList(6, connectivity))
            {
                if (connectivity.Count == 1)
                {
                    startConnectivity = connectivity[0];
                    endConnectivity = connectivity[0];
                }
                else if (connectivity.Count == 2)
                {
                    startConnectivity = connectivity[0];
                    endConnectivity = connectivity[1];
                }
                else
                {
                    throw new System.ArgumentException($"Connectivity must contain 1 or 2 items. Number of items is {connectivity.Count}");
                }
            }

            Vector3d v = Vector3d.Zero;
            DA.GetData(7, ref v);

            bool orientLCS = true;
            DA.GetData(8, ref orientLCS);

            Truss_chr_type trussBehaviour = null;
            DA.GetData(9, ref trussBehaviour);

            string name = "BF";
            DA.GetData(10, ref name);

            // check inputs
            if (curve == null || startConnectivity == null || endConnectivity == null || v == null || name == null)
            {
                return;
            }

            // convert geometry
            Geometry.Edge edge = curve.FromRhinoLineOrArc2();

            // create virtual bar
            ModellingTools.FictitiousBar bar;
            if (trussBehaviour is null)
            {
                bar = new ModellingTools.FictitiousBar(edge, edge.Plane.LocalY, startConnectivity, endConnectivity, name, ae, itg, i1e, i2e, mass);
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Bended bar behaviour ({bar.Name}).");
            }
            else
            {
                // convert Truss_chr_type into Simple_truss_chr_type
                var (behaviour, limForces) = GetFictTrussBehaviourData(trussBehaviour);
                Simple_truss_chr_type fictTrussBehaviour = ModellingTools.FictitiousBar.SetTrussBehaviour(behaviour[0], behaviour[1], limForces[0], limForces[1]);

                if (!curve.IsLinear())
                {
                    throw new System.ArgumentException("For Truss objects, Curve must be a LineCurve!");
                }

                bar = new ModellingTools.FictitiousBar(edge, edge.Plane.LocalY, name, ae, mass, fictTrussBehaviour);
                AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, $"Truss behaviour ({bar.Name}). Connectivity, ItG, I1E and I2E parameters are ignored.");
            }

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.LocalY = v.FromRhino();
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {
                    bar.OrientCoordinateSystemToGCS();
                }
            }

            // output
            DA.SetData(0, bar);

        }
        private (ItemChoiceType1[] behaviour, double[] limForces) GetFictTrussBehaviourData(Truss_chr_type trussBehaviour)
        {
            double[] limits = new double[2];
            ItemChoiceType1[] behav = new ItemChoiceType1[2];
            int i = 0;

            var behavType = new List<Truss_behaviour_type>
            {
                trussBehaviour.Compression,     // order matters!
                trussBehaviour.Tension
            };
            foreach (var type in behavType)
            {
                behav[i] = GetBehaviour(type);
                limits[i] = GetLimitForces(type);
                i++;
            }

            return (behav, limits);
        }
        private ItemChoiceType1 GetBehaviour(Truss_behaviour_type type)
        {
            switch (type.ItemElementName)
            {
                case ItemChoiceType.Elastic:
                    return ItemChoiceType1.Elastic;
                case ItemChoiceType.Brittle:
                    return ItemChoiceType1.Brittle;
                case ItemChoiceType.Plastic:
                    return ItemChoiceType1.Plastic;
                default: throw new Exception("Unknown behaviour type!");
            }
        }
        private double GetLimitForces(Truss_behaviour_type type)
        {
            double limit = 0;

            if (type.ItemElementName is ItemChoiceType.Brittle || type.ItemElementName is ItemChoiceType.Plastic)
            {
                var obj = type.Item;
                PropertyInfo limForcePropName = typeof(Truss_capacity_type).GetProperty(nameof(Truss_capacity_type.Limit_force));
                var values = limForcePropName.GetValue(obj);

                if (values is IEnumerable<Truss_limit_type>)
                {
                    PropertyInfo countPropName = values.GetType().GetProperty("Count");
                    if (countPropName != null)
                    {
                        int count = (int)countPropName.GetValue(values);
                        if (count > 1)
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "For FictitiousBars, the limit force is the same for all calculations! " +
                                "This object uses only the first compression and tension limit force values from the 'TrussBehaviour' component.");

                        IEnumerable<Truss_limit_type> list = (IEnumerable<Truss_limit_type>)values;
                        limit = list.ToList()[0].Value;
                    }
                }
                else
                {
                    throw new Exception("'Item.Limit_force' must be List<Truss_limit_type>!");
                }
            }

            return limit;
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FictBar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{4E93CC5F-E3E4-4CE1-BB5A-2E5922E97BA0}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}