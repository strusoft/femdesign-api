// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign;
using System.Threading.Tasks;
using FemDesign.Calculate;
using System.Reflection;
using Grasshopper;

namespace FemDesign.Grasshopper
{
    public partial class Rebar
    {
        public Rhino.Geometry.Point3d Pos { get; set; }
        public FemDesign.Materials.Material ReinforcingMaterial { get; set; }
        public double Diameter { get; set; }
        public FemDesign.Reinforcement.WireProfileType WireProfileType { get; set; }

        public Rebar()
        {

        }
        public Rebar(Point3d point, double diameter, Materials.Material reinfMaterial, Reinforcement.WireProfileType wireProfileType)
        {
            Pos = point;
            Diameter = diameter;
            if(reinfMaterial.Family != Materials.Family.ReinforcingSteel)
                throw new ArgumentException("Material must be Reinforcing Steel");
            ReinforcingMaterial = reinfMaterial;
            WireProfileType = wireProfileType;

        }

        public static implicit operator FemDesign.Reinforcement.BarReinforcement(Rebar rebar)
        {
            var pos = new FemDesign.Geometry.Point2d(rebar.Pos.X, rebar.Pos.Y);
            var longBar = new FemDesign.Reinforcement.LongitudinalBar(pos, 1.0, 1.0, 0.0, 1.0, false);
            var wire = new FemDesign.Reinforcement.Wire(rebar.Diameter, rebar.ReinforcingMaterial, Reinforcement.WireProfileType.Ribbed);

            return new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, longBar);
        }
    }

    public class Patch
    {
        public Rhino.Geometry.Brep Srf { get; set; }

        private FemDesign.Materials.Material _material { get; set; }
        public FemDesign.Materials.Material Material
        {
            get { return _material; }
            set
            {
                if (value.Family != Materials.Family.Concrete)
                {
                    throw new ArgumentException("Material must be Concrete");
                }
                else
                    _material = value;
            }
        }
        public Patch()
        {

        }
        public Patch(Brep srf, Materials.Material material)
        {
            Srf = srf;
            Material = material;
        }
    }

    public class Layer
    {
        public Rhino.Geometry.Curve Curve { get; set; }
        public int NumberOfBar { get; set; }
        public FemDesign.Materials.Material ReinforcingMaterial { get; set; }
        public double Diameter { get; set; }
        public FemDesign.Reinforcement.WireProfileType WireProfileType { get; set; }

        public Layer()
        {

        }
        public Layer(Curve curve, int numberOfRebar, double diameter, Materials.Material material, Reinforcement.WireProfileType wireProfileType)
        {
            Curve = curve;
            NumberOfBar = numberOfRebar;
            Diameter = diameter;
            if (material.Family != Materials.Family.ReinforcingSteel)
                throw new ArgumentException("Material must be Reinforcing Steel");
            ReinforcingMaterial = material;
            WireProfileType = wireProfileType;
        }

        public static implicit operator List<FemDesign.Reinforcement.BarReinforcement>(Layer layer)
        {
            var barReinfs = new List<FemDesign.Reinforcement.BarReinforcement>();

            layer.Curve.DivideByCount(layer.NumberOfBar - 1, true, out Point3d[] points);

            foreach(Point3d point in points)
            {
                var pos = new FemDesign.Geometry.Point2d(point.X, point.Y);
                var longBar = new FemDesign.Reinforcement.LongitudinalBar(pos, 1.0, 1.0, 0.0, 1.0, false);
                var wire = new FemDesign.Reinforcement.Wire(layer.Diameter, layer.ReinforcingMaterial, Reinforcement.WireProfileType.Ribbed);

                var barReinf = new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, longBar);
                barReinfs.Add(barReinf);
            }

            return barReinfs;
        }

    }


    public class InteractionSurface_section : GH_Component
    {
        public InteractionSurface_section() : base("InteractionSurface.OnSection", "InteractionSurface.OnSection", "", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Patch", "Patch", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rebars", "Rebars", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Layers", "Layers", "", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fUlt", "fUlt", "fUlt is true for Ultimate, false for Accidental or Seismic  combination (different gammaC)", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_MeshParam("InteractionSurface", "InteractionSurface", "");
            pManager.Register_IntervalParam("N", "N", "");
            pManager.Register_IntervalParam("My", "My", "");
            pManager.Register_IntervalParam("Mz", "Mz", "");
            pManager.Register_GenericParam("Bar", "Bar", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var patch = new Patch();
            if (!DA.GetData(0, ref patch)) return;

            var rebars = new List<Rebar>();
            if (!DA.GetDataList(1, rebars)) return;

            var layers = new List<Layer>();
            DA.GetDataList(2, layers);

            bool fUlt = true;
            DA.GetData(3, ref fUlt);


            var areamassproperty = Rhino.Geometry.AreaMassProperties.Compute(patch.Srf);
            var centroid = areamassproperty.Centroid;
            var normal = patch.Srf.Surfaces.First().NormalAt(0,0);
            var initialPosition = new Rhino.Geometry.Plane(centroid, normal);
            var finalPosition = Rhino.Geometry.Plane.WorldXY;
            var transformation = Rhino.Geometry.Transform.PlaneToPlane(initialPosition, finalPosition);

            // Copy surface from Patch
            var _srf = patch.Srf.DuplicateBrep();
            _srf.Transform(transformation);

            // Create Section
            List<FemDesign.Geometry.Region> regions = new List<FemDesign.Geometry.Region>();
            regions.Add(_srf.FromRhino());
            FemDesign.Geometry.RegionGroup regionGroup = new FemDesign.Geometry.RegionGroup(regions);
            var section = new FemDesign.Sections.Section(regionGroup, "custom", Materials.MaterialTypeEnum.Concrete, "groupName", "typeName", "sizeName");


            var reinforcements = new List<FemDesign.Reinforcement.BarReinforcement>();

            foreach(var reinf in rebars)
            {
                var movedPoint = new Rhino.Geometry.Point3d( reinf.Pos );
                movedPoint.Transform(transformation);

                reinf.Pos = movedPoint;
                reinforcements.Add(reinf);
            }

            foreach (var layer in layers)
            {
                var _curve = layer.Curve.DuplicateCurve();
                _curve.Transform(transformation);
                layer.Curve = _curve;

                var _reinf = (List<FemDesign.Reinforcement.BarReinforcement>)layer;
                reinforcements.AddRange(_reinf);
            }


            var edge = new FemDesign.Geometry.Edge( new Geometry.Point3d(0,0,0), new Geometry.Point3d(1,0,0) );
            var bar = new FemDesign.Bars.Bar(edge, Bars.BarType.Beam, patch.Material, section);
            bar = FemDesign.Reinforcement.BarReinforcement.AddReinforcementToBar(bar, reinforcements, true);


            // Outputs
            List<Rhino.Geometry.Mesh> interSrf = new List<Mesh>();

            var n = new List<Rhino.Geometry.Interval>();
            var my = new List<Rhino.Geometry.Interval>();
            var mz = new List<Rhino.Geometry.Interval>();

            // Create Task
            var t = Task.Run(() =>
            {
                var connection = new FemDesignConnection(minimized: true);

                // our dummy beam has length == 1
                var offset = 0.5;
                var intSrf = connection.RunInteractionSurface(bar, offset, fUlt);
                foreach (var _intSrf in intSrf)
                {
                    interSrf.Add(_intSrf.ToRhino());

                    var nMin = _intSrf.Vertices.Values.Select(x => x.Z).Min();
                    var nMax = _intSrf.Vertices.Values.Select(x => x.Z).Max();

                    var interval = new Rhino.Geometry.Interval(nMin, nMax);
                    n.Add(interval);

                    var myMin = _intSrf.Vertices.Values.Select(x => x.X).Min();
                    var myMax = _intSrf.Vertices.Values.Select(x => x.X).Max();

                    interval = new Rhino.Geometry.Interval(myMin, myMax);
                    my.Add(interval);

                    var mzMin = _intSrf.Vertices.Values.Select(x => x.Y).Min();
                    var mzMax = _intSrf.Vertices.Values.Select(x => x.Y).Max();

                    interval = new Rhino.Geometry.Interval(mzMin, mzMax);
                    mz.Add(interval);
                }

                // Close FEM-Design
                connection.Dispose();
            });

            t.ConfigureAwait(false);
            t.Wait();


            DA.SetDataList("InteractionSurface", interSrf);
            DA.SetDataList("N", n);
            DA.SetDataList("My", my);
            DA.SetDataList("Mz", mz);
            DA.SetData("Bar", bar);
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
            get { return new Guid("{AC808251-F7B0-4892-8106-225F9FCBF124}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}