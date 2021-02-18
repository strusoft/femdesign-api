// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class ProfiledPlateDefine: GH_Component
    {
        public ProfiledPlateDefine(): base("ProfiledPlate.Define", "Define", "Create a profiled plate", "FemDesign", "Shells")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Mat", "Material.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Section", "Sec", "Section.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ShellEccentricity", "Eccentricity", "ShellEccentricity. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("OrthoRatio", "OrthoRatio", "Transversal flexural stiffness factor.", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ShellEdgeConnection", "EdgeConnection", "ShellEdgeConnection. Optional.", GH_ParamAccess.item);
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
            pManager.AddGenericParameter("ProfiledPlate", "PP", "-", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep surface = null;
            if (!DA.GetData(0, ref surface))
            {
                return;
            }

            FemDesign.Materials.Material material = null;
            if (!DA.GetData(1, ref material))
            {
                return;
            }

            FemDesign.Sections.Section section = null;
            if (!DA.GetData(2, ref section))
            {
                return;
            }

            FemDesign.Shells.ShellEccentricity eccentricity = FemDesign.Shells.ShellEccentricity.Default();
            if(!DA.GetData(3, ref eccentricity))
            {
                // pass
            }
            
            double orthoRatio = 1;
            if(!DA.GetData(4, ref orthoRatio))
            {
                // pass
            }
            
            FemDesign.Shells.ShellEdgeConnection edgeConnection = FemDesign.Shells.ShellEdgeConnection.Rigid();
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

            if (surface == null || material == null || section == null || eccentricity == null || edgeConnection == null || identifier == null)
            {
                return;
            }

            
            FemDesign.Geometry.Region region = FemDesign.Geometry.Region.FromRhino(surface);

            //
            FemDesign.Shells.Panel obj = FemDesign.Shells.Panel.DefaultContreteContinuous(region, edgeConnection.Rigidity, material, section, identifier, orthoRatio, eccentricity);

            // set local x-axis
            if (!x.Equals(Vector3d.Zero))
            {
                obj.LocalX = FemDesign.Geometry.FdVector3d.FromRhino(x);
            }

            // set local z-axis
            if (!z.Equals(Vector3d.Zero))
            {
                obj.LocalZ = FemDesign.Geometry.FdVector3d.FromRhino(z);
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
                return FemDesign.Properties.Resources.ProfiledPlateDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("f2cc84f9-9831-414f-916d-65b1163ac1ce"); }
        }
    }
}