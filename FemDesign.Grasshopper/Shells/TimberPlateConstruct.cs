// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class TimberPlateConstruct: GH_Component
    {
        public TimberPlateConstruct(): base("TimberPlate.Construct", "Construct", "Construct a timber plate", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddGenericParameter("TimberPlateMaterial", "Material", "Timber plate material.", GH_ParamAccess.item);
            pManager.AddVectorParameter("SpanDirection", "Direction", "Span direction of the timber plate.", GH_ParamAccess.item);
            pManager.AddNumberParameter("PanelWidth", "PanelWidth", "Width of each individual CLT panel in region. 1.5m if undefined.", GH_ParamAccess.item); 
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ShellEccentricity", "Eccentricity", "ShellEccentricity. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("BorderEdgeConnection", "BorderEdgeConnection", "EdgeConnection of the external border of the panel. Optional. If not defined hinged will be used.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalX", "LocalX", "Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalZ", "LocalZ", "Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("AvgMeshSize", "AverageMeshSize", "Average mesh size. If zero an automatic value will be used by FEM-Design. Optional. [m]", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional.", GH_ParamAccess.item, "PP");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TimberPlate", "TP", "Timber plate.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep surface = null;
            if (!DA.GetData("Surface", ref surface)) return;

            Materials.TimberPanelType timberPlateMaterialData = null;
            if (!DA.GetData("TimberPlateMaterial", ref timberPlateMaterialData)) return;

            Vector3d spanDirection = new Vector3d();
            if (!DA.GetData("SpanDirection", ref spanDirection)) return;

            double panelWidth = 1.5;
            DA.GetData("PanelWidth", ref panelWidth);

            Shells.ShellEccentricity eccentricity = Shells.ShellEccentricity.Default;
            DA.GetData("ShellEccentricity", ref eccentricity);
            
            Shells.EdgeConnection edgeConnection = Shells.EdgeConnection.Hinged;
            DA.GetData("BorderEdgeConnection", ref edgeConnection);

            Rhino.Geometry.Vector3d x = Vector3d.Zero;
            DA.GetData("LocalX", ref x);

            Rhino.Geometry.Vector3d z = Vector3d.Zero;
            DA.GetData("LocalZ", ref z);

            double meshSize = 0;
            DA.GetData("AvgMeshSize", ref meshSize);
            
            string identifier = "PP";
            DA.GetData("Identifier", ref identifier);

            if (surface == null || timberPlateMaterialData == null || eccentricity == null || edgeConnection == null || identifier == null)
                return;

            
            Geometry.Region region = surface.FromRhino();
            Geometry.Vector3d dir = spanDirection.FromRhino();
            Shells.Panel obj = Shells.Panel.DefaultTimberContinuous(region, timberPlateMaterialData, dir, edgeConnection, identifier, eccentricity,  panelWidth);

            // set local x-axis
            if (!x.Equals(Vector3d.Zero))
                obj.LocalX = x.FromRhino();

            // set local z-axis
            if (!z.Equals(Vector3d.Zero))
                obj.LocalZ = z.FromRhino();

            // set uniform average mesh size
            obj.UniformAvgMeshSize = meshSize;

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PanelTimberPlateDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("22baacb3-0b76-41f4-a5bc-cd9e60d13be7"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}