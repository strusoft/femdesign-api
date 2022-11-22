using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.GenericClasses;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // EXAMPLE 7: CREATING A WALL
            // This example shows how to model a concrete wall,
            // with a line support and line load. Before running,
            // make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // Geometry
            var p1 = new Geometry.Point3d(2, 0, 0);
            var p2 = new Geometry.Point3d(10, 0, 0);

            double height = 2.0;
            double thickness = 0.3;

            var materialsDatabase = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");
            Materials.Material material = materialsDatabase.MaterialByName("C35/45");

            var wall = Shells.Slab.Wall(p1, p2, height, thickness, material);

            var bottomEdge = wall.SlabPart.Region.Contours[0].Edges[0];
            var topEdge = wall.SlabPart.Region.Contours[0].Edges[2];
            var support = Supports.LineSupport.Rigid(bottomEdge, false);


            // Loads
            var deadload = new Loads.LoadCase("Dead load", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var imposed = new Loads.LoadCase("Imposed", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var lineLoad = new Loads.LineLoad(topEdge, -Geometry.Vector3d.UnitZ, imposed, Loads.ForceLoadType.Force);

            var uls = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary,
                (deadload, 1.2),
                (imposed, 1.35));


            // Model
            Model model = new Model(Country.S)
                .AddElements<IStructureElement>(wall, support)
                .AddLoads(lineLoad)
                .AddLoadCases(deadload, imposed)
                .AddLoadCombinations(uls);

            using (var femDesign = new FemDesignConnection(keepOpen: true))
            {
                femDesign.Open(model);
            }
        }
    }
}
