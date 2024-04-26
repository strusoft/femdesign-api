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
using System.Threading.Tasks;
using FemDesign.Results;

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
            pManager.AddTextParameter("Units", "Units", "Connect `Units for section properties. By default, the output is in millimetres. Accepted input are: \n " +
                "mm, cm, dm, m, inch, feet, yd", GH_ParamAccess.item, "mm");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Height", "h", "Section height.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Width", "w", "Section width.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area", "A", "Section area.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iy", "Iy", "Moment of inertia about the local y-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Iz", "Iz", "Moment of inertia about the local z-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Wy", "Wy", "Section modulus about the local y-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Wz", "Wz", "Section modulus about the local z-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("It", "It", "Torsional moment of inertia.", GH_ParamAccess.list);
            pManager.AddGenericParameter("General", "Gen", "General section properties.\n" +
                "1 - Height\n2 - Width\n3 - A\n4 - P\n5 - A/P\n6 - Yg\n7 - Zg\n8 - Ys\n9 - Zs\n10 - Iy\n11 - Wy\n12 - ez.max\n13 - ez.min\n14 - iy\n15 - Sy\n" +
                "16 - Iz\n17 - Wz\n18 - ey.max\n19 - ey.min\n20 - iz\n21 - Sz\n22 - It\n23 - Wt\n24 - Iw\n25 - Iyz\n26 - zomega", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Principal 1", "1", "Section properties computed around the first principal axis.\n" +
                "1 - alfa1\n2 - I1\n3 - W1.min\n4 - W1.max\n5 - e2.max\n6 - e2.min\n7 - i1\n8 - S1\n9 - S01\n10 - c1\n11 - rho1\n12 - z2", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Principal 2", "2", "Section properties computed around the second principal axis.\n" +
                "1 - alfa2\n2 - I2\n3 - W2.min\n4 - W2.max\n5 - e1.max\n6 - e1.min\n7 - i2\n8 - S2\n9 - S02\n10 - c2\n11 - rho2\n12 - z1", GH_ParamAccess.tree);
            pManager.AddTextParameter("SectionNames", "Names", "Section names.", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get inputs
            var sections = new List<Section>();
            if (!DA.GetDataList(0, sections)) { return; }

            SectionalData unit = SectionalData.mm;
            string sectionalData = null;
            if(DA.GetData(1, ref sectionalData))
            {
                unit = (Results.SectionalData)Enum.Parse(typeof(Results.SectionalData), sectionalData);
            }

            // Create Task to get section properties
            List<Results.SectionProperties> secProps = new List<Results.SectionProperties>();
            var t = Task.Run(() =>
            {
                secProps = sections.GetSectionProperties(unit);
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
                var tree = new DataTree<double>();
                int path = 0;
                foreach(var sec in secProps)
                {
                    var values = propList[i].Select(p => (double)p.GetValue(sec));
                    tree.AddRange(values, new GH_Path(path));
                    path++;
                }
                trees.Add(tree);
            }

            DataTree<double> general = trees[0];
            DataTree<double> firstP = trees[1];
            DataTree<double> secondP = trees[2];

            // Set outputs
            DA.SetDataList("Height", secProps.Select(s => s.Height).ToList());
            DA.SetDataList("Width", secProps.Select(s => s.Width).ToList());
            DA.SetDataList("Area", secProps.Select(s => s.A).ToList());
            DA.SetDataList("Iy", secProps.Select(s => s.Iy).ToList());
            DA.SetDataList("Iz", secProps.Select(s => s.Iz).ToList());
            DA.SetDataList("Wy", secProps.Select(s => s.Wy).ToList());
            DA.SetDataList("Wz", secProps.Select(s => s.Wz).ToList());
            DA.SetDataList("It", secProps.Select(s => s.It).ToList());
            DA.SetDataTree(8, general);
            DA.SetDataTree(9, firstP);
            DA.SetDataTree(10, secondP);
            DA.SetDataList("SectionNames", secProps.Select(s => s.Section).ToList());
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SectionGetSectionProperties;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{3AFD6834-3CF0-4F1E-AE3A-C4A1C796420C}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}