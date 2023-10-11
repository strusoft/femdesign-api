// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Rhino.Geometry;

using FemDesign;

using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using FemDesign.Bars;

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
            pManager.AddGenericParameter("BarType", "Type", "Connect 'ValueList' to get the options.\nBarType can be:\nBeam\nColumn\n. Default value is Beam.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("CompositeSection", "Section", "Steel-concrete composite section.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity. If 1 item this item defines both start and end. If two items the first item defines the start and the last item defines the end.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "", GH_ParamAccess.item, "B");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CompositeBar", "Bar", "Steel-concrete composite bar element.", GH_ParamAccess.item);
        }
        protected override void BeforeSolveInstance()
        {
            var valListNames = new List<string>() { Bars.BarType.Beam.ToString(), Bars.BarType.Column.ToString() };
            ValueListUtils.updateValueLists(this, 1, valListNames, null, GH_ValueListMode.DropDown);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) { return; }

            string barType = Bars.BarType.Beam.ToString();
            DA.GetData(1, ref barType);
            BarType _barType = FemDesign.GenericClasses.EnumParser.Parse<BarType>(barType);

            Composites.CompositeSection section = null;
            if (!DA.GetData(2, ref section)) { return; }

            List<Bars.Connectivity> connectivity = new List<Bars.Connectivity>();
            if (!DA.GetDataList(3, connectivity))
            {
                var endConn = new List<Bars.Connectivity>() { Bars.Connectivity.Rigid, Bars.Connectivity.Rigid };
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
                var ecc = new List<Bars.Eccentricity>() { Bars.Eccentricity.Default, Bars.Eccentricity.Default };
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
            DA.GetData(5, ref v);

            bool orientLCS = true;
            DA.GetData(6, ref orientLCS);

            string identifier = "B";
            DA.GetData(7, ref identifier);

            // check input data
            if (curve == null || section == null || connectivity == null || eccentricity == null || identifier == null)
            {
                return;
            }
            if (_barType == Bars.BarType.Truss)
            {
                throw new ArgumentException($"BarType must be Beam or Column, but it is {barType}.");
            }

            // create composite bar
            Geometry.Edge line = curve.FromRhinoLineOrArc2();
            var bar = new Bars.Bar(line, _barType, section, eccentricity[0], eccentricity[1], connectivity[0], connectivity[1], identifier);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.BarPart.LocalY = v.FromRhino();
            }
            // else orient coordinate system to GCS
            else if (orientLCS)
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            // get output
            DA.SetData(0, bar);
            v.IsPerpendicularTo(v);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BeamDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{4C301630-8109-4962-8933-0FFCCAE39501}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}