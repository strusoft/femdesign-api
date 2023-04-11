// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign;
using System.Threading.Tasks;
using FemDesign.Calculate;

namespace FemDesign.Grasshopper
{
    public class InteractionSurface_HiddenPipe : GH_Component
    {
        public InteractionSurface_HiddenPipe() : base("InteractionSurface_HiddenPipe", "InteractionSurface_HiddenPipe", "", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("Offset", "Offset", "", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fUlt", "fUlt", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_MeshParam("InteractionSurface", "InteractionSurface", "", GH_ParamAccess.item);
            pManager.Register_DoubleParam("N", "N", "", GH_ParamAccess.item);
            pManager.Register_DoubleParam("My", "My", "", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Mz", "Mz", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<FemDesign.Bars.Bar> bars = new List<Bars.Bar>();
            if (!DA.GetDataList(0, bars)) return;
            
            double offset = 0.0;
            DA.GetData(1, ref offset);

            bool fUlt = true;
            DA.GetData(2, ref fUlt);


            List<Rhino.Geometry.Mesh> interSrf = new List<Mesh>();

            var t = Task.Run( () =>
            {
                var connection = new FemDesignConnection(minimized: true);
                //var script = new FdScript(femDesign.OutputDir, new CmdUser(CmdUserModule.RCDESIGN), new CmdInteractionSurface(bar, outFile, offset, fUlt));
                foreach(var bar in bars)
                {
                    var intSrf = connection.RunInteractionSurface(bar, offset, fUlt);
                    interSrf.Add( intSrf.ToRhino() );
                }
                //femDesign.RunScriptAsync(script).Wait();
                connection.Dispose();
            });

            t.ConfigureAwait(false);
            t.Wait();


            DA.SetDataList("InteractionSurface", interSrf);
            DA.SetData("My", 0);
            DA.SetData("Mz", 0);
            DA.SetData("N", 0);
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
            get { return new Guid("{AF25C82B-6396-44FE-BDDC-AC2B53D89DA3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}