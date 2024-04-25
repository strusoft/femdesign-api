// https://strusoft.com/
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using FemDesign.Sections;
using FemDesign.Reinforcement;

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
            pManager[pManager.ParamCount - 1].Optional = true;
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


            List<string> genPropNames = new List<string>()
            {
                nameof(Results.SectionProperties.Height),
                nameof(Results.SectionProperties.Width),
                nameof(Results.SectionProperties.A),
                nameof(Results.SectionProperties.P),
                nameof(Results.SectionProperties.AP),
                nameof(Results.SectionProperties.Yg),
                nameof(Results.SectionProperties.Zg),
                nameof(Results.SectionProperties.Ys),
                nameof(Results.SectionProperties.Zs),
                nameof(Results.SectionProperties.Iy),
                nameof(Results.SectionProperties.Wy),
                nameof(Results.SectionProperties.ezmax),
                nameof(Results.SectionProperties.ezmin),
                nameof(Results.SectionProperties.iy),
                nameof(Results.SectionProperties.Sy),
                nameof(Results.SectionProperties.Iz),
                nameof(Results.SectionProperties.Wz),
                nameof(Results.SectionProperties.eymax),
                nameof(Results.SectionProperties.eymin),
                nameof(Results.SectionProperties.iz),
                nameof(Results.SectionProperties.Sz),
                nameof(Results.SectionProperties.It),
                nameof(Results.SectionProperties.Wt),
                nameof(Results.SectionProperties.Iw),
                nameof(Results.SectionProperties.Iyz),
                nameof(Results.SectionProperties.zomega)
            };

            List<string> firstPropNames = new List<string>()
            {
                nameof(Results.SectionProperties.alfa1),
                nameof(Results.SectionProperties.I1),
                nameof(Results.SectionProperties.W1min),
                nameof(Results.SectionProperties.W1max),
                nameof(Results.SectionProperties.e2max),
                nameof(Results.SectionProperties.e2min),
                nameof(Results.SectionProperties.i1),
                nameof(Results.SectionProperties.S1),
                nameof(Results.SectionProperties.S01),
                nameof(Results.SectionProperties.c1),
                nameof(Results.SectionProperties.rho1),
                nameof(Results.SectionProperties.z2)
            };

            List<string> secondPropNames = new List<string>()
            {
                nameof(Results.SectionProperties.alfa2),
                nameof(Results.SectionProperties.I2),
                nameof(Results.SectionProperties.W2min),
                nameof(Results.SectionProperties.W2max),
                nameof(Results.SectionProperties.e1max),
                nameof(Results.SectionProperties.e1min),
                nameof(Results.SectionProperties.i2),
                nameof(Results.SectionProperties.S2),
                nameof(Results.SectionProperties.S02),
                nameof(Results.SectionProperties.c2),
                nameof(Results.SectionProperties.rho2),
                nameof(Results.SectionProperties.z1)
            };

            PropertyInfo[] genProps = genPropNames.Select(n => typeof(Results.SectionProperties).GetProperty(n)).ToArray();
            PropertyInfo[] firstProps = firstPropNames.Select(n => typeof(Results.SectionProperties).GetProperty(n)).ToArray();
            PropertyInfo[] secondProps = secondPropNames.Select(n => typeof(Results.SectionProperties).GetProperty(n)).ToArray();
            List<PropertyInfo[]> propList = new List<PropertyInfo[]>() { genProps, firstProps, secondProps };

            List<DataTree<double>> trees = new List<DataTree<double>>() { };

            for (int i = 0; i < propList.Count; i++)
            {
                int path = 0;
                foreach(var prop in propList[i])
                { 
                    var values = secProps.Select(s => (double)prop.GetValue(s));

                    trees[i].AddRange(values, new GH_Path(path));
                    path++;
                }
            }

            DataTree<double> general = trees[0];
            DataTree<double> firstP = trees[1];
            DataTree<double> secondP = trees[2];

            // Set outputs
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