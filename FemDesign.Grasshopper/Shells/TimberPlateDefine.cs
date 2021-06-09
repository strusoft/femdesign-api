// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class TimberPlateDefine: GH_Component
    {
        public TimberPlateDefine(): base("TimberPlate.Define", "Define", "Create a timber plate", "FemDesign", "Shells")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface.", GH_ParamAccess.item);
            pManager.AddGenericParameter("TimberPlateMaterial", "Mat", "Timber plate material.", GH_ParamAccess.item);
            pManager.AddNumberParameter("PanelWidth", "PanelWidth", "Width of each individual CLT panel in region. 1.5m if undefined.", GH_ParamAccess.item); 
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ShellEccentricity", "Eccentricity", "ShellEccentricity. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("BorderShellEdgeConnection", "BorderEdgeConnection", "ShellEdgeConnection of the external border of the panel. Optional. If not defined hinged will be used.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalX", "LocalX", "Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalZ", "LocalZ", "Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("AvgMeshSize", "AverageMeshSize", "Average mesh size. If zero an automatic value will be used by FEM-Design. Optional.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional.", GH_ParamAccess.item, "PP");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TimberPlate", "PP", "-", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep surface = null;
            if (!DA.GetData(0, ref surface))
            {
                return;
            }

            FemDesign.Materials.TimberApplicationData timberAppData = null;
            if (!DA.GetData(1, ref timberAppData))
            {
                return;
            }

            double panelWidth = 1.5;
            if (!DA.GetData(2, ref panelWidth))
            {
                // pass
            }

            FemDesign.Shells.ShellEccentricity eccentricity = FemDesign.Shells.ShellEccentricity.GetDefault();
            if(!DA.GetData(2, ref eccentricity))
            {
                // pass
            }
            
            FemDesign.Shells.ShellEdgeConnection edgeConnection = FemDesign.Shells.ShellEdgeConnection.GetHinged();
            if(!DA.GetData(5, ref edgeConnection))
            {
                // pass
            }

            Rhino.Geometry.Vector3d x = Vector3d.Zero;
            if (!DA.GetData(6, ref x))
            {
                // pass
            }

            Rhino.Geometry.Vector3d z = Vector3d.Zero;
            if (!DA.GetData(7, ref z))
            {
                // pass
            }

            double meshSize = 0;
            if (!DA.GetData(8, ref meshSize))
            {
                // pass
            }
            
            string identifier = "PP";
            if (!DA.GetData(9, ref identifier))
            {
                // pass
            }

            if (surface == null || timberAppData == null || eccentricity == null || edgeConnection == null || identifier == null)
            {
                return;
            }

            
            FemDesign.Geometry.Region region = surface.FromRhino();

            //
            FemDesign.Shells.Panel obj = FemDesign.Shells.Panel.DefaultTimberContinuous(region,timberAppData, edgeConnection, identifier, eccentricity,  panelWidth);

            // set local x-axis
            if (!x.Equals(Vector3d.Zero))
            {
                obj.LocalX = x.FromRhino();
            }

            // set local z-axis
            if (!z.Equals(Vector3d.Zero))
            {
                obj.LocalZ = z.FromRhino();
            }

            // set uniform average mesh size
            obj.UniformAvgMeshSize = meshSize;

            // return
            DA.SetData(0, obj);
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
            get { return new Guid("22baacb3-0b76-41f4-a5bc-cd9e60d13be7"); }
        }
    }
}