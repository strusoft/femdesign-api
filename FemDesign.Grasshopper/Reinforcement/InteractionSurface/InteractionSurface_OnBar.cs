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
    public class InteractionSurface_OnBar : GH_Component
    {
        public InteractionSurface_OnBar() : base("InteractionSurface.OnBar", "InteractionSurface.OnBar", "Calculate the interaction surface for a concrete beam with rebars", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("fUlt", "fUlt", "fUlt is true for Ultimate, false for Accidental or Seismic  combination (different gammaC)", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Offset", "Offset", "cross-section position, measured along the bar from the starting point [m]", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_MeshParam("InteractionSurface", "InteractionSurface", "Mesh representation of the 'Onion Shape'. The mesh vertices have been mapped in such a way that x,y,z matched My [kNm], Mz [kNm], N [kN].");
            pManager.Register_IntervalParam("N", "N", "Min/Max capacity value. [kN]");
            pManager.Register_IntervalParam("My", "My", "Min/Max capacity value. [kNm]");
            pManager.Register_IntervalParam("Mz", "Mz", "Min/Max capacity value. [kNm]");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var bars = new List<FemDesign.GenericClasses.IStructureElement>();
            if (!DA.GetDataList(0, bars)) return;
            
            bool fUlt = true;
            DA.GetData(1, ref fUlt);

            double offset = 0.0;
            DA.GetData(2, ref offset);


            #region FILE CREATION
            // set Output directory
            bool fileExist = OnPingDocument().IsFilePathDefined;
            if (!fileExist)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Save your .gh script to run the Interaction Surface.");
                return;
            }

            var _ghfileDir = System.IO.Path.GetDirectoryName(OnPingDocument().FilePath);
            System.IO.Directory.SetCurrentDirectory(_ghfileDir);
            #endregion

            // Outputs
            List<Rhino.Geometry.Mesh> interSrf = new List<Mesh>();

            var n = new List<Rhino.Geometry.Interval>();
            var my = new List<Rhino.Geometry.Interval>();
            var mz = new List<Rhino.Geometry.Interval>();

            // Create Task
            var t = Task.Run( () =>
            {
                var connection = new FemDesignConnection(minimized: true, tempOutputDir: true);

                // our dummy beam has length == 1
                var intSrf = connection.RunInteractionSurface( bars, offset, fUlt);
                foreach(var _intSrf in intSrf)
                {
                    interSrf.Add( _intSrf.ToRhino() );

                    var nMin = _intSrf.Vertices.Values.Select(x => x.Z ).Min();
                    var nMax = _intSrf.Vertices.Values.Select(x => x.Z ).Max();

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
            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }


            DA.SetDataList("InteractionSurface", interSrf);
            DA.SetDataList("N", n);
            DA.SetDataList("My", my);
            DA.SetDataList("Mz", mz);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.InteractionSurface;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{AF25C82B-6396-44FE-BDDC-AC2B53D89DA3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}