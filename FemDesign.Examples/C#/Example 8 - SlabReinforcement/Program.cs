using FemDesign.Calculate;
using FemDesign.Loads;
using FemDesign.Results;
using FemDesign.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Reinforcement;
using FemDesign.Bars;
using FemDesign.Materials;
namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // define slab region
            var region = FemDesign.Geometry.Region.RectangleXY( new Geometry.Point3d(0,0,0), 5, 5 );

            // define materials
            var concrete = FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37");
            var reinforcement = FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("B500B");

            var thickness = new List<FemDesign.Shells.Thickness>{
                new FemDesign.Shells.Thickness(region.Plane.Origin, 0.3)
            };

            // slab
            var slab = FemDesign.Shells.Slab.Plate("S", concrete, region, FemDesign.Shells.EdgeConnection.Rigid, null, null, thickness);

            // define reinforcement properties
            var diamenter = 0.012;
            var cover = 0.025;
            var space = 0.15;
            var wire = new Wire(diamenter, reinforcement, WireProfileType.Ribbed);

            var straight_x_top = new Straight(ReinforcementDirection.X, space, GenericClasses.Face.Top, cover);
            var straight_x_bottom = new Straight(ReinforcementDirection.X, space, GenericClasses.Face.Bottom, cover);

            var straight_y_top = new Straight(ReinforcementDirection.Y, space, GenericClasses.Face.Top, cover);
            var straight_y_bottom = new Straight(ReinforcementDirection.Y, space, GenericClasses.Face.Bottom, cover);

            var straight = new List<Straight>
            {
                straight_x_top,
                straight_x_bottom,
                straight_y_top,
                straight_y_bottom,
            };

            // create the straight reinforcement objects
            // Note: Region can be a subregion of the slab.

            var srfReinf = new List<SurfaceReinforcement>();
            foreach (var s in straight)
            {
                var straightReinf = SurfaceReinforcement.DefineStraightSurfaceReinforcement(region, s, wire);
                srfReinf.Add(straightReinf);
            }

            // define the slab with reinforcement
            var reinfSlab = FemDesign.Reinforcement.SurfaceReinforcement.AddReinforcementToSlab(slab, srfReinf);

            var elements = new List<FemDesign.GenericClasses.IStructureElement> { reinfSlab };

            var model = new Model(Country.S, elements);

            using( var connection = new FemDesignConnection(keepOpen: true))
            {
                connection.Open(model);
            }
        }
    }
}