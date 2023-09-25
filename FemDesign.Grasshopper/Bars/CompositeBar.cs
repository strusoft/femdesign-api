using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Rhino.Geometry;

using FemDesign;

namespace FemDesign.Grasshopper
{
    public class CompositeBar : GH_Component
    {
        public CompositeBar() : base("Bars.CompositeBar", "CompositeBar", "Create a steel-concrete composite bar element.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
            pManager.AddGenericParameter("BarType", "Type", "Beam or Column. Default value is Beam.",GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("CompositeSection", "Section", "Composite section.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity.If 1 item this item defines both start and end.If two items the first item defines the start and the last item defines the end.Optional, default value if undefined.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity. Optional, default value if undefined.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "B");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CompositeBar", "Bar", "Steel-concrete composite bar element", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input data
            Curve curve = null;
            if(!DA.GetData(0, ref curve)) { return; }

            Bars.BarType barType = Bars.BarType.Beam;
            if(!DA.GetData(1, ref barType)) { /* pass */ }

            Composites.CompositeSection section = null;
            if(!DA.GetData(2, ref section)) { return; }

            List<Bars.Connectivity> connectivity = new List<Bars.Connectivity>();
            if (!DA.GetDataList(3, connectivity))
            {
                var endConn = new List<Bars.Connectivity>() { FemDesign.Bars.Connectivity.Rigid, FemDesign.Bars.Connectivity.Rigid };
                connectivity.AddRange(endConn);
            }

            if (connectivity.Count == 1)
            {
                connectivity.Add(connectivity[0]);
            }
            else if (connectivity.Count != 2)
            {
                throw new ArgumentException("Connectivity list lenght must be equal to 1 or 2!");
            }

            List<Bars.Eccentricity> eccentricity = new List<Bars.Eccentricity>();
            if (!DA.GetDataList(4, eccentricity))
            {
                var ecc = new List<Bars.Eccentricity>() { FemDesign.Bars.Eccentricity.Default, FemDesign.Bars.Eccentricity.Default };
                eccentricity.AddRange(ecc);
            }
            if (eccentricity.Count == 1)
            {
                eccentricity.Add(eccentricity[0]);
            }
            else if (eccentricity.Count != 2)
            {
                throw new ArgumentException("Eccentricity list lenght must be equal to 1 or 2!");
            }

            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(5, ref v))
            {
                // pass
            }

            bool orientLCS = true;
            if (!DA.GetData(6, ref orientLCS))
            {
                // pass
            }

            string identifier = "B";
            if (!DA.GetData(7, ref identifier))
            {
                // pass
            }
        }
    }
}
