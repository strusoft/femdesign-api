// https://strusoft.com/
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using FemDesign.Sections;


namespace FemDesign.Grasshopper
{
    public class SectionGetSetionProperties : FEM_Design_API_Component
    {
        public SectionGetSetionProperties() : base("Section.GetSectionProperties", "SecProps", "Retrieve geometric properties for cross-sections.", CategoryName.Name(), SubCategoryName.Cat4b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sections", "Sections", "Section list.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Units", "Units", "Connect `Units` to modify the measurement units for section properties. By default, the output is in millimetres.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Height", "h", "Section height.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Width", "w", "Section width.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ey.max", "ey.max", "Max. distance from extreme fibre in the y-direction.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ez.max", "ez.max", "Max. distance from extreme fibre in the z-direction.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area", "A", "Section area.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iy", "Iy", "Moment of inertia about the local y-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iz", "Iz", "Moment of inertia about the local z-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Wy", "Wy", "Section modulus about the local y-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Wz", "Wz", "Section modulus about the local z-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Sy", "Sy", "Max. statical moment about the local y-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Sz", "Sz", "Max. statical moment about the local z-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("It", "It", "Torsional moment of inertia.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Wt", "Wt", "Torsional section modulus.", GH_ParamAccess.list);
            //pManager.AddGenericParameter("Iw", "Iw", "Warping parameter.", GH_ParamAccess.item);
            //pManager.AddGenericParameter("Iyz", "Iyz", "Centroidal product of inertia.", GH_ParamAccess.item);
            //pManager.AddGenericParameter("z omega", "z omega", "Wagner warping parameter.", GH_ParamAccess.item);
            pManager.AddGenericParameter("General", "Gen", "General section properties.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Principal 1", "1", "Section properties computed around the first principal axis.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Principal 2", "2", "Section properties computed around the second principal axis.", GH_ParamAccess.tree);
            pManager.AddTextParameter("SectionNames", "Names", "", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get inputs
            var sections = new List<Section>();
            if (!DA.GetDataList(0, sections)) { return; }

            var units = new Results.UnitResults();
            if(!DA.GetData(1, ref units))
            {
                units.SectionalData = Results.SectionalData.mm;
            }

            // Get section properties
            var secProps = sections.GetSectionProperties(units.SectionalData);

            PropertyInfo[] properties = typeof(Results.SectionProperties).GetProperties();
            properties = properties.Where(p => p.Name != nameof(Results.SectionProperties.Section) && p.Name != nameof(Results.SectionProperties.Composite) && p.Name != nameof(Results.SectionProperties.Other)).ToArray();

            DataTree<double> general = new DataTree<double>();
            DataTree<double> firstP = new DataTree<double>();
            DataTree<double> secondP = new DataTree<double>();

            for (int i = 0; i < properties.Length; i++)
            {

            }

            int i = 0;
            foreach(var prop in properties)
            { 
                var values = secProps.Select(s => (double)prop.GetValue(s));

                general.AddRange(values, new GH_Path(i));

                i++;
            }


            DA.SetDataList("Height", secProps.Select(s => s.Height).ToList());
            DA.SetDataList("Width", secProps.Select(s => s.Width).ToList());
            DA.SetDataList("ey.max", secProps.Select(s => s.eymax).ToList());
            DA.SetDataList("ez.max", secProps.Select(s => s.ezmax).ToList());
            DA.SetDataList("Area", secProps.Select(s => s.A).ToList());
            DA.SetDataList("Iy", secProps.Select(s => s.Iy).ToList());
            DA.SetDataList("Iz", secProps.Select(s => s.Iz).ToList());
            DA.SetDataList("Wy", secProps.Select(s => s.Wy).ToList());
            DA.SetDataList("Wz", secProps.Select(s => s.Wz).ToList());
            DA.SetDataList("Sy", secProps.Select(s => s.Sy).ToList());
            DA.SetDataList("Sz", secProps.Select(s => s.Sz).ToList());
            DA.SetDataList("It", secProps.Select(s => s.It).ToList());
            DA.SetDataList("Wt", secProps.Select(s => s.Wt).ToList());
            DA.SetDataList("SectionNames", secProps.Select(s => s.Section).ToList());
            DA.SetDataTree(13, general);
            DA.SetDataTree(14, firstP);
            DA.SetDataTree(15, secondP);
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
            get { return new Guid("{3AFD6834-3CF0-4F1E-AE3A-C4A1C796420C}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}